import asyncio
from fastapi import FastAPI, BackgroundTasks
import paho.mqtt.client as mqtt

app = FastAPI()
mqtt_connections = {}


@app.on_event("startup")
async def startup_event():
    pass


@app.on_event("shutdown")
async def shutdown_event():
    for connection_key, client in mqtt_connections.items():
        client.disconnect()


async def send_heartbeat(connection_key: str):
    client = mqtt_connections.get(connection_key)
    if client is None:
        return

    while True:
        await asyncio.sleep(5)
        client.publish("Heartbeat", f"{connection_key}")


@app.post("/start-simulation/{smart_home_id}")
async def start_simulation(smart_home_id: str, background_tasks: BackgroundTasks):
    client = mqtt.Client()
    client.connect("localhost", 1883)
    mqtt_connections[smart_home_id] = client
    background_tasks.add_task(send_heartbeat, smart_home_id)
    return {"message": f"Simulation started for connection {smart_home_id}"}


@app.post("/stop-simulation/{connection_key}")
async def stop_simulation(connection_key: str):
    client = mqtt_connections.get(connection_key)
    if client is not None:
        client.disconnect()
        del mqtt_connections[connection_key]
        return {"message": f"Simulation stopped for connection {connection_key}"}
    else:
        return {"message": f"Connection {connection_key} not found"}

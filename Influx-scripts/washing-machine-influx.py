from influxdb_client import InfluxDBClient, Point
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS
import random

BATCH_SIZE = 100  # Set the desired batch size
modes=[{"mode":"antiallergy","temperature":60},
       {"mode":"white wash","temperature":90},
       {"mode":"mixed wash","temperature":30}]



def send_power_influx_data_batch(points):
    token = "mK1Ccv2QzCm8EF75FMbeHe6owjn4O6010IC2TjsTpuF3FaaKOrfbcU7XMd28V5hWcv2Oe6ABVuENyTLIUmO9yw=="
    org = "IntelliHome"
    url = "http://localhost:8086"
    bucket = "intellihome_influx"
    influxdb_client = InfluxDBClient(url=url, token=token, org=org)
    write_api = influxdb_client.write_api(write_options=SYNCHRONOUS)

    write_api.write(bucket=bucket, org=org, record=points)




def generate_data(id, start_date, end_date,current_mode):
    current_date = start_date
    points = []
    availability_points = []
    actions=[]
    mode=current_mode

    while current_date <= end_date:

        if current_date.day % 5 == 0:
            availability_point = {
                "measurement": "availability",
                "tags": {
                    "deviceId": id,
                },
                "time": current_date,
                "fields": {
                    "isConnected": 0
                }
            }
            availability_points.append(availability_point)
            current_date += timedelta(minutes=1)
            continue

        if current_date.minute%30==0:
            new_mode=modes[random.randint(0,2)]
            if new_mode['mode']!=mode['mode']:
                mode=new_mode
                action = {"measurement": "washingMachineAction",
                          "tags": {
                              "deviceId": id,
                              "actionBy": "user1"
                          },
                          "time": current_date,
                          "fields": {
                              "action": f"CHANGE MODE {mode['mode']}"
                          }}

                actions.append(action)



        point = {
            "measurement": "washingMachine",
            "tags": {
                "deviceId": id,
                "mode":mode['mode']
            },
            "time": current_date,
            "fields": {
                "consumptionPerMinute": 1. / 60.,
                "temperature": float(mode['temperature']),
            }
        }

        availability_point = {
            "measurement": "availability",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "isConnected": 1
            }
        }


        points.append(point)
        availability_points.append(availability_point)

        if len(points) >= BATCH_SIZE:
            send_power_influx_data_batch(points)
            points = []

        if len(availability_points) >= BATCH_SIZE:
            send_power_influx_data_batch(availability_points)
            availability_points = []

        if len(actions) >= BATCH_SIZE:
            send_power_influx_data_batch(actions)
            actions = []

        current_date += timedelta(minutes=1)

    if points:
        send_power_influx_data_batch(points)
    if availability_points:
        send_power_influx_data_batch(availability_points)
    if actions:
        send_power_influx_data_batch(actions)


if __name__ == "__main__":
    current_mode = modes[0]
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    ambiend_sendor_ids = ["894b6b2a-dbee-4554-8b28-34b2b9c9558c","e13f8404-d9d5-4b90-a3b7-f52589b23c65"]
    for id in ambiend_sendor_ids:
        print(f"Generating data for washing machine {id}")
        generate_data(id, start_date, end_date,current_mode)
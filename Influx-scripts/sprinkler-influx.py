from influxdb_client import InfluxDBClient, Point
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS
import random

BATCH_SIZE = 100  # Set the desired batch size

def send_power_influx_data_batch(points):
    token = "13aT-AkxH1J9Gy0ehYNSqFZD1VflR8h51G32EcCsyutU31US3YKmzZU0lX8QTo-Igy2BlvXEy8afRl7r8CCOCQ=="
    org = "IntelliHome"
    url = "http://localhost:8086"
    bucket = "intellihome_influx"
    influxdb_client = InfluxDBClient(url=url, token=token, org=org)
    write_api = influxdb_client.write_api(write_options=SYNCHRONOUS)
    
    write_api.write(bucket=bucket, org=org, record=points)
    

def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []
    availability_points = []
    actions_points = []

    last_isSpraying = False

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

        if current_date.hour < 6 or current_date.hour > 18:
            isSpraying = True
        else:
            isSpraying = False
        
        if isSpraying != last_isSpraying:
            last_isSpraying = isSpraying
            action_point = {
                "measurement": "sprinklerAction",
                "tags": {
                    "deviceId": id,
                    "actionBy": "System"
                },
                "time": current_date,
                "fields": {
                    "action": "SPRAYING: " + ("ON" if isSpraying else "OFF")
                }
            }
            actions_points.append(action_point)


        point = {
            "measurement": "sprinkler",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "consumptionPerMinute": 1./60.,
                "isSpraying": isSpraying
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

        if len(actions_points) >= BATCH_SIZE:
            send_power_influx_data_batch(actions_points)
            actions_points = []

        current_date += timedelta(minutes=1)

    if points:
        send_power_influx_data_batch(points)
    if availability_points:
        send_power_influx_data_batch(availability_points)
    if actions_points:
        send_power_influx_data_batch(actions_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)  
    end_date = datetime.utcnow()
    lamp_ids = ["d678e21f-8d43-4fd0-88be-0d7cc7cfc8a5", "09e74ff3-c07c-4d3a-89a0-6eadf8841a36", "f700086d-8002-4108-9dc8-4829470f8813", "cced7f50-9579-4a95-a9ff-30691303fc40"]
    for id in lamp_ids:
        print(f"Generating data for sprinkler {id}")
        generate_data(id, start_date, end_date)
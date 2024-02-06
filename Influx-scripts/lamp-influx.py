from influxdb_client import InfluxDBClient, Point
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS
import random

BATCH_SIZE = 100  # Set the desired batch size

def send_power_influx_data_batch(points):
    token = "-oWMMrTAzgMKRevnkmvYtRvstG9KWMS4PZuJiA7Rjng7qQXvRI5GXZcEdX_UsP1n10k4aCe6GRS2paB5eNGTpQ=="
    org = "IntelliHome"
    url = "http://localhost:8086"
    bucket = "intellihome_influx"
    influxdb_client = InfluxDBClient(url=url, token=token, org=org)
    write_api = influxdb_client.write_api(write_options=SYNCHRONOUS)
    
    write_api.write(bucket=bucket, org=org, record=points)


def generate_lumens(current_time):
    hours = current_time.hour + current_time.minute / 60

    time_diff_noon = abs(12 - hours)
    if time_diff_noon > 12:
        time_diff_noon = 24 - time_diff_noon

    lumens = 1000 * (1 - (time_diff_noon / 12)) 

    lumens = max(0, round(lumens, 2)) 

    return lumens


def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []
    availability_points = []

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


        if current_date.day % 2 == 0:
            isAuto = float(1)
        else:
            isAuto = float(0)

        brightnessLimit = 300
        currentBrightness = generate_lumens(current_date)
        currentBrightness = float(currentBrightness)
        isShining = float(1) if brightnessLimit > currentBrightness else float(0)

        point = {
            "measurement": "lamp",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "consumptionPerMinute": 1./60.,
                "isAuto": isAuto,
                "isShining": isShining,
                "currentBrightness": currentBrightness
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

        current_date += timedelta(minutes=1)

    if points:
        send_power_influx_data_batch(points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)  
    end_date = datetime.utcnow()
    lamp_ids = ["7e543f5f-4d2c-46c7-9db1-9f3dc1d5fb91", "228cb0e3-5a3f-4526-8688-7af3e7611c19", "e59de779-3d59-4457-a4e8-317fe9adaa0a", "3c57f7b3-27ed-4720-92f8-c761805c2f7b"]
    for id in lamp_ids:
        print(f"Generating data for lamp {id}")
        generate_data(id, start_date, end_date)
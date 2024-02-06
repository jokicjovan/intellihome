from influxdb_client import InfluxDBClient, Point
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS
import random

BATCH_SIZE = 100  # Set the desired batch size


def send_power_influx_data_batch(points):
    token = "mK1Ccv2QzCm8EF75FMbeHe6owjn4O6010IC2TjsTpuF3FaaKOrfbcU7XMd28V5hWcv2Oe6ABVuENyTLIUmO9yw=="
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


        point = {
            "measurement": "ambientSensor",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "consumptionPerMinute": 1. / 60.,
                "temperature": round(random.uniform(22.0, 28.0), 1),
                "humidity": round(random.uniform(30.0, 60.0), 1),
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
    ambiend_sendor_ids = ["129cfe43-7ac1-4ca8-92f2-edfe996ba509","b537c6e4-9c89-4b7f-b087-2a8e0d73d7d2"]
    for id in ambiend_sendor_ids:
        print(f"Generating data for ambient sensor {id}")
        generate_data(id, start_date, end_date)
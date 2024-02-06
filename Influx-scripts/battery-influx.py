import random

from influxdb_client import InfluxDBClient
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS

BATCH_SIZE = 100


def send_influx_data_batch(points):
    token = "sGZO_SK-sINKX48v5yDyZQ3e-p4cdhE8pdGRzHiztLEBJXzVaOXwkz-MvBSX9enffAtczxH6_BNhb9UH1Y7K0w=="
    org = "IntelliHome"
    url = "http://localhost:8086"
    bucket = "intellihome_influx"
    influxdb_client = InfluxDBClient(url=url, token=token, org=org)
    write_api = influxdb_client.write_api(write_options=SYNCHRONOUS)
    write_api.write(bucket=bucket, org=org, record=points)


def generate_battery_capacity(current_date, intervals_per_day=24 * 60):
    def random_fluctuation(current_capacity):
        if random.random() < 0.1:
            return 0
        change = random.uniform(0, 20)
        new_capacity = current_capacity + change
        return min(1000, new_capacity)

    initial_capacity = 0
    current_time = current_date.hour * 60 + current_date.minute
    interval_size = intervals_per_day // 24
    generated_capacity = initial_capacity
    for _ in range(current_time // interval_size):
        generated_capacity = random_fluctuation(generated_capacity)
    return round(generated_capacity, 4)


def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []
    availability_points = []

    while current_date <= end_date:
        if current_date.day % 20 == 0:
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
            "measurement": "batterySystemCapacity",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "currentCapacity": float(generate_battery_capacity(current_date))
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
            send_influx_data_batch(points)
            points = []
        if len(availability_points) >= BATCH_SIZE:
            send_influx_data_batch(availability_points)
            availability_points = []

        current_date += timedelta(minutes=1)

    if points:
        send_influx_data_batch(points)
    if availability_points:
        send_influx_data_batch(availability_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    batteries_ids = ["25684b94-46a0-465c-ba74-fb2512c49b04", "4072fc8a-50cf-4c47-b514-63fdee94561e",
                     "a9ef7e50-7854-4820-9c61-ceb020f01ba5", "f502e63c-b8c4-431f-b7bb-263381b24bc3"]
    for id in batteries_ids:
        print(f"Generating data for battery {id}")
        generate_data(id, start_date, end_date)

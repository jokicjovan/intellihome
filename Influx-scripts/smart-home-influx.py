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


def generate_power_data(current_date):
    def random_power_value():
        return random.uniform(1, 1000)

    power_consumption = random_power_value() / 60
    power_production = random_power_value() / 60

    power_consumption = max(0., power_consumption)
    power_production = max(0., power_production)

    consumption_from_grid = power_consumption - power_production

    return {
        'consumptionPerMinute': power_consumption,
        'productionPerMinute': power_production,
        'gridPerMinute': consumption_from_grid
    }


def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []
    availability_points = []

    while current_date <= end_date:
        point = {
            "measurement": "smartHomeUsage",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": generate_power_data(current_date)
        }

        points.append(point)
        if len(points) >= BATCH_SIZE:
            send_influx_data_batch(points)
            points = []

        current_date += timedelta(minutes=1)

    if points:
        send_influx_data_batch(points)
    if availability_points:
        send_influx_data_batch(availability_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    smart_homes_ids = ["f9e0ffaa-8021-4080-9dbe-f6390ee35317", "356f2eac-f0b5-4bbc-9eca-3d9755f6ca44",
                       "4dae0572-609c-4561-9325-720233760611", "ded85e2b-8b77-4255-b0ba-ed87e42ef0d3"]
    for id in smart_homes_ids:
        print(f"Generating data for smart homes {id}")
        generate_data(id, start_date, end_date)

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


def generate_charging_point_capacity(current_date, intervals_per_day=24 * 60, increase_interval=120):
    initial_capacity = 0
    max_capacity = 1000

    def random_fluctuation(current_capacity):
        if random.random() < 0.1:
            return 0
        change = random.uniform(0, 10)
        new_capacity = current_capacity + change
        return min(max_capacity, new_capacity)

    current_time = current_date.hour * 60 + current_date.minute
    interval_size = intervals_per_day // 24
    reset_interval = intervals_per_day // increase_interval

    generated_capacity = initial_capacity
    for _ in range(current_time // interval_size):
        generated_capacity = random_fluctuation(generated_capacity)

        if _ % reset_interval == 0 and _ != 0:
            generated_capacity = initial_capacity

    return round(generated_capacity, 4)


def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []

    while current_date <= end_date:
        point = {
            "measurement": "vehicleChargingPointData",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "currentCapacity": float(generate_charging_point_capacity(current_date))
            }
        }
        points.append(point)
        if len(points) >= BATCH_SIZE:
            send_influx_data_batch(points)
            points = []

        current_date += timedelta(minutes=1)

    if points:
        send_influx_data_batch(points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    charging_points_ids = ["7b0490df-0600-4ff0-a64c-945e40bfc43c", "9f14f8c2-81c1-4f55-884d-a810cf4a7c1a",
                           "923626fe-7434-4e1a-a1e1-2bda49afaed8", "0641e80f-0aa4-4a50-a6fb-83da8328898c",
                           "5a578c6c-f175-48f7-9a48-1bf038e7defd"]
    for id in charging_points_ids:
        print(f"Generating data for charging point {id}")
        generate_data(id, start_date, end_date)

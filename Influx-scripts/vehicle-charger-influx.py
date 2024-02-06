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


def generate_data(id, start_date, end_date):
    current_date = start_date
    availability_points = []
    actions_points = []

    while current_date <= end_date:
        if current_date.day % 10 == 0:
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

            action_point = {
                "measurement": "vehicleChargerAction",
                "tags": {
                    "deviceId": id,
                    "actionBy": "SYSTEM"
                },
                "time": current_date,
                "fields": {
                    "action": "ON"
                }
            }
            actions_points.append(action_point)
            action_point = {
                "measurement": "vehicleChargerAction",
                "tags": {
                    "deviceId": id,
                    "actionBy": "SYSTEM"
                },
                "time": current_date + timedelta(hours=2),
                "fields": {
                    "action": "OFF"
                }
            }
            actions_points.append(action_point)

            current_date += timedelta(minutes=1)
            continue

        if current_date.hour % 5 == 0:
            action_point = {
                "measurement": "vehicleChargerAction",
                "tags": {
                    "deviceId": id,
                    "actionBy": "SYSTEM"
                },
                "time": current_date,
                "fields": {
                    "action": "CHARGING STARTED"
                }
            }
            actions_points.append(action_point)

        if current_date.hour % 8 == 0:
            action_point = {
                "measurement": "vehicleChargerAction",
                "tags": {
                    "deviceId": id,
                    "actionBy": "SYSTEM"
                },
                "time": current_date,
                "fields": {
                    "action": "CHARGING FINISHED"
                }
            }
            actions_points.append(action_point)

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

        availability_points.append(availability_point)
        if len(availability_points) >= BATCH_SIZE:
            send_influx_data_batch(availability_points)
            availability_points = []
        if len(actions_points) >= BATCH_SIZE:
            send_influx_data_batch(actions_points)
            actions_points = []

        current_date += timedelta(minutes=1)

    if availability_points:
        send_influx_data_batch(availability_points)
    if actions_points:
        send_influx_data_batch(actions_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    vehicle_chargers = ["10378a11-6d52-432a-bd63-69714f4a9053", "38fe2a62-f37a-47d5-9269-a40b057e3347",
                        "94ac9c33-de4b-48ea-bf49-94af0d6e6ad6", "d41fa3e0-fe15-4257-a6c8-88848dbd3726"]
    for id in vehicle_chargers:
        print(f"Generating data for vehicle charger {id}")
        generate_data(id, start_date, end_date)

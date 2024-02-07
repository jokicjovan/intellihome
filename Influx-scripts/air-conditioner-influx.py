from influxdb_client import InfluxDBClient, Point
from datetime import datetime, timedelta
from influxdb_client.client.write_api import SYNCHRONOUS
import random

BATCH_SIZE = 100  # Set the desired batch size
modes = [{"mode": "auto", "temperature": [16, 28]},
         {"mode": "heat", "temperature": [16, 22]},
         {"mode": "cool", "temperature": [20, 28]},
         {"mode": "fan", "temperature": [20, 28]}]


def send_power_influx_data_batch(points):
    token = "mK1Ccv2QzCm8EF75FMbeHe6owjn4O6010IC2TjsTpuF3FaaKOrfbcU7XMd28V5hWcv2Oe6ABVuENyTLIUmO9yw=="
    org = "IntelliHome"
    url = "http://localhost:8086"
    bucket = "intellihome_influx"
    influxdb_client = InfluxDBClient(url=url, token=token, org=org)
    write_api = influxdb_client.write_api(write_options=SYNCHRONOUS)

    write_api.write(bucket=bucket, org=org, record=points)


def generate_data(id, start_date, end_date, current_mode, current_temperature=20):
    current_date = start_date
    points = []
    availability_points = []
    actions = []
    mode = current_mode
    temperature = current_temperature

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

        if current_date.minute % 10 == 0:
            if random.randint(1, 100) % 2 == 0:
                new_mode = modes[random.randint(0, 3)]
                if new_mode['mode'] != mode['mode']:
                    mode = new_mode
                    action = {"measurement": "airConditionerAction",
                              "tags": {
                                  "deviceId": id,
                                  "actionBy": "user1"
                              },
                              "time": current_date,
                              "fields": {
                                  "action": f"CHANGE MODE {mode['mode'].upper()}"
                              }}

                    actions.append(action)
                else:
                    new_temperature=random.randint(mode['temperature'][0],mode['temperature'][1])
                    if new_temperature != temperature:
                        temperature=new_temperature
                        action = {"measurement": "airConditionerAction",
                                  "tags": {
                                      "deviceId": id,
                                      "actionBy": "user1"
                                  },
                                  "time": current_date,
                                  "fields": {
                                      "action": f"CHANGE TEMPERATURE {temperature}"
                                  }}

                        actions.append(action)

        point = {
            "measurement": "airConditioner",
            "tags": {
                "deviceId": id,
                "mode": mode['mode']
            },
            "time": current_date,
            "fields": {
                "consumptionPerMinute": 1. / 60.,
                "temperature": float(temperature),
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
    ambiend_sendor_ids = ["8817fb61-9423-49df-b64a-327de9ba935c", "73d5d3f6-aa08-4cf3-befb-87c44392640b"]
    for id in ambiend_sendor_ids:
        print(f"Generating data for air conditioner={id}")
        generate_data(id, start_date, end_date, current_mode)

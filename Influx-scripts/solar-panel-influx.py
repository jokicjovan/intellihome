import pandas as pd
import pvlib
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


def generate_production_per_minute(current_date, area, efficiency):
    pd_current_datetime = pd.to_datetime([current_date]).tz_localize('UTC')
    solar_position = pvlib.solarposition.get_solarposition(pd_current_datetime, latitude=44.786568,
                                                           longitude=20.448921, altitude=155.813446)
    solar_zenith = solar_position['apparent_zenith']
    solar_azimuth = solar_position['azimuth']

    power_per_minute = 0
    # Check if it's daytime (Sun is above the horizon)
    if solar_zenith.values < 90:
        solar_irradiance = pvlib.irradiance.get_total_irradiance(
            solar_zenith=solar_zenith,
            solar_azimuth=solar_azimuth,
            surface_tilt=0,  # Assuming a horizontal panel
            surface_azimuth=180,  # Facing south
            dni=5.0,  # Direct Normal Irradiance assumed for simplicity
            ghi=5.0,  # Global Horizontal Irradiance assumed for simplicity
            dhi=2.5  # Diffuse Horizontal Irradiance assumed for simplicity
        )
        # Calculate energy produced per minute
        power_per_minute = round(solar_irradiance['poa_global'].mean() * area * efficiency / 100 / 60, 4)
    return power_per_minute


def generate_data(id, start_date, end_date, area, efficiency):
    current_date = start_date
    points = []
    availability_points = []
    actions_points = []

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

            action_point = {
                "measurement": "solarPanelSystemAction",
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
                "measurement": "solarPanelSystemAction",
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

        point = {
            "measurement": "solarPanelSystemProduction",
            "tags": {
                "deviceId": id,
            },
            "time": current_date,
            "fields": {
                "productionPerMinute": float(generate_production_per_minute(current_date, area, efficiency))
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
        if len(actions_points) >= BATCH_SIZE:
            send_influx_data_batch(actions_points)
            actions_points = []

        current_date += timedelta(minutes=1)

    if points:
        send_influx_data_batch(points)
    if availability_points:
        send_influx_data_batch(availability_points)
    if actions_points:
        send_influx_data_batch(actions_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)
    end_date = datetime.utcnow()
    solar_panels = {"339cdfe7-5b20-4caa-a0ef-c55a29872593": [70, 65], "7c1aa725-b6f7-4112-b632-74ddd5894c4e": [50, 60],
                    "c2cbe74b-e6d5-41b8-a500-2fd914736d58": [80, 45], "ee5f7e05-97d8-43f7-aed5-0306b22f008f": [120, 70]}
    for key, value in solar_panels.items():
        print(f"Generating data for solar panel {key}")
        generate_data(key, start_date, end_date, value[0], value[1])

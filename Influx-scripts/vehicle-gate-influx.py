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


def yield_licence_plate(allowed_licence_plates):
    while True:
        if random.random() < 0.5:
            yield random.choice(allowed_licence_plates)
        else:
            yield random.choice(["ZR001AA", "ZR002AA", "ZR003AA", "ZR004AA", "ZR005AA", "ZR006AA", "ZR007AA", "ZR008AA",
                                 "SM001AA", "SM002AA", "SM003AA", "SM004AA", "SM005AA", "SM006AA", "SM007AA", "SM008AA",
                                 "BG001AA", "BG002AA", "BG003AA", "BG004AA", "BG005AA", "BG006AA", "BG007AA", "BG008AA",
                                 "NS001AA", "NS002AA", "NS003AA", "NS004AA", "NS005AA", "NS006AA", "NS007AA",
                                 "NS008AA"])
            


    

def generate_data(id, start_date, end_date):
    current_date = start_date
    points = []
    availability_points = []
    actions_points = []

    last_is_open_by_user = float(0)

    allowed_licence_plates = ["AB123BA", "ZR023ZR", "SM123SM"]
    licence_plates_in_home = []

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

        isPublic = float(1)
        isOpenedByUser = float(1) if random.random() > 0.9 else float(0)
        licencePlate = next(yield_licence_plate(allowed_licence_plates))
        user = "user1" if isOpenedByUser == 1 else "System"

        if isOpenedByUser != last_is_open_by_user:
            last_is_open_by_user = isOpenedByUser
            action_point = {
                "measurement": "vehicleGateAction",
                "tags": {
                    "deviceId": id,
                    "username": user
                },
                "time": current_date,
                "fields": {
                    "action": "OPENED_BY_USER" if isOpenedByUser == 1 else "CLOSED_BY_USER"
                }
            }
            actions_points.append(action_point)


        if isOpenedByUser == 1:
            if licencePlate in licence_plates_in_home:
                point = {
                    "measurement": "vehicleGate",
                    "tags": {
                        "deviceId": id,
                        "licencePlate": licencePlate
                    },
                    "time": current_date,
                    "fields": {
                        "consumptionPerMinute": 1./60.,
                        "isEntering": float(1),
                        "isOpen": float(1),
                        "isOpenedByUser": isOpenedByUser,
                        "isPublic": isPublic,
                    }
                }
                points.append(point)   
                licence_plates_in_home.remove(licencePlate)
            else:
                point = {
                    "measurement": "vehicleGate",
                    "tags": {
                        "deviceId": id,
                        "licencePlate": licencePlate
                    },
                    "time": current_date,
                    "fields": {
                        "consumptionPerMinute": 1./60.,
                        "isEntering": float(0),
                        "isOpen": float(1),
                        "isOpenedByUser": isOpenedByUser,
                        "isPublic": isPublic,
                    }
                }
                points.append(point)
                licence_plates_in_home.append(licencePlate)
            continue


        if licencePlate in licence_plates_in_home:
            point = {
                "measurement": "vehicleGate",
                "tags": {
                    "deviceId": id,
                    "licencePlate": licencePlate
                },
                "time": current_date,
                "fields": {
                    "consumptionPerMinute": 1./60.,
                    "isEntering": float(0),
                    "isOpen": float(1),
                    "isOpenedByUser": isOpenedByUser,
                    "isPublic": isPublic,
                }
            }
            points.append(point)

            action_point = {
                "measurement": "vehicleGateAction",
                "tags": {
                    "deviceId": id,
                    "username": user
                },
                "time": current_date,
                "fields": {
                    "action": "OPENED_BY_SYSTEM"
                }
            }
            actions_points.append(action_point)

            current_date += timedelta(minutes=3)

            point = {
                "measurement": "vehicleGate",
                "tags": {
                    "deviceId": id,
                    "licencePlate": licencePlate
                },
                "time": current_date,
                "fields": {
                    "consumptionPerMinute": 1./60.,
                    "isEntering": float(0),
                    "isOpen": float(0),
                    "isOpenedByUser": isOpenedByUser,
                    "isPublic": isPublic,
                }
            }
            points.append(point)

            action_point = {
                "measurement": "vehicleGateAction",
                "tags": {
                    "deviceId": id,
                    "username": user
                },
                "time": current_date,
                "fields": {
                    "action": "CLOSED_BY_SYSTEM"
                }
            }
            actions_points.append(action_point)
            licence_plates_in_home.remove(licencePlate)
        else:
            if isPublic == 1:
                point = {
                    "measurement": "vehicleGate",
                    "tags": {
                        "deviceId": id,
                        "licencePlate": licencePlate
                    },
                    "time": current_date,
                    "fields": {
                        "consumptionPerMinute": 1./60.,
                        "isEntering": float(1),
                        "isOpen": float(1),
                        "isOpenedByUser": isOpenedByUser,
                        "isPublic": isPublic,
                    }
                }
                points.append(point)

                action_point = {
                    "measurement": "vehicleGateAction",
                    "tags": {
                        "deviceId": id,
                        "username": user
                    },
                    "time": current_date,
                    "fields": {
                        "action": "OPENED_BY_SYSTEM"
                    }
                }
                actions_points.append(action_point)
                licence_plates_in_home.append(licencePlate)

                current_date += timedelta(minutes=3)

                point = {
                    "measurement": "vehicleGate",
                    "tags": {
                        "deviceId": id,
                        "licencePlate": licencePlate
                    },
                    "time": current_date,
                    "fields": {
                        "consumptionPerMinute": 1./60.,
                        "isEntering": float(1),
                        "isOpen": float(0),
                        "isOpenedByUser": isOpenedByUser,
                        "isPublic": isPublic,
                    }
                }
                points.append(point)

                action_point = {
                    "measurement": "vehicleGateAction",
                    "tags": {
                        "deviceId": id,
                        "username": user
                    },
                    "time": current_date,
                    "fields": {
                        "action": "CLOSED_BY_SYSTEM"
                    }
                }
                actions_points.append(action_point)
            else:
                if licencePlate in allowed_licence_plates:
                    point = {
                        "measurement": "vehicleGate",
                        "tags": {
                            "deviceId": id,
                            "licencePlate": licencePlate
                        },
                        "time": current_date,
                        "fields": {
                            "consumptionPerMinute": 1./60.,
                            "isEntering": float(1),
                            "isOpen": float(1),
                            "isOpenedByUser": isOpenedByUser,
                            "isPublic": isPublic,
                        }
                    }
                    points.append(point)

                    action_point = {
                        "measurement": "vehicleGateAction",
                        "tags": {
                            "deviceId": id,
                            "username": user
                        },
                        "time": current_date,
                        "fields": {
                            "action": "OPENED_BY_SYSTEM"
                        }
                    }
                    actions_points.append(action_point)

                    licence_plates_in_home.append(licencePlate)

                    current_date += timedelta(minutes=3)
                    point = {
                        "measurement": "vehicleGate",
                        "tags": {
                            "deviceId": id,
                            "licencePlate": licencePlate
                        },
                        "time": current_date,
                        "fields": {
                            "consumptionPerMinute": 1./60.,
                            "isEntering": float(1),
                            "isOpen": float(0),
                            "isOpenedByUser": isOpenedByUser,
                            "isPublic": isPublic,
                        }
                    }
                    points.append(point)

                    action_point = {
                        "measurement": "vehicleGateAction",
                        "tags": {
                            "deviceId": id,
                            "username": user
                        },
                        "time": current_date,
                        "fields": {
                            "action": "CLOSED_BY_SYSTEM"
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

        current_date += timedelta(minutes=10)

    if points:
        send_power_influx_data_batch(points)
    if availability_points:
        send_power_influx_data_batch(availability_points)
    if actions_points:
        send_power_influx_data_batch(actions_points)


if __name__ == "__main__":
    start_date = datetime.utcnow() - timedelta(days=90)  
    end_date = datetime.utcnow()
    lamp_ids = ["406a64e2-e36e-4b69-ae98-525db56ffddc", "a7eb6e16-33b0-4d6c-a654-e81ab3cc544d", "5797c9c4-c6c3-4a6a-8a2d-9962c87eba1e", "359df155-2c39-4c95-b299-bb172caddd5b"]
    for id in lamp_ids:
        print(f"Generating data for vehicleGate {id}")
        generate_data(id, start_date, end_date)
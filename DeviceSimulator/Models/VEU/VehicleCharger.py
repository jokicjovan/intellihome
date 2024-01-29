import asyncio
import datetime
import json
from dataclasses import dataclass
from enum import Enum

from Models.SmartDevice import SmartDevice


class ChargingStatus(str, Enum):
    CHARGING = "CHARGING",
    FINISHED = "FINISHED"


@dataclass
class ChargingPoint:
    charging_point_id: str
    capacity: float
    current_capacity: float
    charge_limit: float
    start_time: datetime.datetime = datetime.datetime.now()
    end_time: datetime.datetime = datetime.datetime.min
    status: ChargingStatus = ChargingStatus.CHARGING

    # total_consumption: float = 0

    def serialize(self):
        return {
            "chargingPointId": self.charging_point_id,
            "capacity": round(self.capacity, 4),
            "chargeLimit": round(self.charge_limit, 2),
            "currentCapacity": round(self.current_capacity, 4),
            # "totalConsumption": round(self.total_consumption, 4),
            "startTime": self.start_time.isoformat(),
            "endTime": self.end_time.isoformat(),
            "status": self.status
        }


class VehicleCharger(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, power_per_hour, charging_points_ids):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.power_per_hour = power_per_hour
        self.free_charging_points = charging_points_ids
        self.busy_charging_points = {}

    def on_data_receive(self, client, user_data, msg):
        super().on_data_receive(client, user_data, msg)
        if msg.topic == self.receive_topic:
            data = json.loads(msg.payload.decode())
            if data.get("action") == "chargingPointConnected":
                if len(self.free_charging_points) > 0:
                    self.busy_charging_points[data.get("charging_pointId")] = (
                        ChargingPoint(data.get("chargingPointId"),
                                      float(data.get("capacity")),
                                      float(data.get("currentCapacity")),
                                      float(data.get("chargeLimit"))))
                    self.free_charging_points.remove(data.get("chargingPointId"))
                    self.client.publish(self.send_topic, json.dumps({
                        "action": "chargingStarted", "chargingPointId": data.get("chargingPointId")}), retain=False)

            elif data.get("action") == "chargingPointDisconnected":
                self.busy_charging_points.pop(data.get("chargingPointId"))
                self.free_charging_points.append(data.get("chargingPointId"))

    async def send_data(self):
        while self.is_on.is_set():
            consumption_per_minute = 0
            unfinished_charging_points_number = sum(1 for charging_point in self.busy_charging_points.values()
                                                    if charging_point.status is ChargingStatus.CHARGING)
            if unfinished_charging_points_number > 0:
                minute_charge_per_point = (self.power_per_hour / 60 / unfinished_charging_points_number)

                for charging_point in self.busy_charging_points.values():
                    if charging_point.status is ChargingStatus.CHARGING:
                        current_charge_per_point = minute_charge_per_point

                        # Ako je limit prekoracen, smanji struju da odgovara limitu i postavi status
                        if (charging_point.current_capacity + current_charge_per_point >=
                                charging_point.capacity * charging_point.charge_limit):
                            current_charge_per_point -= ((charging_point.current_capacity +
                                                          current_charge_per_point) -
                                                         charging_point.capacity *
                                                         charging_point.charge_limit)

                            charging_point.end_time = datetime.datetime.now()
                            charging_point.status = ChargingStatus.FINISHED
                            self.client.publish(self.send_topic, json.dumps({
                                "action": "chargingFinished", "chargingPointId": charging_point.charging_point_id}),
                                                retain=False)

                        charging_point.current_capacity += current_charge_per_point
                        # charging_point.total_consumption += current_charge_per_point
                        consumption_per_minute += current_charge_per_point

            serialized_data = [charging_point.serialize() for charging_point in
                               self.busy_charging_points.values()]
            self.client.publish(self.send_topic, json.dumps({
                "busyChargingPoints": serialized_data,
                "consumptionPerMinute": round(consumption_per_minute, 4)}), retain=False)

            await asyncio.sleep(10)

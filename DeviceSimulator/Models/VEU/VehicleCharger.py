import asyncio
import datetime
import json
from dataclasses import dataclass, asdict
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
    consumption: float = 0

    def serialize(self):
        return {
            "charging_point_id": self.charging_point_id,
            "capacity": round(self.capacity, 4),
            "current_capacity": round(self.current_capacity, 4),
            "charge_limit": round(self.charge_limit, 4),
            "start_time": self.start_time.isoformat(),
            "end_time": self.end_time.isoformat(),
            "status": self.status,
            "consumption": round(self.consumption, 4)
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
                    self.busy_charging_points[data.get("id")] = ChargingPoint(data.get("id"),
                                                                              float(data.get("capacity")),
                                                                              float(data.get("current_capacity")),
                                                                              float(data.get("charge_limit")))
                    self.free_charging_points.remove(data.get("id"))
            elif data.get("action") == "chargingPointDisconnected":
                self.busy_charging_points.pop(data.get("id"))
                self.free_charging_points.append(data.get("id"))

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

                        charging_point.current_capacity += current_charge_per_point
                        charging_point.consumption += current_charge_per_point
                        consumption_per_minute += current_charge_per_point

            serialized_data = [charging_point.serialize() for charging_point in
                               self.busy_charging_points.values()]
            self.client.publish(self.send_topic, json.dumps({
                "busyChargingPoints": serialized_data,
                "consumptionPerMinute": round(consumption_per_minute, 4)}), retain=False)

            await asyncio.sleep(10)

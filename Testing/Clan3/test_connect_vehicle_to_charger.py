from locust import HttpUser, task, between


class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def connect_vehicle_to_charger(self):
        vehicle_charger_id = "f901e6d3-9fba-4506-a23a-06c91ab97a27"
        vehicle_charging_point_id = "f901e6d3-9fba-4506-a23a-06c91ab97a27"
        vehicle_data = {
            "InitialCapacity": "10",
            "Capacity": "200",
            "ChargeLimit": "0.7"
        }
        response = self.client.put(
            f"/api/VehicleCharger/ConnectToCharger?vehicleChargerId={vehicle_charger_id}&"
            f"vehicleChargingPointId={vehicle_charging_point_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, json=vehicle_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

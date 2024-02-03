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
    def register_smart_device(self):
        smart_home_id = "32e8e8fd-354f-4b61-92da-dfaa3f6288f6"
        vehicle_gate_data = {
            "AllowedLicencePlates": ["SM023SA", "SM023AS"],
            "PowerPerHour": 2,
            "Name": "Gate"
        }
        response = self.client.put(
            f"/api/SPU/CreateVehicleGate/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=vehicle_gate_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

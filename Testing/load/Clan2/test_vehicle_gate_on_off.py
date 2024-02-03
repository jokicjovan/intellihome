from locust import HttpUser, task, between

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def toggle_vehicle_gate(self):
        vehicle_gate_id = "ce456c90-3bb2-42be-bd2a-4d75a6990291"
        turn_on = True

        response = self.client.put(
            f"/api/VehicleGate/Toggle?id={vehicle_gate_id}&turnOn={turn_on}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )

        if response.status_code != 200:
            self.environment.runner.quit()


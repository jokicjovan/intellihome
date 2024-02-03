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
        vehicle_gate_id = "3f457bd5-bd6e-494f-9841-a52c746e3896"
        turn_on = True
        response = self.client.put(
            f"/api/AmbientSensor/Toggle?id={vehicle_gate_id}&turnOn={turn_on}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )

        if response.status_code != 200:
            self.environment.runner.quit()


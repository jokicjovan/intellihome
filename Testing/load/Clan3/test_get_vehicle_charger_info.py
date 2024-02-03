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
    def get_vehicle_charger_data(self):
        vehicle_charger_id = "2b8eac46-868b-445e-bb7f-e2e9e2a6e153"
        response = self.client.get(
            f"/api/VehicleCharger/Get?id={vehicle_charger_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        )
        if response.status_code != 200:
            self.environment.runner.quit()

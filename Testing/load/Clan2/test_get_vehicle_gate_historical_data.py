from locust import HttpUser, task, between
from datetime import datetime

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def get_historical_action_data(self):
        vehicle_gate_id = "ce456c90-3bb2-42be-bd2a-4d75a6990291"
        from_date = datetime(2023, 1, 1)
        to_date = datetime(2023, 12, 31)

        response = self.client.get(
            f"/api/VehicleGate/GetHistoricalActionData?id={vehicle_gate_id}&from={from_date}&to={to_date}",
        )

        if response.status_code != 200:
            self.environment.runner.quit()
from datetime import datetime
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
    def get_solar_panel_action_historical_data(self):
        solar_panel_id = "6452026e-aa69-436b-8e53-c77b29cfc496"
        from_date = datetime(2023, 1, 1)
        to_date = datetime(2023, 12, 31)

        # Simulate retrieving historical action data
        response = self.client.get(
            f"/api/SolarPanelSystem/GetActionHistoricalData?id={solar_panel_id}&from={from_date}&to={to_date}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()

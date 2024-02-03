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
    def get_smart_home_usage(self):
        smart_home_id = "8385e781-88d2-4717-a939-39b1a16b6266"
        from_date = datetime(2023, 1, 1)  # Replace with the actual from date
        to_date = datetime(2023, 12, 31)  # Replace with the actual to date

        response = self.client.get(
            f"/api/SmartHome/GetUsageHistoricalData?id={smart_home_id}&from={from_date}&to={to_date}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()

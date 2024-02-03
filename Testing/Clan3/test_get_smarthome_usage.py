from datetime import datetime
from locust import HttpUser, task, between

class Test(HttpUser):
    wait_time = between(1, 3)
    host = "http://192.168.1.188:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def get_smarthome_usage(self):
        smarthome_id = "a6d8769c-d7d1-4ca6-b88c-1d72643d1075"
        from_date = datetime(2023, 1, 1)  # Replace with the actual from date
        to_date = datetime(2023, 12, 31)  # Replace with the actual to date

        response = self.client.get(
            f"/api/City/GetUsageHistoricalData?id={smarthome_id}&from={from_date}&to={to_date}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()
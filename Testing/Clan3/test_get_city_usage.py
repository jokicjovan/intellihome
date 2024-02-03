from datetime import datetime
from locust import HttpUser, task, between


class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "joki", "password": "Adminadmin1"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def get_city_usage(self):
        city_id = "43ef2fe2-750b-4889-9f3f-12155a0e8e92"
        from_date = datetime(2023, 1, 1)
        to_date = datetime(2023, 12, 31)
        response = self.client.get(
            f"/api/City/GetCityHistoricalData?id={city_id}&from={from_date}&to={to_date}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()

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
    def get_ambient_sensor_historical_data(self):
        ambient_sensor_id = "4f84c687-8338-4200-958e-0ae99a38c1ad"
        from_date = datetime(2023, 1, 1)
        to_date = datetime(2023, 12, 31)

        response = self.client.get(
            f"/api/AmbientSensor/GetHistoricalData?id={ambient_sensor_id}&from={from_date}&to={to_date}",
        )

        if response.status_code != 200:
            self.environment.runner.quit()


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
    def add_scheduled_work(self):
        schedule_data = {
            "id": "c1173988-1eac-4478-8af3-e518156d32bb",
            "temperature": 22.5,
            "mode": "auto",
            "startDate": "01/02/2024 10:00",
            "endDate": "02/02/2024 10:00"
        }

        response = self.client.post("/api/AirConditioner/AddScheduledWork", json=schedule_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()



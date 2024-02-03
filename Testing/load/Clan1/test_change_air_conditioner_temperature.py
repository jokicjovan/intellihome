from locust import HttpUser, task, between
import random

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def change_temperature(self):
        id = "c1173988-1eac-4478-8af3-e518156d32bb"
        temperature = random.randint(16, 30)

        response = self.client.put(f"/api/AirConditioner/ChangeTemperature?id={id}&temperature={temperature}", headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()

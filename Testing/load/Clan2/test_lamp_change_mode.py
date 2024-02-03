from locust import HttpUser, task, between
import random

class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def change_mode(self):
        lamp_id = "027bfe12-7eb4-42e8-88e2-9dd0025464f8"
        is_auto = ["true", "false"][random.randint(0, 1)]
        response = self.client.put(
            f"/api/Lamp/ChangeMode?id={lamp_id}&isAuto={is_auto}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        )
        if response.status_code != 200:
            self.environment.runner.quit()

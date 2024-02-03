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
    def change_mode(self):
        id = "39c9d210-d914-48a1-9eb9-6dde382b8e34"
        modes = ["Antiallergy", "Mixed wash", "White wash"]
        mode = random.choice(modes)

        response = self.client.put(f"/api/WashingMachine/ChangeMode?id={id}&mode={mode}", headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()



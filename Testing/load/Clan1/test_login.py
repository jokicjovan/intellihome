from locust import HttpUser, between, task
import random

class UserBehavior(HttpUser):
    wait_time = between(2, 5)
    host = "http://192.168.1.188:5283"

    @task
    def login(self):
        names = ["crni", "crni1", "crni2"]
        name = random.choice(names)
        self.client.post("/api/User/login", json={"username": name, "password": "crni"})



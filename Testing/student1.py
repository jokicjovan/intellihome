from locust import HttpUser, between, task


class UserBehavior(HttpUser):
    wait_time = between(5, 30)
    host = "http://192.168.1.188:5283"

    @task
    def login(self):
        self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})


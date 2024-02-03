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
    def get_lamp_data(self):
        lamp_id = "ac7ab1e0-d9f7-4cd2-b297-30059e10b35c"

        response = self.client.get(
            f"/api/Lamp/Get?id={lamp_id}",
        )

        if response.status_code != 200:
            self.environment.runner.quit()


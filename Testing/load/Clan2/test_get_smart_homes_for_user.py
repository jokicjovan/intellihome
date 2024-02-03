from locust import HttpUser, task, between


class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def get_smart_houses_for_user_paged(self):
        user_id = "1afb65ac-0785-4d70-b869-fff560a7b1ff"
        response = self.client.get(
            f"/api/SmartHome/GetSmartHomesForUser?PageNumber=1&PageSize=10&search=\"\"",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        )
        if response.status_code != 200:
            self.environment.runner.quit()
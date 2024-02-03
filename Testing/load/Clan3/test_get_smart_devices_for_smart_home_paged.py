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
    def get_smart_devices_for_home_paged(self):
        smart_home_id = "f901e6d3-9fba-4506-a23a-06c91ab97a27"
        response = self.client.get(
            f"/api/SmartDevice/GetSmartDevicesForHome/{smart_home_id}?PageNumber=1&PageSize=10",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        )
        if response.status_code != 200:
            self.environment.runner.quit()

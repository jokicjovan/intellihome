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
    def get_battery_system(self):
        battery_system_id = "ed6823db-34d6-43ba-ad6f-bc8a91ae874f"
        response = self.client.get(
            f"/api/BatterySystem/Get?id={battery_system_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})
        if response.status_code != 200:
            self.environment.runner.quit()

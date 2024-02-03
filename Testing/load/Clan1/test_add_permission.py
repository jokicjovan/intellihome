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
    def add_permission(self):
        smart_home_id = "f901e6d3-9fba-4506-a23a-06c91ab97a27"
        user_id = "bmijanovic02@gmail.com"

        response = self.client.put(
            f"/api/SmartHome/AddPermission", 
            json={"user": user_id, "home": smart_home_id},
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )

        if response.status_code != 200:
            self.environment.runner.quit()


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
    def add_scheduled_work(self):
        schedule_data = {
            "Id": "32e8e8fd-354f-4b61-92da-dfaa3f6288f6",
            "IsSpraying": True,
            "StartDate": "01/01/2024 12:00",
            "EndDate": "02/01/2024 12:00"
        }

        response = self.client.post("/api/Sprinkler/AddScheduledWork", json=schedule_data, headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()


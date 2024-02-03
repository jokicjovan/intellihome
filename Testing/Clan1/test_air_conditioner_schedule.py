from locust import HttpUser, task, between


class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        # Simulate authentication by logging in and store the authentication cookie
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def add_scheduled_work(self):
        # Replace the placeholder values with your actual values
        schedule_data = {
            "id": "c1173988-1eac-4478-8af3-e518156d32bb",
            "temperature": 22.5,
            "mode": "auto",
            "startDate": "01/02/2024 10:00",  # Assuming a format compatible with your backend
            "endDate": "02/02/2024 10:00"  # optional, can be None if not needed
        }

        # Send a POST request to the AddScheduledWork endpoint
        response = self.client.post("/api/AirConditioner/AddScheduledWork", json=schedule_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()



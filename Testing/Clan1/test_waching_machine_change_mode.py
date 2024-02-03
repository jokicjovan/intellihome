from locust import HttpUser, task, between
import random

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
    def change_mode(self):
        # Generate a random ID (assuming you don't have one readily available)
        id = "39c9d210-d914-48a1-9eb9-6dde382b8e34"
        modes = ["Antiallergy", "Mixed wash", "White wash"]
        mode = random.choice(modes)

        # Send a PUT request to the ChangeMode endpoint
        response = self.client.put(f"/api/WashingMachine/ChangeMode?id={id}&mode={mode}", headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()



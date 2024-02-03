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
    def change_temperature(self):
        # Generate a random ID (assuming you don't have one readily available)
        id = "c1173988-1eac-4478-8af3-e518156d32bb"
        # Replace the placeholder temperature value with your desired temperature
        temperature = random.randint(16, 30)

        # Send a PUT request to the ChangeTemperature endpoint
        response = self.client.put(f"/api/AirConditioner/ChangeTemperature?id={id}&temperature={temperature}", headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

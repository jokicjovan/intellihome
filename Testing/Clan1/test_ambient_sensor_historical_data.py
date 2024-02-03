from locust import HttpUser, task, between
from datetime import datetime

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://192.168.1.188:5283"

    def on_start(self):
        # Simulate authentication by logging in and store the authentication cookie
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def get_ambient_sensor_historical_data(self):
        # Replace the placeholder values with your actual ambient sensor ID, from, and to dates
        ambient_sensor_id = "4f84c687-8338-4200-958e-0ae99a38c1ad"
        from_date = datetime(2023, 1, 1)  # Replace with the actual from date
        to_date = datetime(2023, 12, 31)  # Replace with the actual to date

        # Simulate retrieving historical ambient sensor data
        response = self.client.get(
            f"/api/AmbientSensor/GetHistoricalData?id={ambient_sensor_id}&from={from_date}&to={to_date}",
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
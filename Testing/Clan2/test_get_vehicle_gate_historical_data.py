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
    def get_historical_action_data(self):
        # Replace the placeholder values with your actual vehicle gate ID, from, and to dates
        vehicle_gate_id = "ce456c90-3bb2-42be-bd2a-4d75a6990291"
        from_date = datetime(2023, 1, 1)  # Replace with the actual from date
        to_date = datetime(2023, 12, 31)  # Replace with the actual to date

        # Simulate retrieving historical action data
        response = self.client.get(
            f"/api/VehicleGate/GetHistoricalActionData?id={vehicle_gate_id}&from={from_date}&to={to_date}",
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()
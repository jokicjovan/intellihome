from locust import HttpUser, task, between

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
    def get_vehicle_charger_data(self):
        # Replace the placeholder value with your actual vehicle charger ID
        vehicle_charger_id = "2b8eac46-868b-445e-bb7f-e2e9e2a6e153"

        # Simulate retrieving data for a vehicle charger
        response = self.client.get(
            f"/api/VehicleCharger/Get?id={vehicle_charger_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
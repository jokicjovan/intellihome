from locust import HttpUser, task, between
import random


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
    def remove_license_plate(self):
        vehicle_gate_id = "ce456c90-3bb2-42be-bd2a-4d75a6990291"

        response = self.client.put(f'/api/VehicleGate/RemoveLicencePlate?id={vehicle_gate_id}&licencePlate=NS' + str(random.randint(100, 999))  + 'AA', 
                                   headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})
        if response.status_code != 200:
            print(f"Failed to add license plate. Status code: {response.status_code}")
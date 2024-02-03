from locust import HttpUser, task, between

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://192.168.1.188:5283"

    def on_start(self):
        # Simulate authentication by logging in
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def get_battery_system(self):
        # Replace the placeholder ID with the actual one
        battery_system_id = "ed6823db-34d6-43ba-ad6f-bc8a91ae874f"
        h = "6h"

        # Simulate retrieving a BatterySystem by ID
        response = self.client.get(
            f"/api/SmartDevice/GetAvailabilityData?id={battery_system_id}&h={h}", headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})
        print(response)
        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
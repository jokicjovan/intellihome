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
    def add_scheduled_work(self):
        # Replace the placeholder values with your actual values
        schedule_data = {
            "Id": "32e8e8fd-354f-4b61-92da-dfaa3f6288f6",
            "IsSpraying": True,  # or False, depending on your use case
            "StartDate": "01/01/2024 12:00",
            "EndDate": "02/01/2024 12:00"  # optional, can be None if not needed
        }

        # Send a POST request to the AddScheduledWork endpoint
        response = self.client.post("/api/Sprinkler/AddScheduledWork", json=schedule_data, headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)

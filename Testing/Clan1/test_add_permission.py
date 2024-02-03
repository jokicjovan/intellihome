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
    def add_permission(self):
        # Replace the placeholder values with your actual values
        smart_home_id = "f901e6d3-9fba-4506-a23a-06c91ab97a27"
        user_id = "bmijanovic02@gmail.com"

        # Simulate toggling a solar panel system
        response = self.client.put(
            f"/api/SmartHome/AddPermission", 
            json={"user": user_id, "home": smart_home_id},
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
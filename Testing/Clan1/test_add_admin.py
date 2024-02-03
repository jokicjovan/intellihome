from locust import HttpUser, task, between
import uuid
import os

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        # Simulate authentication by logging in and store the authentication cookie
        response = self.client.post("/api/User/login", json={"username": "superadmin", "password": "Adminadmin1"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def add_admin(self):
        # Generate a unique username and email for each iteration
        username = "admin" + str(uuid.uuid4())[:8]
        email = username + "@example.com"

        # Prepare form data for the admin user
        admin_data = {
            "firstName": "John",
            "lastName": "Doe",
            "email": email,
            "username": username,
            "password": "Admin2001.",
        }

        files = {
            'Image': ('a.jpg', open('a.jpg', 'rb'), 'image/jpeg')
        }
        # Send a POST request to the addAdmin endpoint with authorization headers
        response = self.client.post("/api/User/addAdmin", files=files, data=admin_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed

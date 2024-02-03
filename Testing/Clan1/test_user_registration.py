from locust import HttpUser, task, between
import uuid
import os

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    @task
    def register_user(self):
        # Generate a unique username and email for each iteration
        username = "user" + str(uuid.uuid4())[:8]
        email = username + "@example.com"

        # Prepare form data for user registration
        user_data = {
            "FirstName": "Dimitrije",
            "LastName": "Doe",
            "Email": email,
            "Username": username,
            "Password": "Password123!",
        }

        files = {
            'Image': ('a.jpg', open('a.jpg', 'rb'), 'image/jpeg')
        }
        # Send a POST request to the addAdmin endpoint with authorization headers
        response = self.client.post("/api/User/register", files=files, data=user_data)

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can

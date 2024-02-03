from locust import HttpUser, task, between
import uuid
import os

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    @task
    def register_user(self):
        username = "user" + str(uuid.uuid4())[:8]
        email = username + "@example.com"

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
        response = self.client.post("/api/User/register", files=files, data=user_data)

        if response.status_code != 200:
            self.environment.runner.quit()


from locust import HttpUser, task, between
import uuid
import os

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "superadmin", "password": "Adminadmin1"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def add_admin(self):
        username = "admin" + str(uuid.uuid4())[:8]
        email = username + "@example.com"

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
        response = self.client.post("/api/User/addAdmin", files=files, data=admin_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()


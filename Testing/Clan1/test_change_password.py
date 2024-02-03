from locust import HttpUser, task, between
import uuid

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        # Simulate authentication by logging in and store the authentication cookie
        response = self.client.post("/api/User/login", json={"username": "joki", "password": "Adminadmin1"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def change_password(self):
        # Prepare payload for changing password
        change_password_data = {
            "Id": "f944d925-9f5e-44fd-986b-93b43ba21eb9",  # Replace with the actual user ID
            "Password": "novasifra"  # Replace with the new password
        }

        # Send a POST request to the changePassword endpoint with authorization headers
        response = self.client.post("/api/User/changePassword", json=change_password_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)

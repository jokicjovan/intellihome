from locust import HttpUser, task, between
import uuid

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "joki", "password": "Adminadmin1"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def change_password(self):
        change_password_data = {
            "Id": "f944d925-9f5e-44fd-986b-93b43ba21eb9",
            "Password": "novasifra"
        }

        response = self.client.post("/api/User/changePassword", json=change_password_data,
                                    headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))})

        if response.status_code != 200:
            self.environment.runner.quit()



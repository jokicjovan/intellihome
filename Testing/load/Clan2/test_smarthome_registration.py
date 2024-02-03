from locust import HttpUser, task, between

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        if response.status_code != 200:
            self.environment.runner.quit()

        self.client.cookies.update(response.cookies)

    @task
    def create_smart_home(self):
        headers = {"Cookie": "auth=" + str(self.client.cookies.get("auth"))}
        data = {
            "name": "YourSmartHomeName",
            "address": "YourSmartHomeAddress",
            "city": "Novi Sad",
            "country": "Serbia",
            "area": "22",
            "type": 0,
            "numberOfFloors": 2,
            "latitude": 37.7749,
            "longitude": -122.4194,
        }
        files = {
            'Image': ('a.jpg', open('a.jpg', 'rb'), 'image/jpeg')
        }

        response = self.client.post("/api/SmartHome/CreateSmartHome", headers=headers, data=data, files=files)

        if response.status_code != 200:
            self.environment.runner.quit()



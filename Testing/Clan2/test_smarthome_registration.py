from locust import HttpUser, task, between

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"


    def on_start(self):
        # Simulate authentication by logging in
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def create_smart_home(self):
        # Simulate creating a SmartHome
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

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)

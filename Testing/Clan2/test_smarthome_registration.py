from locust import HttpUser, task, between

class MyUser(HttpUser):
    wait_time = between(1, 3)
    host = "http://192.168.1.188:5283"
    number_of_users = 1000
    spawn_rate = 50


    def on_start(self):
        # Simulate authentication by logging in
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        # response = self.client.post("/api/User/login", json={"username": name, "password": "crni"})

        # Check if the login was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # Save the authentication cookie for subsequent requests
        self.client.cookies.update(response.cookies)

    @task
    def create_smart_home(self):
        # Simulate creating a SmartHome
        response = self.client.post("/api/SmartHome/CreateSmartHome",
            headers={"Authorization": "Bearer " + self.client.cookies.get("auth")},
            data={
                "Name": "YourSmartHomeName",
                "Address": "YourSmartHomeAddress",
                "City": "Novi Sad",
                "Country": "Serbia",
                "Area": "22",
                "Type": 0,  # Replace with the actual value
                "NumberOfFloors": 2,  # Replace with the actual value
                "Latitude": 37.7749,  # Replace with the actual value
                "Longitude": -122.4194,  # Replace with the actual value
            },
            files={"Image": ("a.jpg", open("../a.jpg", "rb"))},  # Replace with the actual path
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
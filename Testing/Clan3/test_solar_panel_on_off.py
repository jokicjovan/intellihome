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
    def toggle_solar_panel_system(self):
        # Replace the placeholder values with your actual values
        solar_panel_id = "83d1dd00-7f15-42d3-ab66-60802292d0ad"
        turn_on = True  # or False, depending on your use case

        # Simulate toggling a solar panel system
        response = self.client.put(
            f"/api/SolarPanelSystem/Toggle?id={solar_panel_id}&turnOn={turn_on}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )

        # Check if the request was successful (adjust based on your application's response)
        if response.status_code != 200:
            self.environment.runner.quit()

        # You can print or handle the response as needed
        print(response.text)
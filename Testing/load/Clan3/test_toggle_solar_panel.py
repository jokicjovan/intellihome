from locust import HttpUser, task, between


class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "crni", "password": "crni"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def toggle_solar_panel_system(self):
        solar_panel_id = "83d1dd00-7f15-42d3-ab66-60802292d0ad"
        turn_on = True
        response = self.client.put(
            f"/api/SolarPanelSystem/Toggle?id={solar_panel_id}&turnOn={turn_on}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()

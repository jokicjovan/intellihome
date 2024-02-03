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
    def create_air_conditioner(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        air_conditioner_data = {
            "Name": "Living Room AC",
            "PowerPerHour": 2.5,
            "MinTemperature": 15,
            "MaxTemperature": 30,
            "Modes": ["Cool", "Heat", "Fan"],
            # Add image data if necessary
        }
        response = self.client.post(
            f"/api/PKA/CreateAirConditioner/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=air_conditioner_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_ambient_sensor(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        ambient_sensor_data = {
            "Name": "Living Room Sensor",
            "PowerPerHour": 0.5,
        }
        response = self.client.post(
            f"/api/PKA/CreateAmbientSensor/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=ambient_sensor_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_washing_machine(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        washing_machine_data = {
            "Name": "Laundry Room Washer",
            "PowerPerHour": 3,
            "ModesIds": ["1645321c-eef2-4b00-814d-7b8df6ccca05", "3e09cb8f-cd7f-4db6-b3f9-3c3bcc9bbb0b"],
        }
        response = self.client.post(
            f"/api/PKA/CreateWashingMachine/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=washing_machine_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def register_vehicle_gate(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        vehicle_gate_data = {
            "AllowedLicencePlates": ["SM023SA", "SM023AS"],
            "PowerPerHour": 2,
            "Name": "Gate"
        }
        response = self.client.post(
            f"/api/SPU/CreateVehicleGate/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=vehicle_gate_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_lamp(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        lamp_data = {
            "Name": "Bedroom Lamp",
            "PowerPerHour": 1.5,
            "BrightnessLimit": 1000,
            # Add image data if necessary
        }
        response = self.client.post(
            f"/api/SPU/CreateLamp/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=lamp_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_sprinkler(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        sprinkler_data = {
            "Name": "Garden Sprinkler",
            "PowerPerHour": 4,
        }
        response = self.client.post(
            f"/api/SPU/CreateSprinkler/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=sprinkler_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_battery_system(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        battery_system_data = {
            "Name": "Home Battery",
            "Capacity": 500,
        }
        response = self.client.post(
            f"/api/VEU/CreateBatterySystem/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=battery_system_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_solar_panel_system(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        solar_panel_system_data = {
            "Name": "Roof Solar Panels",
            "Area": 50,
            "Efficiency": 20,
        }
        response = self.client.post(
            f"/api/VEU/CreateSolarPanelSystem/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=solar_panel_system_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()

    @task
    def create_vehicle_charger(self):
        smart_home_id = "603223ce-8a82-41d6-aadc-9f96dfce35d9"
        vehicle_charger_data = {
            "Name": "Garage Charger",
            "PowerPerHour": 7.5,
            "NumberOfChargingPoints": 2,
        }
        response = self.client.post(
            f"/api/VEU/CreateVehicleCharger/{smart_home_id}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))}, data=vehicle_charger_data
        )
        if response.status_code != 200:
            self.environment.runner.quit()




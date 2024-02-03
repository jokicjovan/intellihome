from locust import HttpUser, task, between


class User(HttpUser):
    wait_time = between(1, 3)
    host = "http://localhost:5283"

    def on_start(self):
        response = self.client.post("/api/User/login", json={"username": "joki", "password": "joki"})
        if response.status_code != 200:
            self.environment.runner.quit()
        self.client.cookies.update(response.cookies)

    @task
    def get_cities_paged(self):
        search_string = "  "
        response = self.client.get(
            f"/GetAllCitiesPaged?pageNumber=1&pageSize=10&search={search_string}",
            headers={"Cookie": "auth=" + str(self.client.cookies.get("auth"))},
        )
        if response.status_code != 200:
            self.environment.runner.quit()

# Intellihome
Intellihome is a smart home automation system that allows you to control and monitor various devices in your home. Whether it's managing lights, temperature, or security, Intellihome provides a centralized platform for seamless integration and control.

## Features
Device Control: Control and monitor smart devices from one centralized dashboard.
Automation: Set up automation rules to make your home smarter and more efficient.
Security: Keep your home secure with integrated security features.
Energy Efficiency: Optimize energy consumption with intelligent control.
## Getting Started
Follow these steps to get Intellihome up and running on your local machine.

### Prerequisites
* Node.js
* Docker (with Nginx, Redis, PostgreSQL, Influxdb and Mosquitto images)
* .NET
* Python (with requirements installed)

### Installation
* Clone the repository
```sh
git clone https://gitlab.com/mrmijan/intellihome.git)
```

#### Simulator server
1. Position in DeviceSimulator directory
2. Install python requirements from requirements.txt (preferably in new virtual environment)
```sh
pip install -r requirements.txt
```
3. Run device simulator server 
```sh
uvicorn main:app --reload --host 0.0.0.0 --port 8080
```

#### Client
1. Position in client directory (IntelliHome-Frontend)
2. Install dependencies with npm
```sh
npm install
``` 
3. Build app
```sh
vite build
```

#### Docker
1. Position in directory root
2. Execute docker compose up
```sh
Docker compose up
```

#### Server
1. Position in server directory (IntelliHome-Frontend)
2. Open visual studio solution
3. Add migrations from nuget package manager console
```sh
Add-Migration InitialMigration -Project Data
```
4. Apply migrations (update database) from nuget package manager console
```sh
Update-Database
```
5. Execute seed.sql (from SQL-scripts directory) inside your newly create Database
6. Run Dotnet application

#### Your app is up and running now! Try it on http://localhost:8800

## Contributors
- Vukašin Bogdanović (https://github.com/vukasinb7)
- Bojan Mijanović (https://github.com/bmijanovic)
- Jovan Jokić (https://github.com/jokicjovan)
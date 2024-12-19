# IntelliHome - Smart Home Automation Platform

IntelliHome is a smart home automation platform designed to provide users with real-time monitoring, management, and reporting of various devices, from small appliances to large systems. The platform allows users to control and track device data, while also optimizing energy use and providing admins with control over user requests and multi-home management.

## Features

- **Real-Time Monitoring & Management**: Track and control devices in real-time, from small appliances to large systems (e.g., air conditioners, car chargers).
- **Device Reports**: View historical data and generate reports on device performance over time.
- **Multi-Home Management**: Users can manage multiple homes under one account.
- **Admin Controls**: Admins have the ability to oversee and approve home requests from users.
- **Performance Optimization**: Improved performance using server caching and a centralized gateway, ensuring efficient device control and smooth user experience.

## Technologies

- **Frontend**: React for building the user interface.
- **Backend**: .NET for API and server-side logic.
- **Database**: PostgreSQL for storing user data and device information.
- **Real-Time Database**: InfluxDB for storing time-series data from devices.
- **Caching**: Redis for caching to enhance system performance.
- **Gateway**: NGINX used as a reverse proxy and load balancer.

## Getting Started

Follow these steps to get IntelliHome up and running on your local machine.

### Prerequisites

Before setting up IntelliHome, ensure you have the following installed:

- **Node.js**
- **Docker** (with NGINX, Redis, PostgreSQL, InfluxDB, and Mosquitto images)
- **.NET**
- **Python** (with required packages installed)

### Installation

#### 1. Clone the Repository

Start by cloning the IntelliHome repository:

```bash
git clone https://github.com/jokicjovan/intellihome.git 
```

#### 2. Navigate to the Project Directory

Once the repository is cloned, navigate into the project directory:

```bash
cd intellihome
```

#### 3. Setup Device Simulator

The device simulator is an essential component for simulating real device data for the IntelliHome platform. Follow these steps to set up and run the device simulator.

##### 3.1. Navigate to the Device Simulator Directory

```bash
cd DeviceSimulator
```

##### 3.2. Install Python Dependencies

It is recommended to use a virtual environment for Python dependencies. Install the necessary Python packages using:

```bash
pip install -r requirements.txt
```

##### 3.3. Run the Device Simulator

Start the device simulator server with:

```bash
uvicorn main:app --reload --host 0.0.0.0 --port 8080
```

The server will be accessible at `http://localhost:8080`.

#### 4. Set Up Backend Server

The backend API is built using .NET. Follow these steps to set up and run the server.

##### 4.1. Open the Project in Visual Studio

Navigate to the `server` directory and open the solution file in Visual Studio.

##### 4.2. Add Database Migrations

Run the following command to add the initial database migration:

```bash
Add-Migration InitialMigration -Project Data
```

##### 4.3. Apply Database Migrations

To apply the migrations and update the database, use:

```bash
Update-Database
```

##### 4.4. Seed the Database

Run the `seed.sql` script from the `SQL-scripts` directory to populate your database with initial data.

##### 4.5. Run the Backend Server

Once the database is set up, run the backend server using Visual Studio.

#### 5. Set Up Frontend Client

The frontend of IntelliHome is built using React and Vite.

##### 5.1. Navigate to the Client Directory

Change to the `client` directory where the frontend code is located:

```bash
cd client
```

##### 5.2. Install Dependencies

Install the necessary dependencies using npm:

```bash
npm install
```

##### 5.3. Build the Frontend

Once the dependencies are installed, build the frontend application:

```bash
npm run build
```

#### 6. Set Up Docker

Docker is used to orchestrate the necessary services for IntelliHome, such as Redis, PostgreSQL, and InfluxDB.

##### 6.1. Run Docker Compose

From the root project directory, run Docker Compose to start the services:

```bash
docker-compose up
```

#### 7. Access the Application

Once everything is set up, you can access the IntelliHome platform by navigating to `http://localhost:8800` in your web browser.

## Conclusion

You now have the IntelliHome platform running locally, including the backend, frontend, and device simulator. You can begin testing, monitoring, and managing smart devices in your home. If you'd like to contribute to the project or make changes, refer to the contributing section below.

## Contributors

- Jovan Jokić (https://github.com/jokicjovan)
- Vukašin Bogdanović (https://github.com/vukasinb7)
- Bojan Mijanović (https://github.com/bmijanovic)

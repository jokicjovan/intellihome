DO $$ 
DECLARE 
    User1Id uuid;
    User2Id uuid;

    City1Id uuid;
    City2Id uuid;
    City3Id uuid;

    WashingMachineMode1Id uuid;
    WashingMachineMode2Id uuid;
    WashingMachineMode3Id uuid;

    SmartHome1Id uuid;
    SmartHome2Id uuid;
    SmartHome3Id uuid;
    SmartHome4Id uuid;

    AmbientSensor1Id uuid;
    AmbientSensor2Id uuid;
    AmbientSensor3Id uuid;
    AmbientSensor4Id uuid;

    AirConditioner1Id uuid;
    AirConditioner2Id uuid;
    AirConditioner3Id uuid;
    AirConditioner4Id uuid;

    WashingMachine1Id uuid;
    WashingMachine2Id uuid;
    WashingMachine3Id uuid;
    WashingMachine4Id uuid;

    Lamp1Id uuid;
    Lamp2Id uuid;
    Lamp3Id uuid;
    Lamp4Id uuid;

    Sprinkler1Id uuid;
    Sprinkler2Id uuid;
    Sprinkler3Id uuid;
    Sprinkler4Id uuid;

    VehicleGate1Id uuid;
    VehicleGate2Id uuid;
    VehicleGate3Id uuid;
    VehicleGate4Id uuid;

    BatterySystem1Id uuid;
    BatterySystem2Id uuid;
    BatterySystem3Id uuid;
    BatterySystem4Id uuid;

    SolarPanelSystem1Id uuid;
    SolarPanelSystem2Id uuid;
    SolarPanelSystem3Id uuid;
    SolarPanelSystem4Id uuid;

    VehicleCharger1Id uuid;
    VehicleCharger2Id uuid;
    VehicleCharger3Id uuid;
    VehicleCharger4Id uuid;

    VehicleChargingPoint1Id uuid;
    VehicleChargingPoint2Id uuid;
    VehicleChargingPoint3Id uuid;
    VehicleChargingPoint4Id uuid;
    VehicleChargingPoint5Id uuid;
    VehicleChargingPoint6Id uuid;
    VehicleChargingPoint7Id uuid;
    VehicleChargingPoint8Id uuid;
    VehicleChargingPoint9Id uuid;
    VehicleChargingPoint10Id uuid;
    VehicleChargingPoint11Id uuid;
    VehicleChargingPoint12Id uuid;
BEGIN 
    TRUNCATE "AirConditioners", "AirConditionerWorks", "AmbientSensors", "BatterySystems", "Cities", "Confirmations", "Lamps", 
        "SharingDevices","SmartDevice", "SmartHomeApproveRequests", "SmartHomes", "SolarPanelSystems", "Sprinklers", 
        "SprinklerWorks", "Users", "VehicleChargers", "VehicleChargingPoints", "VehicleGates", "WashingMachineModes", "WashingMachines", 
        "WashingMachineWashingMachineMode", "WashingMachineWork";

    -- Cities
    City1Id := gen_random_uuid();
    City2Id := gen_random_uuid();
    City3Id := gen_random_uuid();
    INSERT INTO "Cities" ("Id", "Name", "Country", "ZipCode") VALUES
        (City1Id, 'Zrenjanin', 'Serbia', '23000'),
        (City2Id, 'Novi Sad', 'Serbia', '21000'),
        (City3Id, 'Sremska Mitrovica', 'Serbia', '22000');

    -- Users
    User1Id := gen_random_uuid();
    User2Id := gen_random_uuid();
    INSERT INTO "Users" ("Id", "FirstName", "LastName", "Email", "Username", "Password","IsActivated","Discriminator","Image")
    VALUES
        (User1Id, 'Bojan', 'Mijanovic', 'user1@example.com', 'user1', '$2a$11$tNeLBfyKR4q.nfATgQdL4.CGLmT3asWVAxp0suCYrkQO.22bC.fPu',true,'User', 'static/profilePictures\User1.jpg'),
        (User2Id, 'Vukasin', 'Bogdanovic', 'user2@example.com', 'user2', '$2a$11$thJM9Mc.BonWdDHTTllJpO1dF5TxerpgoGXY7iqevDKRfF6G7HSO6',true,'User', 'static/profilePictures\User2.jpg');

    INSERT INTO "Users" ("Id", "FirstName", "LastName", "Email", "Username", "Password","IsSuperAdmin","IsActivated", "PasswordChanged", "Discriminator","Image")
    VALUES
        (gen_random_uuid(), 'Jovan', 'Jokic', 'admin1@example.com', 'admin1', '$2a$11$KeWkaVH6cu4fEHW5Jki/oeSDd52888KubIMUyHvgdyBmNZZ8z7Rei',false,true,true,'Admin', 'static/profilePictures\Admin1.jpg');

    -- WashingMachineModes
    WashingMachineMode1Id := gen_random_uuid();
    WashingMachineMode2Id := gen_random_uuid();
    WashingMachineMode3Id := gen_random_uuid();
    INSERT INTO "WashingMachineModes" ("Id", "Duration", "Name", "Temperature") VALUES
        (WashingMachineMode1Id, 30, 'Mixed wash', 30),
        (WashingMachineMode2Id, 60, 'Antiallergy', 60),
        (WashingMachineMode3Id, 60, 'White wash', 90);

    -- SmartHomes
    SmartHome1Id := gen_random_uuid();
    SmartHome2Id := gen_random_uuid();
    SmartHome3Id := gen_random_uuid();
    SmartHome4Id := gen_random_uuid();

    INSERT INTO "SmartHomes" ("Id", "Name", "Address", "CityId", "Area", "Type", "NumberOfFloors", "Latitude",
                            "Longitude", "IsApproved", "OwnerId", "Image")
    VALUES
        (SmartHome1Id, 'SmartHome1', 'Zarka Zrenjanina 24', City1Id,
        '143', 0, 2, 45.244320, 20.031070, true, User1Id, 'static/smartHomes\SmartHome1.jpg'),
        (SmartHome2Id, 'SmartHome2', 'Zarka Zrenjanina 12', City1Id,
        '50', 0, 2, 45.244320, 20.001070, true, User1Id, 'static/smartHomes\SmartHome2.jpg'),
        (SmartHome3Id, 'SmartHome3', 'Mise Dimitrijevica 124', City2Id,
        '120', 0, 2, 45.244320, 19.831070, true, User2Id, 'static/smartHomes\SmartHome3.jpg'),
        (SmartHome4Id, 'SmartHome4', 'Mise Dimitrijevica 123', City2Id,
        '72', 0, 2, 45.244320, 19.801070, true, User2Id, 'static/smartHomes\SmartHome4.jpg');

    -- SmartDevices
    AmbientSensor1Id := gen_random_uuid();
    AmbientSensor2Id := gen_random_uuid();
    AmbientSensor3Id := gen_random_uuid();
    AmbientSensor4Id := gen_random_uuid();

    AirConditioner1Id := gen_random_uuid();
    AirConditioner2Id := gen_random_uuid();
    AirConditioner3Id := gen_random_uuid();
    AirConditioner4Id := gen_random_uuid();

    WashingMachine1Id := gen_random_uuid();
    WashingMachine2Id := gen_random_uuid();
    WashingMachine3Id := gen_random_uuid();
    WashingMachine4Id := gen_random_uuid();

    Lamp1Id := gen_random_uuid();
    Lamp2Id := gen_random_uuid();
    Lamp3Id := gen_random_uuid();
    Lamp4Id := gen_random_uuid();

    Sprinkler1Id := gen_random_uuid();
    Sprinkler2Id := gen_random_uuid();
    Sprinkler3Id := gen_random_uuid();
    Sprinkler4Id := gen_random_uuid();

    VehicleGate1Id := gen_random_uuid();
    VehicleGate2Id := gen_random_uuid();
    VehicleGate3Id := gen_random_uuid();
    VehicleGate4Id := gen_random_uuid();

    BatterySystem1Id := gen_random_uuid();
    BatterySystem2Id := gen_random_uuid();
    BatterySystem3Id := gen_random_uuid();
    BatterySystem4Id := gen_random_uuid();

    SolarPanelSystem1Id := gen_random_uuid();
    SolarPanelSystem2Id := gen_random_uuid();
    SolarPanelSystem3Id := gen_random_uuid();
    SolarPanelSystem4Id := gen_random_uuid();

    VehicleCharger1Id := gen_random_uuid();
    VehicleCharger2Id := gen_random_uuid();
    VehicleCharger3Id := gen_random_uuid();
    VehicleCharger4Id := gen_random_uuid();

    INSERT INTO "SmartDevice" ("Id", "Name", "Category", "Type", "IsConnected", "IsOn", "SmartHomeId", "Image")
    VALUES
        (AmbientSensor1Id, 'AmbientSensor1', '0', '0', 'false', 'false', SmartHome1Id, 'static/smartDevices\AmbientSensor1.jpg'),
        (AmbientSensor2Id, 'AmbientSensor2', '0', '0', 'false', 'false', SmartHome2Id, 'static/smartDevices\AmbientSensor2.jpg'),
        (AmbientSensor3Id, 'AmbientSensor3', '0', '0', 'false', 'false', SmartHome3Id, 'static/smartDevices\AmbientSensor3.jpg'),
        (AmbientSensor4Id, 'AmbientSensor4', '0', '0', 'false', 'false', SmartHome4Id, 'static/smartDevices\AmbientSensor4.jpg'),

        (AirConditioner1Id, 'AirConditioner1', '0', '1', 'false', 'false', SmartHome1Id, 'static/smartDevices\AirConditioner1.jpg'),
        (AirConditioner2Id, 'AirConditioner2', '0', '1', 'false', 'false', SmartHome2Id, 'static/smartDevices\AirConditioner2.jpg'),
        (AirConditioner3Id, 'AirConditioner3', '0', '1', 'false', 'false', SmartHome3Id, 'static/smartDevices\AirConditioner3.jpg'),
        (AirConditioner4Id, 'AirConditioner4', '0', '1', 'false', 'false', SmartHome4Id, 'static/smartDevices\AirConditioner4.jpg'),

        (WashingMachine1Id, 'WashingMachine1', '0', '2', 'false', 'false', SmartHome1Id, 'static/smartDevices\WashingMachine1.jpg'),
        (WashingMachine2Id, 'WashingMachine2', '0', '2', 'false', 'false', SmartHome2Id, 'static/smartDevices\WashingMachine2.jpg'),
        (WashingMachine3Id, 'WashingMachine3', '0', '2', 'false', 'false', SmartHome3Id, 'static/smartDevices\WashingMachine3.jpg'),
        (WashingMachine4Id, 'WashingMachine4', '0', '2', 'false', 'false', SmartHome4Id, 'static/smartDevices\WashingMachine4.jpg'),

        (Lamp1Id, 'Lamp1', '1', '3', 'false', 'false', SmartHome1Id, 'static/smartDevices\Lamp1.jpg'),
        (Lamp2Id, 'Lamp2', '1', '3', 'false', 'false', SmartHome2Id, 'static/smartDevices\Lamp2.jpg'),
        (Lamp3Id, 'Lamp3', '1', '3', 'false', 'false', SmartHome3Id, 'static/smartDevices\Lamp3.jpg'),
        (Lamp4Id, 'Lamp4', '1', '3', 'false', 'false', SmartHome4Id, 'static/smartDevices\Lamp4.jpg'),

        (Sprinkler1Id, 'Sprinkler1', '1', '5', 'false', 'false', SmartHome1Id, 'static/smartDevices\Sprinkler1.jpg'),
        (Sprinkler2Id, 'Sprinkler2', '1', '5', 'false', 'false', SmartHome2Id, 'static/smartDevices\Sprinkler2.jpg'),
        (Sprinkler3Id, 'Sprinkler3', '1', '5', 'false', 'false', SmartHome3Id, 'static/smartDevices\Sprinkler3.jpg'),
        (Sprinkler4Id, 'Sprinkler4', '1', '5', 'false', 'false', SmartHome4Id, 'static/smartDevices\Sprinkler4.jpg'),

        (VehicleGate1Id, 'VehicleGate1', '1', '4', 'false', 'false', SmartHome1Id, 'static/smartDevices\VehicleGate1.jpg'),
        (VehicleGate2Id, 'VehicleGate2', '1', '4', 'false', 'false', SmartHome2Id, 'static/smartDevices\VehicleGate2.jpg'),
        (VehicleGate3Id, 'VehicleGate3', '1', '4', 'false', 'false', SmartHome3Id, 'static/smartDevices\VehicleGate3.jpg'),
        (VehicleGate4Id, 'VehicleGate4', '1', '4', 'false', 'false', SmartHome4Id, 'static/smartDevices\VehicleGate4.jpg'),

        (BatterySystem1Id, 'BatterySystem1', '2', '7', 'false', 'false', SmartHome1Id, 'static/smartDevices\BatterySystem1.jpg'),
        (BatterySystem2Id, 'BatterySystem2', '2', '7', 'false', 'false', SmartHome2Id, 'static/smartDevices\BatterySystem2.jpg'),
        (BatterySystem3Id, 'BatterySystem3', '2', '7', 'false', 'false', SmartHome3Id, 'static/smartDevices\BatterySystem3.jpg'),
        (BatterySystem4Id, 'BatterySystem4', '2', '7', 'false', 'false', SmartHome4Id, 'static/smartDevices\BatterySystem4.jpg'),

        (SolarPanelSystem1Id, 'SolarPanelSystem1', '2', '6', 'false', 'false', SmartHome1Id, 'static/smartDevices\SolarPanelSystem1.jpg'),
        (SolarPanelSystem2Id, 'SolarPanelSystem2', '2', '6', 'false', 'false', SmartHome2Id, 'static/smartDevices\SolarPanelSystem2.jpg'),
        (SolarPanelSystem3Id, 'SolarPanelSystem3', '2', '6', 'false', 'false', SmartHome3Id, 'static/smartDevices\SolarPanelSystem3.jpg'),
        (SolarPanelSystem4Id, 'SolarPanelSystem4', '2', '6', 'false', 'false', SmartHome4Id, 'static/smartDevices\SolarPanelSystem4.jpg'),

        (VehicleCharger1Id, 'VehicleCharger1', '2', '8', 'false', 'false', SmartHome1Id, 'static/smartDevices\VehicleCharger1.jpg'),
        (VehicleCharger2Id, 'VehicleCharger2', '2', '8', 'false', 'false', SmartHome2Id, 'static/smartDevices\VehicleCharger2.jpg'),
        (VehicleCharger3Id, 'VehicleCharger3', '2', '8', 'false', 'false', SmartHome3Id, 'static/smartDevices\VehicleCharger3.jpg'),
        (VehicleCharger4Id, 'VehicleCharger4', '2', '8', 'false', 'false', SmartHome4Id, 'static/smartDevices\VehicleCharger4.jpg');
        

    INSERT INTO "AmbientSensors" ("Id", "Temperature", "Humidity", "PowerPerHour")
    VALUES
        (AmbientSensor1Id, '0', '0', '1'),
        (AmbientSensor2Id, '0', '0', '1'),
        (AmbientSensor3Id, '0', '0', '1'),
        (AmbientSensor4Id, '0', '0', '1');

    INSERT INTO "AirConditioners" ("Id", "MinTemperature", "MaxTemperature", "Modes", "PowerPerHour")
    VALUES
        (AirConditioner1Id, '15', '25', ARRAY[0, 1, 2, 3], '10'),
        (AirConditioner2Id, '15', '25', ARRAY[0, 1, 2, 3], '10'),
        (AirConditioner3Id, '15', '25', ARRAY[0, 1, 2, 3], '10'),
        (AirConditioner4Id, '15', '25', ARRAY[0, 1, 2, 3], '10');
        
    INSERT INTO "WashingMachines" ("Id", "StartTime", "PowerPerHour")
    VALUES
        (WashingMachine1Id, '-infinity', '10'),
        (WashingMachine2Id, '-infinity', '10'),
        (WashingMachine3Id, '-infinity', '10'),
        (WashingMachine4Id, '-infinity', '10');

    INSERT INTO "WashingMachineWashingMachineMode" ("ModesId", "WashingMachineId")
    VALUES
        (WashingMachineMode1Id, WashingMachine1Id),
        (WashingMachineMode2Id, WashingMachine1Id),

        (WashingMachineMode1Id, WashingMachine2Id),
        (WashingMachineMode2Id, WashingMachine2Id),
        (WashingMachineMode3Id, WashingMachine2Id),

        (WashingMachineMode1Id, WashingMachine3Id),
        (WashingMachineMode2Id, WashingMachine3Id),

        (WashingMachineMode1Id, WashingMachine4Id),
        (WashingMachineMode2Id, WashingMachine4Id),
        (WashingMachineMode3Id, WashingMachine4Id);

    INSERT INTO "Lamps" ("Id", "BrightnessLimit", "PowerPerHour", "IsAuto")
    VALUES
        (Lamp1Id, '100', '1', 'false'),
        (Lamp2Id, '100', '1', 'false'),
        (Lamp3Id, '100', '1', 'false'),
        (Lamp4Id, '100', '1', 'false');

    INSERT INTO "Sprinklers" ("Id", "PowerPerHour")
    VALUES
        (Sprinkler1Id, '1'),
        (Sprinkler2Id, '1'),
        (Sprinkler3Id, '1'),
        (Sprinkler4Id, '1');

    INSERT INTO "VehicleGates" ("Id", "IsPublic", "AllowedLicencePlates", "PowerPerHour")
    VALUES
        (VehicleGate1Id, 'false', ARRAY['AB123BA'], '1'),
        (VehicleGate2Id, 'false', ARRAY['AB123BA'], '1'),
        (VehicleGate3Id, 'false', ARRAY['AB123BA'], '1'),
        (VehicleGate4Id, 'false', ARRAY['AB123BA'], '1');
        
    INSERT INTO "BatterySystems" ("Id", "Capacity")
    VALUES
        (BatterySystem1Id, '1000'),
        (BatterySystem2Id, '1000'),
        (BatterySystem3Id, '1000'),
        (BatterySystem4Id, '1000');

    INSERT INTO "SolarPanelSystems" ("Id", "Area", "Efficiency")
    VALUES
        (SolarPanelSystem1Id, '50', '60'),
        (SolarPanelSystem2Id, '120', '70'),
        (SolarPanelSystem3Id, '70', '65'),
        (SolarPanelSystem4Id, '80', '45');

    INSERT INTO "VehicleChargers" ("Id", "PowerPerHour")
    VALUES
        (VehicleCharger1Id, '1000'),
        (VehicleCharger2Id, '1100'),
        (VehicleCharger3Id, '1200'),
        (VehicleCharger4Id, '1300');

    VehicleChargingPoint1Id := gen_random_uuid();
    VehicleChargingPoint2Id := gen_random_uuid();
    VehicleChargingPoint3Id := gen_random_uuid();
    VehicleChargingPoint4Id := gen_random_uuid();
    VehicleChargingPoint5Id := gen_random_uuid();
    VehicleChargingPoint6Id := gen_random_uuid();
    VehicleChargingPoint7Id := gen_random_uuid();
    VehicleChargingPoint8Id := gen_random_uuid();
    VehicleChargingPoint9Id := gen_random_uuid();
    VehicleChargingPoint10Id := gen_random_uuid();
    VehicleChargingPoint11Id := gen_random_uuid();
    VehicleChargingPoint12Id := gen_random_uuid();

    INSERT INTO "VehicleChargingPoints" ("Id", "IsFree", "VehicleChargerId")
    VALUES
        (VehicleChargingPoint1Id, 'true', VehicleCharger1Id),
        (VehicleChargingPoint2Id, 'true', VehicleCharger1Id),
        (VehicleChargingPoint3Id, 'true', VehicleCharger1Id),
        (VehicleChargingPoint4Id, 'true', VehicleCharger1Id),
        (VehicleChargingPoint5Id, 'true', VehicleCharger2Id),
        (VehicleChargingPoint6Id, 'true', VehicleCharger2Id),

        (VehicleChargingPoint7Id, 'true', VehicleCharger3Id),
        (VehicleChargingPoint8Id, 'true', VehicleCharger3Id),
        (VehicleChargingPoint9Id, 'true', VehicleCharger3Id),
        (VehicleChargingPoint10Id, 'true', VehicleCharger3Id),
        (VehicleChargingPoint11Id, 'true', VehicleCharger4Id),
        (VehicleChargingPoint12Id, 'true', VehicleCharger4Id);
END $$;
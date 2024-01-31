TRUNCATE "AirConditioners", "AirConditionerWorks", "AmbientSensors", "BatterySystems", "Cities", "Lamps", 
    "SmartDevice", "SmartDeviceUser", "SmartHomeApproveRequests", "SmartHomes", "SolarPanelSystems", "Sprinklers", 
    "SprinklerWorks", "Confirmations", "Users", "VehicleChargers", "VehicleChargingPoints", "VehicleGates", "WashingMachineModes", "WashingMachines", 
    "WashingMachineWashingMachineMode";

-- Cities
INSERT INTO "Cities" ("Id", "Name", "Country", "ZipCode") VALUES
    (gen_random_uuid(), 'Novi Sad', 'Serbia', '21000'),
    (gen_random_uuid(), 'Sremska Mitrovica', 'Serbia', '22000'),
    (gen_random_uuid(), 'Zrenjanin', 'Serbia', '23000');

-- Users
INSERT INTO "Users" ("Id", "FirstName", "LastName", "Email", "Username", "Password","IsActivated","Discriminator")
VALUES
    (gen_random_uuid(), 'Bojan', 'Mijanovic', 'bmijanovic02@gmail.com', 'boki', '$2a$12$.wm6WqKngt9DNguUjmrb3.b5joVq6hP0YJdwEY9mQFBEfWH7IYB/a',true,'User'),
    (gen_random_uuid(), 'Vukasin', 'Bogdanovic', 'user1@example.com', 'crni', '$2a$12$mfJPoMyts5MPZJJnedXCmOLo7YSA9h5gtfJyh17d2Irbd6Tq1J5LO',true,'User');

INSERT INTO "Users" ("Id", "FirstName", "LastName", "Email", "Username", "Password","IsSuperAdmin","IsActivated","Discriminator")
VALUES
    (gen_random_uuid(), 'Jovan', 'Jokic', 'user2@example.com', 'joki', '$2a$12$jTGU6L.LR5JcgllNYi/WCOPGhZjxwlAeNm/xqKv.9F.uc8yV4DAaC',false,true,'Admin');

-- Homes
INSERT INTO "SmartHomes" ("Id", "Name", "Address", "CityId", "Area", "Type", "NumberOfFloors", "Latitude",
                          "Longitude", "IsApproved", "OwnerId")
VALUES
    (gen_random_uuid(), 'Lepa kuca u zrenjaninu', 'Mise Dimitrijevica 124',
     (SELECT "Cities"."Id" FROM "Cities" WHERE "Cities"."Name" = 'Zrenjanin' AND "Cities"."Country"='Serbia'),
     '50', 0, 2, 45.244320, 19.831070, true, (SELECT "Users"."Id" FROM "Users" WHERE "Username" = 'crni'));

-- WashingMachineModes
INSERT INTO "WashingMachineModes" ("Id", "Duration", "Name", "Temperature") VALUES
    (gen_random_uuid(), 30, 'Mixed wash', 30),
    (gen_random_uuid(), 60, 'Antiallergy', 60),
    (gen_random_uuid(), 60, 'White wash', 90);
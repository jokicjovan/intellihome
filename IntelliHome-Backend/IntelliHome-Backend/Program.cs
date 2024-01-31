using Data.Context;
using MQTTnet;
using MQTTnet.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using IntelliHome_Backend.Features.Security;
using IntelliHome_Backend.Features.Home.Repositories;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Home.Services;
using IntelliHome_Backend.Features.Home.Services.Interfaces;
using IntelliHome_Backend.Features.Home.Handlers;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Home.DataRepository;
using IntelliHome_Backend.Features.PKA.DataRepositories;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.PKA.Handlers;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Handlers;
using IntelliHome_Backend.Features.SPU.DataRepositories;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Repositories;
using IntelliHome_Backend.Features.VEU.Repositories.Interfaces;
using IntelliHome_Backend.Features.VEU.Services;
using IntelliHome_Backend.Features.VEU.Services.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers.Interfaces;
using IntelliHome_Backend.Features.VEU.Handlers;
using IntelliHome_Backend.Features.VEU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.VEU.DataRepositories;
using IntelliHome_Backend.Features.Users.Repositories;
using IntelliHome_Backend.Features.Users.Repositories.Interfaces;
using IntelliHome_Backend.Features.Users.Services;
using IntelliHome_Backend.Features.Users.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Infrastructure;
using IntelliHome_Backend.Features.Shared.Services;
using IntelliHome_Backend.Features.Shared.Services.Interfacted;
using IntelliHome_Backend.Features.Shared.Handlers;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;
using IntelliHome_Backend.Features.Shared.BackgroundServices;
using IntelliHome_Backend.Features.Shared.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Hubs;
using IntelliHome_Backend.Features.Shared.Influx;
using Microsoft.Extensions.ObjectPool;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB context
builder.Services.AddDbContext<PostgreSqlDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//InfluxDB context
// builder.Services.AddScoped<InfluxRepository>();
builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
builder.Services.AddSingleton(provider =>
{
    var objectPoolProvider = provider.GetRequiredService<ObjectPoolProvider>();
    var configuration = provider.GetRequiredService<IConfiguration>();

    var url = configuration["InfluxDB:Url"];
    // var token = configuration["InfluxDB:Token"];
    var organization = configuration["InfluxDB:Organization"];
    var bucket = configuration["InfluxDB:Bucket"];

    //read token from file
    StreamReader sr = new("InfluxDBToken.txt");
    string token = sr.ReadLine();

    return new InfluxDbConnectionPool(objectPoolProvider, url, token, organization, bucket);
});

builder.Services.AddScoped(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionPool = provider.GetRequiredService<InfluxDbConnectionPool>();
    var database = configuration["InfluxDB:Database"];
    var bucket = configuration["InfluxDB:Bucket"];

    return new InfluxRepository(connectionPool, bucket, database);
});


//Data repositories
builder.Services.AddScoped<IAmbientSensorDataRepository, AmbientSensorDataRepository>();
builder.Services.AddScoped<IAirConditionerDataRepository, AirConditionerDataRepository>();
builder.Services.AddScoped<ILampDataRepository, LampDataRepository>();
builder.Services.AddScoped<IVehicleGateDataRepository, VehicleGateDataRepository>();
builder.Services.AddScoped<ISprinklerDataRepository, SprinklerDataRepository>();
builder.Services.AddScoped<IBatterySystemDataRepository, BatterySystemDataRepository>();
builder.Services.AddScoped<ISolarPanelSystemDataRepository, SolarPanelSystemDataRepository>();
builder.Services.AddScoped<ISmartHomeDataRepository, SmartHomeDataRepository>();
builder.Services.AddScoped<IVehicleChargerDataRepository, VehicleChargerDataRepository>();
builder.Services.AddScoped<IVehicleChargingPointDataRepository, VehicleChargingPointDataRepository>();
builder.Services.AddScoped<ISmartDeviceDataRepository, SmartDeviceDataRepository>();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConfirmationRepository, ConfirmationRepository>();
builder.Services.AddScoped<ISmartDeviceRepository, SmartDeviceRepository>();
builder.Services.AddScoped<ISmartHomeRepository, SmartHomeRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IAirConditionerRepository, AirConditionerRepository>();
builder.Services.AddScoped<IAirConditionerWorkRepository, AirConditionerWorkRepository>();
builder.Services.AddScoped<IAmbientSensorRepository, AmbientSensorRepository>();
builder.Services.AddScoped<IWashingMachineRepository, WashingMachineRepository>();
builder.Services.AddScoped<IWashingMachineModeRepository, WashingMachineModeRepository>();
builder.Services.AddScoped<ILampRepository, LampRepository>();
builder.Services.AddScoped<ISprinklerRepository, SprinklerRepository>();
builder.Services.AddScoped<ISprinklerWorkRepository, SprinklerWorkRepository>();
builder.Services.AddScoped<IVehicleGateRepository, VehicleGateRepository>();
builder.Services.AddScoped<IBatterySystemRepository, BatterySystemRepository>();
builder.Services.AddScoped<ISolarPanelSystemRepository, SolarPanelSystemRepository>();
builder.Services.AddScoped<IVehicleChargerRepository, VehicleChargerRepository>();
builder.Services.AddScoped<IVehicleChargingPointRepository, VehicleChargingPointRepository>();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IConfirmationService, ConfirmationService>();
builder.Services.AddScoped<ISmartDeviceService, SmartDeviceService>();
builder.Services.AddScoped<ISmartHomeService, SmartHomeService>();
builder.Services.AddScoped<IAirConditionerService, AirConditionerService>();
builder.Services.AddScoped<IAmbientSensorService, AmbientSensorService>();
builder.Services.AddScoped<IWashingMachineService, WashingMachineService>();
builder.Services.AddScoped<ILampService, LampService>();
builder.Services.AddScoped<ISprinklerService, SprinklerService>();
builder.Services.AddScoped<IVehicleGateService, VehicleGateService>();
builder.Services.AddScoped<IBatterySystemService, BatterySystemService>();
builder.Services.AddScoped<ISolarPanelSystemService, SolarPanelSystemService>();
builder.Services.AddScoped<IVehicleChargerService, VehicleChargerService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddSingleton<MqttFactory>();

//Handlers
builder.Services.AddSingleton<ISimulationsHandler, SimulationsHandler>();
builder.Services.AddSingleton<ILastWillHandler, LastWillHandler>();
builder.Services.AddSingleton<IAmbientSensorHandler, AmbientSensorHandler>();
builder.Services.AddSingleton<IAirConditionerHandler, AirConditionerHandler>();
builder.Services.AddSingleton<IWashingMachineHandler, WashingMachineHandler>();
builder.Services.AddSingleton<ILampHandler, LampHandler>();
builder.Services.AddSingleton<ISprinklerHandler, SprinklerHandler>();
builder.Services.AddSingleton<IVehicleGateHandler, VehicleGateHandler>();
builder.Services.AddSingleton<IBatterySystemHandler, BatterySystemHandler>();
builder.Services.AddSingleton<ISolarPanelSystemHandler, SolarPanelSystemHandler>();
builder.Services.AddSingleton<IVehicleChargerHandler, VehicleChargerHandler>();
builder.Services.AddSingleton<ISmartDeviceHandler, SmartDeviceHandler>();
builder.Services.AddSingleton<ISmartHomeHandler, SmartHomeHandler>();

//Hosted services
builder.Services.AddHostedService<StartupHostedService>();

//SignalR
builder.Services.AddSignalR();

//export port 5238
builder.WebHost.UseUrls("http://*:5283");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8000", "http://localhost:4173", "https://accounts.google.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddTransient<CustomCookieAuthenticationEvents>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.Cookie.SameSite = SameSiteMode.None;
           options.Cookie.Name = "auth";
           options.SlidingExpiration = true;
           options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
           options.Cookie.MaxAge = options.ExpireTimeSpan;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
           options.EventsType = typeof(CustomCookieAuthenticationEvents);
       }).AddGoogle(options =>
       {
           StreamReader sr = new StreamReader("oauth_key.txt");
           String clientId = sr.ReadLine();
           String clientSecret = sr.ReadLine();
           options.ClientId = clientId;
           options.ClientSecret = clientSecret;
           options.CallbackPath = "/api/User/handle-signin-google";
           options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
           options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
       });


builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"static/")),
    RequestPath = new PathString("/static")
});

app.UseMiddleware<ExceptionMiddleware>(true);
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//Hubs
app.MapHub<SmartDeviceHub>("/hub/SmartDeviceHub");
app.MapHub<SmartHomeHub>("/hub/SmartHomeHub");

app.Run();

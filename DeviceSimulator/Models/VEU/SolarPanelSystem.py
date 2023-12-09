import asyncio
import json

import pandas as pd
import pvlib

from Models.SmartDevice import SmartDevice
from datetime import datetime


class SolarPanelSystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, area, efficiency):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.area = area
        self.efficiency = efficiency

    async def send_data(self):
        while True:
            pd_current_datetime = pd.to_datetime([datetime.utcnow()]).tz_localize('UTC')
            solar_position = pvlib.solarposition.get_solarposition(pd_current_datetime, latitude=44.786568,
                                                                   longitude=20.448921, altitude=155.813446)
            solar_zenith = solar_position['apparent_zenith']
            solar_azimuth = solar_position['azimuth']

            power_per_minute = 0
            # Check if it's daytime (Sun is above the horizon)
            if solar_zenith.values < 90:
                solar_irradiance = pvlib.irradiance.get_total_irradiance(
                    solar_zenith=solar_zenith,
                    solar_azimuth=solar_azimuth,
                    surface_tilt=0,  # Assuming a horizontal panel
                    surface_azimuth=180,  # Facing south
                    dni=5.0,  # Direct Normal Irradiance assumed for simplicity
                    ghi=5.0,  # Global Horizontal Irradiance assumed for simplicity
                    dhi=2.5  # Diffuse Horizontal Irradiance assumed for simplicity
                )

                # Calculate energy produced per minute
                power_per_minute = solar_irradiance['poa_global'].mean() * self.area * self.efficiency / 100 / 60
            if not self.is_on:
                break
            self.client.publish(self.send_topic, json.dumps({"production_per_minute": round(power_per_minute, 4)}),
                                retain=False)
            await asyncio.sleep(10)

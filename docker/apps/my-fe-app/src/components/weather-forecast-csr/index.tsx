'use client';

import WeatherForecastModel from "@/models/weather-forecast";
import { useEffect, useState } from "react";
import { WeatherForecastComponent } from "..";
import { IDynamicSettings } from "@/constants";

interface IWeatherForecastCsrProps {
    dynamicSettings: IDynamicSettings
}

const WeatherForecastCsrComponent: React.FC<IWeatherForecastCsrProps> = ({
    dynamicSettings
}) => {
    const [weatherForecasts, setWeatherForecast] = useState<WeatherForecastModel[]>();

    useEffect(() => {
        fetch(`${dynamicSettings.webApiUrl}/weatherforecast`)
            .then(resp => resp.json())
            .then(resp => {
                setWeatherForecast(resp);
            });
    }, []);

    return <WeatherForecastComponent
        source="CSR"
        weatherForecasts={weatherForecasts}
    />
}

export default WeatherForecastCsrComponent;


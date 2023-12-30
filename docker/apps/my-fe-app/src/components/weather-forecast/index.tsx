import WeatherForecastModel from "@/models/weather-forecast";

interface IWeatherForecastProps {
    source: string;
    weatherForecasts?: WeatherForecastModel[];
}

const WeatherForecastComponent: React.FC<IWeatherForecastProps> = ({
    source, weatherForecasts
}) => {
    return <div>
        <h2 className={`mb-3 text-2xl font-semibold`}>
            Weather forecast from API ({source})
        </h2>
        <p className="text-yellow-400">
            {JSON.stringify(weatherForecasts)}
        </p>
    </div>
}

export default WeatherForecastComponent;
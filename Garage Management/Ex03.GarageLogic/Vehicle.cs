using System;

namespace GarageLogic
{
    public abstract class Vehicle
    {
        private readonly VehicleFactory.eVehicleType r_VehicleType;
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;
        private readonly Tire[] r_Tires;
        private readonly Engine r_Engine;
        private float m_EnergyPercentageLeft;

        protected Vehicle(VehicleFactory.eVehicleType i_VehicleType, string i_ModelName, string i_LicenseNumber, Tire[] i_Tires, Engine i_Engine, float i_EnergyPercentageLeft, string i_TireManufactureName, float i_MaxAirPressure, float i_TireCurrentAirPressure)
        {
            r_VehicleType = i_VehicleType;
            r_ModelName = i_ModelName;
            r_LicenseNumber = i_LicenseNumber;
            r_Tires = i_Tires;
            r_Engine = i_Engine;
            EnergyPercentageLeft = i_EnergyPercentageLeft;
            for (int currentTire = 0; currentTire < i_Tires.Length; currentTire++)
            {
                Tires[currentTire] = new Tire(i_TireManufactureName, i_MaxAirPressure, i_TireCurrentAirPressure);
            }
        }

        public VehicleFactory.eVehicleType VehicleType => r_VehicleType;

        public string LicenseNumber => r_LicenseNumber;

        public string ModelName => r_ModelName;

        public Tire[] Tires => r_Tires;

        public Engine Engine => r_Engine;

        public float EnergyPercentageLeft
        {
            get => m_EnergyPercentageLeft;
            set => m_EnergyPercentageLeft = value;
        }

        public static float GetMaxAirPressure(VehicleFactory.eVehicleType i_VehicleType)
        {
            float maxAirPressure;
            switch (i_VehicleType)
            {
                case VehicleFactory.eVehicleType.Bike:
                case VehicleFactory.eVehicleType.ElectricBike:
                    maxAirPressure = Bike.k_MaxAirPressure;
                    break;
                case VehicleFactory.eVehicleType.Car:
                case VehicleFactory.eVehicleType.ElectricCar:
                    maxAirPressure = Car.k_MaxAirPressure;
                    break;
                case VehicleFactory.eVehicleType.Truck:
                    maxAirPressure = Truck.k_MaxAirPressure;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i_VehicleType), i_VehicleType, null);
            }

            return maxAirPressure;
        }

        public override string ToString()
        {
            return $@"{VehicleType}
Model name: {r_ModelName}
License number: {r_LicenseNumber}
Tires manufacture name: {r_Tires[0].ManufactureName}
Tires air pressure: {r_Tires[0].CurrentAirPressure}
{r_Engine}
Energy percentage left: {m_EnergyPercentageLeft}%";
        }
    }
}
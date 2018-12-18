using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public static class VehicleFactory
    {
        public enum eVehicleType
        {
            Bike = 1,
            ElectricBike = 2,
            Car = 3,
            ElectricCar = 4,
            Truck = 5
        }

        internal enum eParametersInputOrder
        {
            VehicleTypeName = 0,
            ModelName = 1,
            LicenseNumber = 2,
            EnergyPercentageLeft = 3,
            TireManufactureName = 4,
            TiresCurrentAirPressure = 5
        }

        internal enum eExtraParametersInputOrder
        {
            BikeLicenseType = 6,
            BikeEngineCapacity = 7,
            CarColor = 6,
            CarNumOfDoors = 7,
            TruckHazardousMaterials = 6,
            TruckMaxCarryingWeight = 7
        }

        public static Vehicle CreateVehicle(string i_VehicleTypeString, List<string> i_VehicleParametersStrings)
        {
            Vehicle vehicle;
            eVehicleType vehicleType = CheckVehicleType(i_VehicleTypeString);
            switch (vehicleType)
            {
                case eVehicleType.Bike:
                    vehicle = createBike(Engine.eEngineType.FuelEngine, i_VehicleParametersStrings, eVehicleType.Bike);
                    break;
                case eVehicleType.ElectricBike:
                    vehicle = createBike(Engine.eEngineType.ElectricEngine, i_VehicleParametersStrings, eVehicleType.ElectricBike);
                    break;
                case eVehicleType.Car:
                    vehicle = createCar(Engine.eEngineType.FuelEngine, i_VehicleParametersStrings, eVehicleType.Car);
                    break;
                case eVehicleType.ElectricCar:
                    vehicle = createCar(Engine.eEngineType.ElectricEngine, i_VehicleParametersStrings, eVehicleType.ElectricCar);
                    break;
                case eVehicleType.Truck:
                    vehicle = createTruck(i_VehicleParametersStrings, eVehicleType.Truck);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            setCurrentEnergyQuantity(vehicle);
            return vehicle;
        }

        public static List<ParameterChecker> BuildExtraParametersInfo(eVehicleType i_VehicleType)
        {
            List<ParameterChecker> extraParameterProcessors;
            switch (i_VehicleType)
            {
                case eVehicleType.Bike:
                case eVehicleType.ElectricBike:
                    extraParameterProcessors = Bike.BuildExtraParametersList();
                    break;
                case eVehicleType.Car:
                case eVehicleType.ElectricCar:
                    extraParameterProcessors = Car.BuildExtraParametersList();
                    break;
                case eVehicleType.Truck:
                    extraParameterProcessors = Truck.BuildExtraParametersList();
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            return extraParameterProcessors;
        }

        public static eVehicleType CheckVehicleType(string i_VehicleTypeString)
        {
            eVehicleType vehicleType;
            if (!Enum.TryParse(i_VehicleTypeString, out vehicleType)
                || !Enum.IsDefined(typeof(eVehicleType), vehicleType))
            {
                throw new ArgumentException(Garage.k_InvalidEnumExceptionString);
            }

            return vehicleType;
        }

        private static void setCurrentEnergyQuantity(Vehicle i_Vehicle)
        {
            i_Vehicle.Engine.CurrentEnergyQuantity = (i_Vehicle.EnergyPercentageLeft / Utilities.k_MaxPercentage) * i_Vehicle.Engine.MaxEnergyQuantity;
        }

        private static Vehicle createBike(Engine.eEngineType i_Engine, List<string> i_VehicleParametersStrings, eVehicleType i_VehicleType)
        {
            string modelName;
            string licenseNumber;
            float energyPercentageLeft;
            string tireManufactureName;
            float tireCurrentAirPressure;

            Bike.eLicenseType licenseType = (Bike.eLicenseType)Enum.Parse(typeof(Bike.eLicenseType), i_VehicleParametersStrings[(int)eExtraParametersInputOrder.BikeLicenseType]);

            getVehicleParameters(
                i_VehicleParametersStrings,
                out modelName,
                out licenseNumber,
                out energyPercentageLeft,
                out tireManufactureName,
                out tireCurrentAirPressure);

            Engine engine = i_Engine == Engine.eEngineType.FuelEngine
                                ? createFuelEngine(Bike.k_FuelType, Bike.k_MaxTankCapacity)
                                : (Engine)createElectricEngine(Bike.k_MaxBatteryTime);

            Bike bike = new Bike(i_VehicleType, modelName, licenseNumber, new Tire[Bike.k_NumberOfTires], engine, energyPercentageLeft, tireManufactureName, tireCurrentAirPressure, licenseType, Bike.k_MaxTankCapacity);
            return bike;
        }

        private static Vehicle createCar(Engine.eEngineType i_Engine, List<string> i_VehicleParametersStrings, eVehicleType i_VehicleType)
        {
            string modelName;
            string licenseNumber;
            float energyPercentageLeft;
            string tireManufactureName;
            float tireCurrentAirPressure;

            Car.eCarColors carColor = (Car.eCarColors)Enum.Parse(typeof(Car.eCarColors), i_VehicleParametersStrings[(int)eExtraParametersInputOrder.CarColor]);
            Car.eNumberOfDoors numberOfDoors = (Car.eNumberOfDoors)Enum.Parse(typeof(Car.eNumberOfDoors), i_VehicleParametersStrings[(int)eExtraParametersInputOrder.CarNumOfDoors]);
            getVehicleParameters(
                i_VehicleParametersStrings,
                out modelName,
                out licenseNumber,
                out energyPercentageLeft,
                out tireManufactureName,
                out tireCurrentAirPressure);

            Engine engine = i_Engine == Engine.eEngineType.FuelEngine
                                ? (Engine)createFuelEngine(Car.k_FuelType, Car.k_MaxTankCapacity)
                                : createElectricEngine(Car.k_MaxBatteryTime);

            Car car = new Car(i_VehicleType, modelName, licenseNumber, new Tire[Car.k_NumberOfTires], engine, energyPercentageLeft, tireManufactureName, tireCurrentAirPressure, carColor, numberOfDoors);
            return car;
        }

        private static Vehicle createTruck(List<string> i_VehicleParametersStrings, eVehicleType i_VehicleType)
        {
            string modelName;
            string licenseNumber;
            float energyPercentageLeft;
            string tireManufactureName;
            float tireCurrentAirPressure;
            bool isContainingHazardousMaterials =
                i_VehicleParametersStrings[(int)eExtraParametersInputOrder.TruckHazardousMaterials] == "1";
            float maxCarryingWeight = Convert.ToSingle(
                i_VehicleParametersStrings[(int)eExtraParametersInputOrder.TruckMaxCarryingWeight]);
            getVehicleParameters(
                i_VehicleParametersStrings,
                out modelName,
                out licenseNumber,
                out energyPercentageLeft,
                out tireManufactureName,
                out tireCurrentAirPressure);

            Engine engine = createFuelEngine(Truck.k_FuelType, Truck.k_MaxTankCapacity);

            Truck truck = new Truck(i_VehicleType, modelName, licenseNumber, new Tire[Truck.k_NumberOfTires], engine, energyPercentageLeft, tireManufactureName, tireCurrentAirPressure, maxCarryingWeight, isContainingHazardousMaterials);
            return truck;
        }

        private static FuelEngine createFuelEngine(FuelEngine.eFuelType i_FuelType, float i_MaxTankCapacity)
        {
            return new FuelEngine(i_FuelType, i_MaxTankCapacity, i_MaxTankCapacity);
        }

        private static ElectricEngine createElectricEngine(float i_MaxBatteryTime)
        {
            return new ElectricEngine(i_MaxBatteryTime, i_MaxBatteryTime);
        }

        private static void getVehicleParameters(
            List<string> i_VehicleParametersStrings,
            out string o_ModelName,
            out string o_LicenseNumber,
            out float o_EnergyPercentageLeft,
            out string o_TireManufactureName,
            out float o_TireCurrentAirPressure)
        {
            o_ModelName = i_VehicleParametersStrings[(int)eParametersInputOrder.ModelName];
            o_LicenseNumber = i_VehicleParametersStrings[(int)eParametersInputOrder.LicenseNumber];
            o_EnergyPercentageLeft = Convert.ToSingle(i_VehicleParametersStrings[(int)eParametersInputOrder.EnergyPercentageLeft]);
            o_TireManufactureName = i_VehicleParametersStrings[(int)eParametersInputOrder.TireManufactureName];
            o_TireCurrentAirPressure = Convert.ToSingle(i_VehicleParametersStrings[(int)eParametersInputOrder.TiresCurrentAirPressure]);
        }
    }
}

using System.Collections.Generic;

namespace GarageLogic
{
    public class Car : Vehicle
    {
        internal const FuelEngine.eFuelType k_FuelType = FuelEngine.eFuelType.Octan98;
        internal const byte k_NumberOfTires = 4;
        internal const byte k_MaxAirPressure = 32;
        internal const float k_MaxTankCapacity = 50;
        internal const float k_MaxBatteryTime = 2.8f;
        private readonly eCarColors r_CarColor;
        private readonly eNumberOfDoors r_NumOfDoors;
      
        public enum eCarColors
        {
            Green = 1,
            Silver = 2,
            White = 3,
            Black = 4
        }

        public enum eNumberOfDoors
        {
            Two = 1,
            Three = 2,
            Four = 3,
            Five = 4
        }

        public Car(
            VehicleFactory.eVehicleType i_VehicleType,
            string i_ModelName,
            string i_LicenseNumber,
            Tire[] i_Tires,
            Engine i_Engine,
            float i_EnergyPercentageLeft,
            string i_TireManufactureName,
            float i_TireCurrentAirPressure,
            eCarColors i_CarColor,
            eNumberOfDoors i_NumOfDoors)
            : base(i_VehicleType, i_ModelName, i_LicenseNumber, i_Tires, i_Engine, i_EnergyPercentageLeft, i_TireManufactureName, k_MaxAirPressure, i_TireCurrentAirPressure)
        {
            r_CarColor = i_CarColor;
            r_NumOfDoors = i_NumOfDoors;
        }

        public eCarColors CarColor
        {
            get => r_CarColor;
        }

        public eNumberOfDoors NumOfDoors
        {
            get => r_NumOfDoors;
        }

        public static List<ParameterChecker> BuildExtraParametersList()
        {
            List<ParameterChecker> parameterProcessors =
                new List<ParameterChecker>
                    {
                        new ParameterChecker(
                            "Please choose the color of the car from the list.",
                            new[] { "Green", "Silver", "White", "Black" },
                            ParameterChecker.eExpectedInputType.NumbersOnly),
                        new ParameterChecker(
                            "Please choose the number of doors in the car from the list.",
                            new[] { "Two", "Three", "Four", "Five" },
                            ParameterChecker.eExpectedInputType.NumbersOnly)
                    };

            return parameterProcessors;
        }

        public override string ToString()
        {
            return $@"{base.ToString()}
Car color: {r_CarColor}
Number of doors: {r_NumOfDoors}";
        }
    }
}

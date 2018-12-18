using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class Bike : Vehicle
    {
        internal const FuelEngine.eFuelType k_FuelType = FuelEngine.eFuelType.Octan95;
        internal const byte k_NumberOfTires = 2;
        internal const byte k_MaxAirPressure = 28;
        internal const float k_MaxTankCapacity = 5.5f;
        internal const float k_MaxBatteryTime = 1.6f;

        public enum eLicenseType
        {
            BB = 1,
            AA = 2,
            B1 = 3,
            A1 = 4
        }

        private readonly eLicenseType r_LicenseType;
        private readonly float r_EngineCapacity;

        public Bike(VehicleFactory.eVehicleType i_VehicleType, string i_ModelName, string i_LicenseNumber, Tire[] i_Tires, Engine i_Engine, float i_EnergyPercentageLeft, string i_TireManufactureName, float i_TireCurrentAirPressure, eLicenseType i_LicenseType, float i_EngineCapacity)
            : base(i_VehicleType, i_ModelName, i_LicenseNumber, i_Tires, i_Engine, i_EnergyPercentageLeft, i_TireManufactureName, k_MaxAirPressure, i_TireCurrentAirPressure)
        {
            r_LicenseType = i_LicenseType;
            r_EngineCapacity = i_EngineCapacity;
        }

        public eLicenseType LicenseType => r_LicenseType;

        public static eLicenseType CheckLicenseType(string i_LicenseTypeString)
        {
            if (!Enum.TryParse(i_LicenseTypeString, out eLicenseType licensesType)
                || !Enum.IsDefined(typeof(eLicenseType), licensesType))
            {
                throw new ArgumentException(Garage.k_InvalidEnumExceptionString);
            }

            return licensesType;
        }

        public float EngineCapacity => r_EngineCapacity;

        public static List<ParameterChecker> BuildExtraParametersList()
        {
            List<ParameterChecker> parameterProcessors =
                new List<ParameterChecker>
                    {
                        new ParameterChecker(
                            "Please enter the Bike's license type.",
                            new[] { "BB", "AA", "B1", "A1" },
                            ParameterChecker.eExpectedInputType.NumbersOnly),
                        new ParameterChecker(
                            "Please enter the Bike engine's capacity (in numbers).",
                            ParameterChecker.eExpectedInputType.Float )
                    };

            return parameterProcessors;
        }

        public override string ToString()
        {
            return $@"{base.ToString()}
License type: {r_LicenseType}
Engine capacity: {r_EngineCapacity}";
        }
    }
}

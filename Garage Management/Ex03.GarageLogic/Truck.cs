using System.Collections.Generic;

namespace GarageLogic
{
    public class Truck : Vehicle
    {
        internal const FuelEngine.eFuelType k_FuelType = FuelEngine.eFuelType.Soler;
        internal const byte k_NumberOfTires = 12;
        internal const byte k_MaxAirPressure = 34;
        internal const float k_MaxTankCapacity = 130;
        private readonly float r_MaxCarryingWeight;
        private bool m_IsContainingHazardMaterials;

        public Truck(VehicleFactory.eVehicleType i_VehicleType, string i_ModelName, string i_LicenseNumber, Tire[] i_Tires, Engine i_Engine, float i_EnergyPercentageLeft, string i_TireManufactureName, float i_TireCurrentAirPressure, float i_MaxCarryingWeight, bool i_IsContainingHazardMaterials)
            : base(i_VehicleType, i_ModelName, i_LicenseNumber, i_Tires, i_Engine, i_EnergyPercentageLeft, i_TireManufactureName, k_MaxAirPressure, i_TireCurrentAirPressure)
        {
            r_MaxCarryingWeight = i_MaxCarryingWeight;
            IsContainingHazardMaterials = i_IsContainingHazardMaterials;
        }

        public float MaxCarryingWeight => r_MaxCarryingWeight;

        public bool IsContainingHazardMaterials
        {
            get => m_IsContainingHazardMaterials;
            set => m_IsContainingHazardMaterials = value;
        }

        public static List<ParameterChecker> BuildExtraParametersList()
        {
            List<ParameterChecker> parameterProcessors =
                new List<ParameterChecker>
                    {
                        new ParameterChecker(
                            "Are there any hazardous materials on the truck? ",
                            new[] { "Yes", "No" },
                            ParameterChecker.eExpectedInputType.NumbersOnly),
                        new ParameterChecker(
                            "Please enter truck's max carrying weight.",
                            ParameterChecker.eExpectedInputType.Float)
                    };

            return parameterProcessors;
        }

        public override string ToString()
        {
            string hazardousMaterialsString = m_IsContainingHazardMaterials ? "Yes" : "No";
            return $@"{base.ToString()}
Maximum carrying weight: {r_MaxCarryingWeight}
Containing hazardous materials: {hazardousMaterialsString}";
        }
    }
}
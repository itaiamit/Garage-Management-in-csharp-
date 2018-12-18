using System;

namespace GarageLogic
{
    public class FuelEngine : Engine
    {
        public enum eFuelType
        {
            Octan95 = 1,
            Octan96 = 2,
            Octan98 = 3,
            Soler = 4
        }

        public FuelEngine(eFuelType i_FuelType, float i_CurrentFuelAmountInLiters, float i_MaxFuelAmountInLiters)
            : base(eEngineType.FuelEngine, i_CurrentFuelAmountInLiters, i_MaxFuelAmountInLiters)
        {
            FuelType = i_FuelType;
        }

        public eFuelType FuelType { get; }

        public static eFuelType CheckFuelType(string i_FuelTypeChoice)
        {
            if (!Enum.TryParse(i_FuelTypeChoice, out eFuelType fuelType)
                || !Enum.IsDefined(typeof(eFuelType), fuelType))
            {
                throw new ArgumentException(Garage.k_InvalidEnumExceptionString);
            }

            return fuelType;
        }

        public override void FillEnergySource(params object[] i_EnergySourceObjects)
        {
            const byte k_FuelTypeIndex = 1;

            if (FuelType == (eFuelType)i_EnergySourceObjects[k_FuelTypeIndex])
            {
                FillEnergySourceAmount((float)i_EnergySourceObjects[k_EnergySourceAmountIndex]);
            }
            else
            {
                throw new ArgumentException("Invalid fuel type: ", i_EnergySourceObjects[k_FuelTypeIndex].ToString());
            }
        }

        public override string ToString()
        {
            return $@"{base.ToString()}
Fuel type: {FuelType}";
        }
    }
}
namespace GarageLogic
{
    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_MaxBatteryChargeTimeInHours, float i_BatteryTimeLeftInHours)
            : base(eEngineType.ElectricEngine, i_BatteryTimeLeftInHours, i_MaxBatteryChargeTimeInHours)
        {
        }

        public override void FillEnergySource(params object[] i_EnergySourceObjects)
        {
            FillEnergySourceAmount((float)i_EnergySourceObjects[k_EnergySourceAmountIndex]);
        }
    }
}

namespace GarageLogic
{
    public abstract class Engine
    {
        protected const byte k_EnergySourceAmountIndex = 0;
        protected readonly eEngineType r_EngineType;
        protected readonly float r_MaxEnergyQuantity;
        protected float m_CurrentEnergyQuantity; // Fuel in Liters and Electric in Hours

        public enum eEngineType
        {
            FuelEngine = 0,
            ElectricEngine = 1
        }

        protected Engine(eEngineType i_EngineType, float i_CurrentEnergyQuantity, float i_MaxEnergyQuantity)
        {
            r_EngineType = i_EngineType;
            m_CurrentEnergyQuantity = i_CurrentEnergyQuantity;
            r_MaxEnergyQuantity = i_MaxEnergyQuantity;
        }

        public eEngineType EngineType => r_EngineType;

        internal float MaxEnergyQuantity => r_MaxEnergyQuantity;

        internal float CurrentEnergyQuantity
        {
            get => m_CurrentEnergyQuantity;
            set => m_CurrentEnergyQuantity = value;
        }

        protected void FillEnergySourceAmount(float i_EnergySourceToFill)
        {
            if (m_CurrentEnergyQuantity + i_EnergySourceToFill <= r_MaxEnergyQuantity)
            {
                m_CurrentEnergyQuantity += i_EnergySourceToFill;
            }
            else
            {
                throw new ValueOutOfRangeException(0, r_MaxEnergyQuantity - m_CurrentEnergyQuantity);
            }
        }

        public abstract void FillEnergySource(params object[] i_EnergySourceObjects);

        internal float GetEnergySourcePercentageLeft()
        {
            return (r_MaxEnergyQuantity - m_CurrentEnergyQuantity) / Utilities.k_MaxPercentage;
        }

        public override string ToString()
        {
            bool isFuelEngine = r_EngineType == eEngineType.FuelEngine;
            string currentEnergyLeftString = r_EngineType == eEngineType.ElectricEngine
                                   ? "left"
                                   : string.Empty;
            string engineTypeString = isFuelEngine ? "fuel quantity (in liters)" : "battery hours";
            return $@"Maximum {engineTypeString}: {r_MaxEnergyQuantity}
Current {engineTypeString} {currentEnergyLeftString}: {m_CurrentEnergyQuantity:F}";
        }
    }
}
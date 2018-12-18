namespace GarageLogic
{
    public class Tire
    {
        private readonly string r_ManufactureName;
        private readonly float r_MaxAirPressure;
        private float m_CurrentAirPressure;

        public Tire(string i_ManufactureName, float i_MaxAirPressure, float i_CurrentAirPressure)
        {
            r_ManufactureName = i_ManufactureName;
            r_MaxAirPressure = i_MaxAirPressure;
            m_CurrentAirPressure = i_CurrentAirPressure;
        }

        public string ManufactureName => r_ManufactureName;

        public float MaxAirPressure => r_MaxAirPressure;

        public float CurrentAirPressure
        {
            get => m_CurrentAirPressure;
            set => m_CurrentAirPressure = value;
        }

        public void InflateTire(float i_AirPressureAmountToAdd)
        {
            if (CurrentAirPressure + i_AirPressureAmountToAdd <= MaxAirPressure)
            {
                CurrentAirPressure += i_AirPressureAmountToAdd;
            }
            else
            {
                throw new ValueOutOfRangeException(0, MaxAirPressure - CurrentAirPressure);
            }
        }
    }
}
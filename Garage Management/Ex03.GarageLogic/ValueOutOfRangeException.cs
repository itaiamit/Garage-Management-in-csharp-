using System;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MinValue;
        private float m_MaxValue;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base($"Value of variable was out of range ({i_MinValue} - {i_MaxValue}) ") 
        {
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }

        public float MinValue
        {
            get => m_MinValue;
            set => m_MinValue = value;
        }

        public float MaxValue
        {
            get => m_MaxValue;
            set => m_MaxValue = value;
        }
    }
}
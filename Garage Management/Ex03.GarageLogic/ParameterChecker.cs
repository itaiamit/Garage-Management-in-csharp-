using System;

namespace GarageLogic
{
    public class ParameterChecker
    {
        private const string k_InvalidInput = "Invalid input.";
        private const int k_EmptyListIndicator = 0;
        private const int k_MinIndex = 1;
        private string m_InputRequestString;
        private string[] m_ParameterValues;
        private eExpectedInputType m_ExpectedInputType;

        public ParameterChecker(string i_InputRequestString, eExpectedInputType i_ExpectedInputType)
            : this(i_InputRequestString, new string[] { }, i_ExpectedInputType)
        {
        }

        public ParameterChecker(
            string i_InputRequestString,
            string[] i_ParameterValues,
            eExpectedInputType i_ExpectedInputType)
        {
            InputRequestString = i_InputRequestString;
            ParameterValues = i_ParameterValues;
            ExpectedInputType = i_ExpectedInputType;
        }

        public enum eExpectedInputType
        {
            All = 0,
            NumbersOnly = 1,
            LettersOnly = 2,
            Float = 3,
        }

        public string InputRequestString
        {
            get => m_InputRequestString;
            set => m_InputRequestString = value;
        }

        public string[] ParameterValues
        {
            get => m_ParameterValues;
            set => m_ParameterValues = value;
        }

        public eExpectedInputType ExpectedInputType
        {
            get => m_ExpectedInputType;
            set => m_ExpectedInputType = value;
        }

        public static void CheckParameterTypeValidity(eExpectedInputType i_ParametersTypeAllowed, string i_DetailToGet)
        {
            switch (i_ParametersTypeAllowed)
            {
                case eExpectedInputType.All:
                    break;
                case eExpectedInputType.NumbersOnly:
                    Utilities.IsDigitsOnly(i_DetailToGet);
                    break;
                case eExpectedInputType.LettersOnly:
                    Utilities.IsLettersOnly(i_DetailToGet);
                    break;
                case eExpectedInputType.Float:
                    Utilities.IsNonNegativeFloat(i_DetailToGet);
                    break;
                default:  
                    throw new ArgumentOutOfRangeException(
                        nameof(i_ParametersTypeAllowed),
                        i_ParametersTypeAllowed,
                        null);
            }
        }

        public static void CheckParameterInRange(float i_MinValue, float i_MaxValue, float i_ParameterToCheck)
        {
            if (i_ParameterToCheck >= i_MinValue && i_ParameterToCheck <= i_MaxValue)
            {
                return;
            }

            throw new ValueOutOfRangeException(i_MinValue, i_MaxValue);
        }

        public void CheckParameterValidity(byte i_MaxNumOfParameterValueOptions, ParameterChecker i_ParameterChecker, string i_InputString)
        {
            if (i_MaxNumOfParameterValueOptions == k_EmptyListIndicator)
            {
                CheckParameterTypeValidity(i_ParameterChecker.ExpectedInputType, i_InputString);
            }
            else
            {
                if (byte.TryParse(i_InputString, out byte inputNumber))
                {
                    CheckParameterInRange(k_MinIndex, i_MaxNumOfParameterValueOptions, inputNumber);
                }
                else
                {
                    throw new FormatException(k_InvalidInput);
                }
            }
        }
    }
}

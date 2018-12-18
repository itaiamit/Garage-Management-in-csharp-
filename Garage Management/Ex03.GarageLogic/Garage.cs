using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class Garage
    {
        public const string k_InvalidEnumExceptionString = "The number you selected was undefined in the shown options.";

        public class Client
        {
            private string m_OwnerName;

            public string OwnerName
            {
                get => m_OwnerName;
                set => m_OwnerName = value;
            }

            private string m_OwnerPhoneNumber;

            public string OwnerPhoneNumber
            {
                get => m_OwnerPhoneNumber;
                set => m_OwnerPhoneNumber = value;
            }

            private eVehicleStatus m_VehicleStatus;

            public eVehicleStatus VehicleStatus
            {
                get => m_VehicleStatus;
                set => m_VehicleStatus = value;
            }

            private Vehicle m_Vehicle;

            public Vehicle Vehicle
            {
                get => m_Vehicle;
                set => m_Vehicle = value;
            }

            public Client(string i_OwnerName, string i_OwnerPhoneNumber, eVehicleStatus i_VehicleStatus, Vehicle i_Vehicle)
            {
                OwnerName = i_OwnerName;
                OwnerPhoneNumber = i_OwnerPhoneNumber;
                VehicleStatus = i_VehicleStatus;
                Vehicle = i_Vehicle;
            }

            public override string ToString()
            {
                return $@"Owner's name: {m_OwnerName}
Owner's phone number: {m_OwnerPhoneNumber}
Vehicle status: {m_VehicleStatus}
Vehicle type: {m_Vehicle}

";
            }
        }

        private Dictionary<string, Client> m_VehiclesDictionary;

        public Dictionary<string, Client> VehiclesDictionary
        {
            get => m_VehiclesDictionary;
            set => m_VehiclesDictionary = value;
        }

        public Garage()
        {
            m_VehiclesDictionary = new Dictionary<string, Client>();
        }

        public eVehicleStatus CheckFilterChoice(int i_NumberOfFilteringOptions, string i_UserChoiceString)
        {
            eVehicleStatus filterChoice;
            if (!Enum.TryParse(i_UserChoiceString, out filterChoice) || (int)filterChoice != i_NumberOfFilteringOptions)
            {
                if (!Enum.IsDefined(typeof(eVehicleStatus), filterChoice))
                {
                    throw new ArgumentException(k_InvalidEnumExceptionString);
                }
            }

            return filterChoice;
        }

        public eVehicleStatus CheckVehicleStatus(string i_UserChoiceString)
        {
            eVehicleStatus vehicleStatus;
            if (!Enum.TryParse(i_UserChoiceString, out vehicleStatus) && !Enum.IsDefined(typeof(eVehicleStatus), vehicleStatus))
            {
                    throw new ArgumentException(k_InvalidEnumExceptionString);
            }

            return vehicleStatus;
        }

        public bool CheckIfVehicleInGarageByLicenseNumber(string i_RequestedCarLicenseNumber)
        {
            return i_RequestedCarLicenseNumber != null && VehiclesDictionary.ContainsKey(i_RequestedCarLicenseNumber);
        }

        public void AddVehicle(string i_OwnerName, string i_OwnerPhoneNumber, List<string> i_VehicleParametersList)
        {
            const int k_VehicleTypeNameIndex = (int)VehicleFactory.eParametersInputOrder.VehicleTypeName;
            Vehicle vehicle = VehicleFactory.CreateVehicle(i_VehicleParametersList[k_VehicleTypeNameIndex], i_VehicleParametersList);
            Client client = new Client(i_OwnerName, i_OwnerPhoneNumber, eVehicleStatus.InRepair, vehicle);
            m_VehiclesDictionary.Add(vehicle.LicenseNumber, client);
        }

        public void InflateVehicleTiresToMax(string i_RequestedCarLicenseNumber)
        {
            Vehicle vehicle = VehiclesDictionary[i_RequestedCarLicenseNumber].Vehicle;
            foreach (Tire vehicleTire in vehicle.Tires)
            {
                vehicleTire.InflateTire(vehicleTire.MaxAirPressure - vehicleTire.CurrentAirPressure);
            }
        }

        public void HandleEnergySourceFilling(string i_RequestedCarLicenseNumber, params object[] i_ParametersToCheck)
        {
            const int k_EnergyAmountIndex = 0;
            float fuelAmountToAdd = (float)i_ParametersToCheck[k_EnergyAmountIndex];
            Vehicle vehicle = m_VehiclesDictionary[i_RequestedCarLicenseNumber].Vehicle;
            Engine engine = vehicle.Engine;
            Engine.eEngineType engineType = vehicle.Engine.EngineType;
            if (engineType == Engine.eEngineType.FuelEngine)
            {
                const int k_FuelTypeIndex = 1;
                FuelEngine.eFuelType fuelType = (FuelEngine.eFuelType)i_ParametersToCheck[k_FuelTypeIndex];
                engine.FillEnergySource(fuelAmountToAdd, fuelType);
            }
            else
            { // electric engine
                engine.FillEnergySource(fuelAmountToAdd);
            }

            updateFuelPercentage(vehicle);
        }

        private void updateFuelPercentage(Vehicle i_Vehicle)
        {
            i_Vehicle.EnergyPercentageLeft = i_Vehicle.Engine.GetEnergySourcePercentageLeft();
        }

        public List<string> ShowLicenseNumbersByFilter(eVehicleStatus i_FilterChoice, int i_NumberOfFilteringOptions)
        {
            List<string> filteredList = new List<string>();

            if(i_FilterChoice == (eVehicleStatus)i_NumberOfFilteringOptions)
            {
                foreach (KeyValuePair<string, Client> client in m_VehiclesDictionary)
                {
                        filteredList.Add(client.Key);
                }
            }
            else
            {
                foreach (KeyValuePair<string, Client> client in m_VehiclesDictionary)
                {
                    if ((int)client.Value.VehicleStatus == (int)i_FilterChoice) 
                    {
                        filteredList.Add(client.Key);
                    }
                }
            }

            return filteredList;
        }
    }
}
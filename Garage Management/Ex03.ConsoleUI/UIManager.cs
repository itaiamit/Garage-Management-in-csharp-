using System;
using System.Collections.Generic;
using System.Text;
using GarageLogic;

namespace ConsoleUI
{
    public class UIManager
    {
        private const int k_EmptyListIndicator = 0;

        private const string k_InvalidLicenseNumberExceptionString =
            "The vehicle with the above license number doesn't exist in the garage.";

        private const string k_LicenseNumber = "Enter the Vehicle's license number.";

        private const int k_MinIndex = 1;

        private const int k_MinValue = 0;

        internal Garage m_Garage;

        public UIManager()
        {
            this.m_Garage = new Garage();
        }

        private enum eUserChoices
        {
            EnterNewVehicle = 1,

            ShowLicenseNumbers = 2,

            ChangeVehicleStatus = 3,

            InflateTiresToMax = 4,

            FuelVehicle = 5,

            ChargeVehicle = 6,

            ShowFullDetails = 7,

            Exit = 8
        }

        public void Run()
        {
            bool quitGarageFlag = false;

            while(!quitGarageFlag)
            {
                try
                {
                    this.getUserChoiceFromMenu(ref quitGarageFlag);
                }
                catch(FormatException formatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(formatException.Message);
                }
                catch(ArgumentException argumentException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(argumentException.Message);
                }
                catch(ValueOutOfRangeException valueOutOfRangeException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(valueOutOfRangeException.Message);
                }
                catch(Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.Message);
                }
                finally
                {
                    Utilities.SleepAndClearScreen(true);
                }
            }
        }

        private static void printEnumList(Enum i_EnumType)
        {
            string[] enumNamesArray = Enum.GetNames(i_EnumType.GetType());
            byte enumIndex = 1;

            foreach(string name in enumNamesArray)
            {
                Console.WriteLine($"{enumIndex}. {name}");
                enumIndex++;
            }
        }

        private void addClientDetailsToGarage(string i_VehicleLicenseNumber)
        {
            string ownerName = this.getRegularParameterDetails(
                "Please insert owner's name.",
                ParameterChecker.eExpectedInputType.LettersOnly);
            string ownerPhoneNumber = this.getRegularParameterDetails(
                "Please insert owner's phone number.",
                ParameterChecker.eExpectedInputType.NumbersOnly);
            List<string> vehicleParameters = this.getVehicleParametersFromUser(i_VehicleLicenseNumber);
            this.m_Garage.AddVehicle(ownerName, ownerPhoneNumber, vehicleParameters);
        }

        private void changeVehicleStatus()
        {
            string requestedCarLicenseNumber =
                this.getRegularParameterDetails(k_LicenseNumber, ParameterChecker.eExpectedInputType.All);

            if(this.m_Garage.CheckIfVehicleInGarageByLicenseNumber(requestedCarLicenseNumber))
            {
                this.printVehicleStatusOrFilteringOptions(false, null);
                string vehicleStatusString = Console.ReadLine();
                eVehicleStatus vehicleStatus = this.m_Garage.CheckVehicleStatus(vehicleStatusString);
                this.m_Garage.VehiclesDictionary[requestedCarLicenseNumber].VehicleStatus = vehicleStatus;
                Console.WriteLine($@"Vehicle's status updated to ""{vehicleStatus}"".");
            }
            else
            {
                throw new ArgumentException(k_InvalidLicenseNumberExceptionString);
            }
        }

        private void enterNewVehicle()
        {
            string vehicleLicenseNumber =
                this.getRegularParameterDetails(k_LicenseNumber, ParameterChecker.eExpectedInputType.All);
            if(!this.m_Garage.CheckIfVehicleInGarageByLicenseNumber(vehicleLicenseNumber))
            {
                this.addClientDetailsToGarage(vehicleLicenseNumber);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Client's vehicle was added successfully!");
            }
            else
            {
                Console.WriteLine("Vehicle is already in the garage.");
                this.m_Garage.VehiclesDictionary[vehicleLicenseNumber].VehicleStatus = eVehicleStatus.InRepair;
            }
        }

        private void fillEnergySource(bool i_FuelRequest)
        {
            string requestedCarLicenseNumber =
                this.getRegularParameterDetails(k_LicenseNumber, ParameterChecker.eExpectedInputType.All);
            if(this.m_Garage.CheckIfVehicleInGarageByLicenseNumber(requestedCarLicenseNumber))
            {
                const string k_FuelString = "Please enter how much fuel (in liters) you want to add.";
                const string k_BatteryString = "Please enter how much battery time (in hours) you want to add.";
                Engine.eEngineType engineType = this.m_Garage.VehiclesDictionary[requestedCarLicenseNumber].Vehicle
                    .Engine.EngineType;
                bool isFuelEngine = engineType == Engine.eEngineType.FuelEngine;
                if(isFuelEngine != i_FuelRequest)
                {
                    throw new ArgumentException($"Wrong request to fill the {engineType}.");
                }

                Console.WriteLine(isFuelEngine ? k_FuelString : k_BatteryString);
                if(float.TryParse(Console.ReadLine(), out float energySourceAmountToAdd))
                {
                    if(engineType == Engine.eEngineType.FuelEngine)
                    {
                        FuelEngine.eFuelType fuelType = this.getFuelType();
                        this.m_Garage.HandleEnergySourceFilling(
                            requestedCarLicenseNumber,
                            energySourceAmountToAdd,
                            fuelType);
                    }
                    else
                    {
                        this.m_Garage.HandleEnergySourceFilling(requestedCarLicenseNumber, energySourceAmountToAdd);
                    }

                    Console.WriteLine(
                        energySourceAmountToAdd + (engineType == Engine.eEngineType.FuelEngine
                                                       ? " liters were added."
                                                       : " hours were charged."));
                }
            }
            else
            {
                throw new ArgumentException(k_InvalidLicenseNumberExceptionString);
            }
        }

        private void getExtraVehicleParameters(
            List<string> i_VehicleParameters,
            List<ParameterChecker> i_ExtraParameterInfo)
        {
            foreach(ParameterChecker parameterInfo in i_ExtraParameterInfo)
            {
                byte index = k_MinIndex;
                byte maxNumOfParameterValueOptions = (byte)parameterInfo.ParameterValues.Length;
                Console.WriteLine(parameterInfo.InputRequestString);
                if(maxNumOfParameterValueOptions != k_EmptyListIndicator)
                {
                    foreach(string parameterValue in parameterInfo.ParameterValues)
                    {
                        Console.WriteLine($"{index++}. {parameterValue}");
                    }
                }

                string parameterInputString = Console.ReadLine();
                parameterInfo.CheckParameterValidity(
                    maxNumOfParameterValueOptions,
                    parameterInfo,
                    parameterInputString);
                i_VehicleParameters.Add(parameterInputString);
            }
        }

        private FuelEngine.eFuelType getFuelType()
        {
            Console.WriteLine("Please enter fuel type.");
            printEnumList(new FuelEngine.eFuelType());
            FuelEngine.eFuelType fuelType = FuelEngine.CheckFuelType(Console.ReadLine());
            return fuelType;
        }

        private string getRangeParametersDetails(string i_DetailToAsk, float i_MinValue, float i_MaxValue)
        {
            Console.WriteLine(i_DetailToAsk);
            string detailToGet = Console.ReadLine();
            if(float.TryParse(detailToGet, out float detailResult))
            {
                ParameterChecker.CheckParameterInRange(i_MinValue, i_MaxValue, detailResult);
            }
            else
            {
                throw new FormatException("Invalid input (expected numbers only).");
            }

            return detailToGet;
        }

        private string getRegularParameterDetails(
            string i_DetailToAsk,
            ParameterChecker.eExpectedInputType i_ParametersTypeAllowed)
        {
            Console.WriteLine(i_DetailToAsk);
            string detailToGet = Console.ReadLine();
            ParameterChecker.CheckParameterTypeValidity(i_ParametersTypeAllowed, detailToGet);
            return detailToGet;
        }

        private void getUserChoiceFromMenu(ref bool io_QuitGarageFlag)
        {
            while(!io_QuitGarageFlag)
            {
                const bool k_FuelRequest = true;
                bool clearScreen = true;
                this.printUserMenu();
                if(Enum.TryParse(Console.ReadLine(), out eUserChoices userChoice)
                   && Enum.IsDefined(typeof(eUserChoices), userChoice))
                {
                    switch(userChoice)
                    {
                        case eUserChoices.EnterNewVehicle:
                            this.enterNewVehicle();
                            break;
                        case eUserChoices.ShowLicenseNumbers:
                            this.showVehiclesInGarageByLicensesNumber(ref clearScreen);
                            break;
                        case eUserChoices.ChangeVehicleStatus:
                            this.changeVehicleStatus();
                            break;
                        case eUserChoices.InflateTiresToMax:
                            this.inflateVehicleTiresToMax();
                            break;
                        case eUserChoices.FuelVehicle:
                            this.fillEnergySource(k_FuelRequest);
                            break;
                        case eUserChoices.ChargeVehicle:
                            this.fillEnergySource(!k_FuelRequest);
                            break;
                        case eUserChoices.ShowFullDetails:
                            this.showCarFullDetails();
                            clearScreen = false;
                            break;
                        case eUserChoices.Exit:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Good bye! ;) ");
                            io_QuitGarageFlag = true;
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    throw new ValueOutOfRangeException((float)eUserChoices.EnterNewVehicle, (float)eUserChoices.Exit);
                }

                Utilities.SleepAndClearScreen(clearScreen);
            }
        }

        private eVehicleStatus getUserFilterChoice(out int o_NumberOfFilteringOptions)
        {
            o_NumberOfFilteringOptions = Enum.GetNames(typeof(eVehicleStatus)).Length + 1;
            this.printVehicleStatusOrFilteringOptions(true, o_NumberOfFilteringOptions);
            string userChoiceString = Console.ReadLine();
            eVehicleStatus filterChoice = this.m_Garage.CheckFilterChoice(o_NumberOfFilteringOptions, userChoiceString);
            return filterChoice;
        }

        private List<string> getVehicleParametersFromUser(string i_VehicleLicenseNumber)
        {
            VehicleFactory.eVehicleType vehicleType = this.getVehicleType();
            List<string> vehicleParameters =
                new List<string>
                    {
                        vehicleType.ToString(),
                        this.getRegularParameterDetails(
                            @"Please insert the vehicle's model name.",
                            ParameterChecker.eExpectedInputType.All),
                        i_VehicleLicenseNumber,
                        this.getRangeParametersDetails(
                            @"Please insert how much energy \ fuel left (0 to 100%) in the vehicle.",
                            k_MinValue,
                            Utilities.k_MaxPercentage),
                        this.getRegularParameterDetails(
                            "Please type the tires' manufacture name.",
                            ParameterChecker.eExpectedInputType.All),
                        this.getRangeParametersDetails(
                            "Please enter the vehicle's current tire's air pressure.",
                            k_MinValue,
                            Vehicle.GetMaxAirPressure(vehicleType))
                    };
            List<ParameterChecker> extraParameterInfo = VehicleFactory.BuildExtraParametersInfo(vehicleType);

            this.getExtraVehicleParameters(vehicleParameters, extraParameterInfo);

            return vehicleParameters;
        }

        private VehicleFactory.eVehicleType getVehicleType()
        {
            Console.WriteLine("Please choose vehicle type.");
            printEnumList(new VehicleFactory.eVehicleType());
            string vehicleTypeString = Console.ReadLine();
            VehicleFactory.eVehicleType vehicleType = VehicleFactory.CheckVehicleType(vehicleTypeString);
            return vehicleType;
        }

        private void inflateVehicleTiresToMax()
        {
            string requestedCarLicenseNumber =
                this.getRegularParameterDetails(k_LicenseNumber, ParameterChecker.eExpectedInputType.All);

            if(this.m_Garage.CheckIfVehicleInGarageByLicenseNumber(requestedCarLicenseNumber))
            {
                this.m_Garage.InflateVehicleTiresToMax(requestedCarLicenseNumber);
                Console.WriteLine("Tires were inflated to maximum successfully.");
            }
            else
            {
                throw new ArgumentException(k_InvalidLicenseNumberExceptionString);
            }
        }

        private void printFilteredLicensesList(
            List<string> i_FilteredList,
            eVehicleStatus i_FilterChoice,
            ref bool io_ClearScreen)
        {
            bool noFilterChoice = i_FilterChoice == (eVehicleStatus)Enum.GetNames(typeof(eVehicleStatus)).Length + 1;
            if(i_FilteredList.Count == 0)
            {
                Console.WriteLine(
                    noFilterChoice
                        ? @"No vehicles in the garage."
                        : $@"No ""{i_FilterChoice}"" vehicles in the garage.");
            }
            else
            {
                io_ClearScreen = false;
                Console.WriteLine(
                    noFilterChoice
                        ? @"These are all the license numbers in the garage:"
                        : $@"The ""{i_FilterChoice}"" license numbers are:");
                StringBuilder licensesListBuilder = new StringBuilder();
                foreach(string license in i_FilteredList)
                {
                    licensesListBuilder.AppendLine(license);
                }

                Console.WriteLine(licensesListBuilder);
            }
        }

        private void printUserMenu()
        {
            Console.Write(
                @"Welcome to our garage!
----------------------
Please choose one of the following options:
1. Add a new vehicle to the garage.
2. Show All the license numbers.
3. Change a car status.
4. Inflate a vehicle's tires to maximum.
5. Fuel a vehicle.
6. Charge a vehicle's battery.
7. Show a vehicle's full information.
8. Exit.
----------------------
Your choice: ");
        }

        private void printVehicleStatusOrFilteringOptions(bool i_IsFilteringFlag, int? i_NumberOfFilteringOptions)
        {
            const string k_Unfiltered = "Show All vehicles.";
            Console.WriteLine(i_IsFilteringFlag ? "Filtering options:" : "New vehicle's status options");
            printEnumList(new eVehicleStatus());
            Console.WriteLine(i_IsFilteringFlag ? $"{i_NumberOfFilteringOptions}. {k_Unfiltered}" : string.Empty);
        }

        private void showCarFullDetails()
        {
            string requestedCarLicenseNumber =
                this.getRegularParameterDetails(k_LicenseNumber, ParameterChecker.eExpectedInputType.All);
            if(this.m_Garage.CheckIfVehicleInGarageByLicenseNumber(requestedCarLicenseNumber))
            {
                Console.WriteLine(this.m_Garage.VehiclesDictionary[requestedCarLicenseNumber]);
            }
            else
            {
                throw new ArgumentException(k_InvalidLicenseNumberExceptionString);
            }
        }

        private void showVehiclesInGarageByLicensesNumber(ref bool io_ClearScreen)
        {
            eVehicleStatus filterChoice = this.getUserFilterChoice(out int numberOfFilteringOptions);
            List<string> filteredList =
                this.m_Garage.ShowLicenseNumbersByFilter(filterChoice, numberOfFilteringOptions);
            this.printFilteredLicensesList(filteredList, filterChoice, ref io_ClearScreen);
        }
    }
}
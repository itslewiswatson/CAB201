using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Main entry point for MRRCManagement
    /// 
    /// Lewis Watson 2020
    /// </summary>
    class Program
    {
        private static FleetRepository fleetRepository { get; set; }
        private static CRMRepository crmRepository { get; set; }

        /// <summary>
        /// Handle graceful exit and make it less ungraceful
        /// </summary>
        /// <param name="sender">Who sent the argument</param>
        /// <param name="e">Object with arguments passed to event</param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                Program.fleetRepository.Flush();
                Program.crmRepository.Flush();
            }
            catch (Exception)
            {
                Console.WriteLine("Could not save data...");
            }
            finally
            {
                Console.WriteLine("Exiting unsafely...");
                Environment.Exit(0);
            }
        }

        static void Main(string[] args)
        {
            // Handle unsafe exit somewhat gracefully
            Console.CancelKeyPress += Console_CancelKeyPress;

            // Header/banner information
            Console.WriteLine("MATES RATES RENT-A-CAR");
            Console.WriteLine("Where you rent a car for cheap and don't even need to return it with a full tank");
            Console.WriteLine();
            Console.WriteLine("Press 'q' to quit and save. Press 'h' to go home to the main menu. Press 'p' to go to the parent menu.");
            Console.WriteLine();

            // Initial name prompt
            string name = "";
            do {
                if (name.Length == 1)
                {
                    Console.WriteLine("C'mon, your name surely has at least two letters...");
                }
                Console.WriteLine("Let's begin, what's your name?");
                Console.Write("$ ");
                name = Console.ReadLine().Trim();
                Console.WriteLine();
            } while (name == "" || name.Length == 1);

            Console.WriteLine("Welcome to MMRC, {0}", name);
            Console.WriteLine();

            // Parse given arguments
            string customerFile = null;
            string vehicleFile = null;
            string rentalFile = null;

            // Assign values
            if (args.Length > 0)
            {
                customerFile = args[0];
            }
            if (args.Length > 1)
            {
                vehicleFile = args[1];
            }
            if (args.Length > 2)
            {
                rentalFile = args[2];
            }

            // Let the user know if defaults are being used
            if (customerFile == null)
            {
                Console.WriteLine("Using default customer file in data directory...");
            }
            if (vehicleFile == null)
            {
                Console.WriteLine("Using default vehicle file in data directory...");
            }
            if (rentalFile == null)
            {
                Console.WriteLine("Using default rental file in data directory...");
            }
            Console.WriteLine();

            // Load in model data
            CRMRepository crmRepository = new CRMRepositoryFactory(customerFile).GetRepo();
            Program.crmRepository = crmRepository;
            FleetRepository fleetRepository = new FleetRepositoryFactory(vehicleFile, rentalFile).GetRepo();
            Program.fleetRepository = fleetRepository;

            // Validate files are readable
            try
            {
                crmRepository.entityParser.LoadAll();
                fleetRepository.vehicleEntityParser.LoadAll();
                fleetRepository.rentalEntityParser.LoadAll();
            }
            catch (Exception e)
            {
                // Close the program and let the user know what is wrong
                Console.WriteLine(e.Message);
                Console.WriteLine("Exiting the program now... Remove this arg to use the default directory instead.");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }

            // Input validators
            VehicleRegoValidator vehicleRegoValidator = new VehicleRegoValidator();
            CustomerIDValidator customerIDValidator = new CustomerIDValidator();
            NumSeatValidator numSeatValidator = new NumSeatValidator();
            GPSValidator gpsValidator = new GPSValidator();
            SunRoofValidator sunRoofValidator = new SunRoofValidator();
            DailyRateValidator dailyRateValidator = new DailyRateValidator();
            YearValidator yearValidator = new YearValidator();
            NumDayValidator numDayValidator = new NumDayValidator();

            // Handlers
            AddCustomerHandler addCustomerHandler = new AddCustomerHandler(crmRepository);
            EditCustomerHandler editCustomerHandler = new EditCustomerHandler(crmRepository, fleetRepository);
            DeleteCustomerHandler deleteCustomerHandler = new DeleteCustomerHandler(crmRepository, fleetRepository);
            AddVehicleHandler addVehicleHandler = new AddVehicleHandler(fleetRepository);
            EditVehicleHandler editVehicleHandler = new EditVehicleHandler(fleetRepository);
            DeleteVehicleHandler deleteVehicleHandler = new DeleteVehicleHandler(fleetRepository);
            RentVehicleHandler rentVehicleHandler = new RentVehicleHandler(fleetRepository, crmRepository);
            ReturnVehicleHandler returnVehicleHandler = new ReturnVehicleHandler(fleetRepository);
            SearchVehicleHandler searchVehicleHandler = new SearchVehicleHandler(fleetRepository);

            // Menus
            OptionMenu mainMenu = new OptionMenu(null, "Enter a number to select a function from the list below");

            // Customer Menus
            OptionMenu crmMenu = mainMenu.AddOptionMenu("Please select a number below");
            mainMenu.AddOption("Customer Management", crmMenu);

            // View all customers
            CustomerTable customerTable = (CustomerTable)crmMenu.AddTable(TableType.Customer, crmRepository);
            crmMenu.AddOption("View All Customers", customerTable);

            // Add customer
            InputMenu addCustomerMenu = crmMenu.AddInputMenu("AddCustomer", "Please fill out the following fields (* = required)");
            crmMenu.AddOption("Add Customer", addCustomerMenu);
            addCustomerMenu.AddInput("Title*");
            addCustomerMenu.AddInput("First Name*");
            addCustomerMenu.AddInput("Last Name*");
            addCustomerMenu.AddInput("Gender (male, female or other)*");
            addCustomerMenu.AddInput("DoB (dd/mm/yyyy)*");

            // Edit customer
            InputMenu editCustomerMenu = crmMenu.AddInputMenu("EditCustomer", "Please enter the ID of the customer to edit, followed by the " +
                "new data (leave blank to retain current information)");
            crmMenu.AddOption("Edit Customer", editCustomerMenu);
            editCustomerMenu.AddInput("Customer ID", customerIDValidator);
            editCustomerMenu.AddInput("New ID (leave blank for current)", customerIDValidator, true);
            editCustomerMenu.AddInput("Title");
            editCustomerMenu.AddInput("First Name");
            editCustomerMenu.AddInput("Last Name");
            editCustomerMenu.AddInput("Gender (male, female or other)");
            editCustomerMenu.AddInput("DoB (dd/mm/yyyy)");

            // Delete customer
            InputMenu deleteCustomerMenu = crmMenu.AddInputMenu("DeleteCustomer", "Please enter the ID of the customer you want to delete");
            crmMenu.AddOption("Delete Customer", deleteCustomerMenu);
            deleteCustomerMenu.AddInput("Customer ID", customerIDValidator);

            // Fleet management
            OptionMenu fleetMenu = mainMenu.AddOptionMenu("Please select a number below");
            mainMenu.AddOption("Fleet Management", fleetMenu);

            // View vehicles
            FleetTable fleetTable = (FleetTable)fleetMenu.AddTable(TableType.Fleet, fleetRepository);
            fleetMenu.AddOption("View Fleet", fleetTable);

            // Add vehicle
            InputMenu addVehicleMenu = fleetMenu.AddInputMenu("AddVehicle", "Please fill out the following fields (* = required)");
            fleetMenu.AddOption("Add Vehicle", addVehicleMenu);
            addVehicleMenu.AddInput("Registration*", vehicleRegoValidator);
            addVehicleMenu.AddInput("Grade*");
            addVehicleMenu.AddInput("Make*");
            addVehicleMenu.AddInput("Model*");
            addVehicleMenu.AddInput("Year*", yearValidator);
            addVehicleMenu.AddInput(string.Format("Num Seats ({0} - {1})", Vehicle.Min_Num_Seats, Vehicle.Max_Num_Seats), numSeatValidator, true);
            addVehicleMenu.AddInput("Transmission (automatic or manual)");
            addVehicleMenu.AddInput("Petrol (petrol or diesel)");
            addVehicleMenu.AddInput("GPS (yes or no)", gpsValidator, true);
            addVehicleMenu.AddInput("Sunroof (yes or no)", sunRoofValidator, true);
            addVehicleMenu.AddInput("Daily Rate (eg: 50, 78.65, etc)", dailyRateValidator, true);
            addVehicleMenu.AddInput("Colour");
            
            // Edit vehicle
            InputMenu editVehicleMenu = fleetMenu.AddInputMenu("EditVehicle", "Please enter the registration of the vehicle to edit, followed by " +
                "the new data (leave blank to retain current information)");
            fleetMenu.AddOption("Edit Vehicle", editVehicleMenu);
            editVehicleMenu.AddInput("Current Registration*", vehicleRegoValidator);
            editVehicleMenu.AddInput("New Registration (leave blank for current)", vehicleRegoValidator, true);
            editVehicleMenu.AddInput("Grade");
            editVehicleMenu.AddInput("Make");
            editVehicleMenu.AddInput("Model");
            editVehicleMenu.AddInput("Year", yearValidator, true);
            editVehicleMenu.AddInput("Num Seats (2 - 10)", numSeatValidator, true);
            editVehicleMenu.AddInput("Transmission (automatic or manual)");
            editVehicleMenu.AddInput("Petrol (petrol or diesel)");
            editVehicleMenu.AddInput("GPS (yes or no)", gpsValidator, true);
            editVehicleMenu.AddInput("Sunroof (yes or no)", sunRoofValidator, true);
            editVehicleMenu.AddInput("Daily Rate (eg: 50, 78.65, etc)", dailyRateValidator, true);
            editVehicleMenu.AddInput("Colour");

            // Delete vehicle
            InputMenu deleteVehicleMenu = fleetMenu.AddInputMenu("DeleteVehicle", "Please enter the registration of the vehicle you want to delete");
            fleetMenu.AddOption("Delete Vehicle", deleteVehicleMenu);
            deleteVehicleMenu.AddInput("Registration");

            // Rental menu
            OptionMenu rentalMenu = mainMenu.AddOptionMenu("Please select a number below");
            mainMenu.AddOption("Rental Management", rentalMenu);

            // View rentals
            RentalTable rentalTable = (RentalTable)rentalMenu.AddTable(TableType.Rental, fleetRepository);
            rentalMenu.AddOption("View Rentals", rentalTable);

            // Search
            InputMenu searchMenu = rentalMenu.AddInputMenu("SearchVehicle", "Please enter your search query");
            rentalMenu.AddOption("Search Vehicle", searchMenu);
            searchMenu.AddInput("Search*");

            // Rent vehicle
            InputMenu rentVehicleMenu = rentalMenu.AddInputMenu("RentVehicle", "Please enter the information of the vehicle and " +
                                                                "customer you want to rent to");
            rentalMenu.AddOption("Rent Vehicle", rentVehicleMenu);
            rentVehicleMenu.AddInput("Customer ID*", customerIDValidator);
            rentVehicleMenu.AddInput("Registration*", vehicleRegoValidator);
            rentVehicleMenu.AddInput("Number of days*", numDayValidator);

            // Return vehicle
            InputMenu returnVehicleMenu = rentalMenu.AddInputMenu("ReturnVehicle", "Enter the registration of the vehicle to return");
            rentalMenu.AddOption("Return Vehicle", returnVehicleMenu);
            returnVehicleMenu.AddInput("Registration*", vehicleRegoValidator);

            // Refresh tables for good measure
            customerTable.Refresh();
            fleetTable.Refresh();
            rentalTable.Refresh();

            // Set current menu to main menu
            BasicDisplayable currentDisplay = mainMenu;

            // Inital display and prompt separate from loop
            currentDisplay.Print();
            Console.Write("{0}@MMRC$ ", name);
            string action = Console.ReadLine();
            if (action == null)
            {
                return;
            }
            action = action.Trim();
            Console.WriteLine();

            do
            {
                // Handle worst case scenario of no menu being present
                if (currentDisplay == null)
                {
                    Console.WriteLine("Critical error - taking you back to the main menu");
                    currentDisplay = mainMenu;
                }

                // Handle core helper inputs first
                if (action.ToLower() == "q")
                {
                    Console.WriteLine("It is sad to see you go... But don't worry - everything is being saved as we speak!");
                    System.Threading.Thread.Sleep(1000); // Sleep to give the user a chance to read the message
                    crmRepository.Flush();
                    fleetRepository.Flush();
                    Environment.Exit(0);
                }
                if (action.ToLower() == "h")
                {
                    Console.WriteLine("Got it chief! Taking you back to the main menu.");
                    Console.WriteLine();
                    currentDisplay = mainMenu;
                }
                if (action.ToLower() == "p")
                {
                    Menu currentMenu = (Menu)currentDisplay;
                    if (currentMenu != null && currentMenu.parentMenu != null)
                    {
                        currentDisplay = currentMenu.parentMenu;
                    }
                }

                // Display option menu
                if (currentDisplay.GetType() == typeof(OptionMenu))
                {
                    OptionMenu currentMenu = (OptionMenu)currentDisplay;
                    InputValidator optionSelectionValidator = new OptionSelectionValidator(currentMenu.items);

                    // Try to parse selected option and navigate to child menu
                    try
                    {
                        optionSelectionValidator.Validate(action);

                        int selectedValue = int.Parse(action) - 1; // Account for alternate indexing

                        Option selectedOption = currentMenu.items.ElementAt(selectedValue);
                        currentDisplay = selectedOption.childMenu;
                    }
                    catch (Exception e)
                    {
                        // Don't input an error message for action keys or a blank input
                        if (action != "" && action.ToLower() != "q" && action.ToLower() != "h" && action.ToLower() != "p")
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine();
                        }
                    }

                    // Input menus usually result from option menus
                    // Attempt to catch errors as input is passed to field validators
                    try
                    {
                        currentDisplay.Print();
                    }
                    catch (Exception e)
                    {
                        // Write friendly message
                        Console.WriteLine();
                        Console.WriteLine(e.Message);
                        Console.WriteLine();

                        // Take them back to the main menu and let them know where they are at
                        currentDisplay = mainMenu;
                        currentDisplay.Print();
                    }
                }

                // Display input menu
                if (currentDisplay.GetType() == typeof(InputMenu))
                {
                    InputMenu currentMenu = (InputMenu)currentDisplay;
                    List<Input> items = currentMenu.items;
                    List<string> answers = currentMenu.answers;

                    // Match current input menu to the appropriate handler
                    switch (currentMenu.id)
                    {
                        case "AddCustomer":
                            addCustomerHandler.Handle(items, answers);
                            break;
                        case "EditCustomer":
                            editCustomerHandler.Handle(items, answers);
                            break;
                        case "DeleteCustomer":
                            deleteCustomerHandler.Handle(items, answers);
                            break;
                        case "AddVehicle":
                            addVehicleHandler.Handle(items, answers);
                            break;
                        case "EditVehicle":
                            editVehicleHandler.Handle(items, answers);
                            break;
                        case "DeleteVehicle":
                            deleteVehicleHandler.Handle(items, answers);
                            break;
                        case "RentVehicle":
                            rentVehicleHandler.Handle(items, answers);
                            break;
                        case "ReturnVehicle":
                            returnVehicleHandler.Handle(items, answers);
                            break;
                        case "SearchVehicle":
                            searchVehicleHandler.Handle(items, answers);
                            break;
                        default:
                            Console.WriteLine("Something has gone wrong.");
                            Console.WriteLine();
                            break;
                    }

                    currentDisplay = currentMenu.parentMenu;
                    currentDisplay.Print();
                }

                // Display table
                if (currentDisplay.GetType().IsSubclassOf(typeof(Table)))
                {
                    Table currentTable = (Table)currentDisplay;

                    currentDisplay = currentTable.parentMenu;
                    currentDisplay.Print();
                }

                // Prompt for next set of input
                Console.Write("{0}@MMRC$ ", name);
                action = Console.ReadLine();
                if (action == null)
                {
                    return;
                }
                action = action.Trim();
                Console.WriteLine();

                // Refresh the tables in case an action occurs during this iteration
                customerTable.Refresh();
                fleetTable.Refresh();
                rentalTable.Refresh();

            } while (action != null);
        }
    }
}

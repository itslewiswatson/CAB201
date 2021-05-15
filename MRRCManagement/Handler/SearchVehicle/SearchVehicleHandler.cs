using MRRC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MRRCManagement
{
    /// <summary>
    /// Handling search functionality of rentals
    /// Lewis Watson & Shlomo 2020
    /// </summary>
    public class SearchVehicleHandler : RentalHandler
    {
        private SortedList attributeSets { get; set; }

        public SearchVehicleHandler(FleetRepository repository) : base(repository) { }

        /// <summary>
        /// Parses search query and performs necessary formatting for validation
        /// Shlomo 2020
        /// </summary>
        /// <param name="userQuery"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private string getQuery(string userQuery, out ArrayList query)
        {
            // accept user query and do some validation
            // note that some non-sensical queries can still pass validation
            // for instance,  AND RED (( OR ) BLUE)
            // but these will be caught later 

            query = new ArrayList();
            string queryText = "";
            if (userQuery.Length > 0)
            {
                // separate parenthesis before splitting string
                for (int i = 0; i < userQuery.Length; i++)
                {
                    if (userQuery[i].Equals('(') || userQuery[i].Equals(')'))
                    {
                        queryText += " ";
                        queryText += userQuery[i];
                        queryText += " ";
                    }
                    else if (userQuery[i].Equals('"'))
                    {
                        // Hijack iteration to cater for spaced names
                        int n = i + 1;
                        do
                        {
                            // Catch errors
                            if (n >= userQuery.Length)
                            {
                                throw new FormatException("You must close quotation marks in your search. Like \"this\".");
                            }

                            if (userQuery[n].ToString() == " ")
                            {
                                // Add _ instead of spaces if within the bounds of " and "
                                queryText += "_";
                                n++;
                            }
                            else
                            {
                                queryText += userQuery[n];
                                n++;
                            }

                            // Catch errors
                            if (n >= userQuery.Length)
                            {
                                throw new FormatException("You must close quotation marks in your search. Like \"this\".");
                            }
                        } while (!userQuery[n].Equals('"'));
                        i = n;
                    }
                    else
                    {
                        queryText += userQuery[i];
                    }
                }

                queryText = queryText.ToUpper();
                query.AddRange(queryText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)); // split to tokens (delimiter is space)
            }
            else
            {
                throw new Exception("You provided an incorrect input. Please enter a search parameter like: 'Toyota AND Blue'");
            }
            return userQuery;
        }

        /// <summary>
        /// Search using specified algorithm
        /// </summary>
        /// <param name="searchAlgorithm">Search algorithm instance to use</param>
        /// <returns></returns>
        private string[] search(SearchAlgorithm searchAlgorithm)
        {
            searchFleet(out string[] result, searchAlgorithm);

            if (result.Length == 0)
            {
                throw new Exception("No matches found for this search query. Perhaps make your query less specific.");
            }

            return result;
        }

        /// <summary>
        /// Final part of the shunting yard algorithm
        /// Pop and push elements to evaluate the search expression
        /// Shlomo 2020
        /// </summary>
        /// <param name="result">Array of vehicle registrations that match search</param>
        /// <param name="searchAlgorithm">Search algorithm instance to use</param>
        public void searchFleet(out string[] result, SearchAlgorithm searchAlgorithm)
        {
            // Create and instantiate a new empty Stack for result sets.
            Stack setStack = new Stack();
            HashSet<string> hs1;
            HashSet<string> hs2;
            HashSet<string> hs;
            int idx;
            String[] temp = new string[] { };
            for (int i = 0; i < searchAlgorithm.Postfix.Count; i++)
            {
                if (searchAlgorithm.Postfix[i].Equals("AND"))
                {
                    // pop two sets off the stack and apply Intersect, push back result
                    hs1 = (HashSet<string>)setStack.Pop();
                    hs2 = (HashSet<string>)setStack.Pop();
                    temp = hs1.ToArray<string>(); // copy the elements of the set hs1
                    hs = new HashSet<string>(temp); // make a deep copy of hs1
                    hs.IntersectWith(hs2);// apply the Intersect to the new set
                    setStack.Push(hs); // push a reference to a new set
                }
                else if (searchAlgorithm.Postfix[i].Equals("OR"))
                {
                    // pop two sets off the stack and apply Union
                    hs1 = (HashSet<string>)setStack.Pop();
                    hs2 = (HashSet<string>)setStack.Pop();
                    temp = hs1.ToArray<string>(); // copy the elements of the set hs1
                    hs = new HashSet<string>(temp); // make a deep copy of hs1
                    hs.UnionWith(hs2); // apply the Union to the new set
                    setStack.Push(hs); // push a reference to a new set
                }
                else
                {
                    // here if an operand
                    idx = attributeSets.IndexOfKey(searchAlgorithm.Postfix[i]); // identify attribute set
                    if (idx >= 0)
                    {
                        hs1 = (HashSet<string>)attributeSets.GetByIndex(idx);
                        setStack.Push(hs1); // note: pushing a reference, not the actual set
                    }
                    else
                    {
                        throw new FormatException(string.Format("Invalid attribute {0}. Use attributes that exist on a vehicle.", 
                                                    searchAlgorithm.Postfix[i]));
                    }
                }
            }
            if (setStack.Count == 1)
            {
                //hs1 = (HashSet<string>)attributeSets.GetByIndex(1);
                hs1 = (HashSet<string>)setStack.Pop();
                result = hs1.ToList().ToArray();
            }
            else
            {
                throw new Exception("Invalid search query provided. Did you add an extra parenthesis?");
            }
        }

        /// <summary>
        /// Add vehicle and its attribute to the sets of attributes for search use
        /// Shlomo 2020
        /// </summary>
        /// <param name="vehicle">Vehicle to pass attributes of</param>
        private void InsertVehicle(Vehicle vehicle)
        {
            int idx;
            HashSet<string> hs;

            // Rego
            idx = attributeSets.IndexOfKey(vehicle.vehicleRego.ToUpper());
            if (idx >= 0)
            {   // here if rego set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Grade
            idx = attributeSets.IndexOfKey(vehicle.vehicleGrade.ToString().ToUpper());
            if (idx >= 0)
            {   // here if grade set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Make
            idx = attributeSets.IndexOfKey(vehicle.make.ToUpper());
            if (idx >= 0)
            {   // here if make set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Model
            idx = attributeSets.IndexOfKey(vehicle.model.ToUpper());
            if (idx >= 0)
            {   // here if model set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
            
            // Year
            idx = attributeSets.IndexOfKey(vehicle.year.ToString());
            if (idx >= 0)
            {   // here if year set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Num Seats
            idx = attributeSets.IndexOfKey(string.Format("{0}-SEATER", vehicle.numSeats.ToString()));
            if (idx >= 0)
            {   // here if num seats set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Transmission
            idx = attributeSets.IndexOfKey(vehicle.transmission.ToString().ToUpper());
            if (idx >= 0)
            {   // here if transmission set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Fuel
            idx = attributeSets.IndexOfKey(vehicle.fuel.ToString().ToUpper());
            if (idx >= 0)
            {   // here if fuel set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // GPS
            idx = attributeSets.IndexOfKey("GPS");
            if (idx >= 0 && vehicle.GPS)
            {   // here if gps set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Sunroof
            idx = attributeSets.IndexOfKey("SUNROOF");
            if (idx >= 0 && vehicle.sunRoof)
            {   // here if sunroof set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego);// add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }

            // Colour
            idx = attributeSets.IndexOfKey(vehicle.colour.ToUpper());
            if (idx >= 0)
            {   // here if colour set found
                hs = (HashSet<string>)attributeSets.GetByIndex(idx); // get set
                hs.Add(vehicle.vehicleRego); // add to set
                attributeSets.SetByIndex(idx, hs);  // save set (replaces old set)
            }
        }

        /// <summary>
        /// Search vehicles to rent from user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            // Setup variables
            string text = args[0];
            Fleet fleet = repository.Get();
            ArrayList attributes = fleet.GetAttributes();

            // Set up attribute list
            attributeSets = new SortedList();
            foreach (string attribute in attributes)
            {
                attributeSets.Add(attribute.ToUpper(), new HashSet<string>());
            }

            // Add vehicles to attribute list
            foreach (Vehicle vehicle in fleet.vehicles)
            {
                if (!fleet.IsVehicleRented(vehicle.vehicleRego))
                {
                    InsertVehicle(vehicle);
                }
            }

            // Perform lookup
            getQuery(text, out ArrayList query);
            string[] results = search(new SearchAlgorithm(query, attributes));

            // Filter results again by only those that are rentable
            List<Vehicle> resultVehicles = new List<Vehicle>();
            foreach (string vehicleRego in results)
            {
                Vehicle lookUp = fleet.vehicles.Find((vehicle) => vehicle.vehicleRego == vehicleRego);
                if (lookUp != null)
                {
                    resultVehicles.Add(lookUp);
                }
            }

            // Construct new to show to user
            SearchResultTable searchResultTable = new SearchResultTable(null, null, resultVehicles);
            searchResultTable.Refresh();
            searchResultTable.Print();
        }
    }
}

//Author: David Barnes
//CIS 237
//Assignment 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1
{
    class UserInterface
    {
        const int maxMenuChoice = 6;
        BeverageDHattenEntities beverageEntities = new BeverageDHattenEntities();

        //---------------------------------------------------
        //Public Methods
        //---------------------------------------------------

        //Display Welcome Greeting
        public void DisplayWelcomeGreeting()
        {
            Console.WriteLine("Welcome to the wine program");
        }

        //Display Menu And Get Response
        public int DisplayMenuAndGetResponse()
        {
            //declare variable to hold the selection
            string selection;

            //Display menu, and prompt
            this.displayMenu();
            this.displayPrompt();

            //Get the selection they enter
            selection = this.getSelection();

            //While the response is not valid
            while (!this.verifySelectionIsValid(selection))
            {
                //display error message
                this.displayErrorMessage();

                //display the prompt again
                this.displayPrompt();

                //get the selection again
                selection = this.getSelection();
            }
            //Return the selection casted to an integer
            return Int32.Parse(selection);
        }

        //Get the search query from the user
        public string GetSearchQuery()
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to search for?");
            Console.Write("> ");
            return Console.ReadLine();
        }

        //Get New Item Information From The User.
        public Beverage GetNewItemInformation()
        {
            Console.WriteLine();
            string id = ID();
            Console.WriteLine("What is the new items Description?");
            Console.Write("> ");
            string description = Console.ReadLine();
            Console.WriteLine("What is the new items Pack?");
            Console.Write("> ");
            string pack = Console.ReadLine();
            decimal priceToReturn = parsePrice();            
            bool active = IsActive();
            Beverage beverageToReturn = new Beverage();
            beverageToReturn.id = id;
            beverageToReturn.name = description;
            beverageToReturn.pack = pack;
            beverageToReturn.price = priceToReturn;
            beverageToReturn.active = active;
            return beverageToReturn;
        }
        /// <summary>
        /// This method prompts the user for an id for a new product then makes sure this id does not already
        /// exist in the database before returning it.
        /// </summary>
        /// <returns></returns>
        private string ID()
        {
            string id = "";
            Console.WriteLine("What is the new items Id?");
            Console.Write("> ");
            id = Console.ReadLine();
            var idSearch = beverageEntities.Beverages.Where(beverage => beverage.id.Trim().Contains(id));
            List<Beverage> searchResults = idSearch.ToList();
            if (searchResults.Count == 0)
            {
                return id;
            }
            else
            {
                Console.WriteLine("Error, beverage with that ID already exists");
                ID();
                return id;
            }
        }
        //Set the IsActive flag
        private bool IsActive()
        {
            //Prompt the user to know if the beverage is active
            Console.WriteLine("Is the beverage active?");
            Console.WriteLine("Y or N");
            //Read the user input
            string input = Console.ReadLine().ToLower();
            bool active = false; 
            //Parse the input
            switch (input)
            {
                case "y":
                    active = true;
                    break;
                case "n":
                    active = false;
                    break;
                default:
                    Console.WriteLine("That is not a valid selection, please enter Y or N");
                    IsActive();
                    break;
            }
            return active;
        }

        //Display All Items
        public void DisplayAllItems(BeverageDHattenEntities beverageEntities)
        {
            foreach (Beverage beverage in beverageEntities.Beverages)
            {
                Console.WriteLine(beverage.id.PadRight(7) + beverage.price.ToString("C").PadRight(8)
                    + beverage.name.Trim().PadRight(49) + beverage.pack.Trim().PadLeft(15));
            }
        }

        //Display Item Found Success
        public void DisplayItemFound(List<Beverage> itemInformation)
        {
            Console.WriteLine();
            Console.WriteLine("Item Found!");
            foreach (Beverage beverage in itemInformation)
            {
                Console.WriteLine(beverage.id.PadRight(7) + beverage.price.ToString("C").PadRight(8)
                    + beverage.name.Trim().PadRight(49) + beverage.pack.Trim().PadLeft(15));
            }
        }

        //Display Item Found Error
        public void DisplayItemFoundError()
        {
            Console.WriteLine();
            Console.WriteLine("A Match was not found");
        }

        public void UpdateItem()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the ID of the entry you'd like to edit");
            string id = Console.ReadLine();
            var queryResultsVar = beverageEntities.Beverages.Where(beverage => beverage.id.Trim().Contains(id));
            List<Beverage> queryResults = queryResultsVar.ToList();
            if (queryResults.Count == 1)
            {
                foreach(Beverage beverage in queryResults)
                {
                    Console.WriteLine(beverage.id.PadRight(7) + beverage.price.ToString("C").PadRight(8)
                                    + beverage.name.Trim().PadRight(49) + beverage.pack.Trim().PadLeft(15));
                }
                Beverage updatedBeverage = updateItem(queryResults.First());
                try
                {
                    beverageEntities.Beverages.Add(updatedBeverage);
                    beverageEntities.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    beverageEntities.Beverages.Remove(updatedBeverage);
                    Console.WriteLine("Can't add the record - invalid information");
                }
            }
            else if(queryResults.Count < 1)
            {
                Console.WriteLine("Error, no item with that ID found");
                UpdateItem();
            }
            else if(queryResults.Count > 1)
            {
                Console.WriteLine("Error, multiple items found, please narrow search results");
                UpdateItem();
            }
        }

        public void DeleteItem()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the ID of the beverage you'd like to remove");
            string id = Console.ReadLine();
            var queryResultsVar = beverageEntities.Beverages.Where(beverage => beverage.id.Trim().Contains(id));
            List<Beverage> queryResults = queryResultsVar.ToList();
            Beverage beverageToRemove = queryResults.First();
            if (queryResults.Count == 1)
            {
                Console.WriteLine("You have selected the following item to remove:");
                foreach (Beverage beverage in queryResults)
                {
                    Console.WriteLine(beverage.id.PadRight(7) + beverage.price.ToString("C").PadRight(8)
                                    + beverage.name.Trim().PadRight(49) + beverage.pack.Trim().PadLeft(15));
                }
                Console.WriteLine("Are you sure you want to remove this item?");
                Console.WriteLine("Y or N");
                string confirm = Console.ReadLine();
                switch (confirm.ToLower())
                {
                    case "y":
                        try
                        {
                            beverageEntities.Beverages.Remove(beverageToRemove);
                            beverageEntities.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        break;
                    case "n":
                        break;
                    default:
                        Console.WriteLine("That is not a valid selection, please enter Y or N");
                        IsActive();
                        break;
                }
            }
            else if (queryResults.Count < 1)
            {
                Console.WriteLine("Error, no item with that ID found");
                UpdateItem();
            }
            else if (queryResults.Count > 1)
            {
                Console.WriteLine("Error, multiple items found, please narrow search results");
                UpdateItem();
            }
        }

        //---------------------------------------------------
        //Private Methods
        //---------------------------------------------------

        private Beverage updateItem(Beverage beverage)
        {
            Console.WriteLine();
            Console.WriteLine("Enter a new Name for the item");
            Console.Write("> ");
            beverage.name = Console.ReadLine();
            Console.WriteLine("Enter a new Pack for the item");
            Console.Write("> ");
            beverage.pack = Console.ReadLine();
            beverage.price = parsePrice();
            beverage.active = IsActive();
            return beverage;
        }
        //Display the Menu
        private void displayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine();
            Console.WriteLine("1. Print The Entire List Of Items");
            Console.WriteLine("2. Search For An Item by ID");
            Console.WriteLine("3. Add New Item To The List");
            Console.WriteLine("4. Update an Existing Item");
            Console.WriteLine("5. Delete an Existing Item");
            Console.WriteLine("6. Exit Program");
        }

        //Display the Prompt
        private void displayPrompt()
        {
            Console.WriteLine();
            Console.Write("Enter Your Choice: ");
        }

        //Display the Error Message
        private void displayErrorMessage()
        {
            Console.WriteLine();
            Console.WriteLine("That is not a valid option. Please make a valid choice");
        }

        //Get the selection from the user
        private string getSelection()
        {
            return Console.ReadLine();
        }

        //Verify that a selection from the main menu is valid
        private bool verifySelectionIsValid(string selection)
        {
            //Declare a returnValue and set it to false
            bool returnValue = false;

            try
            {
                //Parse the selection into a choice variable
                int choice = Int32.Parse(selection);

                //If the choice is between 0 and the maxMenuChoice
                if (choice > 0 && choice <= maxMenuChoice)
                {
                    //set the return value to true
                    returnValue = true;
                }
            }
            //If the selection is not a valid number, this exception will be thrown
            catch (Exception e)
            {
                //set return value to false even though it should already be false
                returnValue = false;
            }

            //Return the reutrnValue
            return returnValue;
        }
        /// <summary>
        /// This method displays the prompt for a price for the beverage to add
        /// and then parses it to a decimal to return
        /// </summary>
        /// <returns></returns>
        private decimal parsePrice()
        {
            Console.WriteLine("What is the Price?");
            Console.Write("> ");
            string priceString = Console.ReadLine();
            decimal price = 0m;
            try
            {
                price = decimal.Parse(priceString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error, you must enter a valid price in USD");
                parsePrice();
            }
            return price;
        }
    }
}

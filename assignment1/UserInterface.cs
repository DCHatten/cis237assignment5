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
        public string[] GetNewItemInformation()
        {
            Console.WriteLine();
            Console.WriteLine("What is the new items Id?");
            Console.Write("> ");
            string id = Console.ReadLine();
            Console.WriteLine("What is the new items Description?");
            Console.Write("> ");
            string description = Console.ReadLine();
            Console.WriteLine("What is the new items Pack?");
            Console.Write("> ");
            string pack = Console.ReadLine();
            Console.WriteLine("What is the new items Price?");
            Console.Write("> ");
            string price = Console.ReadLine();
            Console.WriteLine("Is the new item active?");
            Console.Write("Y or N");
            string active = IsActive();
            return new string[] { id, description, pack, price, active };
        }

        private string IsActive()
        {
            Console.WriteLine("Is the new item active?");
            Console.Write("Y or N");
            string input = Console.ReadLine().ToLower();
            string active = "";
            switch (input)
            {
                case "y":
                    active = "true";
                    break;
                case "n":
                    active = "false";
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

        //Display All Items Error
        public void DisplayAllItemsError()
        {
            Console.WriteLine();
            Console.WriteLine("There are no items in the list to print");
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

        //Display Add Wine Item Success
        public void DisplayAddWineItemSuccess()
        {
            Console.WriteLine();
            Console.WriteLine("The Item was successfully added");
        }

        //Display Item Already Exists Error
        public void DisplayItemAlreadyExistsError()
        {
            Console.WriteLine();
            Console.WriteLine("An Item With That Id Already Exists");
        }


        //---------------------------------------------------
        //Private Methods
        //---------------------------------------------------

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
    }
}

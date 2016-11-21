//Author: David Hatten
//CIS 237
//Assignment 5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set a constant for the size of the collection
            BeverageDHattenEntities beverageEntities = new BeverageDHattenEntities();

            //Create an instance of the UserInterface class
            UserInterface userInterface = new UserInterface();

            //Display the Welcome Message to the user
            userInterface.DisplayWelcomeGreeting();

            //Display the Menu and get the response. Store the response in the choice integer
            //This is the 'primer' run of displaying and getting.
            int choice = userInterface.DisplayMenuAndGetResponse();

            while (choice != 6)
            {
                switch (choice)
                {
                    case 1:
                        //Print Entire List Of Items
                        userInterface.DisplayAllItems(beverageEntities);
                        break;

                    case 2:
                        //Search For An Item
                        string query = userInterface.GetSearchQuery();
                        var queryResultsVar = beverageEntities.Beverages.Where(beverage => beverage.id.Trim().Contains(query));
                        List<Beverage> queryResults = queryResultsVar.ToList();
                        if (queryResults != null)
                        {
                            userInterface.DisplayItemFound(queryResults);
                        }
                        else
                        {
                            userInterface.DisplayItemFoundError();
                        }
                        break;

                    case 3:
                        //Add A New Item To The List
                        string[] newItemInformation = userInterface.GetNewItemInformation();
                        
                        break;
                    case 4:
                        //Update an existing item

                        break;

                    case 5:
                        //Delete an existing item

                        break;
                }

                //Get the new choice of what to do from the user
                choice = userInterface.DisplayMenuAndGetResponse();
            }

        }
    }
}

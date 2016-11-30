using System;
using ServiceNow;
using ServiceNow.Interface;
using ServiceNow.TableAPI;
using ServiceNow.TableDefinitions;

using System.Linq;
using System.Collections.Generic;

namespace UsageDemo
{
    class Program
    {
        static string myInstance = "testulare";
        static string instanceUserName = "ApiUserName";
        static string instancePassword = "ApiUserPass";

        static void Main(string[] args)
        {
            // Shows basic CRUD operations with a simple small subset of a record
            basicOperations();

            // Demonstrates a more advanced retrieval including related resource by link and dot traversal.
            retrieveByQuery();

            // Break
            Console.WriteLine("\n\rCompleted.");
            Console.ReadLine();
        }

        static void basicOperations()
        {
            TableAPIClient<sys_user> userClient = new TableAPIClient<sys_user>("sys_user", myInstance, instanceUserName, instancePassword);

            // Deomstrate the Create (POST) operation:
            var createdUser = userClient.Post(new sys_user()
            {
                employee_number = "012345",
                first_name = "Tester",
                last_name = "McTester",
                email = "tester@testcompany.com",
                phone = "",
                // You can use Name instead of sys_id, but if service now does not find your value it will ignore it without any warning.
                location = new ResourceLink() { value = "VISALIA COURTHOUSE" }  
            });
            Console.WriteLine("User Created: " + createdUser.Result.first_name + " " + createdUser.Result.last_name + " (" + createdUser.Result.sys_id + ")");


            // Deonstrate the GetById (GET) operation:
            var retrievedUser = userClient.GetById(createdUser.Result.sys_id);
            Console.WriteLine("User Retrieved: " + retrievedUser.Result.first_name + " " + retrievedUser.Result.last_name + " (" + retrievedUser.Result.sys_id + ")");
            Console.WriteLine("              : eMail: " + retrievedUser.Result.email);
            Console.WriteLine("              : Location: " + retrievedUser.Result.Location_name);


            // Demonstrate Update (PUT) operation:
            Console.WriteLine("\n\nUpdating User");
            if (retrievedUser.Result != null)
            {
                var d = retrievedUser.Result;
                d.email = "newEmail@testcompany.com";

                // Set the location using the Guid of a good location, otherwise handle it.
                try
                {                    
                    d.location = new ResourceLink() { value = findLocationId("VISALIA DISTRICT OFFICE") };
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Unable to set new user location: " + ex.Message);
                }

                var updatedUser = userClient.Put(d);
                Console.WriteLine("              : eMail: " + updatedUser.Result.email);
                Console.WriteLine("              : Location: " + updatedUser.Result.Location_name);
            }


            // Domonstrate Delete operation
            Console.Write("\n\nDeleting User");            
            userClient.Delete(retrievedUser.Result.sys_id);
            Console.WriteLine("...Done");
        }
        

        static void retrieveByQuery()
        {
            Console.WriteLine("\n\nRetrieving active, unresolved incidents");
            var query = @"active=true^u_resolved=false";
            TableAPIClient<incident> client = new TableAPIClient<incident>("incident", myInstance, instanceUserName, instancePassword);

            IRestQueryResponse<incident> response = client.GetByQuery(query);

            Console.WriteLine(response.ResultCount + " records found. \n\nPress return to list results.");
            Console.ReadLine();
            foreach (incident r in response.Result)
            {
                DateTime openedOn = DateTime.Parse(r.opened_at);
                ResourceLink openedFor = r.caller_id;

                Console.WriteLine(r.number + " :  " + r.short_description + " (Opened " + openedOn.ToShortDateString() + " for " + r.caller_first_name + ")");
            }
        }


        // You would of course have these cached somewhere most likely.
        static string findLocationId(string locationName)
        {
            TableAPIClient<location> locationClient = new TableAPIClient<location>("cmn_location", myInstance, instanceUserName, instancePassword);
            var query = @"name=" + locationName;

            IRestQueryResponse<location> locationResult = locationClient.GetByQuery(query);
            if (locationResult.ResultCount == 0) throw new Exception(String.Format("No location by the name {0} was found.", locationName));
            if (locationResult.ResultCount > 1) throw new Exception(String.Format("Multiple locations found by the name {0}.", locationName));

            // We found our location lets return it
            return locationResult.Result.First().sys_id;
        }
    }
}

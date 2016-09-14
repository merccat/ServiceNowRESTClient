using System;
using ServiceNow;
using ServiceNow.Interface;
using ServiceNow.TableAPI;
using ServiceNow.TableDefinitions;

namespace UsageDemo
{
    class Program
    {
        static string myInstance = "instanceName";
        static string instanceUserName = "user";
        static string instancePassword = "pass";

        static void Main(string[] args)
        {
            // Shows basic CRUD operations with a simple small subset of a record
            //basicOperations();

            // Demonstrates a more advanced retrieval including related resource by link and dot traversal.
            retrieveByQuery();

            // Break
            Console.WriteLine("\n\rCompleted.");
            Console.ReadLine();
        }

        static void basicOperations()
        {
            TableAPIClient<sys_user> client = new TableAPIClient<sys_user>("sys_user", myInstance, instanceUserName, instancePassword);

            // Create a user
            var createdUser = client.Post(new sys_user()
            {
                employee_number = "012345",
                first_name = "Tester",
                last_name = "McTester",
                email = "tester@testcompany.com",
                phone = ""
            });
            Console.WriteLine("User Created: " + createdUser.Result.first_name + " " + createdUser.Result.last_name + " (" + createdUser.Result.sys_id + ")");


            // Retrieve the user (even though we already have it
            var retrievedUser = client.GetById(createdUser.Result.sys_id);
            Console.WriteLine("User Retrieved: " + retrievedUser.Result.first_name + " " + retrievedUser.Result.last_name + " (" + retrievedUser.Result.sys_id + ")");


            // Update the User
            if (retrievedUser.Result != null)
            {
                var d = retrievedUser.Result;
                d.email = "newEmail@testcompany.com";

                var updatedUser = client.Put(d);
                Console.WriteLine("Updated email: " + updatedUser.Result.email);
            }
            

            // Delete the user
            client.Delete(retrievedUser.Result.sys_id);
        }

        static void retrieveByQuery()
        {
            var query = "active=true^u_resolved=false";
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
    }
}

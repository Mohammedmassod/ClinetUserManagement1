﻿using System;
using Microsoft.OData.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ClinetUser;

namespace ClinetContact
{
    public class Program
    {
       // private const string ApiKey = "6D5D1ABA-4F78-4DD3-A69D-C2D15F2E259A,709C95E7-F59D-4CC4-9638-4CDE30B2FCFD"; // قم بتغيير القيمة لتكون الـ API Key الخاص بك

        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            bool continueExecution = true;

            while (continueExecution)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Delete");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Retrieve");
                Console.WriteLine("5. RetrieveAllContacts");
                Console.WriteLine("6. Exit");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        await AddUser();
                        break;
                    case 2:
                        await DeleteContact();
                        break;
                    case 3:
                        await UpdateContact();
                        break;
                    case 4:
                        await RetrieveUser();
                        break;
                    case 5:
                        await RetrieveAllUsers();
                        break;
                    case 6:
                        continueExecution = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        //دالة التحقق من المفتاح
       /* private static bool CheckApiKey(string apiKey)
        {
            var apiKeyHeader = client.DefaultRequestHeaders.Authorization?.Parameter;
            if (string.IsNullOrWhiteSpace(apiKeyHeader) || !apiKeyHeader.Equals($"Bearer {apiKey}"))
            {
                return false;
            }
            return true;
        }*/


        // الدالة لإضافة جهة اتصال جديدة
        static async Task AddUser()
        {
           /* if (!CheckApiKey(ApiKey))
            {
                Console.WriteLine("Unauthorized! Please provide a valid API Key.");
                return;
            }*/

            Console.WriteLine("Enter  UserName:");
            string UserName = Console.ReadLine();

            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter IsActive:");
            string isActiveInput = Console.ReadLine();
            bool IsActive = bool.Parse(isActiveInput);


            Console.WriteLine("Enter phone number:");
            string phoneNumber = Console.ReadLine();

            Console.WriteLine("Enter User Group ID:");
            string userGroupIdInput = Console.ReadLine();
            int UserGroupId = int.Parse(userGroupIdInput);

            Console.WriteLine("Enter Password:");
            string Password = Console.ReadLine();

            Console.WriteLine("Enter ConfirmPassword:");
            string ConfirmPassword = Console.ReadLine();


            var newUser = new
            {
                UserName,
                IsActive,
                email,
                phoneNumber,
                UserGroupId,
                Password,
                ConfirmPassword
            };

            string url = "https://localhost:44333/api/users";
            var user = new StringContent(JsonConvert.SerializeObject(newUser), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, user);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Data added successfully");
            }
            else
            {
                Console.WriteLine("Add operation failed");
            }
            Console.ReadLine();
        }
        static async Task DeleteContact()
        {
           /* if (!CheckApiKey(ApiKey))
            {
                Console.WriteLine("Unauthorized! Please provide a valid API Key.");
                return;
            }*/

            Console.WriteLine("Enter contact ID to delete:");
            int idToDelete = int.Parse(Console.ReadLine());

            var response = await client.DeleteAsync($"https://localhost:44333/api/users/{idToDelete}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Data with ID {idToDelete} deleted successfully");
            }
            else
            {
                Console.WriteLine($"Failed to delete data with ID {idToDelete}. Status code: {response.StatusCode}");
            }
            Console.ReadLine();

        }

        static async Task UpdateContact()
        {
            /*if (!CheckApiKey(ApiKey))
            {
                Console.WriteLine("Unauthorized! Please provide a valid API Key.");
                return;
            }*/

            Console.WriteLine("Enter contact ID to update:");
            int contactId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter new first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter new last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Enter new email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter new phone number:");
            string phoneNumber = Console.ReadLine();

            var contact = new
            {
                ContactId = contactId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var json = JsonConvert.SerializeObject(contact);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:44333/api/users/{contactId}", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Data edited successfully");
            }
            else
            {
                Console.WriteLine("Edit operation failed");
            }
            Console.ReadLine();

        }

        static async Task RetrieveUser()
        {
            Console.WriteLine("Enter contact ID to retrieve:");
            int contactId;
            if (!int.TryParse(Console.ReadLine(), out contactId))
            {
                Console.WriteLine("Invalid contact ID entered.");
                return;
            }

            string apiUrl = $"https://localhost:44333/api/users/{contactId}";

            var getResponse = await client.GetAsync(apiUrl);

            if (getResponse.IsSuccessStatusCode)
            {
                var responseBody = await getResponse.Content.ReadAsStringAsync();

                // Deserialize the responseBody to a single contact object
                // Example using Newtonsoft.Json:
                 var user = JsonConvert.DeserializeObject<User>(responseBody);

                // Example contact display in a table format
                Console.WriteLine("Contact data:");
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("| ID |   Name    |  Email          | Phone Number | IsActive |");
                Console.WriteLine("------------------------------------------------");

                // Display the retrieved contact data in a table
                // Example assuming contact is an object of type Contact
                Console.WriteLine($"| {user.UserName,-10} | {user.Email,-15} | {user.IsActive,-3} | {user.UserGroupId,-3} | {user.IsActive,-8} |");

                // Example with dummy data
                /* Console.WriteLine("| 1  | John Doe  | john@example.com | 1234567890   | True     |");*/

                Console.WriteLine("------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"Failed to retrieve contact data. Status code: {getResponse.StatusCode}");
            }
            Console.ReadLine();
        }

        static async Task RetrieveAllUsers()
        {
            var response = await client.GetAsync("https://localhost:44333/api/users");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the responseBody to a list of user objects
                // Example using Newtonsoft.Json:
                 var users = JsonConvert.DeserializeObject<List<User>>(responseBody);

                // Example user display in a table format
                Console.WriteLine("All Users data:");
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("| ID |   Name    |  Email          | Phone Number | IsActive |");
                Console.WriteLine("-------------------------------------------------------------------");

                // Display the retrieved user data in a table
                // Example assuming users is a List<User>
                foreach (var user in users)
                {
                    Console.WriteLine($"| {user.UserName,-10} | {user.Email,-15} | {user.IsActive,-3} | {user.UserGroupId,-3} | {user.IsActive,-8} |");
                }

                // Example with dummy data
               /* Console.WriteLine("| 1  | John Doe  | john@example.com | 1234567890   | True     |");
                Console.WriteLine("| 2  | Jane Smith| jane@example.com | 9876543210   | False    |");*/

                Console.WriteLine("-------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"Failed to retrieve all users. Status code: {response.StatusCode}");
            }
            Console.ReadLine();
        }




    }
}

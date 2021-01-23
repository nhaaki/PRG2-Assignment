using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace PRG2_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] personLines = File.ReadAllLines("Person.csv");

            string[] BusinessLines = File.ReadAllLines("BusinessLocation.csv");

            List<API> apidata = LoadAPI();

            ListVisitors(personLines);



        }

        

        static List<API> LoadAPI()
        {
            List<API> apiList = new List<API>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                responseTask.Wait();
                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<string> apiTask = result.Content.ReadAsStringAsync();
                    apiTask.Wait();
                    string data = apiTask.Result;
                    apiList = JsonConvert.DeserializeObject<List<API>>(data);
                    return apiList;
                }
                else
                {
                    return apiList;
                }
            }
        }

        static void ListVisitors(string[] lines)
        {
            Console.WriteLine("Visitors");
            Console.WriteLine("--------");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] person = lines[i].Split(',');
                if (person[0] == "visitor")
                {

                    Console.WriteLine(person[1]);

                }
            }
        }

        

    }



    
}



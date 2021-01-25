using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace PRG2_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] personLines = File.ReadAllLines("Person.csv");

            string[] BusinessLines = File.ReadAllLines("BusinessLocation.csv");

            List<API> apidata = LoadAPI();
            
            List<Person> personList = LoadPerson(personLines);

            List<SHNFacility> shnList = new List<SHNFacility>();

            for (int i = 0; i < apidata.Count; i++)
            {
                shnList.Add(new SHNFacility(apidata[i].facilityname, apidata[i].facilitycapacity, apidata[i].facilitycapacity, apidata[i].distFromAirCheckpoint, apidata[i].distFromSeaCheckpoint, apidata[i].distFromLandCheckpoint));
            }



            while (true)
            {
                int input = DisplayMenu();

                if (input == -1)
                {
                    continue;
                }
                else if (input == 0)
                {
                    return;
                }
                else if (input == 1)
                {
                    ListVisitors(personList);
                }
                else if (input == 2)
                {
                    Console.WriteLine();
                    Console.Write("Enter name of person: ");
                    string name = Console.ReadLine();
                    ListPersonDetails(name, personList);
                }
                else if (input == 3)
                {
                   CreateVisitor(personList);
                }
            }
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

        static void ListVisitors(List<Person> personList)
        {
            Console.WriteLine();
            Console.WriteLine("Visitors");
            Console.WriteLine("--------");
            int count = 1;
            for (int i = 0; i < personList.Count; i++)
            {
                
                if (personList[i] is Visitor)
                {

                    Console.WriteLine("({0}) {1}", count, personList[i].Name);
                    count++;

                }
            }
        }

        static List<Person> LoadPerson( string[] personLines)
        {
            List<Person> personList = new List<Person>() { };
            for (int i = 1; i < personLines.Length; i++)
            {
                string[] data = personLines[i].Split(',');
                if (data[0] == "resident")
                {
                    if (data[6] != "")
                    {
                        Resident res = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        res.Token = new TraceTogetherToken(data[6], data[7], DateTime.Parse(data[8]));
                    }
                    Resident newres = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    personList.Add(newres);
                }
                else if (data[0] == "visitor")
                {
                    Visitor newvis = new Visitor(data[1], data[4], data[5]);
                    personList.Add(newvis);
                }

            }
            return personList;

            
        }

        static void ListPersonDetails(string name, List<Person> personList)
        {
            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    Console.WriteLine(x.ToString());
                }
            }
        }

        static int DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=========================");
            Console.WriteLine("Main   monitoring   menu");
            Console.WriteLine("=========================");
            Console.WriteLine();
            List<string> choice = new List<string>() { "Exit the application", "Display all visitors", "Display details for a person" };
            
            for (int x=0; x<choice.Count; x++){
                Console.WriteLine("({0}) {1}", x, choice[x]);
            }
            Console.Write("Enter choice: ");
            try
            {
                int input = Convert.ToInt32(Console.ReadLine());
                return input;
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("Wrong input! Please choose one of the options displayed above.");
                return -1;
            }
        }

        static void ListSHN(List<SHNFacility> list)
        {
            Console.WriteLine("Facility");
            Console.WriteLine("--------");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].faclilityName);
            }
        }

        static void CreateVisitor(List<Person> personList)
        {
            Console.Write("Enter Your Name: ");
            string name = Convert.ToString(Console.ReadLine());
            Console.Write("Enter Your Passport No.: ");
            string passportNo = Convert.ToString(Console.ReadLine());
            Console.Write("Enter Your Nationality: ");
            string nationality = Convert.ToString(Console.ReadLine());

            Visitor newvisitor = new Visitor(name, passportNo, nationality);
            personList.Add(newvisitor);
        }

        static void CreateTravelEntryRecord(List<Person> personList)
        {
            Console.Write("Enter Your Name: ");
            string name = Convert.ToString(Console.ReadLine());
            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    Console.Write("Enter Your Last Country Of Embarkation: ");
                    string lcoe = Convert.ToString(Console.ReadLine());
                    Console.Write("Enter Your Entry Mode: ");
                    string entrymode = Convert.ToString(Console.ReadLine());
                    DateTime entrydate = DateTime.Today;
                    DateTime enddate = entrydate.AddDays(x.TravelEntryList[0].CalculateSHNDuration());


                    
                }
            }


        }
        

    }



    
}



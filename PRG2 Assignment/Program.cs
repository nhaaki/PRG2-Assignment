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

            string[] businessLines = File.ReadAllLines("BusinessLocation.csv");

            List<API> apidata = LoadAPI();
            
            List<Person> personList = LoadPerson(personLines);
            List<BusinessLocation> businessList = LoadBusinesses(businessLines);

            List<SHNFacility> shnList = new List<SHNFacility>();
            
            for (int i = 0; i < apidata.Count; i++)
            {
                shnList.Add(new SHNFacility(apidata[i].facilityname, apidata[i].facilitycapacity, apidata[i].distFromAirCheckpoint, apidata[i].distFromSeaCheckpoint, apidata[i].distFromLandCheckpoint));
            }



            while (true)
            {
                int input = DisplayMenu();

                if (input == -123456)
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
                else if (input == 4)
                {
                    ReplaceTraceTogether(personList);
                }
                else if (input == 5)
                {
                    ListBusinessLocations(businessList);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("That option does not exist! Please choose one of the options displayed above.");
                    Console.WriteLine();
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

        static List<BusinessLocation> LoadBusinesses(string[] businessLines)
        {
            List<BusinessLocation> businessList = new List<BusinessLocation>() { };
            for (int i = 1; i<businessLines.Length; i++)
            {
                string[] data = businessLines[i].Split(',');
                BusinessLocation newBL = new BusinessLocation(data[0], data[1], Convert.ToInt32(data[2]));
                businessList.Add(newBL);
            }

            return businessList;
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
                        personList.Add(res);
                    }
                    else
                    {
                        Resident newres = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        personList.Add(newres);
                    }
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
                    if (x is Resident)
                    {
                        
                        Resident z = (Resident)x;
                        
                        if (z.Token != null)
                        {
                            Console.WriteLine();
                            Console.WriteLine("TraceTogether Token Details");
                            Console.WriteLine("---------------------------");
                            Console.WriteLine();
                            Console.WriteLine("Serial Number: {0}", z.Token.SerialNo);
                            Console.WriteLine("Collection Location: {0}", z.Token.CollectionLocation);
                            Console.WriteLine("Expiry date: {0}", z.Token.ExpiryDate);
                        }
                    }
                }
            }
        }

        static int DisplayMenu()
        {
            
            Console.WriteLine("=========================");
            Console.WriteLine("Main   monitoring   menu");
            Console.WriteLine("=========================");
            Console.WriteLine();
            List<string> choice = new List<string>() { "Exit the application", "Display all visitors",
                "Display details for a person", "Create visitor", "Assign/Replace TT Token", "Display business locations" };
            
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
                Console.WriteLine();
                return -123456;
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

        static void ListBusinessLocations(List<BusinessLocation> list)
        {
            Console.WriteLine();
            Console.WriteLine("Business Locations");
            Console.WriteLine("------------------");
            Console.WriteLine();
            for (int x = 0; x < list.Count; x++)
            {
                Console.WriteLine("({0}) {1}", x+1, list[x].BusinessName);
                Console.WriteLine("Branch code: {0}", list[x].BranchCode);
                Console.WriteLine("Maximum capacity: {0}", list[x].MaximumCapacity);
                Console.WriteLine();
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

        static void CreateTravelEntryRecord(List<Person> personList, List<SHNFacility> list)
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
                    TravelEntry newtravelentry = new TravelEntry(lcoe, entrymode, entrydate);
                    x.TravelEntryList[0].CalculateSHNDuration();
                    

                    while (true)
                    {
                        Console.WriteLine("Facility");
                        Console.WriteLine("--------");
                        for (int i = 0; i < list.Count; i++)
                        {
                            Console.WriteLine("[" + (i + 1) + "]" + list[i].faclilityName);
                        }

                        Console.Write("Enter Your SHN Facility Choice: ");
                        int shnchoice = Convert.ToInt32(Console.ReadLine());

                        if (list[shnchoice - 1].facilityVacancy < 0)
                        {
                            Console.WriteLine("The chosen SHN Facility is full");
                        }
                        else
                        {
                            newtravelentry.AssignSHNFacility(list[shnchoice - 1]);
                            break;
                        }

                    }
                    x.AddTravelEntry(newtravelentry);
                }
            }
        }

        static void CalculateSHNCharges(List<Person> personList)
        {
            Console.Write("Enter Your Name: ");
            string name = Convert.ToString(Console.ReadLine());
            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    if (x.TravelEntryList[0].shnEndDate < DateTime.Today)
                    {

                    }
                }
            }
        }

        static void ReplaceTraceTogether(List<Person> personList)
        {
            Console.WriteLine();
            Console.WriteLine("Replace TraceTogether token (For Residents)");
            Console.Write("Enter name: ");
            string resName = Console.ReadLine();

            foreach (Person x in personList)
            {
                if (x is Resident && x.Name == resName)
                {
                    Resident z = (Resident)x;
                    if (z.Token is null)
                    {
                        Random r = new Random();
                        string newSerialNo = r.Next(0, 1000000).ToString("D6");
                        Console.WriteLine();
                        Console.Write("Enter CC to collect token from: ");
                        string newCollectionLoc = Console.ReadLine();

                        z.Token = new TraceTogetherToken(newSerialNo, newCollectionLoc, DateTime.Now.AddMonths(6));
                    }
                    else
                    {
                        if (z.Token.IsEligibleForReplacement())
                        {
                            Random r = new Random();
                            string newSerialNo = r.Next(0, 1000000).ToString("D6");
                            Console.WriteLine();
                            Console.Write("Enter CC to collect token from: ");
                            string newCollectionLoc = Console.ReadLine();

                            z.Token = new TraceTogetherToken(newSerialNo, newCollectionLoc, DateTime.Now.AddMonths(6));
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Your token is not eligible for a replacement.");
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        

    }



    
}



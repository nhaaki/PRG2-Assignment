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


            List<BusinessLocation> businessList = LoadBusinesses(businessLines);

            List<SHNFacility> shnList = new List<SHNFacility>();


            for (int i = 0; i < apidata.Count; i++)
            {
                shnList.Add(new SHNFacility(apidata[i].facilityname, apidata[i].facilitycapacity, apidata[i].distFromAirCheckpoint, apidata[i].distFromSeaCheckpoint, apidata[i].distFromLandCheckpoint));
            }

            List<Person> personList = LoadPerson(personLines, shnList);

            



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
                    ReplaceTraceTogether(personList);
                }
                else if (input == 4)
                {
                    ListBusinessLocations(businessList);
                }
                else if (input == 5)
                {
                    EditBL(businessList);
                }
                else if (input == 6)
                {
                    SafeEntryCheckIn(personList, businessList);
                }
                else if (input == 7)
                {
                    SafeEntryCheckOut(personList, businessList);
                }
                else if (input == 8)
                {
                    ListSHN(shnList);
                }
                else if (input == 9)
                {
                    CreateVisitor(personList);
                }
                else if (input == 10)
                {
                    CreateTravelEntryRecord(personList, shnList);
                }
                else if (input == 11)
                {
                    CalculateSHNCharges(personList);
                }
                else if (input == 12)
                {
                    ContactTracingReporting(personList, businessList);
                }
                else if (input == 13)
                {
                    SHNStatusReporting(personList);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("|ERROR| That option does not exist! Please choose one of the options displayed above.");
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
            Console.WriteLine();
        }

        static List<BusinessLocation> LoadBusinesses(string[] businessLines)
        {
            List<BusinessLocation> businessList = new List<BusinessLocation>() { };
            for (int i = 1; i < businessLines.Length; i++)
            {
                string[] data = businessLines[i].Split(',');
                BusinessLocation newBL = new BusinessLocation(data[0], data[1], Convert.ToInt32(data[2]));
                businessList.Add(newBL);
            }

            return businessList;
        }


        static List<Person> LoadPerson(string[] personLines, List<SHNFacility> shnList)
        {
            List<Person> personList = new List<Person>() { };
            for (int i = 1; i < personLines.Length; i++)
            {
                string[] data = personLines[i].Split(',');
                if (data[0] == "resident")
                {
                    if (data[6] != "" && data[9] != "")
                    {
                        TravelEntry travelentry = new TravelEntry(data[9], data[10], DateTime.ParseExact(data[11], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                        Resident newres = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        newres.Token = new TraceTogetherToken(data[6], data[7], DateTime.Parse(data[8]));
                        newres.AddTravelEntry(travelentry);
                        newres.TravelEntryList[newres.TravelEntryList.Count - 1].isPaid = Convert.ToBoolean(data[13]);


                        for (int x = 0; x < shnList.Count; x++)
                        {


                            if (shnList[x].faclilityName == data[14])
                            {
                                newres.TravelEntryList[newres.TravelEntryList.Count - 1].AssignSHNFacility(shnList[x]);
                                Console.WriteLine("hi");



                            }
                        }
                        
                        personList.Add(newres);
                    }

                    else if (data[6] != "")
                    {
                        Resident res = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        res.Token = new TraceTogetherToken(data[6], data[7], DateTime.Parse(data[8]));
                        personList.Add(res);
                    }

                    else if (data[11] != "")
                    {
                        TravelEntry travelentry = new TravelEntry(data[9], data[10], DateTime.ParseExact(data[11], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                        Person newres = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        newres.AddTravelEntry(travelentry);
                        newres.TravelEntryList[newres.TravelEntryList.Count - 1].isPaid = Convert.ToBoolean(data[13]);


                        for (int x = 0; x < shnList.Count; x++)
                        {


                            if (shnList[x].faclilityName == data[14])
                            {
                                newres.TravelEntryList[newres.TravelEntryList.Count - 1].AssignSHNFacility(shnList[x]);



                            }
                        }
                        personList.Add(newres);
                    }

                    else
                    {
                        Resident newres = new Resident(data[1], data[2], DateTime.ParseExact(data[3], "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        personList.Add(newres);
                    }
                }

                else if (data[0] == "visitor" && data[11] != "")
                {

                    TravelEntry travelentry = new TravelEntry(data[9], data[10], DateTime.ParseExact(data[11], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                    Person newvis = new Visitor(data[1], data[4], data[5]);
                    newvis.AddTravelEntry(travelentry);
                    newvis.TravelEntryList[newvis.TravelEntryList.Count - 1].isPaid = Convert.ToBoolean(data[13]);

                    for (int x = 0; x < shnList.Count; x++)
                    {


                        if (shnList[x].faclilityName == data[14])
                        {
                            newvis.TravelEntryList[newvis.TravelEntryList.Count - 1].AssignSHNFacility(shnList[x]);


                        }
                    }
                    personList.Add(newvis);
                }

                else if (data[0] == "visitor" && data[11] == "")
                {
                    Person newvis = new Visitor(data[1], data[4], data[5]);
                    personList.Add(newvis);
                }

            }
            return personList;


        }

        static void ListPersonDetails(string name, List<Person> personList)
        {
            int found = 0;
            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    found++;
                    Console.WriteLine();
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
                            Console.WriteLine(z.Token.ToString());
                        }
                    }

                    if (x.SafeEntryList.Count != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("SafeEntry Details");
                        Console.WriteLine("-----------------");
                        foreach (SafeEntry i in x.SafeEntryList)
                        {
                            Console.WriteLine();
                            Console.WriteLine(i.ToString());
                        }
                    }

                    if (x.TravelEntryList.Count != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Travel Entry Details");
                        Console.WriteLine("-----------------");
                        Console.WriteLine();
                        Console.WriteLine("Last Country of Embarkation: {0}", x.TravelEntryList[x.TravelEntryList.Count-1].lastCountryOfEmbarkation);
                        Console.WriteLine("Entry Mode: {0}", x.TravelEntryList[x.TravelEntryList.Count - 1].entryMode);
                        Console.WriteLine("Entry Date: {0}", x.TravelEntryList[x.TravelEntryList.Count - 1].entryDate);
                        if (x.TravelEntryList[x.TravelEntryList.Count - 1].lastCountryOfEmbarkation != "Vietnam" && x.TravelEntryList[x.TravelEntryList.Count - 1].lastCountryOfEmbarkation != "New Zealand")
                        {

                            Console.WriteLine("Faclility Name: {0}", x.TravelEntryList[x.TravelEntryList.Count - 1].shnStay.faclilityName);
                        }
                    }
                    Console.WriteLine();
                }
            }
            if (found == 0)
            {
                Console.WriteLine();
                Console.WriteLine("|ERROR| Person not found! Please enter the correct name.");
                Console.WriteLine();
            }
        }

        static int DisplayMenu()
        {

            Console.WriteLine("=========================");
            Console.WriteLine("Main   monitoring   menu");
            Console.WriteLine("=========================");
            Console.WriteLine();
            List<string> choice = new List<string>() { "Exit the application", "Display all visitors",
                "Display details for a person", "Assign/Replace TT Token", "Display business locations",
                "Edit business location capacity", "SafeEntry Check-in", "SafeEntry Check-out", "List SHN Faclilties",
                "Create Visitor", "Create TravelEntry Record", "Calculate SHN Charges", "Contact Tracing Reporing", "SHN Status Reporting"};

            for (int x = 0; x < choice.Count; x++)
            {
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
                Console.WriteLine("|ERROR| Wrong input! Please choose one of the options displayed above.");
                Console.WriteLine();
                return -123456;
            }
        }

        static void ListSHN(List<SHNFacility> list)
        {
            Console.WriteLine();
            Console.WriteLine("Facility");
            Console.WriteLine("--------");
            Console.WriteLine();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("({0}) {1}", i+1,list[i].faclilityName);
            }
            Console.WriteLine();
        }

        static void ListBusinessLocations(List<BusinessLocation> list)
        {
            Console.WriteLine();
            Console.WriteLine("Business Locations");
            Console.WriteLine("------------------");
            Console.WriteLine();
            for (int x = 0; x < list.Count; x++)
            {
                Console.WriteLine("[{0}]", x+1);
                Console.WriteLine(list[x].ToString());
                Console.WriteLine();
            }
        }

        static void EditBL(List<BusinessLocation> list)
        {
            Console.WriteLine();
            Console.WriteLine("Edit business location capacity");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();
            Console.Write("Enter business name: ");
            string bName = Console.ReadLine();

            int found = 0;

            foreach (BusinessLocation x in list)
            {
                if (x.BusinessName == bName)
                {
                    Console.WriteLine();
                    Console.Write("Enter new capacity: ");
                    string newCapacity = Console.ReadLine();

                    x.MaximumCapacity = Convert.ToInt32(newCapacity);
                    found++;
                }
            }

            if (found != 1)
            {
                Console.WriteLine();
                Console.WriteLine("|ERROR| Business not found! Make sure you spelled it correctly.");
                Console.WriteLine();
            }
        }

        static void SafeEntryCheckIn(List<Person> personList, List<BusinessLocation> bList)
        {
            Console.WriteLine();
            Console.WriteLine("SafeEntry Check-In");
            Console.WriteLine("------------------");
            Console.WriteLine();
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            int found = 0;
            
            Person chosenPerson = personList[0];

            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    chosenPerson = x;
                    found++;
                }
            }
            if (found == 0)
            {
                Console.WriteLine();
                Console.WriteLine("|ERROR| Person not found!");
                Console.WriteLine();
            }
            else
            {
                ListBusinessLocations(bList);
                Console.Write("Enter choice [1-4]: ");
                try
                {
                    Boolean asda = false;
                    BusinessLocation chosen = bList[Convert.ToInt32(Console.ReadLine()) - 1];
                    if (chosen.VisitorsNow != chosen.MaximumCapacity)
                    {
                        if (chosenPerson.SafeEntryList.Count != 0)
                        {
                            foreach (SafeEntry x in chosenPerson.SafeEntryList)
                            {
                                if (x.Location == chosen && x.CheckOut == DateTime.MinValue)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("|ERROR| Check out before using SafeEntry for this location!");
                                    Console.WriteLine();
                                    asda = true;
                                }
                            }
                        }
                        if (asda is false)
                        {
                            SafeEntry seObject = new SafeEntry(DateTime.Now, chosen);
                            chosen.VisitorsNow++;

                            chosenPerson.SafeEntryList.Add(seObject);
                            Console.WriteLine();
                            Console.WriteLine("SafeEntry successful!");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("|ERROR| Business Location is full!");
                        Console.WriteLine();
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine();
                    Console.WriteLine("|ERROR| Wrong input! Enter a number.");
                    Console.WriteLine();
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine();
                    Console.WriteLine("|ERROR| Please choose one of the options listed above.");
                    Console.WriteLine();
                }


            }
        }

        static void SafeEntryCheckOut(List<Person> personList, List<BusinessLocation> bList)
        {
            bool found = false;
            bool locationFound = false;
            Console.WriteLine();
            Console.WriteLine("SafeEntry Check-out");
            Console.WriteLine("-------------------");
            Console.WriteLine();
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.WriteLine();

            foreach (Person x in personList)
            {
                if (x.Name == name)
                {
                    found = true;
                    if (x.SafeEntryList.Count != 0)
                    {
                        int asda = 0;
                        foreach (SafeEntry y in x.SafeEntryList)
                        {
                            if (y.CheckOut == DateTime.MinValue)
                            {
                                Console.WriteLine(">>");
                                Console.WriteLine(y.ToString());
                                Console.WriteLine();
                            }
                            else
                                asda++;
                        }

                        if (asda == x.SafeEntryList.Count)
                        {
                            Console.WriteLine("|ERROR| No SafeEntry records that need checking out.");
                            Console.WriteLine();
                            return;
                        }

                        Console.Write("Enter name of business: ");
                        string businessLocation = Console.ReadLine();

                        foreach (SafeEntry z in x.SafeEntryList)
                        {
                            if (z.Location.BusinessName == businessLocation)
                            {
                                locationFound = true;
                                z.CheckOut = DateTime.Now;
                                foreach (BusinessLocation bl in bList)
                                {
                                    if (bl == z.Location)
                                    {
                                        bl.VisitorsNow -=1;
                                    }
                                }
                                
                            }
                        }
                        if (locationFound is false)
                        {
                            Console.WriteLine();
                            Console.WriteLine("|ERROR| Business not found! Check your spelling.");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Successfully checked out!");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("|ERROR| This person does not have any SafeEntry records.");
                        Console.WriteLine();
                    }
                }
            }
            if (found == false)
            {
                Console.WriteLine();
                Console.WriteLine("|ERROR| Person not found! Check your spelling.");
                Console.WriteLine();
            }
        }
       

        static void CreateVisitor(List<Person> personList)
        {

            while (true)
            {
                Console.WriteLine();
                Console.Write("Enter your name: ");
                string name = Convert.ToString(Console.ReadLine());
                if (name == "")
                {
                    Console.WriteLine("|ERROR| Invalid Name. Please Try Again");
                    continue;
                }
                

                Console.WriteLine();
                Console.Write("Enter your passport number: ");
                string passportNo = Convert.ToString(Console.ReadLine());
                if (passportNo == "")
                {
                    Console.WriteLine("|ERROR| Invalid PassportNo. Please Try Again");
                    continue;
                }

                Console.WriteLine();
                Console.Write("Enter your nationality: ");
                string nationality = Convert.ToString(Console.ReadLine());
                if (nationality == "")
                {
                    Console.WriteLine("|ERROR| Invalid Nationality. Please Try Again");
                    continue;
                }

                Visitor newvisitor = new Visitor(name, passportNo, nationality);
                personList.Add(newvisitor);
                Console.WriteLine();
                Console.WriteLine("Visitor successfully created!");
                Console.WriteLine();
                break;
            }
        }

        static void CreateTravelEntryRecord(List<Person> personList, List<SHNFacility> list)
        {

            bool success = false;
            Console.Write("Enter Your Name: ");
            string name = Convert.ToString(Console.ReadLine());
            

            foreach (Person x in personList)
            {
                if (x.Name.ToLower() == name.ToLower())
                {
                    try
                    {
                        Console.Write("Enter Your Last Country Of Embarkation: ");
                        string lcoe = Convert.ToString(Console.ReadLine());
                        lcoe = char.ToUpper(lcoe[0]) + lcoe.Substring(1);
                        
                        while (true)
                        {
                            Console.Write("Enter Your Entry Mode: ");
                            string entrymode = Convert.ToString(Console.ReadLine());
                            entrymode = char.ToUpper(entrymode[0]) + entrymode.Substring(1);

                            if (entrymode == "Land" || entrymode == "Sea" || entrymode == "Air")
                            {
                                DateTime entrydate = DateTime.Now;
                                TravelEntry newtravelentry = new TravelEntry(lcoe, entrymode, entrydate);
                                newtravelentry.CalculateSHNDuration();


                                if(lcoe != "Vietnam" && lcoe != "New Zealand")
                                {
                                    while (true)
                                    {
                                        Console.WriteLine();
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
                                            list[shnchoice - 1].facilityVacancy = list[shnchoice - 1].facilityVacancy - 1;
                                            break;
                                        }
                                        

                                    }
                                    Console.WriteLine("Your Travel Entry Record has been created");
                                    x.AddTravelEntry(newtravelentry);
                                    success = true;

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Your Travel Entry Record has been created");
                                    x.AddTravelEntry(newtravelentry);
                                    success = true;

                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("|ERROR| Please enter a valid Entry Mode");
                            }

                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("|ERROR| Input not valid");
                    }
                }
            }
            if(success is false)
            {
                Console.WriteLine("|ERROR| Name entered is not valid");
            }
        }

        static void CalculateSHNCharges(List<Person> personList)
        {


            bool success = false;
            Console.Write("Enter Your Name: ");
            string name = Convert.ToString(Console.ReadLine());
            foreach (Person x in personList)
            {

                if (x.Name == name)
                {

                    if (x.TravelEntryList[x.TravelEntryList.Count - 1].shnEndDate < DateTime.Now && x.TravelEntryList[x.TravelEntryList.Count - 1].isPaid == false)
                    {
                        double cost = x.CalculateSHNCharges();
                        Console.WriteLine("Your total cost is $" + cost);
                        while (true)
                        {
                            Console.Write("Enter p to make your payment: ");
                            string opt = Convert.ToString(Console.ReadLine());
                            if (opt == "p")
                            {
                                x.TravelEntryList[0].isPaid = true;
                                success = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You have not made your payment. Please Try Again");
                            }
                        }
                    }
                }
            }

            if(success is false)
            {
                Console.WriteLine("|ERROR| The Name entered either is not valid or did not stay at an SHN Faclility. Please Try Again");
            }
        }



        static void ReplaceTraceTogether(List<Person> personList)
        {
            Console.WriteLine();
            Console.WriteLine("Replace TraceTogether token (For Residents)");
            Console.Write("Enter name: ");
            string resName = Console.ReadLine();
            int found = 0;

            foreach (Person x in personList)
            {
                if (x is Resident && x.Name == resName)
                {
                    found++;
                    Resident z = (Resident)x;
                    if (z.Token is null)
                    {
                        Random r = new Random();
                        string newSerialNo = r.Next(0, 1000000).ToString("D6");
                        Console.WriteLine();
                        Console.Write("Enter CC to collect token from: ");
                        string newCollectionLoc = Console.ReadLine();

                        z.Token = new TraceTogetherToken(newSerialNo, newCollectionLoc, DateTime.Now.AddMonths(6));
                        Console.WriteLine();
                        Console.WriteLine("Success! Token assigned.");
                        Console.WriteLine("------------------------");
                        Console.WriteLine(z.Token.ToString());
                        Console.WriteLine();
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
                            Console.WriteLine();
                            Console.WriteLine("Success! Token replaced.");
                            Console.WriteLine("------------------------");
                            Console.WriteLine(z.Token.ToString());
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("|ERROR| Your token is not eligible for a replacement.");
                            Console.WriteLine();
                        }
                    }
                }
                else if (x is Visitor && x.Name == resName)
                {
                    found++;
                    Console.WriteLine();
                    Console.WriteLine("|ERROR| Person is not a resident!");
                    Console.WriteLine();
                }
            }
            if (found == 0)
            {
                Console.WriteLine();
                Console.WriteLine("|ERROR| Invalid input! Enter the name of an existing person.");
                Console.WriteLine();
            }
        }

        static void ContactTracingReporting(List<Person> personList, List<BusinessLocation> bList)
        {
            try
            {
                Console.WriteLine();
                Console.Write("Enter a business location (eg. Cheap Goods Shop): ");
                string location = Console.ReadLine();
                Console.Write("Enter a datetime value (eg. '28/01/21 08:00'): ");
                DateTime date = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);


                File.WriteAllText("reportContactTracing.csv", string.Empty);
                using (StreamWriter sw = new StreamWriter("reportContactTracing.csv", false))
                {
                    sw.WriteLine("Name,Check-in time,Check-out time");
                }

                Boolean found = false;
                BusinessLocation locationItem = new BusinessLocation();

                foreach (BusinessLocation z in bList)
                {
                    if (location == z.BusinessName)
                    {
                        found = true;
                        locationItem = z;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine();
                    Console.WriteLine("|ERROR| Business not found! Enter the name of the business with proper spelling. ");
                }
                else
                {
                    foreach (Person x in personList)
                    {
                        if (x.SafeEntryList.Count > 0)
                        {
                            foreach (SafeEntry se in x.SafeEntryList)
                            {
                                if (se.Location == locationItem)
                                {
                                    if (DateTime.Compare(date, se.CheckIn) >= 0 && (DateTime.Compare(date, se.CheckOut) <= 0 || se.CheckOut == DateTime.MinValue))
                                    {
                                        if (se.CheckOut == DateTime.MinValue)
                                        {
                                            using (StreamWriter sw = new StreamWriter("reportContactTracing.csv", true))
                                            {
                                                sw.WriteLine(x.Name + "," + se.CheckIn + ",Pending");
                                            }
                                        }
                                        else
                                        {
                                            using (StreamWriter sw = new StreamWriter("reportContactTracing.csv", true))
                                            {
                                                sw.WriteLine(x.Name + "," + se.CheckIn + "," + se.CheckOut);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Report successfully generated!");
                    Console.WriteLine();
                }
                
            }
            catch (FormatException)
            {
                Console.WriteLine("|ERROR| Invalid input! Enter a DateTime");
                Console.WriteLine();
            }
        }

        static void SHNStatusReporting(List<Person> personList)
        {
            try
            {
                Console.Write("Enter a Date");
                DateTime date = Convert.ToDateTime(Console.ReadLine());
                foreach (Person x in personList)
                {
                    if (x.TravelEntryList.Count > 0)
                    {
                        if (x.TravelEntryList[x.TravelEntryList.Count - 1].entryDate == date)
                        {
                            File.WriteAllText("reportshn.csv", string.Empty);
                            using (StreamWriter sw = new StreamWriter("reportshn.csv", true))
                            {
                                sw.WriteLine("Name,EntryMode,SHNEndDate,SHNFaclility");
                                sw.WriteLine(x.Name + "," + x.TravelEntryList[x.TravelEntryList.Count - 1].entryDate + "," + x.TravelEntryList[x.TravelEntryList.Count - 1].shnEndDate + "," + x.TravelEntryList[x.TravelEntryList.Count - 1].shnStay);
                            }

                        }
                    }

                }
            }
            catch (FormatException)
            {
                Console.WriteLine("|ERROR| Invalid input! Enter a DateTime");
                Console.WriteLine();
            }
        }
    }
}



    




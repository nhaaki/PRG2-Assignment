using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class Resident:Person
    {
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private DateTime lastLeftCountry;
        public DateTime LastLeftCountry
        {
            get { return lastLeftCountry; }
            set { lastLeftCountry = value; }
        }

        private TraceTogetherToken token;

        public TraceTogetherToken Token
        {
            get { return token; }
            set { token = value; }
        }
        
        public Resident(string n, string a, DateTime llc):base(n)
        { 
            Address = a;
            LastLeftCountry = llc;
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public override double CalculateSHNCharges()
        {
            TravelEntry lastTravelEntry = TravelEntryList[TravelEntryList.Count - 1];


            if (lastTravelEntry.lastCountryOfEmbarkation == "Veitnam" | lastTravelEntry.lastCountryOfEmbarkation == "New Zeland")
            {
                return 200;
            }
            else if(lastTravelEntry.lastCountryOfEmbarkation == "Macao SAR")
            {
                return 200 + 20;
            }
            else
            {
                return 200 + 20 + 1000;
            }
        }
    }
}

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


            if (lastTravelEntry.lastCountryOfEmbarkation == "Vietnam" | lastTravelEntry.lastCountryOfEmbarkation == "New Zealand")
            {
                return 200/100 * 107;
            }
            else if(lastTravelEntry.lastCountryOfEmbarkation == "Macao SAR")
            {
                return (200 + 20)/100 * 107;
            }
            else
            {
                return (200 + 20 + 1000)/100 * 107;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\nType: Resident" + "\nAddress: " + Address + "\nLast left country: " + LastLeftCountry.ToString("dd/MM/yyyy");
        }
    }
}

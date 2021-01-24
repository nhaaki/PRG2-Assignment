using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class Visitor:Person
    {
        private string passportNo;

        public string PassportNo
        {
            get { return passportNo; }
            set { passportNo = value; }
        }

        private string nationality;

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public Visitor(string n, string pn, string na) : base(n)
        {
            PassportNo = pn;
            Nationality = na;
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public override double CalculateSHNCharges()
        {
            TravelEntry lastTravelEntry = TravelEntryList[TravelEntryList.Count - 1];


            if (lastTravelEntry.lastCountryOfEmbarkation == "Veitnam" | lastTravelEntry.lastCountryOfEmbarkation == "New Zeland" | lastTravelEntry.lastCountryOfEmbarkation == "Macao SAR")
            {
                return 200 + 80;
            }
            else
            {
                return 200 + lastTravelEntry.shnStay.CalculateTravelCost(lastTravelEntry.entryMode, lastTravelEntry.entryDate) + 2000;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\nPassport Number: " + PassportNo + "\nNationality: " + Nationality;
        }


    }
}

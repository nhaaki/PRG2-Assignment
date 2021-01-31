// =====================================================
// Student Number: s10206177, s10203386
// Student Name  : Nur Hakimi B Mohd Yasman, Ng Jin Yang
// Module group  : T04
// =====================================================

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


            if (lastTravelEntry.lastCountryOfEmbarkation == "Vietnam" | lastTravelEntry.lastCountryOfEmbarkation == "New Zealand" | lastTravelEntry.lastCountryOfEmbarkation == "Macao SAR")
            {
                return (200 + 80)/100 * 107;
            }
            else
            {
                return (200 + lastTravelEntry.shnStay.CalculateTravelCost(lastTravelEntry.entryMode, lastTravelEntry.entryDate) + 2000)/100 * 107;
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\nType: Visitor" + "\nPassport Number: " + PassportNo + "\nNationality: " + Nationality;
        }


    }
}

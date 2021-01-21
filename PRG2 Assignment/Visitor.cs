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


    }
}

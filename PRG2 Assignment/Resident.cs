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
        }
    }
}

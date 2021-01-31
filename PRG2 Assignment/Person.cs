using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    abstract class Person
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<SafeEntry> SafeEntryList { get; set; }

        public List<TravelEntry> TravelEntryList { get; set; }

        public Person() { SafeEntryList = new List<SafeEntry>(); TravelEntryList = new List<TravelEntry>(); }

        public Person(string n)
        {
            Name = n;
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public void AddTravelEntry(TravelEntry travelEntry)
        {
            TravelEntryList.Add(travelEntry);
        }

        public void AddSafeEntry(SafeEntry safeEntry)
        {
            SafeEntryList.Add(safeEntry);
        }

        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return "Name: " + Name;
        }


    }
}

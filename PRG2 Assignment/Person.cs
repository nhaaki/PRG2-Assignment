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


    }
}

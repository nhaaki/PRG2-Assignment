using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class SafeEntry
    {
        private DateTime checkIn;

        public DateTime CheckIn
        {
            get { return checkIn; }
            set { checkIn = value; }
        }

        private DateTime checkOut;

        public DateTime CheckOut
        {
            get { return CheckOut; }
            set { CheckOut = value; }
        }


        public BusinessLocation Location { get; set; }

        public SafeEntry() { }

        public SafeEntry(DateTime ci, BusinessLocation l)
        {
            CheckIn = ci;
            Location = l;
        }

    }
}

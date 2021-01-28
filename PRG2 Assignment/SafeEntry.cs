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
            get { return checkOut; }
            set { checkOut = value; }
        }


        public BusinessLocation Location { get; set; }

        public SafeEntry() { }

        public SafeEntry(DateTime ci, BusinessLocation l)
        {
            CheckIn = ci;
            Location = l;
            
        }

        public void PerformCheckout()
        {
            DateTime checkOutTime = DateTime.Now;
            CheckOut = checkOutTime;
        }

        public override string ToString()
        {
            if (CheckOut == DateTime.MinValue)
            {
                return "Location name: " + Location.BusinessName + "\nBranch code: " + Location.BranchCode + "\nCheck-in datetime: " + CheckIn + "\nCheck-out datetime: Pending...";
            }
            else
                return "Location name: " + Location.BusinessName + "\nBranch code: " + Location.BranchCode + "\nCheck-in datetime: " + CheckIn + "\nCheck-out datetime: " + CheckOut;
        }

    }
}

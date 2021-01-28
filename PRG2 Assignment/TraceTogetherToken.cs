using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class TraceTogetherToken
    {
        private string serialNo;

        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        private string collectionLocation;

        public string CollectionLocation
        {
            get { return collectionLocation; }
            set { collectionLocation = value; }
        }

        private DateTime expiryDate;

        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        public TraceTogetherToken() { }

        public TraceTogetherToken(string sn, string cn, DateTime ed)
        {
            SerialNo = sn;
            CollectionLocation = cn;
            ExpiryDate = ed;
        }

        public bool IsEligibleForReplacement()
        {
            
            if (DateTime.Compare(ExpiryDate, DateTime.Now) >= 0)
            {
                Console.WriteLine(ExpiryDate);
                Console.WriteLine(DateTime.Now);
                return false;
            }
            else if (DateTime.Compare(ExpiryDate.AddMonths(1), DateTime.Now) >= 0)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "Serial number: " + SerialNo + "\nCollection location: " + CollectionLocation + "\nExpiry date: " + ExpiryDate;
        }
    }
}

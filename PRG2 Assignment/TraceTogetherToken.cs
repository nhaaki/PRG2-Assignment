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

        private string collectionNumber;

        public string CollectionNumber
        {
            get { return collectionNumber; }
            set { collectionNumber = value; }
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
            CollectionNumber = cn;
            ExpiryDate = ed;
        }

    }
}

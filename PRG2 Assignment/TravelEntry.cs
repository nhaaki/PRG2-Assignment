using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class TravelEntry
    {
        private string lastcountryofembarkation;

        public string lastCountryOfEmbarkation
        {
            get { return lastcountryofembarkation; }
            set { lastcountryofembarkation = value; }
        }

        private string entrymode;

        public string entryMode
        {
            get { return entrymode; }
            set { entrymode = value; }
        }

        private DateTime entrydate;

        public DateTime entryDate
        {
            get { return entrydate; }
            set { entrydate = value; }
        }

        private DateTime shnenddate;

        public DateTime shnEndDate
        {
            get { return shnEndDate; }
            set { shnEndDate = value; }
        }


        private SHNFacility shnstay;

        public SHNFacility shnStay
        {
            get { return shnstay; }
            set { shnstay = value; }
        }



        private bool ispaid;

        public bool isPaid
        {
            get { return ispaid; }
            set { ispaid = value; }
        }

        public TravelEntry() { }

        public TravelEntry(string lcoe, string em, DateTime ed, DateTime shned, SHNFacility shns, bool ip)
        {
            lastCountryOfEmbarkation = lcoe;
            entryMode = em;
            entryDate = ed;
            shnEndDate = shned;
            shnStay = shns;
            isPaid = ip;
        }

        public void AssignSHNFacility(SHNFacility x)
        {

        }
    }
}

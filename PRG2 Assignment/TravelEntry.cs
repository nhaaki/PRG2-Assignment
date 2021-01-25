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
            get { return shnenddate; }
            set { shnenddate = value; }
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
            shnStay = x;
        }

        public int CalculateSHNDuration()
        {
            if (lastCountryOfEmbarkation == "New Zealand" | lastCountryOfEmbarkation == "Vietnam")
            {
                return 0;
            }
            else if (lastCountryOfEmbarkation == "Macao SAR")
            {
                return 7;
            }
            else
            {
                return 14;
            }

        }


        public override string ToString()
        {
            return "Last Country Visited: " + lastCountryOfEmbarkation + " Entry Mode: " + entryMode + " SHN Entry Date: " + entryDate + " SHN End Date: " + shnEndDate + " SHN Stay: " + shnStay + " Is Paid: " + isPaid;
        }
    }
}

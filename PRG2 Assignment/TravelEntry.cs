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



        private bool ispaid = false;

        public bool isPaid 
        {
            get { return ispaid; }
            set { ispaid = value; }
        }

        public TravelEntry() { }

        public TravelEntry(string lcoe, string em, DateTime ed)
        {
            lastCountryOfEmbarkation = lcoe;
            entryMode = em;
            entryDate = ed;
            
        }

        public void AssignSHNFacility(SHNFacility x)
        {
            shnStay = x;
        }

        public void CalculateSHNDuration()
        {
            if (lastCountryOfEmbarkation == "New Zealand" | lastCountryOfEmbarkation == "Vietnam")
            {
                shnEndDate = entryDate;
            }
            else if (lastCountryOfEmbarkation == "Macao SAR")
            {
                shnEndDate = entryDate.AddDays(7);
            }
            else
            {
                shnEndDate = entryDate.AddDays(14);
            }

        }


        public override string ToString()
        {
            return "Last Country Visited: " + lastCountryOfEmbarkation + " Entry Mode: " + entryMode + " SHN Entry Date: " + entryDate;
        }
    }
}

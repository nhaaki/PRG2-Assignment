using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class SHNFacility
    {
        private string facilityname;

        public string faclilityName
        {
            get { return facilityname; }
            set { facilityname = value; }
        }


        private int facilitycapacity;

        public int facilityCapacity
        {
            get { return facilitycapacity; }
            set { facilitycapacity = value; }
        }

        private int facilityvacancy;

        public int facilityVacancy
        {
            get { return facilityvacancy; }
            set { facilityvacancy = value; }
        }


        private double distfromaircheckpoint;

        public double distFromAirCheckpoint
        {
            get { return distfromaircheckpoint; }
            set { distfromaircheckpoint = value; }
        }

        private double distfromseacheckpoint;

        public double distFromSeaCheckpoint
        {
            get { return distfromseacheckpoint; }
            set { distfromseacheckpoint = value; }
        }

        private double distfromlandcheckpoint;

        public double distFromLandCheckpoint
        {
            get { return distfromlandcheckpoint; }
            set { distfromlandcheckpoint = value; }
        }

        public SHNFacility() { }

        public SHNFacility(string fn, int fc, int fv, double dfac, double dfsc, double dflc)
        {
            faclilityName = fn;
            facilityCapacity = fc;
            facilityVacancy = fv;
            distFromAirCheckpoint = dfac;
            distFromSeaCheckpoint = dfsc;
            distFromLandCheckpoint = dflc;
        }

        public double CalculateTravelCost(string entrymode, DateTime entrydate)
        {
            if(entrymode == "Land")
            {
                double cost = 50 + distFromLandCheckpoint * 0.22;
                return cost;
            }
            else if (entrymode == "Sea")
            {
                double cost = 50 + distFromSeaCheckpoint * 0.22;
                return cost;
            }
            else if (entrymode == "Air")
            {
                double cost = 50 + distFromAirCheckpoint * 0.22;
                return cost;
            }
            else
            {
                return 0.00;
            }

            
        }

        public bool IsAvailable()
        {
            if(facilityCapacity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "Facility Name: " + faclilityName + " Facility Capacity: " + facilityCapacity + " Facility Vacancy" + facilityVacancy + " Distance From Air Checkpoint: " + distFromAirCheckpoint + " Distance From Sea Checkpoint: " + distFromSeaCheckpoint + " Distance From Land Checkpoint: " + distFromLandCheckpoint;
        }



    }
}

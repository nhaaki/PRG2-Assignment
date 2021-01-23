using System;
using System.Collections.Generic;
using System.Text;

namespace PRG2_Assignment
{
    class API
    {
        public string facilityname { get; set; }
        public int facilitycapacity { get; set; }
        public double distFromAirCheckpoint { get; set; }
        public double distFromSeaCheckpoint { get; set; }
        public double distFromLandCheckpoint { get; set; }


        public API() { }

        public override string ToString()
        {
            return "Facility Name: " + facilityname + " Facility Capacity: " + facilitycapacity + " Distance From Air Checkpoint: " + distFromAirCheckpoint + " Distance From Sea Checkpoint: " + distFromSeaCheckpoint + " Distance From Land Checkpoint: " + distFromLandCheckpoint;
        }
    }
}

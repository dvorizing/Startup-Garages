using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class GarageVisit
    {
        private static int id;
        public GarageVisit()
        {

        }
        public GarageVisit(int idUser)
        {
            id = idUser;
        }
        public int getUserId()
        {
            return id;
        }
        public void SetUserId(int idu)
        {
            id = idu;
        }


        public int VisitID { get; set; }
        public Nullable<int> VehicleID { get; set; }
        public Nullable<int> GarageID { get; set; }
        public Nullable<System.DateTime> VisitDate { get; set; }
        public string OilVolume { get; set; }
        public string OilType { get; set; }
        public string TiresairCuttingAbrasive { get; set; }
        public string TiresairTireChange { get; set; }
        public string BrakingFluidLevel { get; set; }
        public string BrakingBrakeWork { get; set; }
        public string BatteryFluidLevel { get; set; }
        public string BatteryConnections { get; set; }
        public string ElectricaLlightBulbs { get; set; }
        public string ElectricalSwitches { get; set; }
        public string ElectricalConnections { get; set; }
        public string CoolingCoolant { get; set; }
        public string CoolingConnections { get; set; }
        public string BracesFluidLevel { get; set; }
        public string BracesConnections { get; set; }
        public string ReplacementOilFilter { get; set; }
        public string ReplacementAirFilter { get; set; }
        public string ReplacementFuelFilter { get; set; }
        public string TreatmentNote { get; set; }
        public string Signiture { get; set; }
        public Nullable<System.DateTime> DateReport { get; set; }
        public Nullable<decimal> TotalCost { get; set; }


    }
    }
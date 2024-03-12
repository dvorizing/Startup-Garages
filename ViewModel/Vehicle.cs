using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class Vheicle
    {

        private static int id;

        public Vheicle(int idUser)
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
        public int VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<int> Year { get; set; }
        public string VIN { get; set; }
        public Nullable<int> Mileage { get; set; }
        public Nullable<System.DateTime> LastServiceDate { get; set; }
        public Nullable<int> OwnerID { get; set; }
    }
}
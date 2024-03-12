using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class GaragePermission
    {
        public GaragePermission()
        {

        }
        public GaragePermission(int idU, int idG)
        {
            this.UserID = idU;
            this.GarageID = idG;

        }

        public int PermissionID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> GarageID { get; set; }
        public Nullable<bool> CanView { get; set; }
        public Nullable<bool> CanEdit { get; set; }

      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class Permission
    {
        public Permission(int idU, int idC)
        {
            this.UserID = idU;
            this.CustomerID = idC;

        }

        public Permission()
        {

        }
        public int PermissionID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<bool> CanView { get; set; }
        public Nullable<bool> CanEdit { get; set; }

     

    }
}
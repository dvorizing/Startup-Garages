using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class Garages
    {
        private static int id;
        private static bool CanV;
        private static bool CanE;
        public Garages()
        {

        }
        public Garages(int idUser)
        {
            id = idUser;
        }
        public int getUserUId()
        {
            return id;
        }
        public void setCanView(bool CanVC)
        {
            CanV = CanVC;
        }
        public void setCanEdit(bool CanEC)
        {
            CanE = CanEC;
        }
        public bool getCanView()
        {
            return CanV;
        }
        public bool getCanEdit()
        {
            return CanE;
        }

        public int GarageID { get; set; }
        public string GarageName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
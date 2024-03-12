using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class User
    {
        public User()
        {

        }
        private static int id;

        public User(int idUser)
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
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int AmountOfGarages { get; set; }

        
    }
}
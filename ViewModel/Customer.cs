using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbirProjectCars.ViewModel
{
    public class Customer
    {

        public  int id { get; set; }
        public  int aaaa { get; set; }
        public Customer()
        {

        }
        public Customer(int idUser)
        {
            this.id = idUser;
        }
      
        public int CustomerID { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
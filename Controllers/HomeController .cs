using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbirProjectCars.Models;
using PagedList;
using System.Data.SqlClient;
using AbirProjectCars.ViewModel;
//סיסמא לcustomer
//סמספר מוסכים 
namespace AbirProjectCars.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        //מנתב ע"פ הזדהות מנהל מערכת או מנהל מוסך
        [HttpPost]
        public ActionResult Login(Users userLogin)
        {
            if (userLogin.Password == "admin")
                return RedirectToAction("Admin", "Home");
            var parameterValueName = userLogin.Username;
            var password = userLogin.Password;
 
            string queryUser = "SELECT * FROM Users WHERE Username =@Username  ";
            string queryCustomer = "SELECT * FROM Customers WHERE FirstName =@FirstName ";

            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var users = db.Database.SqlQuery<Users>(queryUser, new SqlParameter("Username", parameterValueName)).ToList();
                Users user =new Users();
                Customer customer = new Customer();
                foreach (var u in users)
                {
                    bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(password, u.Password);
                    if (isPasswordMatch)
                    {
                        user = u;
                        break;
                    }
                }              
                    if (user.UserID != 0)
                    {
                        return RedirectToAction("Index", "Customer", user);
                    }
                   
                   var customers = db.Database.SqlQuery<Customer>(queryCustomer,new SqlParameter("FirstName", parameterValueName)).ToList();
                    foreach (var c in customers)
                    {
                    bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(password, c.Password);
                    if (isPasswordMatch)
                    {
                        customer = c;
                        break;
                    }
                }
                    if (customer.CustomerID != 0)
                        {
                            return RedirectToAction("Client", "Customer", customer);
                        }
                    }
                             

            return View();
        }
        public ActionResult Admin()
        {
            return RedirectToAction("GetUsers", "User", 1);
        }
        //ניתוב מנהל לקבלת לקוחות המערכת
      

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
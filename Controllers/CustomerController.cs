using AbirProjectCars.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbirProjectCars.ViewModel;

namespace AbirProjectCars.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 1;
        private HashSet<string> CustomerCollectionPassword;
        public CustomerController()
        {
            CustomerCollectionPassword = new HashSet<string>();
        }

        //מציג ניווט לכל האופציות של מנהל המוסך שנכנס כרגע למערכת
        public ActionResult Index(Users user)
        {
            User u = new User();
            u.UserID = user.UserID;
            return View(u);
        }
        //מציג ניווט לכל האופציות של בעל רכב שנכנס כרגע למערכת
        public ActionResult Client(Customer loginCustomer)
        {
            Customer customer = new Customer();
            customer.CustomerID = loginCustomer.CustomerID;
            return View(customer);
        }

        //הIDמקצה אוביקט לקוח חדש ושולח לשמירה במערכת את   
        // (של המנהל (שיוכל להעביר בהקצאת ההרשאה
        //Whis SQLparameters
        [HttpGet]
        public ActionResult AddCustomer(int id)
        {
            Customer customer = new Customer(id);
            //customer.SetUserId(id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            var parameterValueFName = customer.FirstName;
            var parameterValueLName = customer.LastName;
            var parameterValuePhon = customer.Phone;
            var parameterValueAddress = customer.Address;
            var parameterValueEmail = customer.Email;
            String parameterValuePassword = customer.Password;


            if (!CustomerCollectionPassword.TryGetValue(parameterValuePassword, out _))
            {
                CustomerCollectionPassword.Add(parameterValuePassword);
            }
            else return RedirectToAction("Erorr");
            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(parameterValuePassword);
            string query = "INSERT INTO Customers (FirstName, LastName,Phone,Address,Email,Password) VALUES (@FirstName, @LastName,@Phone,@Address,@Email,@Password)";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@FirstName", parameterValueFName),
               new SqlParameter("@LastName", parameterValueLName),
               new SqlParameter("@Phone", parameterValuePhon),
               new SqlParameter("@Address", parameterValueAddress),
               new SqlParameter("@Email", parameterValueEmail),
               new SqlParameter("@Password", hashedPassword),
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
                query = "SELECT * FROM Customers WHERE Email = @Email";
                var DBCustomer = db.Database.SqlQuery<Customers>(query, new SqlParameter("@Email", parameterValueEmail)).FirstOrDefault();
                Customer newCustomer = new Customer();
                newCustomer.CustomerID = DBCustomer.CustomerID;
                newCustomer.FirstName = DBCustomer.FirstName;
                newCustomer.LastName = DBCustomer.LastName;
                newCustomer.Phone = DBCustomer.Phone;
                newCustomer.Email = DBCustomer.Email;
                newCustomer.Address = DBCustomer.Address;
                newCustomer.id= customer.id;
                return RedirectToAction("AddPermission", "Permission", newCustomer);
            }

        }

        //לא השתמשתי כרגע
        [HttpPost]
        public ActionResult GetById(Customer customer)
        {
            var parameterValueID = customer.CustomerID;
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var user = db.Database.SqlQuery<Customers>($"SELECT * FROM Customers WHERE CustomerID = {parameterValueID}").FirstOrDefault();
                if (user != null)
                    return RedirectToAction("Sucsses", customer);
            }
            return View();

        }

        //מקבל מס לקוח לעריכה ומספר מנהל
        //כשיחזור מהפונקציה יהיה לו מספר מנהל לשליפת רשימת הלקוחות המעודכנת
        [HttpGet]
        public ActionResult Update(int id, int UserId)
        {
            string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerId";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var DBCustomer = db.Database.SqlQuery<Customers>(query, new SqlParameter("CustomerId", id)).FirstOrDefault();
                Customer newCustomer = new Customer(/*UserId*/);
                newCustomer.CustomerID = DBCustomer.CustomerID;
                newCustomer.FirstName = DBCustomer.FirstName;
                newCustomer.LastName = DBCustomer.LastName;
                newCustomer.Phone = DBCustomer.Phone;
                newCustomer.Email = DBCustomer.Email;
                newCustomer.Address = DBCustomer.Address;
                newCustomer.id= UserId;
                newCustomer.aaaa= UserId;
                return View(newCustomer);
            }

        }

        [HttpPost]
        public ActionResult Update(Customer customer)
        {
            if (ModelState.IsValid)
            {
                int UserId = customer.aaaa;
                var parameterValueId = customer.CustomerID;
                var parameterValueFirstName = customer.FirstName;
                var parameterValueLastName = customer.LastName;
                var parameterValueEmail = customer.Email;
                var parameterValuePhon = customer.Phone;
                var parameterValueAdress = customer.Address;

                SqlParameter[] parameters = new SqlParameter[]
             {
               new SqlParameter("@CustomerId", parameterValueId),
               new SqlParameter("@FirstName", parameterValueFirstName),
               new SqlParameter("@LastName", parameterValueLastName),
               new SqlParameter("@Phone", parameterValuePhon),
               new SqlParameter("@Address", parameterValueAdress),
               new SqlParameter("@Email", parameterValueEmail)
             };
                string query = "UPDATE  Customers SET FirstName=@FirstName,LastName=@LastName,Email=@Email,Phone=@Phone,Address=@Address WHERE CustomerID=@CustomerId";
                using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
                {
                    db.Database.ExecuteSqlCommand(query, parameters);
                    return RedirectToAction("GetCustomers", "Customer", new { id = UserId });
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateMe(int id)
        {
            string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerId";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var DBCustomer = db.Database.SqlQuery<Customers>(query, new SqlParameter("CustomerId", id)).FirstOrDefault();
                Customer newCustomer = new Customer();
                newCustomer.CustomerID = DBCustomer.CustomerID;
                newCustomer.FirstName = DBCustomer.FirstName;
                newCustomer.LastName = DBCustomer.LastName;
                newCustomer.Phone = DBCustomer.Phone;
                newCustomer.Email = DBCustomer.Email;
                newCustomer.Address = DBCustomer.Address;
                return View(newCustomer);
            }

        }

        [HttpPost]
        public ActionResult UpdateMe(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var parameterValueId = customer.CustomerID;
                var parameterValueFirstName = customer.FirstName;
                var parameterValueLastName = customer.LastName;
                var parameterValueEmail = customer.Email;
                var parameterValuePhon = customer.Phone;
                var parameterValueAdress = customer.Address;

                SqlParameter[] parameters = new SqlParameter[]
             {
               new SqlParameter("@CustomerId", parameterValueId),
               new SqlParameter("@FirstName", parameterValueFirstName),
               new SqlParameter("@LastName", parameterValueLastName),
               new SqlParameter("@Phone", parameterValuePhon),
               new SqlParameter("@Address", parameterValueAdress),
               new SqlParameter("@Email", parameterValueEmail)
             };
                string query = "UPDATE  Customers SET FirstName=@FirstName,LastName=@LastName,Email=@Email,Phone=@Phone,Address=@Address WHERE CustomerID=@CustomerId";
                using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
                {
                    db.Database.ExecuteSqlCommand(query, parameters);
                    return RedirectToAction("Client", "Customer", customer);
                }
            }
            return View();
        }
        
        //מחיקת הלקוח וניווט חוזר אל רשימת הלקוחות לפי מספר מנהל
        public ActionResult Delete(int id, int UserId)
        {
            ViewBag.UserId = UserId;
            ViewBag.id = id;
            return View();
        }


        public ActionResult DeleteTotal(int id, int UserId)
        {
            string query = "";
            var parameterValueId = id;
            List<GarageVisits> allVisits = new List<GarageVisits>();
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                // שליפת כל ההרשאות המקושרות ללקוח הנמחק
                query = "SELECT * FROM Permissions WHERE CustomerID =@CustomerID ";
                var listPermissions = db.Database.SqlQuery<Permissions>(query, new SqlParameter("CustomerId", id)).ToList();

                //שליפת כל הרכבים המקושרים ללקוח הנמחק
                query = "SELECT * FROM Vehicles WHERE OwnerID =@OwnerID ";
                var listVehicle = db.Database.SqlQuery<Vehicles>(query, new SqlParameter("OwnerID", parameterValueId)).ToList();

                //שליפת כל דוחות המקושרים ללקוח הנמחק

                foreach (var item in listVehicle)
                {
                    query = "SELECT * FROM GarageVisits WHERE VehicleId = @VehicleId ";
                    var listVisit = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("VehicleId", item.VehicleID)).ToList();
                    foreach (var visit in listVisit)
                    {
                        string queryVisit = "DELETE FROM GarageVisits WHERE VisitID = @VisitID ";
                        db.Database.ExecuteSqlCommand(queryVisit, new SqlParameter("VisitID", visit.VisitID));
                    }
                }
                foreach (var item in listPermissions)
                {
                    query = "DELETE FROM Permissions WHERE CustomerID = @CustomerID ";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("CustomerId", item.CustomerID));
                }
                foreach (var item in listVehicle)
                {
                    query = "DELETE FROM Vehicles WHERE OwnerID= @OwnerID ";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("OwnerID", item.OwnerID));
                }

                query = "DELETE FROM Customers WHERE CustomerID = @CustomerID ";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("CustomerID", parameterValueId));
                return RedirectToAction("GetCustomers", "Customer", new { id = UserId });
            }
        }

        ///מציג לכל מנהל את לקוחותיו בטבלה עם ניתוב לאפשרויות   
        // עדכון פרטי לקוח ,מחיקה,ניהול רכבים,ניהול הרשאות
        //עם אפשרות דפדוף בתוצאות ובחירת עמוד מבוקש
        public ActionResult GetCustomers(int id, int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            ViewBag.UserId = id;
            List<Customers> listCustomer = new List<Customers>();
            string query = "SELECT * FROM Permissions WHERE UserID = @UserId";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var listPermissions = db.Database.SqlQuery<Permissions>(query, new SqlParameter("UserId", id)).ToList();
                foreach (var item in listPermissions)
                {
                    query = "SELECT * FROM Customers WHERE CustomerID =@CustomerId";
                    var customer = db.Database.SqlQuery<Customers>(query, new SqlParameter("CustomerId", item.CustomerID)).FirstOrDefault();
                    listCustomer.Add(customer);
                }
            }
            var Customers = listCustomer.ToPagedList(page, PAGE_SIZE);
            ViewBag.HasNextPage = page < Customers.PageCount;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.PageIndex = page;
            ViewBag.TotaPages = Customers.PageCount;
            if (page + 10 <= Customers.PageCount)
            {
                ViewBag.LoopEndPage = page + 9;
            }
            else
            {
                ViewBag.LoopEndPage = Customers.PageCount;
            }
            if (ViewBag.LoopEndPage - 9 >= 1)
                ViewBag.LoopStartPage = ViewBag.LoopEndPage - 9;
            else
                ViewBag.LoopStartPage = 1;
            if (IsMooveP)
            {
                ViewBag.LoopStartPage--;
                ViewBag.LoopEndPage--;
            }
            if (IsMooveN)
            {
                ViewBag.LoopStartPage++;
                ViewBag.LoopEndPage++;
            }
            return View(Customers);

        }


    }
}
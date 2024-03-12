using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbirProjectCars.Models;
using PagedList;
using System.Data.SqlClient;
using AbirProjectCars.ViewModel;

namespace AbirProjectCars.Controllers
{
    public class UserController : Controller
    {
        const int PAGE_SIZE = 1;
        private HashSet<string> UserCollectionPassword;
        public UserController()
        {
            UserCollectionPassword = new HashSet<string>();
        }
        public ActionResult Index()
        {
            return View();
        }

        //הוספת מנהל מוסך עם הצפנת סיסמא 
        //Whis SQLparameters
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(User user)
        {
            var parameterValueUserName = user.Username;
            String parameterValuePassword = user.Password;
            var AmountOfGarages = user.AmountOfGarages;

            if (!UserCollectionPassword.TryGetValue(parameterValuePassword, out _))
            {
                UserCollectionPassword.Add(parameterValuePassword);
            }
            else return RedirectToAction("Erorr");
            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(parameterValuePassword);
            string query = "INSERT INTO Users (Username, Password,AmountOfGarages) VALUES (@UserName, @Password,@AmountOfGarages)";

            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@Password", hashedPassword),
               new SqlParameter("@UserName", parameterValueUserName),
               new SqlParameter("@AmountOfGarages", AmountOfGarages)
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("GetUsers");
            return View(user);
        }

        //לא השתמשתי
        [HttpPost]
        public ActionResult GetById(int id)
        {
            var parameterValueID = id;
            string query = "SELECT * FROM Users WHERE UserID=@UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var user = db.Database.SqlQuery<Users>(query, parameterValueID).FirstOrDefault();
                if (user != null)
                    return RedirectToAction("Delete", user);
                return View();
            }
        }


        //עדכון פרטי מנהל 
        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(User user, int id)
        {
            var parameterValueID = id;
            var parameterValueUserName = user.Username;
            String parameterValuePassword = user.Password;
            if (!UserCollectionPassword.TryGetValue(parameterValuePassword, out _))
            {
                UserCollectionPassword.Add(parameterValuePassword);
            }
            else RedirectToAction("Erorr");
            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(parameterValuePassword);
            string query = "UPDATE Users SET Username = @UserName,Password = @Password WHERE UserID = @UserID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@Password", hashedPassword),
                  new SqlParameter("@UserName", parameterValueUserName),
                  new SqlParameter("@UserID", parameterValueID)
             };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("GetUsers");
        }
        [HttpGet]
        public ActionResult UpdateMe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateMe(User user, int id)
        {
            var parameterValueID = id;
            var parameterValueUserName = user.Username;
            String parameterValuePassword = user.Password;
            if (!UserCollectionPassword.TryGetValue(parameterValuePassword, out _))
            {
                UserCollectionPassword.Add(parameterValuePassword);
            }
            else RedirectToAction("Erorr");
            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(parameterValuePassword);
            string query = "UPDATE Users SET Username = @UserName,Password = @Password WHERE UserID = @UserID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter("@Password", hashedPassword),
                  new SqlParameter("@UserName", parameterValueUserName),
                  new SqlParameter("@UserID", parameterValueID)
             };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            Users u = new Users();
            u.UserID = user.UserID;
            return RedirectToAction("index", "Customer", u);
        }
        //מחיקת מנהל מוסך עי מנהל המערכת ע"פ קוד מנהל        ..njhe, nbvk nuxl gh nbvk vngrf, g"p eus nbvk
        //ומחיקת כל הנתונים הקשורים בו לקוחות,הרשאות,מוסכים 
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            var parameterValueId = id;
            List<Customers> listCustomer = new List<Customers>();
            List<Garage> listGarage = new List<Garage>();
            List<Vehicles> listVehicle = new List<Vehicles>();

            //שליפת הרשאות המוסכים של המנהל הנמחק
            string query = "SELECT * FROM GaragePermissions WHERE UserID =@UserId";
            SqlParameter parameter = new SqlParameter("UserID", parameterValueId);
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var listPermissionsGarage = db.Database.SqlQuery<GaragePermissions>(query, parameter).ToList();

                query = "SELECT* FROM Garage WHERE GarageID = @GarageId";
                foreach (var item in listPermissionsGarage)
                {//שליפת המוסכים של המנהל הנמחק ע"פ הרשאות
                    var garage = db.Database.SqlQuery<Garage>(query, new SqlParameter("GarageId", item.GarageID)).FirstOrDefault();
                    listGarage.Add(garage);
                }

                //שליפת הרשאות  של המנהל הנמחק
                query = "SELECT * FROM Permissions WHERE UserID = @UserId";
                var listPermissions = db.Database.SqlQuery<Permissions>(query, new SqlParameter("UserID", parameterValueId)).ToList();

                query = "SELECT * FROM Customers WHERE CustomerID= @CustomerId";
                foreach (var item in listPermissions)
                {//שליפת הלקוחות של המנהל הנמחק ע"פ הרשאות
                    var customer = db.Database.SqlQuery<Customers>(query, new SqlParameter("CustomerId", item.CustomerID)).FirstOrDefault();
                    listCustomer.Add(customer);
                }

                foreach (var item in listCustomer)
                {//שליפת ומחיקת הרכבים של כל הלקוחות המקושרים למנהל הנמחק
                    query = "SELECT * FROM Vehicles WHERE OwnerID=@OwnerID";
                    listVehicle = db.Database.SqlQuery<Vehicles>(query, new SqlParameter("OwnerID", item.CustomerID)).ToList();

                    foreach (var vehicle in listVehicle)
                    {
                        string query1 = "SELECT * FROM GarageVisits WHERE VehicleId = @VehicleId ";
                        var listVisit = db.Database.SqlQuery<GarageVisits>(query1, new SqlParameter("VehicleId", vehicle.VehicleID)).ToList();
                        foreach (var visit in listVisit)
                        {
                            string queryVisit = "DELETE FROM GarageVisits WHERE VisitID = @VisitID ";
                            db.Database.ExecuteSqlCommand(queryVisit, new SqlParameter("VisitID", visit.VisitID));
                        }

                        string query2 = "DELETE FROM Vehicles WHERE VehicleID = @VehicleID";
                        db.Database.ExecuteSqlCommand(query2, new SqlParameter("VehicleID", vehicle.VehicleID));

                    }
                }
                //מחיקת הנתונים
                foreach (var item in listPermissionsGarage)
                {
                    query = "DELETE FROM GaragePermissions WHERE PermissionID =@PermissionId";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("PermissionId", item.PermissionID));

                }
                foreach (var item in listGarage)
                {
                    query = "DELETE FROM Garage WHERE GarageID =@GarageID";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("GarageID", item.GarageID));
                }
                foreach (var item in listPermissions)
                {
                    query = "DELETE FROM Permissions WHERE PermissionID =@PermissionID";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("VehicleID", item.PermissionID));
                }
                foreach (var item in listCustomer)
                {
                    query = "DELETE FROM Customers WHERE CustomerID =@CustomerId";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("CustomerId", item.CustomerID));
                }

                //מחיקת המנהל בעצמו
                query = "DELETE FROM Users WHERE UserID =@UserId";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("UserId", id));
                return RedirectToAction("GetUsers");
            }
        }  
    
   

        //צפיה בכל המנהלי מוסך הקיימים במערכת עם אפשרויות דפדוף
        // וכן ניתוב לפונקציות עדכון ,מחיקה, הוספה, וניהול מוסכים והרשאות מוסך
        public ActionResult GetUsers(int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                string query = "SELECT * FROM Users ";
                var users = db.Database.SqlQuery<Users>(query).ToList().ToPagedList(page, PAGE_SIZE);
                ViewBag.HasNextPage = page < users.PageCount;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.PageIndex = page;
                ViewBag.TotaPages = users.PageCount;
                if (page + 10 <= users.PageCount)
                {
                    ViewBag.LoopEndPage = page + 9;
                }
                else
                {
                    ViewBag.LoopEndPage = users.PageCount;
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
                return View(users);
            }
        }

    }
}
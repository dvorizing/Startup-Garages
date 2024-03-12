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
    public class PermissionController : Controller
    {
        const int PAGE_SIZE = 1;
        public PermissionController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        //הוספת הרשאה עם קוד מנהל וקוד לקוח נתונים
        //Whis SQLparameters
        [HttpGet]
        public ActionResult AddPermission(Customer customer)
        {
            Permission Permission = new Permission(customer.id, customer.CustomerID);
            return View(Permission);
        }

        [HttpPost]
        public ActionResult AddPermission(Permission Permission)
        {
            var parameterValueCustomerId = Permission.CustomerID;
            var parameterValueUserId = Permission.UserID;
            var parameterValueView = Permission.CanView;
            var parameterValueEdit = Permission.CanEdit;
            string query = "INSERT INTO Permissions (CustomerID, UserID,CanView,CanEdit) VALUES  (@CustomerId, @UserId,@CanView,@CanEdit)  ";

            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@CustomerId", parameterValueCustomerId),
               new SqlParameter("@UserId", parameterValueUserId),
               new SqlParameter("@CanView", parameterValueView),
               new SqlParameter("@CanEdit", parameterValueEdit),
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("GetCustomers", "Customer", new { id = parameterValueUserId });
        }

        //עדכון הרשאה עי מנהל המוסך אם יש לו הרשאה תקינה
        [HttpGet]
        public ActionResult Update(int id)
        {
            var parameterValueID = id;
            string query = "Select * From Permissions WHERE PermissionID=@PermissionID ";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var Permission = db.Database.SqlQuery<Permissions>(query, new SqlParameter("PermissionID", parameterValueID)).FirstOrDefault();
                return View(Permission);
            }
        }

        [HttpPost]
        public ActionResult Update(Permissions Permission)
        {
            var ValueUserId = Permission.UserID;
            var parameterValueId = Permission.PermissionID;
            var parameterValueView = Permission.CanView;
            var parameterValueEdit = Permission.CanEdit;
            string query = "UPDATE Permissions SET CanView =@CanView ,CanEdit=@CanEdit WHERE PermissionID =@PermissionID ";
            SqlParameter[] parameters = new SqlParameter[]
          {
               new SqlParameter("@PermissionID", parameterValueId),
               new SqlParameter("@CanView", parameterValueView),
               new SqlParameter("@CanEdit", parameterValueEdit),
          };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("GetCustomers", "customer", new { id = ValueUserId });
        }

        [HttpGet]
        //מחיקת הרשאה ע"פ קוד הרשאה
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var parameterValueId = id;
            string query = "DELETE FROM GaragePermissions WHERE UserID = @UserId";
            SqlParameter[] parameters = new SqlParameter[]
            {
              new SqlParameter("@UserId", parameterValueId)
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("Delete", "User", parameterValueId);
        }

        [HttpGet]
        //צפייה בהרשאות של לקוח מסוים ע"פ קוד לקוח
        public ActionResult SeePermission(int id)
        {
            var parameterValueId = id;
            string query = "Select * From Permissions WHERE CustomerID = @CustomerID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var Permission = db.Database.SqlQuery<Permissions>(query, new SqlParameter("@CustomerID", parameterValueId)).ToList();
                return View(Permission);

            }


        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbirProjectCars.Models;
using AbirProjectCars.ViewModel;
using PagedList;
using System.Data.SqlClient;

namespace AbirProjectCars.Controllers
{
    public class GaragePermissionController : Controller
    {
        const int PAGE_SIZE = 1;
        public GaragePermissionController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //הוספת הרשאה למוסך עם קוד מוסך וקוד מנהל נתונים
        //האוביקט מוסך המועבר מכיך קוד מוסך וכן פונקציה ששולפת את קוד בהמנהל 
        //Whis SQLparameters
        public ActionResult Register(Garages garage)
        {
            GaragePermission garagePermission = new GaragePermission(garage.getUserUId(), garage.GarageID);
            return View(garagePermission);
        }

        [HttpPost]
        public ActionResult Register(GaragePermission garagePermission)
        {
            if (ModelState.IsValid)
            {
                var parameterValueGarageId = garagePermission.GarageID;
                var parameterValueUserId = garagePermission.UserID;
                var parameterValueView = garagePermission.CanView;
                var parameterValueEdit = garagePermission.CanEdit;

                string query = "INSERT INTO GaragePermissions (GarageID, UserID,CanView,CanEdit) VALUES (@GarageID, @UserID,@CanView,@CanEdit)";
                SqlParameter[] parameters = new SqlParameter[]
                {
               new SqlParameter("@GarageID", parameterValueGarageId),
               new SqlParameter("@UserID", parameterValueUserId),
               new SqlParameter("@CanView", parameterValueView),
               new SqlParameter("@CanEdit", parameterValueEdit),
                };
                using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
                {
                    db.Database.ExecuteSqlCommand(query, parameters);
                    return RedirectToAction("GetMyGarages", "Garage", new { id = garagePermission.UserID });

                }
            }
            return View();
        }

        //עדכון הרשאת מוסך רק ע"י מנהל המערכת ע"פ קוד הרשאה
        [HttpGet]
        public ActionResult Update(int id)
        {
            var parameterValueID = id;
            string query = "Select * From GaragePermissions WHERE PermissionID=@PermissionID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garagePermission = db.Database.SqlQuery<GaragePermissions>(query, new SqlParameter("PermissionID", parameterValueID)).FirstOrDefault();
                return View(garagePermission);
            }
        }
        [HttpPost]
        public ActionResult Update(GaragePermission garagePermission)
        {
            var parameterValueId = garagePermission.PermissionID;
            var parameterValueView = garagePermission.CanView;
            var parameterValueEdit = garagePermission.CanEdit;
            string query = "UPDATE GaragePermissions SET CanView = @CanView,CanEdit=@CanEdit WHERE PermissionID = @PermissionID";
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
            return RedirectToAction("GetUsers", "User");

        }

        //מחיקת הרשאה ע"י מנהל המערכת ע"פ קוד הרשאה
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var parameterValueId = id;
            string query = "DELETE FROM GaragePermissions WHERE UserID = @UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, new SqlParameter("UserID", parameterValueId));
            }
            return RedirectToAction("Delete", "User", parameterValueId);
        }

        [HttpGet]
        //צפייה בהרשאה של מוסך מסוים עם אפשרות ניתוב לפונקציית עדכון
        public ActionResult SeeGargePermission(int id)
        {
            var parameterValueId = id;
            string query = "Select * From GaragePermissions WHERE UserID= @UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garagePermission = db.Database.SqlQuery<GaragePermissions>(query, new SqlParameter("UserID", parameterValueId)).ToList();
                return View(garagePermission);

            }
        }
    }
}
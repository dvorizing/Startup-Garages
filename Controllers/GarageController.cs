using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbirProjectCars.ViewModel;
using AbirProjectCars.Models;
using PagedList;
using System.Data.SqlClient;

namespace AbirProjectCars.Controllers
{
    public class GarageController : Controller
    {
        const int PAGE_SIZE = 1;
        public GarageController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }


        //הIDמקצה אוביקט מוסך חדש ושולח לשמירה במערכת את   
        // (של המנהל (שיוכל להעביר בהקצאת ההרשאה
        //Whis SQLparameters
        [HttpGet]
        public ActionResult AddGarage(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@UserID", id),
            };

            string query = "SELECT AmountOfGarages FROM Users WHERE UserId = @UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                int remainingGarages = db.Database.SqlQuery<int>(query, parameters).FirstOrDefault();
                if (remainingGarages > 0)
                {
                    string queryUpdateSlots = "UPDATE Users SET AmountOfGarages = AmountOfGarages - 1 WHERE UserId = @UserID";
                    db.Database.ExecuteSqlCommand(queryUpdateSlots, new SqlParameter("@UserID", id));
                    Garages garage = new Garages(id);
                    return View(garage);
                }
                else

                    TempData["ErrorMessage"] = "Sorry, the amount of garages you can add is over";
                return RedirectToAction("GetMyGarages", new { id = id });

            }

        }

        [HttpPost]
        public ActionResult AddGarage(Garages garage)
        {
            var parameterValueName = garage.GarageName;
            var parameterValuePhon = garage.PhoneNumber;
            var parameterValueAdress = garage.Address;

            string query = "INSERT INTO Garage (GarageName, PhoneNumber,Address) VALUES (@GarageName, @PhoneNumber,@Address)";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@GarageName", parameterValueName),
               new SqlParameter("@PhoneNumber", parameterValuePhon),
               new SqlParameter("@Address", parameterValueAdress),
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
                query = "SELECT GarageId FROM Garage WHERE GarageName =@GarageName";
                var id = db.Database.SqlQuery<int>(query, new SqlParameter("GarageName", parameterValueName)).FirstOrDefault();
                garage.GarageID = id;
                return RedirectToAction("Register", "GaragePermission", garage);
            }
        }
        //לא השתמשתי
        [HttpGet]
        public ActionResult GetById(int id)
        {
            var parameterValueID = id;
            string query = "SELECT * FROM Garage WHERE GarageID=@GarageId";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garage = db.Database.SqlQuery<Garage>(query, new SqlParameter("GarageID", id)).FirstOrDefault();
                if (garage != null)
                    return View(garage);
                return View();
            }
        }

        //עדכון פרטי מוסך עי מנהל המערכת ע"פ קוד מוסך
        [HttpGet]
        public ActionResult Update(int id, int UserId = 0)
        {
            Garages garage = new Garages(UserId);
            string query = "Select * From Garage WHERE GarageID = @GarageId";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garageselect = db.Database.SqlQuery<Garage>(query, new SqlParameter("GarageID", id)).FirstOrDefault();
                garage.GarageID = garageselect.GarageID;
                garage.GarageName = garageselect.GarageName;
                garage.PhoneNumber = garageselect.PhoneNumber;
                garage.Address = garageselect.Address;
                return View(garage);
            }
        }
        [HttpPost]
        public ActionResult Update(Garages garage)
        {
            var parameterValueID = garage.GarageID;
            var parameterValueName = garage.GarageName;
            var parameterValuePhon = garage.PhoneNumber;
            var parameterValueAdress = garage.Address;
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@GarageID", parameterValueID),
               new SqlParameter("@GarageName", parameterValueName),
               new SqlParameter("@PhoneNumber", parameterValuePhon),
               new SqlParameter("@Address", parameterValueAdress),
            };
            string query = "UPDATE Garage SET  GarageName = @GarageName,PhoneNumber=@PhoneNumber,Address=@Address WHERE GarageID =@GarageID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
                if (garage.getUserUId() != 0)
                    return RedirectToAction("GetMyGarages", new { id = garage.getUserUId() });
                return RedirectToAction("GetGarages");
            }
        }

        //מחיקת מוסך ע"פ קוד מוסך וכן מחיקת כל ההרשאות והביקורים הקשורים בו
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //שליפת ומחיקת כל ההרשאת מוסך שקשורות במוסך הנמחק
            var parameterValueId = id;
            string query = "SELECT * FROM GaragePermissions WHERE GarageID =@GarageID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var listPermissions = db.Database.SqlQuery<GaragePermissions>(query, new SqlParameter("GarageID", parameterValueId)).ToList();
                foreach (var item in listPermissions)
                {
                    query = "DELETE FROM GaragePermissions WHERE PermissionID=@PermissionID";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("PermissionID", item.PermissionID));
                }

                //שליפת ומחיקת כל דוחות מוסך שקשורים במוסך הנמחק
                query = "SELECT * FROM GarageVisits WHERE GarageID =@GarageID";
                var listVisites = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("GarageID", parameterValueId)).ToList();
                foreach (var item in listVisites)
                {
                    query = "DELETE FROM GarageVisits WHERE VisitID=@VisitID";
                    db.Database.ExecuteSqlCommand(query, new SqlParameter("PermissionID", item.VisitID));
                }

                query = "DELETE FROM Garage WHERE GarageID =@GarageID";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("GarageID", parameterValueId));
                return RedirectToAction("GetGarages");
            }


        }

        //קבלת מוסכים של מנהל מסוים ע"פ קוד מנהל (מקושר לפי הרשאות מוסך)ד 
        //עם אפשרויות עדכון ומחיקה ע"הרשאה נתונה 
        //עם אפשרות דפדוף בתוצאות ובחירת עמוד מבוקש
        public ActionResult GetMyGarages(int id, int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            ViewBag.UserId = id;
            List<Garages> listGarage = new List<Garages>();
            var parameterValueId = id;
            ViewBag.UserId = id;
            string query = "Select * From GaragePermissions WHERE UserID=@UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garagesPermission = db.Database.SqlQuery<GaragePermissions>(query, new SqlParameter("UserID", parameterValueId)).ToList();
                foreach (var item in garagesPermission)
                {
                    string query1 = "SELECT * FROM Garage WHERE GarageID=@GarageID";
                    var garageDB = db.Database.SqlQuery<Garage>(query1, new SqlParameter("GarageID", item.GarageID)).FirstOrDefault();
                    Garages garage = new Garages();
                    garage.GarageID = garageDB.GarageID;
                    garage.GarageName = garageDB.GarageName;
                    garage.PhoneNumber = garageDB.PhoneNumber;
                    garage.Address = garageDB.Address;                 
                    garage.setCanView((bool)item.CanView);
                    garage.setCanEdit((bool)item.CanEdit);
                    listGarage.Add(garage);
                }
            }

            var garages = listGarage.ToPagedList(page, PAGE_SIZE);
            ViewBag.HasNextPage = page < garages.PageCount;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.PageIndex = page;
            ViewBag.TotaPages = garages.PageCount;
            if (page + 10 <= garages.PageCount)
            {
                ViewBag.LoopEndPage = page + 9;
            }
            else
            {
                ViewBag.LoopEndPage = garages.PageCount;
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
            return View(garages);

        }

        //קבלת כל המוסכים שיש במערכת עם אפשרות דפדוף בתוצאות ובחירת עמוד מבוקש
        public ActionResult GetGarages(int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            string query = "SELECT * FROM Garage";        

            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garages = db.Database.SqlQuery<Garage>(query).ToList().ToPagedList(page, PAGE_SIZE);
                ViewBag.HasNextPage = page < garages.PageCount;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.PageIndex = page;
                ViewBag.TotaPages = garages.PageCount;
                if (page + 10 <= garages.PageCount)
                {
                    ViewBag.LoopEndPage = page + 9;
                }
                else
                {
                    ViewBag.LoopEndPage = garages.PageCount;
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
                return View(garages);

            }

        }
    }
}
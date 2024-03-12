using AbirProjectCars.Models;
using AbirProjectCars.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbirProjectCars.Controllers
{
    public class VehicleController : Controller
    {
        const int PAGE_SIZE = 5;
        public VehicleController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        //הוספת רכב ללקוח מסויים
        //Whis SQLparameters
        [HttpGet]
        public ActionResult AddVehicle(int id)
        {
            Vehicles vehicle = new Vehicles();
            vehicle.OwnerID = id;
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult AddVehicle(Vehicles vehicle)
        {
            var parameterValueMake = vehicle.Make;
            var parameterValueLastModel = vehicle.Model;
            var parameterValueYear = vehicle.Year;
            var parameterValueVin = vehicle.VIN;
            var parameterValueMileage = vehicle.Mileage;
            var parameterValueLastServiceDate = vehicle.LastServiceDate;
            var parameterValueOwnerId = vehicle.OwnerID;

            var query = "INSERT INTO Vehicles (Make, Model, Year, VIN, Mileage, LastServiceDate, OwnerID) " +
                         "VALUES (@Make, @Model, @Year, @Vin, @Mileage, @LastServiceDate, @OwnerId)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                     new SqlParameter("@Make", parameterValueMake),
                     new SqlParameter("@Model", parameterValueLastModel),
                     new SqlParameter("@Year", parameterValueYear),
                     new SqlParameter("@Vin", parameterValueVin),
                     new SqlParameter("@Mileage", parameterValueMileage),
                     new SqlParameter("@LastServiceDate", parameterValueLastServiceDate),
                     new SqlParameter("@OwnerId", parameterValueOwnerId)
             };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
            }
            return RedirectToAction("GetVehicles", new { id = parameterValueOwnerId });
        }

        //לא השתמשתי
        [HttpPost]
        public ActionResult GetById(int id)
        {
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                string query = "Select * From Vehicles WHERE VehicleID=@VehicleID";
                var vehicle = db.Database.SqlQuery<Vehicles>(query, new SqlParameter("VehicleID", id)).FirstOrDefault();
                return View(vehicle);
            }
            return View();
        }

        //עדכון פרטי רכב ע"פ קוד רכב
        [HttpGet]
        public ActionResult Update(int id)
        {
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                string query = "Select * From Vehicles WHERE VehicleID=@VehicleID";
                var vehicle = db.Database.SqlQuery<Vehicles>(query, new SqlParameter("VehicleID", id)).FirstOrDefault();
                return View(vehicle);

            }
        }

        [HttpPost]
        public ActionResult Update(Vehicles vehicle)
        {
            if (ModelState.IsValid)
            {
                var parameterValueId = vehicle.VehicleID;
                var parameterValueMake = vehicle.Make;
                var parameterValueLastModel = vehicle.Model;
                var parameterValueYear = vehicle.Year;
                var parameterValueVin = vehicle.VIN;
                var parameterValueMileage = vehicle.Mileage;
                var parameterValueLastServiceDate = vehicle.LastServiceDate;
                var parameterValueOwnerId = vehicle.OwnerID;
                string query = $"UPDATE Vehicles SET  Make = @Make ,Model=@Model,Year=@Year,VIN=@VIN,Mileage=@Mileage, LastServiceDate =@LastServiceDate,OwnerID=@OwnerID WHERE VehicleID =@VehicleID";
                SqlParameter[] parameters = new SqlParameter[]
            {
                     new SqlParameter("@Make", parameterValueMake),
                     new SqlParameter("@Model", parameterValueLastModel),
                     new SqlParameter("@Year", parameterValueYear),
                     new SqlParameter("@VIN", parameterValueVin),
                     new SqlParameter("@Mileage", parameterValueMileage),
                     new SqlParameter("@LastServiceDate", parameterValueLastServiceDate),
                     new SqlParameter("@OwnerID", parameterValueOwnerId),
                     new SqlParameter("@VehicleID", parameterValueId)
             };
                using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
                {
                    db.Database.ExecuteSqlCommand(query, parameters);
                }
                return RedirectToAction("GetVehicles", new { id = parameterValueOwnerId });
            }

            return View(vehicle);
        }

        //מחיקת  רכב ע"פ קוד רכב
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            string query = "SELECT * FROM Vehicles WHERE VehicleID=@VehicleID";
            SqlParameter parameter = new SqlParameter("VehicleID", id);
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var vehicle = db.Database.SqlQuery<Vehicles>(query, parameter).FirstOrDefault();
                var parameterValueOwnerId = vehicle.OwnerID;
                string queryVisit = "SELECT * FROM GarageVisits WHERE VehicleID=@VehicleID";
                var visits = db.Database.SqlQuery<GarageVisits>(queryVisit, new SqlParameter("VehicleID",id)).ToList();
                string queryVisitDelet = "DELETE FROM GarageVisits WHERE VisitID=@VisitID";
                foreach (var item in visits)
                {
                    db.Database.ExecuteSqlCommand(queryVisitDelet, new SqlParameter("VisitID", item.VisitID));
                }
                string query2 = "DELETE FROM Vehicles WHERE VehicleID=@VehicleID";
                db.Database.ExecuteSqlCommand(query2, new SqlParameter("VehicleID", id));
                return RedirectToAction("GetVehicles", new { id = parameterValueOwnerId });
            }
        }

        //צפייה ברכים של לקוח מסויים ע"פ קוד לקוח
        //עם אפשרות דפדוף ובחיקת דף רצוי
        public ActionResult GetVehicles(int id, int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            string query = "SELECT * FROM Vehicles WHERE OwnerID=@OwnerID ";
            ViewBag.OwnerID = id;
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var vehicles = db.Database.SqlQuery<Vehicles>(query, new SqlParameter("OwnerID", id)).ToList().ToPagedList(page, PAGE_SIZE);
                ViewBag.HasNextPage = page < vehicles.PageCount;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.PageIndex = page;
                ViewBag.TotaPages = vehicles.PageCount;
                if (page + 10 <= vehicles.PageCount)
                {
                    ViewBag.LoopEndPage = page + 9;
                }
                else
                {
                    ViewBag.LoopEndPage = vehicles.PageCount;
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
                return View(vehicles);
            }
        }
    }
}
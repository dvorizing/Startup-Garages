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
    public class GarageVisitController : Controller
    {
        const int PAGE_SIZE = 1;
        public GarageVisitController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        //הוספת ביקור למוסך צפציפי ע"פ קוד מוסך
        //Whis SQLparameters
        [HttpGet]
        public ActionResult Register(int GarageId)
        {
            GarageVisit garageVisit = new GarageVisit(GarageId);
            return View(garageVisit);
        }


        public ActionResult Register(GarageVisit garageVisit)
        {
            var parameterValueLastGarage = garageVisit.GarageID;
            var parameterValueVehicle = garageVisit.VehicleID;
            var parameterValueDate = garageVisit.VisitDate;
            var OilVolume = garageVisit.OilVolume;
            var OilType = garageVisit.OilType;       
            var TiresairCuttingAbrasive = garageVisit.TiresairCuttingAbrasive;
            var TiresairTireChange = garageVisit.TiresairTireChange;
            var BrakingFluidLevel = garageVisit.BrakingFluidLevel;
            var BrakingBrakeWork = garageVisit.BrakingBrakeWork;
            var BatteryFluidLevel = garageVisit.BatteryFluidLevel;
            var BatteryConnections = garageVisit.BatteryConnections;
            var ElectricaLlightBulbs = garageVisit.ElectricaLlightBulbs;
            var ElectricalSwitches = garageVisit.ElectricalSwitches;
            var ElectricalConnections = garageVisit.ElectricalConnections;
            var CoolingCoolant = garageVisit.CoolingCoolant;
            var CoolingConnections = garageVisit.CoolingConnections;
            var BracesFluidLevel = garageVisit.BracesFluidLevel;
            var BracesConnections = garageVisit.BracesConnections;
            var ReplacementOilFilter = garageVisit.ReplacementOilFilter;
            var ReplacementAirFilter = garageVisit.ReplacementAirFilter;
            var ReplacementFuelFilter = garageVisit.ReplacementFuelFilter;
            var TreatmentNote = garageVisit.TreatmentNote;
            var Signature = garageVisit.Signiture;
            var DateReport = DateTime.Now;
            var parameterValueCost = garageVisit.TotalCost;

            string query = "INSERT INTO GarageVisits (VehicleID,GarageID,VisitDate,OilVolume,OilType,TiresairCuttingAbrasive,TiresairTireChange,BrakingFluidLevel,BrakingBrakeWork," +
                "BatteryFluidLevel,BatteryConnections,ElectricaLlightBulbs,ElectricalSwitches,ElectricalConnections,CoolingCoolant,CoolingConnections," +
                "BracesFluidLevel,BracesConnections,ReplacementOilFilter,ReplacementAirFilter,ReplacementFuelFilter,TreatmentNote,Signiture,DateReport,TotalCost) " +
                " VALUES " +
                "(@VehicleID, @GarageID, @VisitDate, @OilVolume, @OilType, @TiresairCuttingAbrasive, @TiresairTireChange, @BrakingFluidLevel, @BrakingBrakeWork, " +
                "@BatteryFluidLevel,@BatteryConnections,@ElectricaLlightBulbs,@ElectricalSwitches,@ElectricalConnections,@CoolingCoolant,@CoolingConnections," +
                "@BracesFluidLevel,@BracesConnections,@ReplacementOilFilter,@ReplacementAirFilter,@ReplacementFuelFilter,@TreatmentNote,@Signiture,@DateReport,@TotalCost) ";
           
            string query1 = "UPDATE Vehicles SET LastServiceDate =@VisitDate WHERE VehicleID=@VehicleID ";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@VehicleID", parameterValueVehicle),
               new SqlParameter("@GarageID", parameterValueLastGarage),
               new SqlParameter("@VisitDate", parameterValueDate),
               new SqlParameter("@OilVolume", OilVolume),
               new SqlParameter("@OilType", OilType),
               new SqlParameter("@TiresairCuttingAbrasive", TiresairCuttingAbrasive),
               new SqlParameter("@TiresairTireChange", TiresairTireChange),
               new SqlParameter("@BrakingFluidLevel", BrakingFluidLevel),
               new SqlParameter("@BrakingBrakeWork", BrakingBrakeWork),
               new SqlParameter("@BatteryFluidLevel", BatteryFluidLevel),
               new SqlParameter("@BatteryConnections", BatteryConnections),
               new SqlParameter("@ElectricaLlightBulbs", ElectricaLlightBulbs),
               new SqlParameter("@ElectricalSwitches", ElectricalSwitches),
               new SqlParameter("@ElectricalConnections", ElectricalConnections),
               new SqlParameter("@CoolingCoolant", CoolingCoolant),
               new SqlParameter("@CoolingConnections", CoolingConnections),
               new SqlParameter("@BracesFluidLevel", BracesFluidLevel),
               new SqlParameter("@BracesConnections", BracesConnections),
               new SqlParameter("@ReplacementOilFilter", ReplacementOilFilter),
               new SqlParameter("@ReplacementAirFilter", ReplacementAirFilter),
               new SqlParameter("@ReplacementFuelFilter", ReplacementFuelFilter),
               new SqlParameter("@TreatmentNote", TreatmentNote),
               new SqlParameter("@Signiture", Signature),
               new SqlParameter("@DateReport", DateReport),
               new SqlParameter("@TotalCost", parameterValueCost)
            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
                db.Database.ExecuteSqlCommand(query1, new SqlParameter("@VisitDate", parameterValueDate), new SqlParameter("@VehicleID", parameterValueVehicle));
            }
            return RedirectToAction("GetVisites", new { id = parameterValueLastGarage });
        }

        //לא השתמשתי
        [HttpPost]
        public ActionResult GetById(int id)
        {
            var parameterValueID = id;
            string query = "SELECT * FROM Vehicles WHERE UserID=@UserID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var garageP = db.Database.SqlQuery<Garage>(query, new SqlParameter("UserID", id)).FirstOrDefault();
                if (garageP != null)
                    return RedirectToAction("Sucsses", garageP);
                return View();
            }
        }

        //עדכון פרטי ביקור ע"פ קוד ביקור
        [HttpGet]
        public ActionResult Update(int id)
        {
            var parameterValueID = id;
            string query = "Select * From GarageVisits WHERE VisitID=@VisitID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var visit = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("VisitID", parameterValueID)).FirstOrDefault();
                return View(visit);
            }
        }
        [HttpPost]
        public ActionResult Update(GarageVisits garageVisit)
        {

            var parameterID = garageVisit.VisitID;
            var parameterValueLastGarage = garageVisit.GarageID;
            var parameterValueVehicle = garageVisit.VehicleID;
            var parameterValueDate = garageVisit.VisitDate;
            var OilVolume = garageVisit.OilVolume;
            var OilType = garageVisit.OilType;
            var TiresairCuttingAbrasive = garageVisit.TiresairCuttingAbrasive;
            var TiresairTireChange = garageVisit.TiresairTireChange;
            var BrakingFluidLevel = garageVisit.BrakingFluidLevel;
            var BrakingBrakeWork = garageVisit.BrakingBrakeWork;
            var BatteryFluidLevel = garageVisit.BatteryFluidLevel;
            var BatteryConnections = garageVisit.BatteryConnections;
            var ElectricaLlightBulbs = garageVisit.ElectricaLlightBulbs;
            var ElectricalSwitches = garageVisit.ElectricalSwitches;
            var ElectricalConnections = garageVisit.ElectricalConnections;
            var CoolingCoolant = garageVisit.CoolingCoolant;
            var CoolingConnections = garageVisit.CoolingConnections;
            var BracesFluidLevel = garageVisit.BracesFluidLevel;
            var BracesConnections = garageVisit.BracesConnections;
            var ReplacementOilFilter = garageVisit.ReplacementOilFilter;
            var ReplacementAirFilter = garageVisit.ReplacementAirFilter;
            var ReplacementFuelFilter = garageVisit.ReplacementFuelFilter;
            var TreatmentNote = garageVisit.TreatmentNote;
            var Signature = garageVisit.Signiture;
            var DateReport = DateTime.Now;
            var parameterValueCost = garageVisit.TotalCost;

            //string query = " UPDATE GarageVisits SET (VehicleID,GarageID,VisitDate,OilVolume,OilType,TiresairCuttingAbrasive,TiresairTireChange,BrakingFluidLevel,BrakingBrakeWork," +
            //    "BatteryFluidLevel,BatteryConnections,ElectricaLlightBulbs,ElectricalSwitches,ElectricalConnections,CoolingCoolant,CoolingConnections," +
            //    "BracesFluidLevel,BracesConnections,ReplacementOilFilter,ReplacementAirFilter,ReplacementFuelFilter,TreatmentNote,Signiture,DateReport,TotalCost) " +
            //    " VALUES " +
            //    "(@VehicleID, @GarageID, @VisitDate, @OilVolume, @OilType, @TiresairCuttingAbrasive, @TiresairTireChange, @BrakingFluidLevel, @BrakingBrakeWork, " +
            //    "@BatteryFluidLevel,@BatteryConnections,@ElectricaLlightBulbs,@ElectricalSwitches,@ElectricalConnections,@CoolingCoolant,@CoolingConnections," +
            //    "@BracesFluidLevel,@BracesConnections,@ReplacementOilFilter,@ReplacementAirFilter,@ReplacementFuelFilter,@TreatmentNote,@Signiture,@DateReport,@TotalCost) "+
            //    " WHERE VisitID=@VisitID";

            string query = "UPDATE GarageVisits SET " +
               "VehicleID = @VehicleID, " +
               "GarageID = @GarageID, " +
               "VisitDate = @VisitDate, " +
               "OilVolume = @OilVolume, " +
               "OilType = @OilType, " +
               "TiresairCuttingAbrasive = @TiresairCuttingAbrasive, " +
               "TiresairTireChange = @TiresairTireChange, " +
               "BrakingFluidLevel = @BrakingFluidLevel, " +
               "BrakingBrakeWork = @BrakingBrakeWork, " +
               "BatteryFluidLevel = @BatteryFluidLevel, " +
               "BatteryConnections = @BatteryConnections, " +
               "ElectricaLlightBulbs = @ElectricaLlightBulbs, " +
               "ElectricalSwitches = @ElectricalSwitches, " +
               "ElectricalConnections = @ElectricalConnections, " +
               "CoolingCoolant = @CoolingCoolant, " +
               "CoolingConnections = @CoolingConnections, " +
               "BracesFluidLevel = @BracesFluidLevel, " +
               "BracesConnections = @BracesConnections, " +
               "ReplacementOilFilter = @ReplacementOilFilter, " +
               "ReplacementAirFilter = @ReplacementAirFilter, " +
               "ReplacementFuelFilter = @ReplacementFuelFilter, " +
               "TreatmentNote = @TreatmentNote, " +
               "Signiture = @Signiture, " +
               "DateReport = @DateReport, " +
               "TotalCost = @TotalCost " +
               "WHERE VisitID = @VisitID";

            string query1 = "UPDATE Vehicles SET LastServiceDate =@VisitDate WHERE VehicleID=@VehicleID ";
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@VehicleID", parameterValueVehicle),
               new SqlParameter("@GarageID", parameterValueLastGarage),
               new SqlParameter("@VisitDate", parameterValueDate),
               new SqlParameter("@OilVolume", OilVolume),
               new SqlParameter("@OilType", OilType),
               new SqlParameter("@TiresairCuttingAbrasive", TiresairCuttingAbrasive),
               new SqlParameter("@TiresairTireChange", TiresairTireChange),
               new SqlParameter("@BrakingFluidLevel", BrakingFluidLevel),
               new SqlParameter("@BrakingBrakeWork", BrakingBrakeWork),
               new SqlParameter("@BatteryFluidLevel", BatteryFluidLevel),
               new SqlParameter("@BatteryConnections", BatteryConnections),
               new SqlParameter("@ElectricaLlightBulbs", ElectricaLlightBulbs),
               new SqlParameter("@ElectricalSwitches", ElectricalSwitches),
               new SqlParameter("@ElectricalConnections", ElectricalConnections),
               new SqlParameter("@CoolingCoolant", CoolingCoolant),
               new SqlParameter("@CoolingConnections", CoolingConnections),
               new SqlParameter("@BracesFluidLevel", BracesFluidLevel),
               new SqlParameter("@BracesConnections", BracesConnections),
               new SqlParameter("@ReplacementOilFilter", ReplacementOilFilter),
               new SqlParameter("@ReplacementAirFilter", ReplacementAirFilter),
               new SqlParameter("@ReplacementFuelFilter", ReplacementFuelFilter),
               new SqlParameter("@TreatmentNote", TreatmentNote),
               new SqlParameter("@Signiture", Signature),
               new SqlParameter("@DateReport", DateReport),
               new SqlParameter("@TotalCost", parameterValueCost),
                 new SqlParameter("@VisitID", parameterID)

            };
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                db.Database.ExecuteSqlCommand(query, parameters);
                db.Database.ExecuteSqlCommand(query1, new SqlParameter("@VisitDate", parameterValueDate), new SqlParameter("@VehicleID", parameterValueVehicle));
            }
            return RedirectToAction("GetVisites", new { id = parameterValueLastGarage });

        }

        //מחיקת ביקור ע"פ קוד ביקור
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            var parameterValueId = id;
            string query = "SELECT * FROM GarageVisits WHERE VisitID=@VisitID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var visit = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("VisitID", parameterValueId)).FirstOrDefault();
                var garageId = visit.GarageID;
                query = "DELETE FROM GarageVisits WHERE VisitID=@VisitID";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("VisitID", parameterValueId));
                return RedirectToAction("GetVisites", new { id = garageId });
            }
        }

        //קבלת ביקורים לפי מוסך מסוים ע"פ קוד מוסך עם אפשרות דפדוף בתוצאות ובחירת עמוד מבוקש
        public ActionResult GetVisites(int id, int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            ViewBag.GarageId = id;
            string query = "SELECT * FROM GarageVisits WHERE GarageID=@GarageID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var visits = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("GarageID", id)).ToList().ToPagedList(page, PAGE_SIZE);
                ViewBag.HasNextPage = page < visits.PageCount;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.PageIndex = page;
                ViewBag.TotaPages = visits.PageCount;
                if (page + 10 <= visits.PageCount)
                {
                    ViewBag.LoopEndPage = page + 9;
                }
                else
                {
                    ViewBag.LoopEndPage = visits.PageCount;
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
                return View(visits);
            }


        }
        // קבלת דוח ביקורים לפי קוד רכב
        //עם אפשרות דפדוף בתוצאות ובחירת עמוד מבוקש
        public ActionResult GetMyVisites(int id, int page = 1, bool IsMooveP = false, bool IsMooveN = false)
        {
            ViewBag.GarageId = id;
            string query = "SELECT * FROM GarageVisits WHERE VehicleID=@VehicleID";
            using (MVC_Abir_GarageDBEntities db = new MVC_Abir_GarageDBEntities())
            {
                var visits = db.Database.SqlQuery<GarageVisits>(query, new SqlParameter("VehicleID", id)).ToList().ToPagedList(page, PAGE_SIZE);
                ViewBag.HasNextPage = page < visits.PageCount;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.PageIndex = page;
                ViewBag.TotaPages = visits.PageCount;
                if (page + 10 <= visits.PageCount)
                {
                    ViewBag.LoopEndPage = page + 9;
                }
                else
                {
                    ViewBag.LoopEndPage = visits.PageCount;
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
                return View(visits);
            }


        }
    }
}
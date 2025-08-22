using FACULTY_PORTAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FACULTY_PORTAL.Models;
using System.Data.SqlClient;

namespace FACULTY_PORTAL.Controllers
{
    public class CustomChallanController : Controller
    {
        public class AccountsOnlyAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var session = filterContext.HttpContext.Session;
                if (session["UserTypeActive"] == null || session["UserTypeActive"].ToString() != "Accounts")
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                base.OnActionExecuting(filterContext);
            }
        }
        [AccountsOnly]
        public ActionResult ChallanType()
        {
            DAL d = new DAL();
            DataSet ds = d.Select("SELECT ChallanTypeID, ChallanType FROM Challan_Types");
            ViewBag.ChallanTypeList = ds.Tables[0];
            return View();
        }

        // GET: Hostel
        public ActionResult CustomChallan()
        {
            DAL d = new DAL();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            //get degree info
            string s = "select DegreeName from DegreeInfo Order by DegreeName";
            ds = d.Select(s);
            int num = ds.Tables[0].Rows.Count;
            ViewBag.TotalDegree = num;
            string[,] array = new string[num, 1];
            for (int i = 0; i < num; i++)
            {
                array[i, 0] = ds.Tables[0].Rows[i][0].ToString();
            }
            // ds.Clear();
            // ds.Tables[0].Columns.Clear();

            //get batch info
            string s1 = "select SemesterSession,ID from SemesterSessionInfo Where SemesterSession NOT LIKE 'sU%' order by ID desc";
            ds1 = d.Select(s1);
            int num1 = ds1.Tables[0].Rows.Count;
            ViewBag.Batch = ds1.Tables[0].Rows.Count;
            string[,] array1 = new string[num1, 2];
            for (int i = 0; i < num1; i++)
            {
                array1[i, 0] = ds1.Tables[0].Rows[i][0].ToString();
                array1[i, 1] = ds1.Tables[0].Rows[i][1].ToString();
            }
            DateTime datetime = DateTime.Now;
            //ds.Clear();
            //ds.Tables[0].Columns.Clear();

            ViewBag.array = array;
            ViewBag.array1 = array1;

            //get HeadDetails data sent to view START
            string s6 = "select ChallanTypeID, ChallanType from Challan_Types";
            ds2 = d.Select(s6);
            int num5 = ds2.Tables[0].Rows.Count;
            ViewBag.challantypecount = num5;
            string[,] challantypes = new string[num5, 3];
            for (int i = 0; i < num5; i++)
            {
                challantypes[i, 0] = ds2.Tables[0].Rows[i][0].ToString();
                challantypes[i, 1] = ds2.Tables[0].Rows[i][1].ToString();
            }
            ViewBag.challantypes = challantypes;

            //get HeadDetails data sent to view END

            return View();
        }

        [HttpPost]
        public ActionResult CustomChallan(FormCollection data)
        {
            CustomChallan();
            //data sent to form
            DAL d = new DAL();
            DataSet ds = new DataSet();
            DataSet fds = new DataSet();
            DataSet fds1 = new DataSet();
            DataSet hstddata = new DataSet();
            DataSet challantype = new DataSet();
            DataSet feeid = new DataSet();
            DataSet challaninfo = new DataSet();
            DataSet challandetail = new DataSet();
            DataSet Admissiondetail = new DataSet();
            DataSet hosteloutstanding = new DataSet();
            //get student data and sent to view START
            string Batch = Convert.ToString(data["hostelbatch"]);
            string Degree = Convert.ToString(data["hosteldegree"]);
            string Sequence = Convert.ToString(data["hostelsequence"]);
            int challantypeid = Convert.ToInt32(data["challantype"]);

            if (Batch == "Batch" || Degree == "Degree" || Sequence == "")
            {
                return View();
            }
            string s = "select  StdRollNo, StudentName, FatherName, ID from StudentInfo where StdRollNo = ((select SemesterSession from SemesterSessionInfo where id = '" + Batch + "') + '/' +(select DegreeName from DegreeInfo where DegreeName = '" + Degree + "') + '/' + '" + Sequence + "')";
            ds = d.Select(s);
            int num = ds.Tables[0].Rows.Count;
            string stdrollno = ds.Tables[0].Rows[0][0].ToString();
            string stdrollnoid = ds.Tables[0].Rows[0][3].ToString();
            ViewBag.stdinfo = num;
            string[,] stdarray = new string[num, 4];
            for (int i = 0; i < num; i++)
            {
                stdarray[i, 0] = ds.Tables[0].Rows[i][0].ToString();
                stdarray[i, 1] = ds.Tables[0].Rows[i][1].ToString();
                stdarray[i, 2] = ds.Tables[0].Rows[i][2].ToString();
                stdarray[i, 3] = ds.Tables[0].Rows[i][3].ToString();                
            }
            
            ViewBag.stdarray = stdarray;
            //get student data and sent to view END


            //get challan info data sent to view START'

            string s5 = "select ChallanNo, ChallanTypeID, IssueDate, DueDate, DebitAmount, CreditAmount, PaidDate from ChallanInfo_HTF where StdRollNoIdORuserID =  '" + stdrollnoid + "' and ChallanTypeID = " + challantypeid + " order by ChallanNo desc";
            challaninfo = d.Select(s5);
            int num4 = challaninfo.Tables[0].Rows.Count;
            ViewBag.hostelchallaninfocount = num4;
            string[] challans = new string[num4];
            string[,] hostelchallaninfo = new string[num4, 7];
            for (int i = 0; i < num4; i++)
            {
                hostelchallaninfo[i, 0] = challaninfo.Tables[0].Rows[i][0].ToString();
                hostelchallaninfo[i, 1] = challaninfo.Tables[0].Rows[i][1].ToString();
                hostelchallaninfo[i, 2] = challaninfo.Tables[0].Rows[i][2].ToString();
                hostelchallaninfo[i, 3] = challaninfo.Tables[0].Rows[i][3].ToString();
                hostelchallaninfo[i, 4] = challaninfo.Tables[0].Rows[i][4].ToString();
                hostelchallaninfo[i, 5] = challaninfo.Tables[0].Rows[i][5].ToString();
                hostelchallaninfo[i, 6] = challaninfo.Tables[0].Rows[i][6].ToString();
            }
            if (num4 > 0)
            {
                ViewBag.hostelchallaninfo = hostelchallaninfo;
            }
            else
            {
                ViewBag.hostelchallaninfo = null;
            }
            
            //get HeadDetails data sent to view START
            string s6 = "select ChallanTypeID, ChallanType from Challan_Types";
            challantype = d.Select(s6);
            int num5 = challantype.Tables[0].Rows.Count;
            ViewBag.challantypecount = num5;
            string[,] challantypes = new string[num5, 3];
            for (int i = 0; i < num5; i++)
            {
                challantypes[i, 0] = challantype.Tables[0].Rows[i][0].ToString();
                challantypes[i, 1] = challantype.Tables[0].Rows[i][1].ToString();
            }
            ViewBag.challantypes = challantypes;

            //get HeadDetails data sent to view END

            //get outstanding payment START
            //string s7 = "select sum(debitamount) -sum(creditamount) from ChallanInfo_HTF where StdRollNoIdORuserID =  '" + stdrollnoid + "'";
            //hosteloutstanding = d.Select(s7);
            //string[,] hosteloutstand = new string[1, 1];
            //hosteloutstand[0, 0] = hosteloutstanding.Tables[0].Rows[0][0].ToString();
            //ViewBag.hostelstand = hosteloutstand;
            //get outstanding payment END




            return View();
        }

        [HttpPost]
        public ActionResult InsertCusChallan(FormCollection subform)
        {
            int rollnumberid = 0;
            string issuedate = "";
            string duedate = "";
            string challantypeid = "";
            string challandesc = "";
            string debitamount = "";
            
            if (subform["input-stdrollnoid"] != "")
            {
                rollnumberid = Convert.ToInt32(subform["input-stdrollnoid"]);
            }
            if (subform["input-customchissuedate"] != "")
            {
                issuedate = Convert.ToString(subform["input-customchissuedate"]);
            }
            if (subform["input-customchduedate"] != "")
            {
                duedate = Convert.ToString(subform["input-customchduedate"]);
            }
            if (subform["challantype"] != "")
            {
                challantypeid = Convert.ToString(subform["challantype"]);
            }
            if (subform["input-challandesc"] != "")
            {
                challandesc = Convert.ToString(subform["input-challandesc"]);
            }
            if (subform["input-customchamount"] != "")
            {
                debitamount = Convert.ToString(subform["input-customchamount"]);
            }
           


            if (rollnumberid == 0 || issuedate == "" || duedate == "" || challantypeid == "" || challandesc == ""
                || debitamount == "" )
            {
                return RedirectToAction("CustomChallan", "CustomChallan");
            }
            else
            {
                string q = "exec PROC_Custom_Challan " + challantypeid + ", '" + rollnumberid + "', '" + issuedate + "', '" + duedate + "', '" + challandesc + "', '" + debitamount + "' ";
                DAL d = new DAL();
                DataSet ds = new DataSet();
                ds = d.Select(q);
            }
            return RedirectToAction("CustomChallan", "CustomChallan");
        }

        public ActionResult HostelFeepaid(FormCollection amountform)
        {
            string challannum = Convert.ToString(amountform["cha"]);

            //string chalanlist = Convert.ToString(amountform["chalanlist"]);
            string paiddate = Convert.ToString(amountform["input-paiddate"]);
            string date1 = paiddate.Replace(',', ' ');
            string amount1 = amountform["input-creditamount"];
            string amnt = amount1.Replace(',', ' ');

            //int num = Convert.ToInt32(ViewBag.hostelchallandetail[0, 0]);


            string q = "PROC_paid_CustomChallan  '" + challannum + "', '" + amnt + "', '" + date1 + "'";
            DAL d = new DAL();
            DataSet ds = new DataSet();
            ds = d.Select(q);

            return RedirectToAction("CustomChallan", "CustomChallan");
        }


       
       
        [HttpPost]
        public ActionResult CreateChallanType(FormCollection typedata)
        {
            DAL d = new DAL();
            string challantype = "";
            string challantypedesc = Convert.ToString(typedata["input-ChallanTypedesc"]);

            if(typedata["input-ChallanType"]!=null)
            {
                challantype = Convert.ToString(typedata["input-ChallanType"]);
            }
            if (typedata["input-ChallanTypedesc"] != null)
            {
                challantypedesc = Convert.ToString(typedata["input-ChallanTypedesc"]);
            }

            if(challantype == "" && challantypedesc == "")
            {
                return RedirectToAction("CustomChallan", "CustomChallan");
            }
            else
            {
                d.conn_open();
                string q = "insert into Challan_Types(ChallanType, Description) values('" + challantype + "','" + challantypedesc + "')";
                d.command(q);
                d.conn_close();
            }


            return RedirectToAction("CustomChallan", "CustomChallan");
        }

        //////////////////////////////////////////////////////
        /// //////////////////////////////////////////////////
        [HttpPost]
        public ActionResult CreateAcademicChallan(FormCollection form)
        {
            string batch = form["batch"];
            string department = form["department"];
            string sequence = form["sequence"];
            string challanType = form["challantype"];
            string headName = form["headname"];
            string amount = form["amount"];
            string issueDate = form["issuedate"];
            string dueDate = form["duedate"];

            if (string.IsNullOrEmpty(batch) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(sequence) ||
                string.IsNullOrEmpty(challanType) || string.IsNullOrEmpty(headName) || string.IsNullOrEmpty(amount) ||
                string.IsNullOrEmpty(issueDate) || string.IsNullOrEmpty(dueDate))
            {
                TempData["error"] = "All fields are required.";
                return RedirectToAction("CustomChallan");
            }

            try
            {
                string q = $"EXEC PROC_CreateChallantype '{batch}', '{department}', '{sequence}', '{challanType}', '{amount}', '{issueDate}', '{dueDate}'";
                DAL d = new DAL();
                d.conn_open();
                d.command(q);
                d.conn_close();

                TempData["success"] = "Challan created successfully.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("CustomChallan");
        }

        public JsonResult VerifyStudent(string rollno)
        {
            DAL d = new DAL();
            string q = $"SELECT TOP 1 ID, StudentName, ClassSection FROM StudentInfo WHERE StdRollNo = '{rollno}'";
            var ds = d.Select(q);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                return Json(new
                {
                    success = true,
                    id = row["ID"].ToString(),
                    name = row["StudentName"].ToString(),
                    section = row["ClassSection"].ToString()  
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateChallan(CreateChallanModel model)
        {
            try
            {
                DAL d = new DAL();

                // 1) Fetch student roll
                var ds = d.Select($"SELECT StdRollNo, DegreeID, JoiningSession FROM StudentInfo WHERE ID = '{model.StdRollNoID}'");
                if (ds.Tables[0].Rows.Count == 0)
                    return Json(new { success = false, message = "❌ Student not found by ID." });

                string fullRoll = ds.Tables[0].Rows[0]["StdRollNo"].ToString();
                var parts = fullRoll.Split('/');
                string batch = parts[0], dept = parts[1], seq = parts[2];

                if (string.IsNullOrWhiteSpace(model.ChallanTypeID))
                    return Json(new { success = false, message = "❌ Challan Type is required." });

                // 2) Duplicate check (StdRollNo + ChallanType + SemesterSession)
                string checkSql = @"
            SELECT COUNT(*) AS Cnt
            FROM ChallanInfo
            WHERE StdRollNo = @StdRollNo
              AND ChallanType = @ChallanType
              AND SemesterSession = @SemesterSession
              AND Status = 'Valid'";

                var checkParams = new[]
                {
            new SqlParameter("@StdRollNo", fullRoll),
            new SqlParameter("@ChallanType", model.ChallanTypeID),
            new SqlParameter("@SemesterSession", model.SemesterSession)
        };

                var dupDs = d.SelectSecure(checkSql, checkParams);
                int exists = Convert.ToInt32(dupDs.Tables[0].Rows[0]["Cnt"]);
                if (exists > 0)
                    return Json(new { success = false, message = "❌ This challan already exists for the selected session." });

                // 3) Create challan via stored procedure (also parameterized)
                d.conn_open();
                using (var cmd = new SqlCommand("PROC_CreateChallantype", d.conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Batch", batch);
                    cmd.Parameters.AddWithValue("@Department", dept);
                    cmd.Parameters.AddWithValue("@SequenceNumber", seq);
                    cmd.Parameters.AddWithValue("@ChallanType", model.ChallanTypeID);
                    cmd.Parameters.AddWithValue("@Amount", model.Amount);
                    cmd.Parameters.AddWithValue("@IssueDate", model.IssueDate);
                    cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                    cmd.Parameters.AddWithValue("@SemesterSession", model.SemesterSession); // your updated SP param
                    cmd.ExecuteNonQuery();
                }
                d.conn_close();

                return Json(new { success = true, message = "✅ Challan created successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "❌ Error: " + ex.Message });
            }
        }


        public ActionResult ChallanTypeView()
        {
            try
            {
                
                DAL d1 = new DAL();
                
                string challanTypeQuery = @"
            SELECT DISTINCT ChallanType, ChallanType AS ChallanTypeID 
            FROM ChallanInfo 
            WHERE ChallanType IN (
               'UMC',
                 'Non Attestation Fine', 'Igrade-FinalTerm',
                'Igrade-MidTerm',  'Additional'
            )";
                var challanTypeDs = d1.Select(challanTypeQuery);
                ViewBag.ChallanTypeList = challanTypeDs.Tables[0];

                
                DAL d2 = new DAL();
                var batchDs = d2.Select("SELECT DISTINCT SemesterSession FROM SemesterSessionInfo ORDER BY SemesterSession DESC");
                ViewBag.BatchList = batchDs != null && batchDs.Tables.Count > 0 ? batchDs.Tables[0] : null;

               
                DAL d3 = new DAL();
                var degreeDs = d3.Select("SELECT DISTINCT DegreeName FROM DegreeInfo");
                ViewBag.DegreeList = degreeDs != null && degreeDs.Tables.Count > 0 ? degreeDs.Tables[0] : null;

                DAL d4 = new DAL();
                var semesterDs = d4.Select("SELECT DISTINCT SemesterSession FROM SemesterSessionInfo ORDER BY SemesterSession DESC");
                ViewBag.SemesterList = semesterDs != null && semesterDs.Tables.Count > 0 ? semesterDs.Tables[0] : null;

                return View();
            }
            catch (Exception ex)
            {
                return Content("❌ Error: " + ex.Message);
            }
        }

    }
}

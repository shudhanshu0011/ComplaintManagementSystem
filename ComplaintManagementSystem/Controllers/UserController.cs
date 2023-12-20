using ClosedXML.Excel;
using ComplaintManagementSystem.IRepository;
using ComplaintManagementSystem.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComplaintManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserRepository user, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _user = user;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public ActionResult UserDashboards()
        {
            return View("UserDashboard");
        }

        [HttpPost("User/UserDashboard/{id = id}")]
        public JsonResult UserDashboard(string SearchType, string SearchValue, int jtStartIndex = 0, int jtPageSize = 0)
        {   
            ComplaintModel model = new ComplaintModel();
            model.ComplaintUserId = (int)HttpContext.Session.GetInt32("userid");
            var user_complaint_data = _user.GetComplaintByUserId( model.ComplaintUserId.ToString() , SearchType, SearchValue , jtStartIndex, jtPageSize);
            return Json(new { Result = "OK", Records = user_complaint_data, TotalRecordCount = user_complaint_data.Count() });
        }

        [HttpPost]
        public ActionResult CreateComplaint(ComplaintModel model)
        {
            model.ComplaintUserId = (int)HttpContext.Session.GetInt32("userid");
            _user.CreateComplaint(model);
            return RedirectToAction("UserDashboards");
        }


        public ActionResult DeleteComplaint(int ComplaintId)
        {
            _user.DeleteComplaint(ComplaintId);
            return RedirectToAction("UserDashboards");
        }


        public ActionResult UpdateComplaint(ComplaintModel model)
        {
            _user.UpdateComplaint(model);
            return RedirectToAction("UserDashboards");
        }

        [HttpPost("User/Register")]
        public ActionResult Register(UserModel user)
        { 
            _user.CreateUser(user);
            return RedirectToAction("Login"); 
        }
        

        public ActionResult AdminDashboards()
        {
            return View("AdminDashboard");
        }

        [HttpPost]
        public JsonResult AdminDashboard(string SearchType, string SearchValue, int jtStartIndex = 0, int jtPageSize = 0)
        {
            var data = _user.GetComplaints(SearchType, SearchValue, jtStartIndex, jtPageSize);
            return Json(new { Result = "OK", Records = data, TotalRecordCount = data.Count() });
        }

        public JsonResult ComplaintsExportReport(string SearchType, string SearchValue)
        {
            var data = _user.GetComplaints(SearchType, SearchValue, 0, 0);
            string FileName = "Complaints" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string webRootPath =  _webHostEnvironment.WebRootPath;
            string FilePath = Path.Combine(webRootPath, "Downloads/" + FileName);

            var dt = ComplaintsReportToDatatabe(data.ToList());

            if (dt.Rows.Count > 65536)
            {
                return Json(new { success = false, Message = "Record IS Exced to Excel Limit(65536)", FileName = string.Empty });
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                string ExcelRecord = "Total(" + dt.Rows.Count + ")";
                wb.AddWorksheet(dt, ExcelRecord);
                wb.SaveAs(FilePath);
            }

            return Json(new { success = true, Message = "Successfully Downloaded", FileName = FileName });
        }

        private System.Data.DataTable ComplaintsReportToDatatabe(List<ComplaintModel> items)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            if (items != null)
            {
                dt.Columns.Add("ComplaintId");
                dt.Columns.Add("ComplaintUserId");
                dt.Columns.Add("ComplaintTopic");
                dt.Columns.Add("ComplaintDate");
                dt.Columns.Add("ComplaintAddress");
                dt.Columns.Add("ComplaintDesc");
            }

            foreach (var item in items)
            {
                var row = dt.NewRow();
                row["ComplaintId"] = item.ComplaintId;
                row["ComplaintUserId"] = item.ComplaintUserId;
                row["ComplaintTopic"] = item.ComplaintTopic;
                row["ComplaintDate"] = item.ComplaintDate;
                row["ComplaintAddress"] = item.ComplaintAddress;
                row["ComplaintDesc"] = item.ComplaintDesc;
                dt.Rows.Add(row);
            }

            return dt;
        }


        [HttpPost("User/Login")]
        public ActionResult Login(UserModel model)
        {
            UserModel data = _user.ValidateUser(model);
            if (data == null)
            {
                return RedirectToAction("Login");
            }
            else if(data.IsAdmin == true)
            {
                if(data.UserName == model.UserName)
                {
                    return RedirectToAction("AdminDashboards");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else if (data.UserName == model.UserName)
            {
                HttpContext.Session.SetInt32("userid",data.Id);
                return RedirectToAction("UserDashboards");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebTest.Tools;
using MVCWebTest.maipuhui;
using System.Collections.Specialized;

namespace MVCWebTest.Controllers
{
    public class ValidateCodeController : Controller
    {
        //
        // GET: /ValidateCode/
        private int index = 0;
        private static string json = "";
        private static string codeJson = @"{%7b%22assessments_id%22%3a1%2c%22examination_id%22%3a111%2c%22company%22%3a%7b%22company_id%22%3a2%2c%22company_name%22%3a%22%u6d4b%u8bd5%u516c%u53f8%22%2c%22apply_post_id%22%3a1212%2c%22apply_post%22%3a%22aaa%22%7d%2c%22resume%22%3a%7b%22resume_id%22%3a122%2c%22fullname%22%3a%22asda%22%2c%22email%22%3a%22asdasdsa%22%2c%22education%22%3a%22asdas%22%2c%22professional%22%3a%22sadasdasdasd%22%7d%7d}";

        public ActionResult Index()
        {
            if (index > 0)
            {
                string str = HttpUtility.UrlDecode(Request.Form[0]);
            }
            index++;
            string j = HttpUtility.UrlDecode(codeJson);

            ExamPeople peo = new ExamPeople();
            peo.assessments_id = 1;
            peo.examination_id = 111;
            peo.company.company_id = 2;
            peo.company.company_name = "测试公司";
            peo.company.apply_post = "aaa";
            peo.company.apply_post_id = 1212;
            peo.resume.education = "asdas";
            peo.resume.email = "asdasdsa";
            peo.resume.fullname = "asda";
            peo.resume.professional = "sadasdasdasd";
            peo.resume.resume_id = 122;

            json = JSONHelper.ObjectToJSON(peo);
            return View();
        }

        public ActionResult ValiCode()
        {

            ValiCode vCode = new ValiCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        public ActionResult Vali()
        {
            MaiPH.WRequest("http://localhost:2378/ValidateCode/index", "post", json);

            string code = Request["Code"].Trim();
            string codeSess = Session["ValidateCode"].ToString();
            if (code.Equals(codeSess))
                return Json("true");
            else
                return Json("false");
        }
    }
}

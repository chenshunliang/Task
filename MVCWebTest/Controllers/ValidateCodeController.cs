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

        private static string json = "";

        public ActionResult Index()
        {
            NameValueCollection param = Request.Form;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebTest.Tools;
using Webdiyer.WebControls.Mvc;

namespace MVCWebTest.Controllers
{
    public class ValidateCodeController : Controller
    {
        //
        // GET: /ValidateCode/

        public ActionResult Index()
        {
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
            string code = Request["Code"].Trim();
            string codeSess = Session["ValidateCode"].ToString();
            if (code.Equals(codeSess))
                return Json("true");
            else
                return Json("false");
        }
    }
}

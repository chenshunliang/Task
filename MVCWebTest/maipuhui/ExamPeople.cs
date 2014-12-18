using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebTest.maipuhui
{
    public class ExamPeople
    {

        /// <summary>
        /// 测评项目ID
        /// </summary>
        public int assessments_id { get; set; }

        /// <summary>
        /// 测评试卷ID
        /// </summary>
        public int examination_id { get; set; }

        /// <summary>
        /// 测评人员公司信息
        /// </summary>
        public ExamCompany company { get; set; }

        /// <summary>
        /// 测评人员简历信息
        /// </summary>
        public ExamResume resume { get; set; }

        public ExamPeople()
        {
            company = new ExamCompany();
            resume = new ExamResume();
        }
    }

    public class ExamCompany
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public int company_id { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// 应聘岗位ID
        /// </summary>
        public int apply_post_id { get; set; }

        /// <summary>
        /// 应聘岗位
        /// </summary>
        public string apply_post { get; set; }
    }

    public class ExamResume
    {
        /// <summary>
        /// 简历ID
        /// </summary>
        public int resume_id { get; set; }

        /// <summary>
        /// 简历姓名
        /// </summary>
        public string fullname { get; set; }

        /// <summary>
        /// 客户邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string education { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string professional { get; set; }
    }
}
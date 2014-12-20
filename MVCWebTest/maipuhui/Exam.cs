using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCWebTest.maipuhui
{
    /// <summary>
    /// 测评项目
    /// </summary>
    public class ExamProject
    {
        /// <summary>
        /// 测评项目ID
        /// </summary>
        public int assessments_id { get; set; }

        /// <summary>
        /// 测评项目名称
        /// </summary>
        public string assessment_name { get; set; }

        /// <summary>
        /// 测评试卷ID
        /// </summary>
        public int examination_id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long created { get; set; }
    }
}

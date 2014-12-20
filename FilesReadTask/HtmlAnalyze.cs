using CRIC.Shanglv.Lib.DiChanRen.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;
using CRIC.Shanglv.Lib.DiChanRen.BLL;

namespace FilesReadTask
{
    public class HtmlAnalyze
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly static Dictionary<string, int> MyWorkType = new Dictionary<string, int>()
        {
            {"全职",1},
            {"兼职",2}
        };

        /// <summary>
        /// 工作年限
        /// </summary>
        public readonly static Dictionary<string, int> MyWorkYear = new Dictionary<string, int>()
        {
            {"在读",-2},
            {"应届毕业生",-1},
            {"一年以上",1},
            {"两年以上",2},
            {"三年以上",3},
            {"五年以上",4},
            {"六年以上",5},
            {"八年以上",8},
            {"十年以上",10},
            {"十五年以上",15},
            {"二十年以上",20},
            {"三十年以上",30}
        };

        /// <summary>
        /// 学历
        /// </summary>
        public readonly static Dictionary<string, int> MyDegree = new Dictionary<string, int>()
        {
            {"博士后",1},
            {"博士",2},
            {"硕士",3},
            {"本科",4},
            {"大专",5},
            {"其他",99}
        };

        /// <summary>
        /// 熟练度级别
        /// </summary>
        public readonly static Dictionary<string, int> MyMasterLevel = new Dictionary<string, int>()
        {
            {"一般",1},
            {"良好",2},
            {"熟练",3},
            {"精通",4}
        };

        public readonly static Dictionary<string, string> PositionCategory = new Dictionary<string, string>()
        {
            {"a","项目开发"},
            {"b","建筑工程"},
            {"c","高级管理"},
            {"d","人力资源"},
            {"e","财务"},
            {"f","市场营销"},
            {"g","公关"}, 
            {"h","销售/客服/技术支持"},
            {"i","法务"},
            {"j","行政"},
            {"k","企业信息化"},
            {"l","招投标中心"},
            {"m","应届生招聘"},
            {"n","规划/设计/装潢"},
            {"o","房地产服务（物业管理/地产经纪）"},
            {"p","互联网/媒体"}
        };

        public readonly static Dictionary<string, string> PositionSubCategory = new Dictionary<string, string>()
        {
             {"a1","房地产招商"},//1101/1102/1103
             {"a2","房地产投资分析"},//  0201
             {"a3","房地产评估"},//0422/0423/0424/0425
             {"a4","房地产项目/开发/策划经理"},// 0613
             {"a5","房地产项目/开发/策划主管/专员"},// 0614
             {"a6","房地产资产管理"},// new         



             {"b1","建筑工程师"},//0801/0802/0307
             {"b2","高级建筑工程师/总工"},// 新增
             {"b3","建筑机电工程师"},// 0309/0311
             {"b4","公路桥梁设计/预算师"},// new
             {"b5","施工员"},// 05019
             {"b6","现场/施工管理"},//05020
             {"b7","爆破工程师"},//05012
             {"b8","工程造价师/预结算经理"},//0403/0404/0405/0406/0407/0408/0409/0410/0411/0412/0413/0414/0415/0416
             {"b9","建筑制图/模型/渲染"},//0316
             {"b10","测绘/测量"},//0509
             {"b11","资料员"},//05018
             {"b12","质量检测员/测试员"},//1809
             {"b13","工程监理"},//1801/1802/1803/1804/1805/1806/1807/1808/05014
             {"b14","质量管理/测试经理"},//新增
             {"b15","故障分析工程师"},//新增
             {"b16","安全防护/安全管理"},//0804
             {"b17","环境/健康/安全经理/主管/工程师"},//0805/0803
             {"b18","可靠度工程师"},//新增
             {"b19","安全管理/安全员"},//05020
             {"b20","建筑工程管理/项目经理"},//1501/1502/1503/1504/1505/0501/0502
             {"b21","设备工程师"},//0510/0311
             {"b22","空调工程师"},//0516/0312
             {"b23","配套主管"},//0205/0203
             {"b24","配套员"},//0206
             {"b25","楼宇自动化"},//新增
             {"b26","建筑工程验收"},//新增
             {"b27","公路/桥梁/港口/隧道工程"},//新增
             {"b28","开发报建专员"},//0204/0202
             {"b29","钢结构工程师"},//05011
             {"b30","结构/土木/土建工程师"},//0503/0304/0504/0505/0506/0507/0508
             {"b31","市政工程师"},//新增
             {"b32","岩土工程"},//05013
             {"b33","幕墙工程师"},//1204/0313
             {"b34","合同管理"},//1008
             {"b35","开发报建经理"},//新增
             {"b36","智能大厦/综合布线/安防/弱电"},//新增



             {"c1","首席执行官CEO/总裁/总经理"},//0101/0106/0107
             {"c2","首席运营官COO"},//0102
             {"c3","副总裁/副总经理"},//0103
             {"c4","合伙人"},//0108
             {"c5","投资者关系"},//新增
             {"c6","总监/事业部总经理"},//0104
             {"c7","总裁助理/总经理助理"},//新增
             {"c8","企业秘书/董事会秘书"},//新增
             {"c9","办事处首席代表"},//0105
             {"c10","部门经理"},//新增




             {"d1","首席人力资源官CHO/HRVC"},//新增
             {"d2","人力资源总监"},//1901
             {"d3","人力资源经理/主管"},//1902
             {"d4","人力资源主管/专员"},//1903
             {"d5","人力资源伙伴"},//新增
             {"d6","招聘经理/主管"},//1905
             {"d7","招聘专员/助理"},//1906
             {"d8","培训经理/主管"},//1907
             {"d9","培训师/培训专员/助理"},//1908
             {"d10","绩效经理/主管"},//1910
             {"d11","绩效专员/助理"},//1911
             {"d12","员工关系/企业文化/工会"},//1909/1912
             {"d13","组织发展"},//新增
             {"d14","薪资福利经理/主管"},//新增
             {"d15","薪资福利专员/助理"},//新增
             {"d16","人力资源信息系统"},//1913


             {"e1","首席财务官CFO"},//新增
             {"e2","财务总监"},//0701
             {"e3","财务经理"},//新增
             {"e4","财务主管/总帐主管"},//0702
             {"e5","财务分析经理/主管"},//新增
             {"e6","财务分析员"},//新增
             {"e7","财务顾问"},//新增
             {"e8","会计经理/主管"},//新增
             {"e9","会计"},//0712/0706
             {"e10","会计文员"},//新增
             {"e11","出纳员"},//0715
             {"e12","资产/资金管理"},//新增
             {"e13","审计经理/主管"},//新增
             {"e14","审计专员/助理"},//新增
             {"e15","税务经理/主管"},//0703
             {"e16","税务专员/助理"},//0707/0711
             {"e17","财务/会计助理"},//0710
             {"e18","统计员"},//0714
             {"e19","投融资经理/主管"},//0704/0705



             {"f1","市场营销经理/主管"},//0602
             {"f2","网络推广总监"},//新增
             {"f3","网络推广经理/主管"},//0606/0610
             {"f4","网络推广专员"},//0607
             {"f5","市场总监"},//0601
             {"f6","市场调研与分析"},//0605/0612
             {"f7","选址拓展/新店开发"},//			新增
             {"f8","市场营销专员/助理"},//			新增
             {"f9","市场拓展经理/主管"},//			新增
             {"f10","市场拓展专员/助理"},//			新增
             {"f11","产品/品牌经理"},//				0608
            {"f12","产品/品牌主管"},//			新增
            {"f13","产品/品牌专员"},//				0609
            {"f14","市场通路经理/主管"},//				新增
            {"f15","市场通路专员"},//			新增
            {"f16","市场企划经理/主管"},//				0604
            {"f17","市场企划专员"},//				新增
            {"f18","促销经理/主管"},//				0611




            {"g1","公关总监"},//				新增

            {"g2","公关经理/主管"},//				新增
{"g3","公关专员/助理"},//				新增
{"g4","媒介经理/主管"},//				新增
{"g5","媒介专员/助理"},//				新增
{"g6","活动策划"},//				新增
{"g7","活动执行"},//				新增
{"g8","媒介销售"},//				新增

{"h1","销售总经理/销售副总裁"},//			新增
{"h2","销售总监"},//			1601
{"h3","区域销售总监"},//			新增
{"h4","渠道/分销总监"},//			新增
{"h5","销售经理/主管"},//			1602
{"h6","区域销售经理"},//		1603
{"h7","渠道/分销经理/主管"},//			新增
{"h8","大客户销售管理"},//			1606
{"h9","客户经理/主管"},//			新增
{"h11","业务拓展经理/主管	"},//		新增
{"h12","零售/百货/连锁管理"},//			新增
{"h13","销售人员"},//			新增
{"h14","销售代表	"},//		1605/1607/1609/1610
{"h15","渠道/分销专员"},//			1615
{"h16","客户代表	"},//		1614
{"h17","销售工程师	"},//		1604
{"h18","电话销售"},//			1612
{"h19","经销商"},//			新增
{"h20","大客户销售"},//			新增
{"h21","医药销售代表"},//			新增
{"h22","销售行政/商务"},//
{"h23","销售行政经理/主管	"},//		新增
{"h24","销售行政专员/助理"},//			1613
{"h25","商务经理/主管"},//			新增
{"h26","商务专员/助理"},//			1608
{"h27","销售运营经理/主管"},//			新增
{"h28","销售运营专员/助理"},//			新增
{"h29","销售培训讲师"},//			新增
{"h30","业务分析经理/主管"},//			新增
{"h31","业务分析专员/助理"},//			1611
{"h32","客户服务/技术支持"},//        新增
{"h33","售前支持经理/主管"},//			新增
{"h34","售后支持经理/主管"},//			1306
{"h35","售前支持工程师"},//			1305/1308/1309
{"h36","售后支持工程师"},//			1304
{"h37","客户服务总监"},//			1301
{"h38","客户服务经理/关系经理/主管"},//			1302
{"h39","客户服务专员/助理"},//			1303
{"h40","咨询热线/呼叫中心服务人员"},//			1307
{"h41","投诉专员"},//			新增
{"h42","VIP专员"},//			新增
{"h43","网店店长/客服经理"},//			新增

{"i1","律师/律师助理"},//			1402/1403
{"i2","法律顾问"},//			新增
{"i3","法务经理/主管"},//			1401
{"i4","法务专员/助理"},//			新增
{"i5","知识产权/专利/商标"},//			新增
{"i6","合规经理	"},//		新增
{"i7","合规主管/专员"},//			新增
			
{"j1","行政总监"},//			1001
{"j2","分公司/分支机构/办事处经理"},//			新增
{"j3","行政经理/行政主管/办公室主任"},//			1002
{"j4","行政专员/助理/文员"},//			1003
{"j5","图书管理员/资料管理员/档案管理员"},//			1006
{"j6","经理助理/秘书"},//			1004
{"j7","前台/总机/接待"},//		1005
{"j8","电脑操作/打字/录入员	"},//		1007
{"j9","后勤/总务"},//			1009
			
{"k1","信息技术经理/主管"},//			新增
{"k2","信息技术专员"},//			新增
			
			
			
{"l1","成本经理/主管"},//			0402
{"l2","成本管理员"},//			新增
{"l3","房地产项目招投标"},//			0417/0418/0419/0420/0421
{"l4","成本总监"},//			0401
{"l5","采购管理	"},//		0901/0902/0903/0904/0905/0906/0806
			
			
			
{"m1","管培生"},//			新增
{"m2","实习生"},//			1010/1104/1207/1310/1404/1506/1616/1706/1810/1914/2002/0109/0307/0426/05022/0615/07016/0807/0907
{"m3","其他"},//			新增


{"n1","钢结构设计"},//		0308
{"n2","给排水/制冷暖通"},//		0310
{"n3","城市规划与设计"},//		新增
{"n4","园艺/园林/景观设计"},//		0314/0515/05017
{"n5","室内外装潢设计"},//		0305/1201/1202/1205/1206
{"n6","设计总监/经理"},//		0301/0302
{"n7","规划设计师"},//		0306
{"n8","公路桥梁设计/预算师"},//		新增
{"n9","建筑设计师"},//		0303


{"o1","物业总经理/副总	"},//	1701
{"o2","物业管理经理/主管"},//		1702
{"o3","物业管理专员/助理"},//		新增
{"o4","物业机电工程师"},//		1703
{"o5","高级物业顾问/物业顾问"},//		新增
{"o6","物业招商/租赁/租售"},//		新增
{"o7","物业设施管理人员"},//		1704/1705
{"o8","房地产销售经理/主管"},//		新增
{"o9","房地产销售人员"},//		新增
{"o10","房地产中介/交易"},//		新增

{"p1","平面设计师"},//	0315/1203
{"p2","编辑"},//	2001
{"p3","记者"},//	新增
{"p4","网站推广"}//	新增

        };

        public static Resume HTMLAnalyze(string filePath)
        {
            Resume resume = new Resume();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    //读取文件内容
                    string content = sr.ReadToEnd();

                    Lexer lex = new Lexer(content);
                    string head = lex.NextNode().ToHtml().Trim();
                    if (head != "简_历")
                        throw new Exception("简历模版不对");

                    resume.Intention = new Intention();
                    resume.Source = "导入";

                    Parser parser = new Parser(lex);
                    NodeFilter filter = new TagNameFilter("table");
                    NodeList tableList = parser.Parse(filter);
                    string s = "";
                    for (int i = 0; i < tableList.Count; i++)
                    {
                        string strTagTitle = tableList[i].ToHtml();
                        Regex reg = new Regex(@"<b>(.+?)</b>");
                        string title = reg.Match(strTagTitle).Groups[1].Value;


                        switch (title)
                        {
                            case "基 本 信 息":
                                s += "1,";
                                string baseInfo = tableList[i + 1].ToHtml();
                                //姓名
                                reg = new Regex(@" 姓    名：</td><td.+?>(.+?)</td>");
                                resume.RealName = reg.Match(baseInfo).Groups[1].Value;
                                //性别
                                reg = new Regex(@"性    别：</td><td.+?>(.+?)</td>");
                                string gender = reg.Match(baseInfo).Groups[1].Value;
                                if (gender == "男") resume.Gender = 1;
                                else if (gender == "女") resume.Gender = 2;
                                else resume.Gender = 3;
                                //出生日期
                                reg = new Regex(@"出生日期：</td><td>(.+?)年(.+?)月(.+?)日</td>");
                                var gropus = reg.Match(baseInfo).Groups;
                                resume.Birthday = new DateTime(int.Parse(gropus[1].Value), int.Parse(gropus[2].Value), int.Parse(gropus[3].Value));
                                //居住地
                                resume.Location = new Location();
                                reg = new Regex(@"居 住 地：</td><td>(.+?)</td>");
                                string stayCity = reg.Match(baseInfo).Groups[1].Value;
                                resume.Location = GetLocation(stayCity);
                                //工作年限
                                reg = new Regex(@"工作年限：</td><td>(.+?)</td>");
                                string years = reg.Match(baseInfo).Groups[1].Value;
                                resume.WorkYears = MyWorkYear[reg.Match(baseInfo).Groups[1].Value];
                                //户口
                                reg = new Regex(@"户    口：</td><td>(.+?)</td>");
                                resume.AccountLoc = new Location();
                                reg = new Regex(@"户    口：</td><td>(.+?)</td>");
                                string houseCity = reg.Match(baseInfo).Groups[1].Value;
                                resume.AccountLoc = GetLocation(houseCity);
                                //当前年薪
                                reg = new Regex(@"目前年薪：</td><td.+?>(.+?)</td>");
                                resume.Salary = reg.Match(baseInfo).Groups[1].Value;
                                //电子邮件
                                reg = new Regex(@"电子邮件：</td><td.+?>(.+?)</td>");
                                resume.Email = reg.Match(baseInfo).Groups[1].Value;
                                //移动电话
                                reg = new Regex(@"移动电话：</td><td.+?>086-\t\t(.+?)</td>");
                                resume.Mobile = reg.Match(baseInfo).Groups[1].Value;
                                break;
                            case "自 我 评 价":
                                s += "2,";
                                string ownerConfident = tableList[i].ToHtml();
                                reg = new Regex(@"自 我 评 价</b></td><td.+?>  <div  >  </div></td>  </tr>  <tr><td.+?>  </td>  </tr>  <tr><td.+?>(.+?)</td>");
                                resume.Evaluate = reg.Match(ownerConfident).Groups[1].Value;
                                break;
                            case "求 职 意 向":
                                s += "3,";
                                string jobIntension = tableList[i].ToHtml();
                                //工作性质
                                reg = new Regex(@"工作性质：</td><td.+?>(.+?)</td>");
                                resume.Intention.WorkType = MyWorkType[reg.Match(jobIntension).Groups[1].Value];
                                //目标职能
                                reg = new Regex(@"目标职能：</td><td.+?>(.+?)</td>");
                                string jobs = reg.Match(jobIntension).Groups[1].Value;
                                List<string> jobList = jobs.Split('/').ToList();
                                foreach (var item in jobList)
                                {
                                    var list = DataDict.PositionSubCategory.Where(p => p.Value.Contains(item)).ToList();
                                    if (list.Count > 0)
                                    {
                                        var key = list[0].Key;
                                        resume.Intention.Job.Add(key);
                                    }
                                }

                                //目标地点
                                reg = new Regex(@"目标地点：</td><td.+?>(.+?)</td>");
                                string loca = reg.Match(jobIntension).Groups[1].Value;
                                string[] citys = loca.Split(new char[] { '，' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int k = 0; k < citys.Length; k++)
                                {
                                    resume.Intention.Location.Add(GetLocation(citys[k]));
                                }
                                //期望工资
                                //reg = new Regex(@"期望工资：</td><td.+?>(.+?)</td>");
                                //resume.Intention.Salary = reg.Match(jobIntension).Groups[1].Value;
                                //目标职能

                                break;
                            case "工 作 经 验":
                                s += "4,";
                                string workExperience = tableList[i].ToHtml();
                                Parser parserExp = new Parser(new Lexer(workExperience));
                                NodeFilter filterExp = new TagNameFilter("tr");
                                NodeList trList = parserExp.Parse(filterExp);
                                for (int j = 0; j < trList.Count; j++)
                                {
                                    string trInfo = trList[j].ToHtml();
                                    Regex regExp = new Regex(@"<b>(.+?)</b>");
                                    string expTitle = regExp.Match(trInfo).Groups[1].Value;
                                    switch (expTitle)
                                    {
                                        case "工 作 经 验":
                                            resume.WorkExp = new List<WorkExp>();
                                            string wExp = trList[j + 1].ToHtml();
                                            Parser wExpParser = new Parser(new Lexer(wExp));
                                            NodeList wExpList = wExpParser.Parse(new StringFilter("所属行业"));
                                            wExpParser = new Parser(new Lexer(wExp));
                                            NodeList report = wExpParser.Parse(new StringFilter("汇报对象"));
                                            wExpParser = new Parser(new Lexer(wExp));
                                            NodeList persons = wExpParser.Parse(new StringFilter("下属人数"));
                                            wExpParser = new Parser(new Lexer(wExp));
                                            NodeList business = wExpParser.Parse(new StringFilter("工作业绩"));
                                            for (int k = 0; k < wExpList.Count; k++)
                                            {
                                                WorkExp we = new WorkExp();
                                                //起止时间,公司名称
                                                string timeAndCompay = wExpList[k].Parent.Parent.PreviousSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?>(\d+?)/(\d+)--(.+?)：(.+?)</td>");
                                                var groups = regExp.Match(timeAndCompay).Groups;
                                                we.StartDate = new DateTime(int.Parse(groups[1].Value), int.Parse(groups[2].Value), 1);
                                                string[] times = groups[3].Value.Split('/');
                                                we.EndDate = times.Length == 1 ? DateTime.Now : new DateTime(int.Parse(times[0]), int.Parse(times[1]), 1);
                                                we.Company = groups[4].Value;
                                                //所属行业
                                                string wP = wExpList[k].Parent.NextSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                string wPos = regExp.Match(wP).Groups[1].Value;
                                                var listPos = DataDict.PositionCategory.Where(p => p.Value.Contains(wPos)).ToList();
                                                if (listPos.Count > 0)
                                                {
                                                    we.Category = listPos[0].Key;
                                                }
                                                //部门名称，职位
                                                string jobCateAndJob = wExpList[k].Parent.Parent.NextSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?><b>(.+?)</b>  </td>");
                                                var matchs = regExp.Matches(jobCateAndJob);
                                                we.Dept = matchs[0].Groups[1].Value;
                                                we.Position = matchs[1].Groups[1].Value;
                                                //工作简要

                                                //汇报对象
                                                if (report.Count > 0 && k < report.Count)
                                                {
                                                    string reportPer = report[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.Superior = regExp.Match(reportPer).Groups[1].Value;
                                                }
                                                //下属人数
                                                if (persons.Count > 0 && k < persons.Count)
                                                {
                                                    string per = persons[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.MemberCount = int.Parse(regExp.Match(per).Groups[1].Value);
                                                }
                                                //工作业绩
                                                if (business.Count > 0 && k < business.Count)
                                                {
                                                    string buss = business[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.Achievement = regExp.Match(buss).Groups[1].Value;
                                                }
                                                resume.WorkExp.Add(we);
                                            }
                                            break;
                                        case "项 目 经 验":
                                            s += "5,";
                                            resume.ProjExp = new List<ProjExp>();
                                            string pExp = trList[j + 1].ToHtml();
                                            Parser pExpParser = new Parser(new Lexer(pExp));
                                            NodeList proList = pExpParser.Parse(new StringFilter("项目描述"));
                                            pExpParser = new Parser(new Lexer(pExp));
                                            NodeList proZeList = pExpParser.Parse(new StringFilter("责任描述"));
                                            for (int k = 0; k < proList.Count; k++)
                                            {
                                                ProjExp pro = new ProjExp();
                                                //项目描述
                                                string proDesc = proList[k].Parent.NextSibling.NextSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                pro.Desc = regExp.Match(proDesc).Groups[1].Value;
                                                //起止时间，名称
                                                string timeAndName = proList[k].Parent.Parent.PreviousSibling.PreviousSibling.PreviousSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?>(\d+?)/(\d+)--(.+?)：(.+?)</td>");
                                                var groups = regExp.Match(timeAndName).Groups;
                                                pro.StartDate = new DateTime(int.Parse(groups[1].Value), int.Parse(groups[2].Value), 1);
                                                string[] timeEnd = groups[3].Value.Trim().Split('/');
                                                pro.EndDate = timeEnd.Length == 1 ? DateTime.Now : new DateTime(int.Parse(timeEnd[0]), int.Parse(timeEnd[1]), 1);
                                                pro.Name = groups[4].Value;
                                                //责任描述
                                                if (proZeList.Count > 0 && k < proZeList.Count)
                                                {
                                                    string proZeDesc = proZeList[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    pro.Duty = regExp.Match(proZeDesc).Groups[1].Value;
                                                }
                                                resume.ProjExp.Add(pro);
                                            }

                                            break;
                                        case "教 育 经 历":
                                            s += "6,";
                                            resume.EduExp = new List<EduExp>();
                                            string eExp = trList[j + 1].ToHtml();
                                            Parser eExpParser = new Parser(new Lexer(eExp));
                                            NodeList eduList = eExpParser.Parse(new TagNameFilter("tr"));
                                            for (int k = 1; k < eduList.Count; k++)
                                            {
                                                EduExp edu = new EduExp();
                                                string eduInfo = eduList[k].ToHtml();
                                                regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                var matchs = regExp.Matches(eduInfo);
                                                if (matchs.Count > 3)
                                                {
                                                    regExp = new Regex(@"<td.+?>");
                                                    string[] timeStart = matchs[0].Groups[1].Value.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries)[0].Split('/');
                                                    string[] timeEnd = matchs[0].Groups[1].Value.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('/');
                                                    edu.StartDate = new DateTime(int.Parse(timeStart[0]), int.Parse(timeStart[1]), 1);
                                                    edu.EndDate = timeEnd.Length == 1 ? DateTime.Now : new DateTime(int.Parse(timeEnd[0]), int.Parse(timeEnd[1]), 1);
                                                    edu.School = regExp.Replace(matchs[1].Groups[1].Value, "").Trim();
                                                    edu.Major = regExp.Replace(matchs[2].Groups[1].Value, "").Trim();
                                                    edu.Degree = MyDegree[regExp.Replace(matchs[3].Groups[1].Value, "").Trim()];
                                                    resume.EduExp.Add(edu);
                                                }
                                            }

                                            break;
                                        case "培 训 经 历":
                                            s += "7,";
                                            //暂无
                                            break;
                                        case "证 书":
                                            s += "8,";
                                            resume.CertificateList = new List<Certificate>();
                                            string cExp = trList[j + 1].ToHtml();
                                            Parser cExpParser = new Parser(new Lexer(cExp));
                                            NodeList certList = cExpParser.Parse(new TagNameFilter("tr"));
                                            for (int k = 1; k < certList.Count; k++)
                                            {
                                                Certificate cert = new Certificate();
                                                if (k % 2 == 1)
                                                {
                                                    string certInfo = certList[k].ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    var matchs = regExp.Matches(certInfo);
                                                    string[] times = matchs[0].Groups[1].Value.Trim().Split('/');
                                                    cert.StartDate = new DateTime(int.Parse(times[0]), int.Parse(times[1]), 1);
                                                    cert.Name = matchs[1].Groups[1].Value;
                                                    resume.CertificateList.Add(cert);
                                                }
                                            }
                                            break;
                                        case "语 言 能 力":
                                            s += "9,";
                                            resume.Language = new List<Language>();
                                            string lExp = trList[j + 1].ToHtml();
                                            Parser lExpParser = new Parser(new Lexer(lExp));
                                            NodeList lanList = lExpParser.Parse(new TagNameFilter("tr"));
                                            for (int k = 1; k < lanList.Count; k++)
                                            {
                                                Language lang = new Language();
                                                string lanCate = lanList[k].ToHtml();
                                                regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                var matchs = regExp.Matches(lanCate);
                                                lang.LanguageDesc = matchs[0].Groups[1].Value.Trim();
                                                lang.ListenSpeak = MyMasterLevel[matchs[1].Groups[1].Value.Trim()];
                                                lang.ReadWrite = MyMasterLevel[matchs[1].Groups[1].Value.Trim()];
                                                resume.Language.Add(lang);
                                            }
                                            break;
                                        case "IT 技 能": s += "10,"; break;//暂无
                                        case "附 加 信 息": s += "11,"; break;//暂无
                                    }
                                }
                                break;
                        }
                    }
                    return resume;
                }
            }
        }

        private static Location GetLocation(string cityOrProvice)
        {
            Location location = new Location();
            string houseCity = cityOrProvice;
            string houseProviceCode = DataDict.GetProvinceCodeByCity(new Regex(@"[市,省]").Replace(houseCity, "").Trim());
            if (houseProviceCode != "100" && houseProviceCode != "")
            {
                string housePro = DataDict.GetProvinceName(houseProviceCode);
                string houseCityCode = DataDict.GetCityCode(housePro, new Regex(@"[市,省]").Replace(houseCity, "").Trim());
                location.ProvinceCode = houseProviceCode;
                location.CityCode = houseCityCode;
            }
            else
            {
                string proCode = DataDict.GetProvinceCode(new Regex(@"[市,省]").Replace(houseCity, "").Trim());
                location.ProvinceCode = proCode;
            }
            return location;
        }
    }
}

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
            {"中专",6},
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
            {"精通",4},
            {"其他",5}
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
                    if (head != "简_历" && head != "简历")
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
                                try
                                {
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
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            case "自 我 评 价":
                                s += "2,";
                                try
                                {
                                    string ownerConfident = tableList[i].ToHtml();
                                    reg = new Regex(@"自 我 评 价</b></td><td.+?>  <div  >  </div></td>  </tr>  <tr><td.+?>  </td>  </tr>  <tr><td.+?>(.+?)</td>");
                                    resume.Evaluate = reg.Match(ownerConfident).Groups[1].Value;
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            case "求 职 意 向":
                                s += "3,";
                                try
                                {
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
                                    if (resume.Intention.Job.Count == 0)
                                    {
                                        resume.Intention.Job.Add("q1");
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
                                    //希望行业
                                    //reg = new Regex(@"希望行业：</td><td.+?>(.+?)</td>");
                                }
                                catch (Exception)
                                {
                                }
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
                                            try
                                            {
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
                                                    if (string.IsNullOrEmpty(we.Category))
                                                    {
                                                        we.Category = "q";
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
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            break;
                                        case "项 目 经 验":
                                            s += "5,";
                                            resume.ProjExp = new List<ProjExp>();
                                            try
                                            {
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
                                            }
                                            catch (Exception)
                                            {

                                            }
                                            break;
                                        case "教 育 经 历":
                                            s += "6,";
                                            resume.EduExp = new List<EduExp>(); try
                                            {
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
                                                        var deg = regExp.Replace(matchs[3].Groups[1].Value, "").Trim();
                                                        if (MyDegree.ContainsKey(deg))
                                                        {
                                                            edu.Degree = MyDegree[deg];
                                                        }
                                                        else
                                                        {
                                                            edu.Degree = 99;
                                                        }
                                                        resume.EduExp.Add(edu);
                                                    }
                                                }
                                            }
                                            catch
                                            {

                                            }
                                            break;
                                        case "培 训 经 历":
                                            s += "7,";
                                            //暂无
                                            break;
                                        case "证 书":
                                            s += "8,";
                                            resume.CertificateList = new List<Certificate>();
                                            try
                                            {
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
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            break;
                                        case "语 言 能 力":
                                            s += "9,";
                                            resume.Language = new List<Language>();
                                            try
                                            {
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
                                                    var ls = matchs[1].Groups[1].Value.Trim();
                                                    if (MyMasterLevel.ContainsKey(ls))
                                                    {
                                                        lang.ListenSpeak = MyMasterLevel[ls];
                                                        lang.ReadWrite = MyMasterLevel[ls];
                                                    }
                                                    else
                                                    {
                                                        lang.ListenSpeak = 5;
                                                        lang.ReadWrite = 5;
                                                    }
                                                    resume.Language.Add(lang);
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            break;
                                        case "IT 技 能": s += "10,"; break;//暂无
                                        case "附 加 信 息": s += "11,"; break;//暂无
                                    }
                                }
                                break;
                        }
                    }
                    if (resume.RealName == null)
                        throw new Exception("简历无法匹配");
                    if (resume.RealName.Contains("性 别"))
                        throw new Exception("简历无法匹配");
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

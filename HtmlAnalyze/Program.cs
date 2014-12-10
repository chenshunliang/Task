using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CRIC.Shanglv.Lib.DiChanRen.BLL;
using CRIC.Shanglv.Lib.DiChanRen.Entity;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;

namespace HtmlAnalyze
{
    class Program
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


        static void Main(string[] args)
        {
            using (FileStream fs = new FileStream(@"C:\Users\chen\Desktop\jianli\100013.html", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    //读取文件内容
                    string content = sr.ReadToEnd();

                    Lexer lex = new Lexer(content);


                    Resume resume = new Resume();
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
                                reg = new Regex(@"居 住 地：</td><td>(.+?)</td>");
                                //string stayCity = reg.Match(baseInfo).Groups[1].Value;
                                //string stayProviceCode = DataDict.GetProvinceCodeByCity(stayCity);
                                //string stayPro = DataDict.GetProvinceName(stayProviceCode);
                                //string stayCityCode = DataDict.GetCityCode(stayPro, stayCity);
                                //resume.Location.ProvinceCode = stayProviceCode;
                                //resume.Location.CityCode = stayCityCode;
                                //工作年限
                                reg = new Regex(@"工作年限：</td><td>(.+?)</td>");
                                string years = reg.Match(baseInfo).Groups[1].Value;
                                resume.WorkYears = MyWorkYear[reg.Match(baseInfo).Groups[1].Value];
                                //户口
                                reg = new Regex(@"户    口：</td><td>(.+?)</td>");
                                //string houseCity = reg.Match(baseInfo).Groups[1].Value;
                                //string houseProviceCode = DataDict.GetProvinceCodeByCity(houseCity);
                                //string housePro = DataDict.GetProvinceName(houseProviceCode);
                                //string houseCityCode = DataDict.GetCityCode(housePro, houseCity);
                                //resume.Location.ProvinceCode = houseProviceCode;
                                //resume.Location.CityCode = houseCityCode;
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
                                resume.Intention = new Intention();
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
                                //希望行业
                                reg = new Regex(@"希望行业：</td><td.+?>(.+?)</td>");
                                string jobs = reg.Match(jobIntension).Groups[1].Value;
                                resume.Intention.Job = jobs.Split('/').ToList();
                                //目标地点

                                //期望工资
                                reg = new Regex(@"期望工资：</td><td.+?>(.+?)</td>");
                                resume.Intention.Salary = reg.Match(jobIntension).Groups[1].Value;
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
                                                we.EndDate = times.Length == 0 ? new DateTime() : new DateTime(int.Parse(times[0]), int.Parse(times[1]), 1);
                                                we.Company = groups[4].Value;
                                                //所属行业

                                                //部门名称，职位
                                                string jobCateAndJob = wExpList[k].Parent.Parent.NextSibling.ToHtml();
                                                regExp = new Regex(@"<td.+?><b>(.+?)</b>  </td>");
                                                var matchs = regExp.Matches(jobCateAndJob);
                                                we.Dept = matchs[0].Groups[1].Value;
                                                we.Position = matchs[1].Groups[1].Value;
                                                //工作简要

                                                //汇报对象
                                                if (report.Count > 0)
                                                {
                                                    string reportPer = report[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.Superior = regExp.Match(reportPer).Groups[1].Value;
                                                }
                                                //下属人数
                                                if (persons.Count > 0)
                                                {
                                                    string per = persons[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.MemberCount = int.Parse(regExp.Match(per).Groups[1].Value);
                                                }
                                                //工作业绩
                                                if (business.Count > 0)
                                                {
                                                    string buss = business[k].Parent.NextSibling.NextSibling.ToHtml();
                                                    regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                    we.Achievement = regExp.Match(buss).Groups[1].Value;
                                                }
                                                resume.WorkExp.Add(we);
                                            }
                                            break;
                                        case "项 目 经 验": s += "5,"; break;
                                        case "教 育 经 历":
                                            s += "6,";
                                            resume.EduExp = new List<EduExp>();
                                            string eExp = trList[j + 1].ToHtml();
                                            Parser eExpParser = new Parser(new Lexer(eExp));

                                            break;
                                        case "培 训 经 历": s += "7,"; break;
                                        case "证 书": s += "8,"; break;
                                        case "语 言 能 力":
                                            s += "9,";
                                            resume.Language = new List<Language>();
                                            string lExp = trList[j + 1].ToHtml();
                                            Parser lExpParser = new Parser(new Lexer(lExp));
                                            NodeList lanList = lExpParser.Parse(new TagNameFilter("tr"));
                                            for (int k = 0; k < lanList.Count; k++)
                                            {
                                                Language lang = new Language();
                                                string lanCate = lanList[k].ToHtml();
                                                regExp = new Regex(@"<td.+?>(.+?)</td>");
                                                var matchs=regExp.Matches(lanCate);
                                                lang.LanguageDesc = matchs[0].Groups[1].Value.Trim();


                                                resume.Language.Add(lang);
                                            }
                                            break;
                                        case "IT 技 能": s += "10,"; break;
                                        case "附 加 信 息": s += "11,"; break;
                                    }
                                }
                                break;
                        }
                    }

                    Console.WriteLine(s.Trim(new char[] { ',' }));
                    Console.ReadKey();
                }
            }
        }
    }
}

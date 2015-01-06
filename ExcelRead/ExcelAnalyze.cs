using CRIC.Shanglv.Lib.DiChanRen.BLL;
using CRIC.Shanglv.Lib.DiChanRen.Entity;
using CRIC.Shanglv.Lib.DiChanRen.Enum;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelRead
{
    public class ExcelAnalyze
    {
        private static int index = 1;

        public static SalaryAnalyze ExcelAnalyzeXLS(object file, string prCode, string cCode, double modulus)
        {
            FileInfo fileInfo = (FileInfo)file;

            byte[] fileContent = null;
            using (System.IO.Stream stream = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fileContent = new byte[stream.Length];
                stream.Read(fileContent, 0, (int)stream.Length);

            };
            MemoryStream memstream = new MemoryStream(fileContent);
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(memstream);
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow row = null;
            for (int i = 0; i < 5; i++)
            {
                index++;
                rows.MoveNext();
            }

            SalaryAnalyze analyze = new SalaryAnalyze();

            //样本城市
            analyze.City.ProvinceCode = prCode;
            analyze.City.CityCode = cCode;
            SalaryWelfare tenS = new SalaryWelfare();
            SalaryWelfare tweS = new SalaryWelfare();
            SalaryWelfare fifS = new SalaryWelfare();
            SalaryWelfare sevS = new SalaryWelfare();
            SalaryWelfare ninS = new SalaryWelfare();
            SalaryWelfare aveS = new SalaryWelfare();
            while (rows.MoveNext())
            {
                row = rows.Current as HSSFRow;
                if (row.Cells.Count == 0)
                {
                    index++;
                    continue;
                }
                switch (index)
                {
                    case 6:
                        //职位
                        var pos = DataDict.CeShenJiaSubCategory;
                        var position = row.Cells[0].ToString().Split(' ')[1].Trim();
                        if (pos.ContainsValue(position))
                        {
                            position = pos.Where(p => p.Value == position).FirstOrDefault().Key;
                        }
                        else if (position == "高级精装修设计设")
                        {
                            position = "p25";
                        }
                        else if (position == "精装修设计设")
                        {
                            position = "p26";
                        }
                        else
                        {
                            position = "w1";
                        }
                        analyze.Position = position;
                        break;
                    case 8:
                        //级别
                        analyze.Level.Low = int.Parse(row.Cells[2].ToString());
                        analyze.Level.High = int.Parse(row.Cells[4].ToString());
                        //博士比例
                        analyze.Educational.DoctorPer = double.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 10:
                        //样本公司数
                        analyze.SampleCompanyNum = int.Parse(row.Cells[3].ToString());
                        //硕士比例
                        analyze.Educational.Graduate = double.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 12:
                        //样本量
                        analyze.SampleNum = int.Parse(row.Cells[3].ToString());
                        //本科比例
                        analyze.Educational.College = double.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 14:
                        //平均年龄
                        analyze.AvgAge = double.Parse(row.Cells[4].ToString());
                        //大专比例
                        analyze.Educational.Junior = double.Parse(row.Cells[8].NumericCellValue.ToString("f1"));
                        break;
                    case 16:
                        //平均经验
                        analyze.AvgWorkExp = double.Parse(row.Cells[5].ToString());
                        //高中比例
                        analyze.Educational.Senior = double.Parse(row.Cells[9].NumericCellValue.ToString("f1"));
                        break;
                    case 18:
                        //高中以下比例
                        analyze.Educational.BelowSenior = double.Parse(row.Cells[2].NumericCellValue.ToString("f1"));
                        break;
                    case 22:
                        //基本月薪收入4,6,8,10,12,14
                        tenS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.BasicSalary = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 24:
                        //月薪数量
                        tenS.MonthSalaryNum = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.MonthSalaryNum = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.MonthSalaryNum = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.MonthSalaryNum = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.MonthSalaryNum = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.MonthSalaryNum = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 26:
                        //现金收入总额
                        tenS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.YearBasicTotalIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 28:
                        //交通补贴
                        tenS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.TrafficIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 30:
                        //车辆补贴
                        tenS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.CarIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 32:
                        //膳食补贴
                        tenS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.EatIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 34:
                        //住房补贴
                        tenS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.HouseIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 36:
                        //通讯补贴
                        tenS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.PhoneIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 38:
                        //岗位补贴
                        tenS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.JobIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 40:
                        //环境补贴
                        tenS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.EnvironmentIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 42:
                        //轮班补贴
                        tenS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OnDutyIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 44:
                        //职装补贴
                        tenS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.ClothesIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 46:
                        //体检补贴
                        tenS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.PhysicalExamIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 48:
                        //其他补贴
                        tenS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OtherIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 50:
                        //补贴总额
                        tenS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.TotalIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 52:
                        //固定现金总收入
                        tenS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.RegularIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 54:
                        //销售提成
                        tenS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.SellIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 56:
                        //绩效奖金
                        tenS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.PerformanceIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 58:
                        //加班费
                        tenS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OvertimeIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 60:
                        //其他变动现金
                        tenS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OtherChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 62:
                        //变动现金
                        tenS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.ChangeIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 64:
                        //现金总收入
                        tenS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.TotalMoneyIncome = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 66:
                        //自动福利
                        tenS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OwnerMaxWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 68:
                        //实物福利
                        tenS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.PhysicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 70:
                        //车辆福利
                        tenS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.CarWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 72:
                        //住房福利
                        tenS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.HouseWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 74:
                        //养老福利
                        tenS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.ProvideWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 76:
                        //医疗福利
                        tenS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.MedicalWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 78:
                        //商业福利
                        tenS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.BusinessWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 80:
                        //法定福利
                        tenS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.LegalWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 82:
                        //人事代理
                        tenS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.PeopleAgent = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 84:
                        //其他福利
                        tenS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.OtherWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 86:
                        //福利总额
                        tenS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.TotalWelfare = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 88:
                        //总薪酬
                        tenS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[4].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        tweS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[6].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        fifS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[8].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        sevS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[10].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        ninS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[12].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        aveS.YearTotalSalary = Math.Round(double.Parse((GetValue(row.Cells[14].NumericCellValue.ToString("f0")) * modulus).ToString()));
                        break;
                    case 91: break;
                }
                index++;
            }
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.TenPercent, tenS);
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.TwentyfivePrecent, tweS);
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.FiftyPercent, fifS);
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.SeventyFivePrecent, sevS);
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.NinetyPrecent, ninS);
            analyze.SalaryWelfare.Add(SalaryWelfareEnum.Average, aveS);
            memstream.Close();
            index = 1;
            return analyze;
        }


        private static double? GetValue(string str)
        {
            if (str == "0.0")
                return null;
            else
                return double.Parse(str);
        }
    }
}

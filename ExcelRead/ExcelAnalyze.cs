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

        public static SalaryAnalyze ExcelAnalyzeXLS(object file)
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
            analyze.City = 1;
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
                        analyze.Position = row.Cells[0].ToString().Split(' ')[1];
                        break;
                    case 8:
                        //级别
                        analyze.Level.Low = int.Parse(row.Cells[2].ToString());
                        analyze.Level.High = int.Parse(row.Cells[4].ToString());
                        //博士比例
                        analyze.Educational.DoctorPer = float.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 10:
                        //样本公司数
                        analyze.SampleCompanyNum = int.Parse(row.Cells[3].ToString());
                        //硕士比例
                        analyze.Educational.Graduate = float.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 12:
                        //样本量
                        analyze.SampleNum = int.Parse(row.Cells[3].ToString());
                        //本科比例
                        analyze.Educational.College = float.Parse(row.Cells[7].NumericCellValue.ToString("f1"));
                        break;
                    case 14:
                        //平均年龄
                        analyze.AvgAge = float.Parse(row.Cells[4].ToString());
                        //大专比例
                        analyze.Educational.Junior = float.Parse(row.Cells[8].NumericCellValue.ToString("f1"));
                        break;
                    case 16:
                        //平均经验
                        analyze.AvgWorkExp = float.Parse(row.Cells[5].ToString());
                        //高中比例
                        analyze.Educational.Senior = float.Parse(row.Cells[9].NumericCellValue.ToString("f1"));
                        break;
                    case 18:
                        //高中以下比例
                        analyze.Educational.BelowSenior = float.Parse(row.Cells[2].NumericCellValue.ToString("f1"));
                        break;
                    case 22:
                        //基本月薪收入4,6,8,10,12,14
                        tenS.BasicSalary = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.BasicSalary = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.BasicSalary = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.BasicSalary = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.BasicSalary = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.BasicSalary = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
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
                        tenS.YearBasicTotalIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.YearBasicTotalIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.YearBasicTotalIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.YearBasicTotalIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.YearBasicTotalIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.YearBasicTotalIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 28:
                        //交通补贴
                        tenS.TrafficIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.TrafficIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.TrafficIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.TrafficIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.TrafficIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.TrafficIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 30:
                        //车辆补贴
                        tenS.CarIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.CarIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.CarIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.CarIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.CarIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.CarIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 32:
                        //膳食补贴
                        tenS.EatIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.EatIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.EatIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.EatIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.EatIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.EatIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 34:
                        //住房补贴
                        tenS.HouseIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.HouseIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.HouseIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.HouseIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.HouseIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.HouseIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 36:
                        //通讯补贴
                        tenS.PhoneIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.PhoneIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.PhoneIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.PhoneIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.PhoneIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.PhoneIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 38:
                        //岗位补贴
                        tenS.JobIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.JobIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.JobIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.JobIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.JobIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.JobIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 40:
                        //环境补贴
                        tenS.EnvironmentIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.EnvironmentIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.EnvironmentIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.EnvironmentIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.EnvironmentIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.EnvironmentIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 42:
                        //轮班补贴
                        tenS.OnDutyIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OnDutyIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OnDutyIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OnDutyIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OnDutyIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OnDutyIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 44:
                        //职装补贴
                        tenS.ClothesIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.ClothesIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.ClothesIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.ClothesIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.ClothesIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.ClothesIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 46:
                        //体检补贴
                        tenS.PhysicalExamIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.PhysicalExamIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.PhysicalExamIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.PhysicalExamIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.PhysicalExamIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.PhysicalExamIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 48:
                        //其他补贴
                        tenS.OtherIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OtherIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OtherIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OtherIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OtherIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OtherIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 50:
                        //补贴总额
                        tenS.TotalIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.TotalIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.TotalIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.TotalIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.TotalIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.TotalIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 52:
                        //固定现金总收入
                        tenS.RegularIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.RegularIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.RegularIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.RegularIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.RegularIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.RegularIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 54:
                        //销售提成
                        tenS.SellIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.SellIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.SellIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.SellIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.SellIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.SellIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 56:
                        //绩效奖金
                        tenS.PerformanceIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.PerformanceIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.PerformanceIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.PerformanceIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.PerformanceIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.PerformanceIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 58:
                        //加班费
                        tenS.OvertimeIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OvertimeIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OvertimeIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OvertimeIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OvertimeIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OvertimeIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 60:
                        //其他变动现金
                        tenS.OtherChangeIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OtherChangeIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OtherChangeIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OtherChangeIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OtherChangeIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OtherChangeIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 62:
                        //变动现金
                        tenS.ChangeIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.ChangeIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.ChangeIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.ChangeIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.ChangeIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.ChangeIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 64:
                        //现金总收入
                        tenS.TotalMoneyIncome = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.TotalMoneyIncome = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.TotalMoneyIncome = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.TotalMoneyIncome = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.TotalMoneyIncome = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.TotalMoneyIncome = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 66:
                        //自动福利
                        tenS.OwnerMaxWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OwnerMaxWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OwnerMaxWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OwnerMaxWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OwnerMaxWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OwnerMaxWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 68:
                        //实物福利
                        tenS.PhysicalWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.PhysicalWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.PhysicalWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.PhysicalWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.PhysicalWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.PhysicalWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 70:
                        //车辆福利
                        tenS.CarWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.CarWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.CarWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.CarWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.CarWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.CarWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 72:
                        //住房福利
                        tenS.HouseWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.HouseWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.HouseWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.HouseWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.HouseWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.HouseWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 74:
                        //养老福利
                        tenS.ProvideWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.ProvideWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.ProvideWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.ProvideWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.ProvideWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.ProvideWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 76:
                        //医疗福利
                        tenS.MedicalWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.MedicalWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.MedicalWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.MedicalWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.MedicalWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.MedicalWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 78:
                        //商业福利
                        tenS.BusinessWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.BusinessWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.BusinessWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.BusinessWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.BusinessWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.BusinessWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 80:
                        //法定福利
                        tenS.LegalWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.LegalWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.LegalWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.LegalWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.LegalWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.LegalWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 82:
                        //人事代理
                        tenS.PeopleAgent = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.PeopleAgent = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.PeopleAgent = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.PeopleAgent = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.PeopleAgent = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.PeopleAgent = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 84:
                        //其他福利
                        tenS.OtherWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.OtherWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.OtherWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.OtherWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.OtherWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.OtherWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 86:
                        //福利总额
                        tenS.TotalWelfare = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.TotalWelfare = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.TotalWelfare = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.TotalWelfare = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.TotalWelfare = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.TotalWelfare = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
                        break;
                    case 88:
                        //总薪酬
                        tenS.YearTotalSalary = GetValue(row.Cells[4].NumericCellValue.ToString("f0"));
                        tweS.YearTotalSalary = GetValue(row.Cells[6].NumericCellValue.ToString("f0"));
                        fifS.YearTotalSalary = GetValue(row.Cells[8].NumericCellValue.ToString("f0"));
                        sevS.YearTotalSalary = GetValue(row.Cells[10].NumericCellValue.ToString("f0"));
                        ninS.YearTotalSalary = GetValue(row.Cells[12].NumericCellValue.ToString("f0"));
                        aveS.YearTotalSalary = GetValue(row.Cells[14].NumericCellValue.ToString("f0"));
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


        private static int? GetValue(string str)
        {
            if (str == "0.0")
                return null;
            else
                return int.Parse(str);
        }
    }
}

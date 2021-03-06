﻿using CRIC.Shanglv.Lib.DiChanRen.BLL;
using CRIC.Shanglv.Lib.DiChanRen.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelRead
{
    public partial class ExcelReadToMongo : Form
    {
        private Stopwatch watch = null;
        private int nums = 0;

        //线程控制相关
        private bool IsStartThread = true;
        private bool IsWhile = true;
        private int index = 0;
        private string p = "";
        private string c = "";
        private bool flag = true;

        public ExcelReadToMongo()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            watch = new Stopwatch();
            this.cmbCitys.Items.AddRange(DataDict.SalaryByCity.Keys.ToArray());
            this.cmbCitys.SelectedItem = "北京";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "选取文件路径";
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    watch.Start();
                    this.btnStart.Enabled = false;
                    //准备线程
                    this.labInfo.Text = "正在扫描文件";
                    this.Prepare(fbd.SelectedPath);
                }
            }
        }

        private void Prepare(object path)
        {
            DirectoryInfo di = new DirectoryInfo(path.ToString());
            this.labInfo.Text = "文件扫描完毕,正在导入";
            FileInfo[] files = di.GetFiles("*.xls");
            string content = "";
            using (FileStream fs = new FileStream(@"d:\ExcelLog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    content = sr.ReadToEnd();
                }
            }
            this.labNum.Text = "0/" + files.Length;
            this.proBar.Maximum = files.Length;

            Queue<FileInfo> fileQueue = new Queue<FileInfo>();

            foreach (FileInfo item in files)
            {
                string selectedCity = this.cmbCitys.SelectedItem.ToString();
                double modulus = DataDict.SalaryByCity[selectedCity];
                Dictionary<string, string> citys = new Dictionary<string, string>();
                if (selectedCity.Contains("-"))
                {
                    p = DataDict.GetProvinceCode(selectedCity.Split('-')[0]);
                    c = DataDict.GetCityCode(selectedCity.Split('-')[0], selectedCity.Split('-')[1]);
                    citys.Add(c, "");
                }
                else
                {
                    p = DataDict.GetProvinceCode(selectedCity);
                    citys = DataDict.GetProviceCitys(p);
                }
                if (!content.Contains(item.Name + "[" + p + "," + c + "]"))
                {
                    fileQueue.Enqueue(item);
                }
                else
                {
                    nums += 1;
                    this.proBar.Value = nums;
                    string numsAll = this.labNum.Text.Split('/')[1];
                    this.labNum.Text = nums + "/" + numsAll;
                    if (nums.ToString() == numsAll)
                    {
                        watch.Stop();
                        IsWhile = false;
                        this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
                        MessageBox.Show(this.labInfo.Text);
                    }
                }
            }

            while (IsWhile)
            {
                while (IsStartThread)
                {
                    if (fileQueue.Count == 0)
                        break;
                    FileInfo item = fileQueue.Dequeue();

                    Thread thread = new Thread(new ParameterizedThreadStart(ReadFileContent));
                    thread.IsBackground = true;
                    thread.Start(item);
                    Application.DoEvents();
                    this.Refresh();
                    index++;
                    if (index >= 5)
                    {
                        IsStartThread = false;
                        index = 0;
                    }
                }
                Application.DoEvents();
            }
        }

        private delegate void ReadFile(object file);

        //读取文件内容
        private void ReadFileContent(object file)
        {
            FileInfo fileInfo = (FileInfo)file;
            try
            {
                lock ("lock")
                {
                    string numsAll = this.labNum.Text.Split('/')[1];
                    string selectedCity = this.cmbCitys.SelectedItem.ToString();
                    double modulus = DataDict.SalaryByCity[selectedCity];
                    Dictionary<string, string> citys = new Dictionary<string, string>();
                    if (selectedCity.Contains("-"))
                    {
                        p = DataDict.GetProvinceCode(selectedCity.Split('-')[0]);
                        c = DataDict.GetCityCode(selectedCity.Split('-')[0], selectedCity.Split('-')[1]);
                        citys.Add(c, "");
                    }
                    else
                    {
                        p = DataDict.GetProvinceCode(selectedCity);
                        citys = DataDict.GetProviceCitys(p);
                    }
                    foreach (var item in citys.Keys)
                    {
                        c = item;
                        //内容进行读取
                        SalaryAnalyze analyze = ExcelAnalyze.ExcelAnalyzeXLS(file, p, c, modulus);

                        //进行导入
                        long UID = SalaryAnalyzeBLL.Add(analyze);

                        if (UID > 0)
                        {
                            //解析导入成功
                            using (FileStream fs = new FileStream(@"d:\ExcelLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                            {
                                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                                {
                                    sw.WriteLine("{0}|{1}", fileInfo.Name + "[" + p + "," + c + "]", "1");
                                    if (flag)
                                    {
                                        nums += 1;
                                        flag = false;
                                        this.proBar.Value = nums;
                                        this.labNum.Text = nums + "/" + numsAll;
                                    }
                                }
                            }
                        }
                        else
                            throw new Exception("MongoDB导入失败");
                    }
                    flag = true;
                    if (nums % 5 == 0)
                        IsStartThread = true;

                    if (nums.ToString() == numsAll)
                    {
                        watch.Stop();
                        this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
                        MessageBox.Show(this.labInfo.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                //解析导入成功
                using (FileStream fs = new FileStream(@"d:\ExcelLog.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.WriteLine("{0}|{1}|{2}", fileInfo.Name + "[" + p + "," + c + "]", "0", ex.Message);
                        string numsAll = this.labNum.Text.Split('/')[1];
                        if (flag)
                        {
                            nums += 1;
                            this.proBar.Value = nums;
                            this.labNum.Text = nums + "/" + numsAll;
                        }

                        if (nums % 5 == 0)
                            IsStartThread = true;

                        if (nums.ToString() == numsAll)
                        {
                            watch.Stop();
                            this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
                            MessageBox.Show(this.labInfo.Text);
                        }
                    }
                }
            }
        }

        private void ExcelReadToMongo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            IsWhile = false;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRIC.Shanglv.Lib.DiChanRen.BLL;
using CRIC.Shanglv.Lib.DiChanRen.Entity;

namespace FilesReadTask
{
    public partial class ReadResumeToMongoDB : Form
    {
        private Stopwatch watch = null;
        private int nums = 0;

        public ReadResumeToMongoDB()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            watch = new Stopwatch();
        }

        //文件目录路径
        //private static readonly string FileDir = ConfigurationManager.AppSettings["FileDir"];

        private void btnStart_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "学选取文件路径";
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

        //准备线程
        private void Prepare(object path)
        {
            DirectoryInfo di = new DirectoryInfo(path.ToString());
            this.labInfo.Text = "文件扫描完毕,正在导入";
            FileInfo[] files = di.GetFiles("*.html");
            string content = "";
            using (FileStream fs = new FileStream(@"d:\ReadLog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    content = sr.ReadToEnd();
                }
            }
            this.labNum.Text = "0/" + files.Length;
            this.proBar.Maximum = files.Length;
            int length = files.Length;
            foreach (FileInfo item in files)
            {
                if (!content.Contains(item.Name))
                {
                    //this.InvokeThread(item);
                    Thread thread = new Thread(new ParameterizedThreadStart(InvokeThread));
                    thread.IsBackground = true;
                    thread.Start(item);
                    Application.DoEvents();
                    this.Refresh();
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
                        this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
                        MessageBox.Show(this.labInfo.Text);
                    }
                }
            }
        }

        private delegate void ReadFile(object file);

        private void InvokeThread(object file)
        {
            //if (!this.InvokeRequired)
            //{
            //    this.BeginInvoke(new ReadFile(ReadFileContent), file);
            //}
            //else
            //{
                ReadFileContent(file);
            //}
        }

        //读取文件内容
        private void ReadFileContent(object file)
        {
            FileInfo fileInfo = (FileInfo)file;
            try
            {
                string numsAll = this.labNum.Text.Split('/')[1];

                //内容进行读取
                Resume resume = HtmlAnalyze.HTMLAnalyze(fileInfo.FullName);
                //进行导入
                long UID = 1; //ResumeBLL.Add(resume);
                nums += 1;
                this.proBar.Value = nums;
                this.labNum.Text = nums + "/" + numsAll;

                if (UID > 0)
                {
                    //解析导入成功
                    using (FileStream fs = new FileStream(@"d:\ReadLog.txt", FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                        {
                            sw.WriteLine("{0}|{1}", fileInfo.Name, "1");
                        }
                    }
                }
                else
                    throw new Exception("MongoDB导入失败");

                if (nums.ToString() == numsAll)
                {
                    watch.Stop();
                    this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
                    MessageBox.Show(this.labInfo.Text);
                }
                //this.Refresh();
            }
            catch (Exception ex)
            {
                //解析导入成功
                using (FileStream fs = new FileStream(@"d:\ReadLog.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.WriteLine("{0}|{1}|{2}", fileInfo.Name, "0", ex.Message);
                        nums += 1;
                        this.proBar.Value = nums;
                        string numsAll = this.labNum.Text.Split('/')[1];
                        this.labNum.Text = nums + "/" + numsAll;
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
    }
}

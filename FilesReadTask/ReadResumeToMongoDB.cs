using System;
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

namespace FilesReadTask
{
    public partial class ReadResumeToMongoDB : Form
    {
        private Stopwatch watch = null;

        public ReadResumeToMongoDB()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
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
                    Thread plan = new Thread(new ParameterizedThreadStart(Prepare));
                    plan.IsBackground = true;
                    plan.Start(fbd.SelectedPath);
                }
            }
        }

        //准备线程
        private void Prepare(object path)
        {
            DirectoryInfo di = new DirectoryInfo(path.ToString());
            this.labInfo.Text = "文件扫描完毕,正在导入";
            FileInfo[] files = di.GetFiles("*.html");
            this.labNum.Text = "0/" + files.Length;
            this.proBar.Maximum = files.Length;
            foreach (FileInfo fi in files)
            {
                Thread t = new Thread(this.InvokeThread);
                t.IsBackground = true;
                t.Start(fi.FullName);
            }
        }

        private delegate void ReadFile(object filePath);

        private void InvokeThread(object filePath)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ReadFile(ReadFileContent), filePath);
            }
            else
            {
                ReadFileContent(filePath);
            }
        }

        //读取文件内容
        private void ReadFileContent(object filePath)
        {
            string nums = this.labNum.Text.Split('/')[0];
            string numsAll = this.labNum.Text.Split('/')[1];
            nums = (int.Parse(nums) + 1).ToString();

            //内容进行读取


            this.proBar.Value = int.Parse(nums);
            this.labNum.Text = nums + "/" + numsAll;

            if (nums == numsAll)
            {
                watch.Stop();
                this.labInfo.Text = "导入完毕,耗时" + watch.Elapsed.TotalSeconds.ToString("f2") + "秒";
            }
        }
    }
}

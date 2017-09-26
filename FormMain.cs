using System;
using System.Data;
using System.Windows.Forms;


namespace FolderSniffer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            _tiltle = Text;
        }
        /// <summary>
        /// 窗口默认标题
        /// </summary>
        private readonly string _tiltle;

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderopen = new FolderBrowserDialog())
            {
                folderopen.ShowDialog();
                tbPath.Text = folderopen.SelectedPath;
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (tbPath.TextLength <= 1)
            {
                MessageBox.Show("请先选择文件夹"); return;
            }
            else
            {
                Calculate(tbPath.Text);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("表中没有数据");
                return;
            }

            using (SaveFileDialog ofd = new SaveFileDialog())
            {
                ofd.Title    = "保存为Excel文件";
                var t        = DateTime.Now;
                ofd.FileName = "目录占用分析_" + t.Year + "_" + t.Month + "_" + t.Day;
                ofd.Filter   = "Excel表格文件(*.xlsx)|*.xlsx";

                try
                {
                    var dr = ofd.ShowDialog();
                    // 判断是否单击了“保存”按钮
                    if (dr == DialogResult.OK)
                    {
                        DT2Excel.OutDataToExcel2((DataTable)dataGridView1.DataSource, ofd.FileName);
                        MessageBox.Show("保存成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 开始分析
        /// </summary>
        /// <param name="fpath">文件夹路径</param>
        private void Calculate(string fpath)
        {
            Text = "正在计算，请稍后。。。界面可能会卡顿，请勿关闭本程序。。";

            // 开启调试，弹出日志窗口
            if (cbxDebug.Checked)
            {
                // 为Worker初始化一个FormLogView对象
                Worker.Flv = new FormLogView();
                Worker.Flv.Text += "  -- " + tbPath.Text;
                Worker.Flv.Show();
            }

            using (DataTable dt = Worker.GenFolderSize(fpath, cbxCountFiles.Checked, cbxDebug.Checked))
            {
                dataGridView1.DataSource = dt;
                var folderCount = dataGridView1.Rows.Count;// 文件夹数量
                Text = _tiltle;
                double length = 0;// 计算文件大小
                var fcount = 0;// 计算文件数量
                foreach (DataRow item in dt.Rows)
                {
                    length += Convert.ToDouble(item["大小"]);

                    fcount += cbxCountFiles.Checked ? Convert.ToInt32(item["文件数量"]) : 0;
                }
                try
                {
                    // 按GB,MB,KB,Bytes计算总大小
                    var slength = length > 1024 * 1024 * 1024 ? Math.Round(length / 1024 / 1024 / 1024, 4) + "GB" : length > 1024 * 1024 ? Math.Round(length / 1024 / 1024, 4) + "MB" : length > 1024 ? length / 1024 + "KB" : length + "Bytes";
                    // 总结提示信息
                    var msg = "共有 " + folderCount + " 个文件夹" + "," + fcount + " 个文件,占用储存空间 " + slength + "\n";
                    msg += "平均文件夹大小为 " + (length > 1024 * 1024 * 1024 ? Math.Round(length / 1024 / 1024 / 1024 / folderCount, 4) + "GB" : length > 1024 * 1024 ? Math.Round(length / 1024 / 1024 / folderCount, 4) + "MB" : length > 1024 ? length / 1024 / folderCount + "KB" : length + "Bytes");
                    msg += "。平均文件数量为 " + fcount / folderCount;
                    // 显示提示信息
                    lbExplain.Text = msg;
                }
                catch (Exception e)
                {
                    if (cbxDebug.Checked)
                    {
                        Worker.Flv.LogCat(e.Message);
                    }
                }
                
            }
        }
    }
}

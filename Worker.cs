using System;
using System.Data;
using System.IO;

namespace FolderSniffer
{
    public static class Worker
    {
        private static readonly object Obj = new object();
        public static FormLogView Flv; // 日志窗口对象

        /// <summary>
        ///     获取文件夹子目录大小并返回DataTable
        ///     Sinsen, 2017-09-25
        /// </summary>
        /// <param name="fpath"></param>
        /// <param name="debug">启用调试:false</param>
        /// <returns></returns>
        public static DataTable GenFolderSize(string fpath, bool debug = false)
        {
            var dt = new DataTable();
            // 新建DT来存储数据
            dt.Columns.Add("文件夹名");
            dt.Columns.Add("大小");
            dt.Columns.Add("可视化大小");
            dt.Columns.Add("最后更新日期");
            dt.Columns.Add("文件数量");
            var di = new DirectoryInfo(fpath);

            foreach (var item in di.GetDirectories())
            {
                var f = FolderSize(item.FullName, debug);
                float length = f.Length;
                var dr = dt.NewRow();
                dr["文件夹名"] = item.Name;
                dr["大小"] = length;
                dr["可视化大小"] = length > 1024 * 1024 * 1024
                    ? length / 1024 / 1024 / 1024 + "GB"
                    : length > 1024 * 1024
                        ? length / 1024 / 1024 + "MB"
                        : length > 1024
                            ? length / 1024 + "KB"
                            : length + "Bytes";
                dr["最后更新日期"] = item.LastWriteTime;
                dr["文件数量"] = f.FileCount;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        ///     获取文件夹大小
        ///     Sinsen, 2017-09-25
        /// </summary>
        /// <param name="fpath">文件夹路径</param>
        /// <param name="debug">调试模式:false</param>
        /// <returns></returns>
        private static FolderInfo FolderSize(string fpath, bool debug)
        {
            var f = new FolderInfo();
            try
            {
                var di = new DirectoryInfo(fpath);
                // 遍历当前目录所有文件
                foreach (var item in di.EnumerateFiles())
                {
                    f.Length += item.Length;
                    f.FileCount += 1;
                }
                // 遍历当前目录子目录
                foreach (var gfile in di.EnumerateDirectories())
                {
                    f.Length += FolderSize(gfile.FullName, debug).Length;
                    f.FileCount += FolderSize(gfile.FullName, debug).FileCount;
                }
            }
            catch (Exception ex)
            {
                if (debug)
                    LogCat(ex.Message);
            }
            return f;
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="msg"></param>
        private static void LogCat(string msg)
        {
            // 锁住
            lock (Obj)
            {
                Flv.LogCat(msg);
            }
        }
    }
}
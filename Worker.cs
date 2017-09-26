using System;
using System.Data;
using System.IO;

namespace FolderSniffer
{
    public static class Worker
    {
        /// <summary>
        /// 获取文件夹子目录大小并返回DataTable
        /// Sinsen, 2017-09-25 
        /// </summary>
        /// <param name="fpath"></param>
        /// <param name="countFile">计算文件数量:false</param>
        /// <param name="debug">启用调试:false</param>
        /// <returns></returns>
        public static DataTable GenFolderSize(string fpath, bool countFile = false, bool debug = false)
        {
            var dt = new DataTable();
            // 新建DT来存储数据
            dt.Columns.Add("文件夹名");
            dt.Columns.Add("大小");
            dt.Columns.Add("可视化大小");
            dt.Columns.Add("最后更新日期");

            if (countFile)
            {
                dt.Columns.Add("文件数量");
            }
            var di = new DirectoryInfo(fpath);

            foreach (var item in di.GetDirectories())
            {
                float length = 0;
                var dr = dt.NewRow();
                dr["文件夹名"] = item.Name;
                length = FolderSize(item.FullName, debug);
                dr["大小"] = length;
                dr["可视化大小"] = length > 1024 * 1024 * 1024 ? length / 1024 / 1024 / 1024 + "GB" : length > 1024 * 1024 ? length / 1024 / 1024 + "MB" : length > 1024 ? length / 1024 + "KB" : length + "Bytes";
                dr["最后更新日期"] = item.LastWriteTime;


                if (countFile)
                {
                    dr["文件数量"] = CountFiles(item.FullName, debug);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 获取文件夹大小
        /// Sinsen, 2017-09-25 
        /// </summary>
        /// <param name="fpath">文件夹路径</param>
        /// <param name="debug">调试模式:false</param>
        /// <returns></returns>
        private static long FolderSize(string fpath, bool debug)
        {
            long length = 0;
            try
            {
                var di = new DirectoryInfo(fpath);
                // 遍历当前目录所有文件
                foreach (var item in di.EnumerateFiles())
                {
                    length += item.Length;
                }
                // 遍历当前目录子目录
                foreach (var gfile in di.EnumerateDirectories())
                {
                    length += FolderSize(gfile.FullName, debug);
                }
            }
            catch (Exception ex)
            {
                if (debug)
                {
                    LogCat(ex.Message);
                }
            }
            return length;
        }


        /// <summary>
        /// 归递获取文件夹下的所有子文件数量
        /// </summary>
        /// <param name="fpath">文件夹路径</param>
        /// <param name="debug">调试模式:false</param>
        /// <returns></returns>
        public static long CountFiles(string fpath, bool debug)
        {
            long i = 0;
            var di = new DirectoryInfo(fpath);
            try
            {
                // 遍历当前目录所有文件
                foreach (var item in di.EnumerateFiles())
                {
                    i += 1;
                }
                // 遍历当前目录子目录
                foreach (var gfile in di.EnumerateDirectories())
                {
                    i += CountFiles(gfile.FullName, debug);
                }
            }
            catch (Exception ex)
            {
                if (debug)
                {
                    LogCat(ex.Message);
                }
            }
            return i;
        }

        static readonly object Obj = new object();
        public static FormLogView Flv; // 日志窗口对象

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        static void LogCat(string msg)
        {
            // 锁住
            lock (Obj)
            {
                Flv.LogCat(msg);
            }
        }
    }
}

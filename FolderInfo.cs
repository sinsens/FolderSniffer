namespace FolderSniffer
{
    /// <summary>
    ///     文件夹类
    /// </summary>
    internal class FolderInfo
    {
        /// <summary>
        ///     文件夹大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        ///     文件数量
        /// </summary>
        public int FileCount { get; set; }
    }
}
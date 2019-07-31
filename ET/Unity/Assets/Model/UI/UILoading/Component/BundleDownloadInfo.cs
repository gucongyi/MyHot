namespace ETModel
{
    public class BundleDownloadInfo
    {
        public long TotalSize = 0;
        public long alreadyDownloadBytes = 0;
        public int Progress => (int)(alreadyDownloadBytes * 100f / this.TotalSize);
        public string AlreadyDownloadString => $" {alreadyDownloadBytes / 1024/1024}M/{TotalSize/1024/1024}M";
        /// <summary>
        /// 每秒下载多少byte
        /// .Format("{0,50}", theObj)
        /// </summary>
        public float DownLoadSpeed;
        //public string SpeedString => $"正在下载{string.Format("{0,8}", (int)DownLoadSpeed/1024)}KB/s ";
        public string SpeedString => $"{(int)DownLoadSpeed/1024}KB/s ";
        public bool IsStart;
        internal bool IsEnd =false;
    }
}

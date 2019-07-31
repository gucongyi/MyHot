using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 下载速度系统
    /// </summary>
    [ObjectSystem]
    public class UiLoadingComponentDownLoadSpeedSystem : UpdateSystem<UILoadingComponent>
    {
        private float lastTime = 0;
        /// <summary>
        /// 20帧刷新一次
        /// </summary>
        private int updateFrameEvery = 40;
        private int frame = 0;
        private long lastDownloaded;
        private bool isEnd;

        /// <summary>
        /// 主要是更新下载速度
        /// </summary>
        /// <param name="self"></param>
        public override void Update(UILoadingComponent self)
        {
            if (isEnd) return;
            if (self.DownLoadInfo == null) return;
            if (!self.DownLoadInfo.IsStart) return;
            if (self.DownLoadInfo.IsEnd)
            {
                isEnd = true;
                return;
            }
            
            var now = Time.realtimeSinceStartup;
            if (lastTime == 0)
            {
                self.UpdateProgress();
                lastDownloaded = self.DownLoadInfo.alreadyDownloadBytes;
                lastTime = now;
                return;
            }
            frame++;
            if (frame < updateFrameEvery) return;
            frame = 0;
            self.UpdateProgress();
            var passedTime = now - lastTime;
            var stepDownload = self.DownLoadInfo.alreadyDownloadBytes - lastDownloaded;

            lastDownloaded = self.DownLoadInfo.alreadyDownloadBytes;
            lastTime = now;

            var t = stepDownload / passedTime;
            self.DownLoadInfo.DownLoadSpeed = t;
            self.UpdateShow();
        }
    }
}

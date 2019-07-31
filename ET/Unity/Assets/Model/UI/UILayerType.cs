
namespace ETModel
{
    public static class UILayerType
    {
        /// <summary>
        /// 隐藏层，当调用Close的时候，实际上是把UI物体移到该层中进行隐藏
        /// </summary>
        public const string Hide = "Hide";

        /// <summary>
        /// 底层，一般用来放置最底层的UI
        /// </summary>
        public const string Bottom = "Bottom";

        /// <summary>
        /// 中间层，比较常用，大部分界面均是放在此层
        /// </summary>
        public const string Medium = "Medium";

        /// <summary>
        /// 上层，一般是用来放各种弹窗，小窗口之类的
        /// </summary>
        public const string Top = "Top";

        /// <summary>
        /// 最上层，一般用来做各种遮罩层，屏蔽输入，或者切换动画等
        /// </summary>
        public const string TopMost = "TopMost";
    }
}
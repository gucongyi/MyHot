using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New PackageConfig", menuName = "Create new PackageConfig asset", order = 1)]
public class PackageConfig : ScriptableObject
{
    //是否是内网
    public bool isInnetNet;
    public string SDK_JAVA_CLASS;
    public int gameId;
    public enum PlatformType
    {
        Andriod,
        IOS
    }
    /// <summary>
    /// 是否显示FPS
    /// </summary>
    public bool isShowFPS;
    /// <summary>
    /// 是否显示GM
    /// </summary>
    public bool isShowGM;
    /// <summary>
    /// 是否关闭程序内的debug
    /// </summary>
    public bool isOpenWindowDebug;
    /// <summary>
    /// 是否走公司平台，默认要勾上
    /// </summary>
    public bool isPassCompanyPlatform;
    /// <summary>
    /// 不从服务器下载AB
    /// </summary>
    public bool isUsedLocalAB;
    [HideInInspector]
    public BuildOptions buildOptions = BuildOptions.AllowDebugging | BuildOptions.Development;
    [HideInInspector]
    public BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;
    /// <summary>
    /// 要打包的平台的类型
    /// </summary>
    public PlatformType platformType;
    /// <summary>
    /// 包输出的路径
    /// </summary>
    public string PackagePath;
    /// <summary>
    /// 要打包的网络配置文件
    /// </summary>
    public NetConfig netConfig;
    /// <summary>
    /// 要打包的公司平台的公共配置文件
    /// </summary>
    public PlatformCommonConfig platformCommonConfig;
    /// <summary>
    /// 要打包的公司平台的url地址信息
    /// </summary>
    public PlatformUrlConfig platformUrlConfig;
    /// <summary>
    /// 公司名称
    /// </summary>
    public string companyName;
    /// <summary>
    /// 产品名称
    /// </summary>
    public string productName;
    /// <summary>
    /// App Icon 路径
    /// </summary>
    public string appIconPathIOS;
    /// <summary>
    /// 编译参数
    /// </summary>
    public string scriptingDefine;
    /// <summary>
    /// 包名
    /// </summary>
    public string applicationIdentifier;
    /// <summary>
    /// 包的版本号
    /// </summary>
    public string bundleVersion;
    /// <summary>
    /// 安卓的bundleVersionCode
    /// </summary>
    public int androidBundleVersionCode;
    /// <summary>
    /// Android 架构
    /// </summary>
    public AndroidArchitecture androidArchitecture;
    /// <summary>
    /// 是否允许UnSafe
    /// </summary>
    public bool allowUnsafeCode;
    /// <summary>
    /// 是否是IL2Cpp
    /// </summary>
    public ScriptingImplementation scriptingBackend;
    /// <summary>
    /// IOS设置teamId
    /// </summary>
    public string AppleDeveloperTeamID;
    /// <summary>
    /// 是否关闭新手引导
    /// </summary>
    public bool IsCloseFirstGuild;
}

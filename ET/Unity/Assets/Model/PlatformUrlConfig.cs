using UnityEngine;

[CreateAssetMenu(fileName = "New PlatformUrlConfig", menuName = "Create new PlatformUrlConfig asset", order = 1)]
public class PlatformUrlConfig : ScriptableObject
{
    public string urlRegister;
    public string urlLogin;
    public string urlServerlist;
    public string urlPassword;
    public string urlGetVerCode;
    public string urlNotice;
    public string urlActivateCode;
    public string urlCustomerService;
}

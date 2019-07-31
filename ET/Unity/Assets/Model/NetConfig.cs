using UnityEngine;

[CreateAssetMenu(fileName = "New NetConfig", menuName = "Create new NetConfig asset", order = 1)]
public class NetConfig : ScriptableObject
{
    public string loginPrefix;
    public string ipAddress;
    public string ipPort;
    public string chatPrefix;
    public string chatIp;
    public string chatPort;
    public string chatWorldChannle;
    public string signKey;
}

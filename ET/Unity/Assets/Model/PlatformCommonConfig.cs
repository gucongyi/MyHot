using UnityEngine;

[CreateAssetMenu(fileName = "New PlatformCommonConfig", menuName = "Create new PlatformCommonConfig asset", order = 1)]
public class PlatformCommonConfig : ScriptableObject
{
    public string clientId;
    public string version;
    public string salt;
    public string clientSecret;
}

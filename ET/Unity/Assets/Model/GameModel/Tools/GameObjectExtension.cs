using UnityEngine; 

public static class GameObjectExtension
{  
    public static void SafeDestroy(this Object obj)
    {
        if (obj != null)
        {
            Object.Destroy(obj);
        }
    } 
}

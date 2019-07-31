using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{

	public class GlobalDefaultCamera : MonoBehaviour
	{
        static GlobalDefaultCamera instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            if (instance == null) instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
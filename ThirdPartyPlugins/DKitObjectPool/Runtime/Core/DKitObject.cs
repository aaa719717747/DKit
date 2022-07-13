using System;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core
{
    public enum RecycleType
    {
        LifeCycle,
        Custom
    }

    [System.Serializable]
    public class DKitObjectData
    {
        public GameObject Prefab;
        public string TId;
        public int InitNum;
        public RecycleType RecycleType;
        public int RepuishNum;
        public int ReleaseTimer;
    }

    public class DKitObject : MonoBehaviour
    {
        public bool isActive;
        int timer;
        [HideInInspector]
        public DKitObjectData data;
        public void On()
        {
            isActive = true;
            gameObject.SetActive(true);
        }

        public void Off(DKitObjectData _data)
        {
            timer = 0;
            isActive = false;
            gameObject.SetActive(false);
            data=_data;
        }
        public void Off()
        {
            timer = 0;
            isActive = false;
            gameObject.SetActive(false);
        }
        private void Update()
        {
            switch (data.RecycleType)
            {
                case RecycleType.LifeCycle:
                    timer++;
                    if (timer >= data.ReleaseTimer)
                    {
                        Off();
                    }

                    break;
            }
        }
    }
}
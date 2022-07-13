using System;
using UnityEngine;
using DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core;
using DKit.ThirdPartyPlugins.LogHelper.Runtime;

namespace DKit.Unity.Test.Runtime.ObjectPool
{
    public class GameManager : MonoBehaviour
    {
        [Range(0,500)]
        public int speed;
        private DKitObject go;
        public bool isTest;
        private void Start()
        {
            ThirdPartyPlugins.DKitObjectPool.Runtime.Core.ObjectPool.Init();
            LogHelper.Log(()=>"你好"+"湖南科学技术");
        }

        private int a = 0;
        private int timer = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                go.data.InitNum = 1;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                LogHelper.Log(()=>"普通消息");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LogHelper.Log(()=>go);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                LogHelper.LogTrack(()=>"Im.Sdk.Init=>Sucess!初始化!");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                LogHelper.LogWarning(() => "Life left a line of good to meet in the future");
            }

            if (!isTest) return;
            //性能測試
            timer++;
            if (timer>speed)
            {
                a++;
                timer = 0;
                LogHelper.Log(()=>"-->"+a);;
            }
        }
    }
}
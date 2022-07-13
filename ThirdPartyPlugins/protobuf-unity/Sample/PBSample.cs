using System;
using System.Collections.Generic;
using DKit.ThirdPartyPlugins.protobuf_unity.Runtime.Core;
using Google.Protobuf;
using Im.Unity.protobuf_unity.Tests.Runtime.ProtoDemo;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.protobuf_unity.Sample
{
    public class PBSample : MonoBehaviour
    {
        Dictionary<Type, IMessage> pbDataDicts =
            new Dictionary<Type, IMessage>();

        public string userName;
        public UserBase a = new UserBase
        {
            
        };
        public HHHHH hhh;
        /// <summary>
        ///  默认初始化pb数据在这里设置
        /// </summary>
        public void Init()
        {
            PBHelper.Init();
            pbDataDicts.Add(typeof(User), new User
            {
                Uid = 0,
                Account = "yso",
                Password = "******************"
            });
            pbDataDicts.Add(typeof(Character), new Character
            {
                Cid = 1,
                RoleName = "苍离浪客",
                Level = 60,
                Sexy = Character.Types.Sexy.Man,
                Location = {  },
                Tags = {  },
                Option = {  },
                UserBs = { new UserBase() }
            });
        }

        public T Get<T>() where T : IMessage
        {
            return (T) pbDataDicts[typeof(T)];
        }

        private void Awake()
        {
            Init();
            foreach (var msg in pbDataDicts)
            {
                PBHelper.Read(msg.Value);
            }

            userName = Get<User>().Account;
        }

        private void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 23;
            if (GUI.Button(new Rect(200, 200, 100, 50), "取值",guiStyle))
            {
                Debug.Log(Get<User>().Account);
            }
            if (GUI.Button(new Rect(200, 250, 100, 50), "修改",guiStyle))
            {
                Get<User>().Account = userName;
            }
            if (GUI.Button(new Rect(200, 300, 100, 50), "保存",guiStyle))
            {
                foreach (var msg in pbDataDicts)
                {
                    PBHelper.Write(msg.Value);
                }
            }
        }
    }
}
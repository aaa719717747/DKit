using System;
using System.Collections.Generic;
using DKit.ThirdPartyPlugins.protobuf_unity.Runtime.Core;
using Google.Protobuf;
using Im.Unity.protobuf_unity.Tests.Runtime.ProtoDemo;
using UnityEngine;

namespace DKit.Unity.Test.Runtime.Pb.Manager
{
    public class PBM : MonoBehaviour
    {
        public static PBM Instance { get; set; }

        Dictionary<Type, IMessage> pbDataDicts =
            new Dictionary<Type, IMessage>();

        /// <summary>
        ///  默认初始化pb数据在这里设置
        /// </summary>
        void Init()
        {
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
                Sexy = Character.Types.Sexy.Man
            });
        }

        public T Get<T>() where T : IMessage
        {
            return (T) pbDataDicts[typeof(T)];
        }

        private void Awake()
        {
            Instance = this;
            Init();
            foreach (var msg in pbDataDicts)
            {
                PBHelper.Read(msg.Value);
            }
        }

    }
}
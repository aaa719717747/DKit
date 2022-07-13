using UnityEngine;

namespace DKit.ThirdPartyPlugins.protobuf_unity.Runtime.Core
{
    public class PbConfig : ScriptableObject
    {
        public PBPathType pbPathType;
        public string suffix;
        public string customPath;
        public string pkey;
    }
}
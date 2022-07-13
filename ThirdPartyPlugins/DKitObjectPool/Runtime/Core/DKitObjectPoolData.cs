using System.Collections.Generic;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core
{
    public class DKitObjectPoolData:ScriptableObject
    {
        public List<DKitObjectData> tObjects = new List<DKitObjectData>();
    }
}

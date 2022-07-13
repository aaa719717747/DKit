using UnityEngine;

namespace DKit.ThirdPartyPlugins.QECS.Runtime.Core.Data
{
    [CreateAssetMenu(fileName = "QECSData",menuName = "QECSData(ScriptableObject)")]
    public class QECSData:ScriptableObject
    {
        [Header("DSL文件所在父级目录")]
        public string inputDSLPath;
        [Header("编译后的DSL对应cs文件存放的父级目录")]
        public string outputDSLPath;
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime
{
    [CreateAssetMenu(fileName = "LLHelperData", menuName = "LLHelperData(ScriptableObject)")]
    public class LLHelperData : ScriptableObject
    {
        public IDictionary<string, LLObject> uuidLlObjectDict = new Dictionary<string, LLObject>();
        public List<LLObject> llObjects = new List<LLObject>();

        public void Init()
        {
            foreach (var VARIABLE in llObjects)
            {
                uuidLlObjectDict.Add(VARIABLE.uuid,VARIABLE);  
            }
        }
        
    }
}
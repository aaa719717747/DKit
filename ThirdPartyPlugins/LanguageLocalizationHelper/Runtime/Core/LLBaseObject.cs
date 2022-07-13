using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime.Core
{
    public abstract class LLBaseObject : MonoBehaviour
    {
        public string uuid;
        public LLTipEnum.LLTip tip;
        public abstract void UpdateContent(LLISO iso);

        private void OnEnable()
        {
            if (LLHelper.Instance == null) return;
            UpdateContent(LLHelper.Instance.currentISOType);
        }

        public LLObject GetLlObject
        {
            get { return LLHelper.Instance.Data.uuidLlObjectDict[uuid]; }
        }

        public LLScheme GetTargetScheme(LLObject obj, LLISO iso)
        {
            for (int i = 0; i < obj.llSchemeList.Count; i++)
            {
                if (iso == obj.llSchemeList[i].llISO) return obj.llSchemeList[i];
            }

            return null;
        }
    }
}
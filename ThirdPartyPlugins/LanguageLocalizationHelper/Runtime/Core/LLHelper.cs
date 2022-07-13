using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime.Core
{
    public class LLHelper : MonoBehaviour
    {
        public static LLHelper Instance { get; private set; }
        public LLISO currentISOType;
        public LLHelperData Data;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            Data = Resources.Load<LLHelperData>("LLHelperData");
            Data.Init();
            UpdateISO(currentISOType);
        }

        /// <summary>
        /// 切换语言类型
        /// </summary>
        /// <param name="iso"></param>
        public void UpdateISO(LLISO iso)
        {
            currentISOType = iso;
            LLBaseObject[] a = FindObjectsOfType<LLBaseObject>();
            foreach (var item in a)
            {
                item.UpdateContent(iso);
            }
        }
    }
}
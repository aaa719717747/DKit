using System.Collections.Generic;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime
{
    /// <summary>
    /// 本地化类型
    /// </summary>
    public enum LLType
    {
        Text,
        Image,
    }
    public enum LLISO
    {
        Chinese,
        English,
    }
    /// <summary>
    /// 一个语言对象
    /// </summary>
    [System.Serializable]
    public class LLObject
    {
        public string tips;
        public string uuid;//自动生成
        public LLType llType;
        public List<LLScheme> llSchemeList = new List<LLScheme>();
    }

    /// <summary>
    /// 本地化方案
    /// </summary>
    [System.Serializable]
    public class LLScheme
    {
        public LLISO llISO;
        public string content;
        public Sprite sprite;
    }
}
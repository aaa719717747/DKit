using UnityEngine;
using UnityEngine.UI;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime.Core
{
    [RequireComponent(typeof(Image))]
    public class LLImage : LLBaseObject
    {
        public Image img;
        public Image IMG
        {
            get
            {
                if (img == null)
                {
                    return GetComponent<Image>();
                }
                return img;
            }
        }
        public override void UpdateContent(LLISO iso)
        {
            IMG.sprite=base.GetTargetScheme(base.GetLlObject,iso).sprite;
        }
    }
}
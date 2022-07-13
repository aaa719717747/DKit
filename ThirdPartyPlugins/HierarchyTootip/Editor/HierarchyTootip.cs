using System.Collections.Generic;
using DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.HierarchyTootip.Editor
{
    public class HierarchyTootip
    {
        #region --- VAR ---

        private const int MAX_ICON_NUM = 4;

        private static List<System.Type> HideTypes =
            new List<System.Type>() {typeof(Transform), 
                typeof(ParticleSystemRenderer), 
                typeof(CanvasRenderer),};
        private static Transform OffsetObject = null;
        private static int Offset = 0;

        public static bool isOpen=true;
        #endregion

        #region --- MSG ---

        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HieGUI;
        }
        [MenuItem("DKit/HierarchyTootip/On", false, 1)]
        public static void On()
        {
            isOpen = true;
        }
        [MenuItem("DKit/HierarchyTootip/Off", false, 2)]
        public static void Off()
        {
            isOpen = false;
        }
        public static void HieGUI(int instanceID, Rect rect)
        {
            if (!isOpen) return;
            // Check
            Object tempObj = EditorUtility.InstanceIDToObject(instanceID);
            if (!tempObj)
            {
                return;
            }
            
            // fix rect
            rect.width += rect.x;
            rect.x = 0;
            
            // Logic
            GameObject obj = tempObj as GameObject;
            List<Component> coms = new List<Component>(obj.GetComponents<Component>());
            for (int i = 0; i < coms.Count; i++)
            {
                if (!coms[i])
                {
                    continue;
                }

                if (TypeCheck(coms[i].GetType()))
                {
                    coms.RemoveAt(i);
                    i--;
                }
            }

            int iconSize = 16;
            int y = 1;
            int offset = obj.transform == OffsetObject ? Offset : 0;

            // Main
            for (int i = 0; i + offset < coms.Count && i < MAX_ICON_NUM; i++)
            {
                Component com = coms[i + offset];

                // Logic
                Texture2D texture = AssetPreview.GetMiniThumbnail(com);

                if (texture)
                {
                    GUI.DrawTexture(new Rect(rect.width - (iconSize + 1) * (i + 1), rect.y + y, iconSize, iconSize), texture);
                }
            }
            
            // More Button
            if (coms.Count == MAX_ICON_NUM + 1)
            {
                Texture2D texture = AssetPreview.GetMiniThumbnail(coms[coms.Count - 1]);
                if (texture)
                {
                    GUI.DrawTexture(new Rect(rect.width - (iconSize + 1) * (coms.Count - 1 + 1), rect.y + y, iconSize, iconSize),
                        texture);
                }
            }
            else if (coms.Count > MAX_ICON_NUM)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.fontSize = 9;
                style.alignment = TextAnchor.MiddleCenter;

                if (GUI.Button(new Rect(rect.width - (iconSize + 2) * (MAX_ICON_NUM + 1), rect.y + y, 22, iconSize), "•••",
                    style))
                {
                    if (OffsetObject != obj.transform)
                    {
                        OffsetObject = obj.transform;
                        Offset = 0;
                    }

                    Offset += MAX_ICON_NUM;
                    if (Offset >= coms.Count)
                    {
                        Offset = 0;
                    }
                }
            }
        }

        #endregion

        #region --- LGC ---

        private static bool TypeCheck(System.Type type)
        {
            for (int i = 0; i < HideTypes.Count; i++)
            {
                if (type==typeof(LLText)||type==typeof(LLImage))//type == HideTypes[i] || type.IsSubclassOf(HideTypes[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
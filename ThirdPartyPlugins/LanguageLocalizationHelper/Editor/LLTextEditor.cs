using System;
using System.Text.RegularExpressions;
using DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime;
using DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Editor
{
    [CustomEditor(typeof(LLText))]
    public class LLTextEditor : UnityEditor.Editor
    {
        private LLText LT;

        public LLHelperData data;
        private Vector2 scrollRect;
        private static bool isFoldout = true;

        private string filterStr;

        private void OnEnable()
        {
            if (data == null)
            {
                data = Resources.Load<LLHelperData>("LLHelperData");
            }
        }

        public override void OnInspectorGUI()
        {
            LT = target as LLText;

            if (string.IsNullOrEmpty(LT.uuid))
            {
                LT.uuid = data.llObjects[(int) LT.tip].uuid;
            }

            EditorGUILayout.LabelField(string.IsNullOrEmpty(LT.uuid) ? "null" : LT.uuid,
                this[12, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white]);

            EditorGUILayout.LabelField(LT.tip.ToString(),
                this[12, FontStyle.Normal, TextAnchor.MiddleCenter, Color.white]);

            EditorGUILayout.Space(10);

            #region 筛选器

            //筛选
            filterStr = EditorGUILayout.TextField(filterStr, GUILayout.ExpandWidth(true));

            #endregion


            isFoldout = EditorGUILayout.Foldout(isFoldout, $"标记(别名)列表:");
            if (isFoldout)
            {
                scrollRect = EditorGUILayout.BeginScrollView(scrollRect, this[LayoutStyle.GroupBox]);
                {
                    for (int i = 0; i < data.llObjects.Count; i++)
                    {
                        if (data.llObjects[i].llType == LLType.Text)
                        {
                            if (!string.IsNullOrEmpty(filterStr))
                            {
                                Regex m_richRegex = new Regex(filterStr);
                                if (m_richRegex.IsMatch(data.llObjects[i].tips))
                                {
                                    EditorGUILayout.BeginVertical(this[LayoutStyle.FoldOut]);
                                    {
                                        if (GUILayout.Button(data.llObjects[i].tips, GUILayout.Height(16)))
                                        {
                                            LT.tip = (LLTipEnum.LLTip) i;
                                        }
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                EditorGUILayout.BeginVertical(this[LayoutStyle.FoldOut]);
                                {
                                    if (GUILayout.Button(data.llObjects[i].tips, GUILayout.Height(16)))
                                    {
                                        LT.tip = (LLTipEnum.LLTip) i;
                                    }
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }


            if (GUI.changed)
            {
                LT.uuid = data.llObjects[(int) LT.tip].uuid;
            }

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("快速配置"))
            {
                LanguageLocalizationHelperEditorWindow.Init();
            }
        }


        #region 样式扩展

        public enum LayoutStyle
        {
            Title,
            AddBtn,
            GroupBox,
            HelpBox,
            FoldOut,
            Toggle,
        }

        /// <summary>
        /// 文字样式索引
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontStyle"></param>
        /// <param name="anchor"></param>
        /// <param name="color"></param>
        GUIStyle this[int size, FontStyle fontStyle, TextAnchor anchor, Color color]
        {
            get
            {
                GUIStyle _style = new GUIStyle();
                _style.fontSize = size;
                _style.fontStyle = fontStyle;
                _style.alignment = anchor;
                _style.normal.textColor = color;
                return _style;
            }
        }

        /// <summary>
        /// 模块样式索引
        /// </summary>
        /// <param name="ls"></param>
        string this[LayoutStyle ls]
        {
            get
            {
                string str = "";
                switch (ls)
                {
                    case LayoutStyle.Title:
                        str = "MeTransOffRight";
                        break;
                    case LayoutStyle.AddBtn:
                        str = "CN CountBadge";
                        break;
                    case LayoutStyle.GroupBox:
                        str = "GroupBox";
                        break;
                    case LayoutStyle.HelpBox:
                        str = "HelpBox";
                        break;
                    case LayoutStyle.FoldOut:
                        str = "Foldout";
                        break;
                    case LayoutStyle.Toggle:
                        str = "BoldToggle";
                        break;
                }

                return str;
            }
        }

        #endregion
    }
}
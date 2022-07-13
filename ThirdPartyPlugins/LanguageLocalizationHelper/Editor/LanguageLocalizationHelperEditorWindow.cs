using System;
using System.Collections.Generic;
using DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime;
using UnityEditor;
using UnityEngine;


namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Editor
{
    public class LanguageLocalizationHelperEditorWindow : EditorWindow
    {
        public static LanguageLocalizationHelperEditorWindow _lLHelperEditorWindow;
        public static LLHelperData _lLHelperData;

        #region 窗口样式变量

        private Vector2 scrollRect;
        private Vector2 scrollRect_Scheme;
        private static bool isFoldout = true;
        private static List<bool> isFoldout_Scheme_Child = new List<bool>();

        #endregion

        private void OnDestroy()
        {
            AutoEnumBuild.BuildUIScript();
        }

        [MenuItem("DKit/LanguageLocalizationHelper", false, 3)]
        public static void Init()
        {
            _lLHelperData = Resources.Load<LLHelperData>("LLHelperData");
            if (_lLHelperEditorWindow == null)
            {
                _lLHelperEditorWindow = ScriptableObject.CreateInstance<LanguageLocalizationHelperEditorWindow>();
            }

            InitWindowStyle();
            _lLHelperEditorWindow.Show();
            isFoldout_Scheme_Child.Clear();
            foreach (var VARIABLE in _lLHelperData.llObjects)
            {
                isFoldout_Scheme_Child.Add(false);
            }
        }

        bool isHavaChar(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int k = 0; k < str.Length; k++)
                {
                    char c = str[k];
                    if (c.Equals('~') || c.Equals('#') || c.Equals('^') || c.Equals('.') || c.Equals('!') ||
                        c.Equals('@') || c.Equals('*') || c.Equals(':') || c.Equals('：')
                        || c.Equals('&') || c.Equals('|') || c.Equals(']') || c.Equals('[') || c.Equals('=') ||
                        c.Equals('(') || c.Equals(')') || c.Equals('+') || c.Equals('-')
                        || c.Equals('?') || c.Equals('/') || c.Equals('\\') || c.Equals('{') || c.Equals('}') ||
                        c.Equals('"') || c.Equals(';') || c.Equals(']') || c.Equals('[') || c.Equals(',') ||
                        c.Equals('，')
                        || c.Equals('<') || c.Equals('>') || c.Equals('$') || c.Equals('%') || c.Equals('“')
                       )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnGUI()
        {
            if (_lLHelperData == null) return;

            #region ScrollView

            isFoldout = EditorGUILayout.Foldout(isFoldout, $"自定义本地化对象列表[{_lLHelperData.llObjects.Count}]");
            if (isFoldout)
            {
                scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
                {
                    for (int i = 0; i < _lLHelperData.llObjects.Count; i++)
                    {
                        EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                        {
                            if (!isHavaChar(_lLHelperData.llObjects[i].tips))
                            {
                                EditorGUILayout.LabelField(_lLHelperData.llObjects[i].tips,
                                    this[20, FontStyle.Bold, TextAnchor.MiddleCenter, Color.white]);
                            }
                            else
                            {
                                EditorGUILayout.LabelField(_lLHelperData.llObjects[i].tips,
                                    this[20, FontStyle.Bold, TextAnchor.MiddleCenter, Color.red]);
                            }

                            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                            {
                                _lLHelperData.llObjects.Remove(_lLHelperData.llObjects[i]);
                                isFoldout_Scheme_Child.RemoveAt(i);
                                return;
                            }
                            //tips

                            if (!isHavaChar(_lLHelperData.llObjects[i].tips))
                            {
                                _lLHelperData.llObjects[i].tips = EditorGUILayout.TextField("Tips:",
                                    _lLHelperData.llObjects[i].tips, GUILayout.ExpandWidth(true));
                            }
                            else
                            {
                                EditorGUILayout.LabelField("Tip不符合规则!",
                                    this[15, FontStyle.Bold, TextAnchor.MiddleCenter, Color.red]);
                                _lLHelperData.llObjects[i].tips = EditorGUILayout.TextField("Tips:",
                                    _lLHelperData.llObjects[i].tips, GUILayout.ExpandWidth(true));
                            }

                            //uuid
                            EditorGUILayout.LabelField("Uuid:", _lLHelperData.llObjects[i].uuid,
                                GUILayout.ExpandWidth(true));
                            //LLType
                            _lLHelperData.llObjects[i].llType = (LLType) EditorGUILayout.EnumPopup("LLType:",
                                _lLHelperData.llObjects[i].llType, GUILayout.ExpandWidth(true));


                            //======================================================================================================================
                            isFoldout_Scheme_Child[i] = EditorGUILayout.Foldout(isFoldout_Scheme_Child[i],
                                $"本地化方案列表[{_lLHelperData.llObjects[i].llSchemeList.Count}]");
                            if (isFoldout_Scheme_Child[i])
                            {
                                // scrollRect_Scheme =
                                //     EditorGUILayout.BeginScrollView(scrollRect_Scheme);
                                // {
                                    EditorGUILayout.BeginVertical();
                                    {
                                        for (int j = 0; j < _lLHelperData.llObjects[i].llSchemeList.Count; j++)
                                        {
                                            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                                            {
                                                
                                                var item = _lLHelperData.llObjects[i].llSchemeList[j];
                                                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                                                {
                                                    _lLHelperData.llObjects[i].llSchemeList.Remove(item);
                                                    return;
                                                }

                                                //tips
                                                item.llISO = (LLISO) EditorGUILayout.EnumPopup("ISO:",
                                                    item.llISO, GUILayout.ExpandWidth(true));
                                                //LLType
                                                switch (_lLHelperData.llObjects[i].llType)
                                                {
                                                    case LLType.Text:
                                                        item.content = EditorGUILayout.TextField("Content:",
                                                            item.content, GUILayout.ExpandWidth(true));
                                                        break;
                                                    case LLType.Image:
                                                        item.sprite = (Sprite)EditorGUILayout.ObjectField("", item.sprite,
                                                            typeof(Sprite), false);
                                                        break;
                                                }
                                               
                                            }EditorGUILayout.EndVertical();
                                           
                                        }

                                        if (GUILayout.Button("+"))
                                        {
                                            _lLHelperData.llObjects[i].llSchemeList.Add(new LLScheme());
                                            scrollRect_Scheme += new Vector2(0, 2);
                                        }
                                    }EditorGUILayout.EndVertical();
                                   
                                // }
                                // EditorGUILayout.EndScrollView();
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }

                    if (GUILayout.Button("+"))
                    {
                        LLObject llObj = new LLObject
                        {
                            uuid = GUID.Generate().ToString()
                        };
                        _lLHelperData.llObjects.Add(llObj);
                        isFoldout_Scheme_Child.Add(false);
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            #endregion

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_lLHelperData);
            }
        }

        /// <summary>
        /// 初始化样式
        /// </summary>
        private static void InitWindowStyle()
        {
            _lLHelperEditorWindow.titleContent.text = "语言本地化编辑器";
            _lLHelperEditorWindow.titleContent.tooltip = "这是一个快捷语言本地化编辑器";
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
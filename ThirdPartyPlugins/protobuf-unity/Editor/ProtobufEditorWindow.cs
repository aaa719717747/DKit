using System;
using DKit.ThirdPartyPlugins.protobuf_unity.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.protobuf_unity.Editor
{
    public class ProtobufEditorWindow : EditorWindow
    {
        public static ProtobufEditorWindow protobufEditorWindow;
        public static PbConfig PbConfigData;

        #region 窗口样式变量

        private Vector2 scrollRect;
        private bool isFoldout = true;

        #endregion

        #region Init 初始化窗口

        [MenuItem("DKit/PB/PB配置")]
        public static void Init()
        {
            if (PbConfigData == null)
            {
                var data = AssetDatabase.LoadAssetAtPath<PbConfig>(
                    "Assets/DKit/ThirdPartyPlugins/protobuf-unity/Runtime/Core/PBConfig.asset");
                if (data)
                {
                    PbConfigData = data;
                }
                else
                {
                    PbConfig n_data = CreateInstance<PbConfig>();
                    AssetDatabase.CreateAsset(n_data,
                        "Assets/DKit/ThirdPartyPlugins/protobuf-unity/Runtime/Core/PBConfig.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    PbConfigData = AssetDatabase.LoadAssetAtPath<PbConfig>(
                        "Assets/DKit/ThirdPartyPlugins/protobuf-unity/Runtime/Core/PBConfig.asset");
                }
            }

            if (protobufEditorWindow == null)
            {
                protobufEditorWindow = CreateInstance<ProtobufEditorWindow>();
            }

            InitWindowStyle();
            protobufEditorWindow.Show();
        }

        #endregion

        #region OnGUI 绘制窗口

        private void OnDestroy()
        {
            EditorUtility.SetDirty(PbConfigData);
        }

        private void OnGUI()
        {
            #region ScrollView

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect, this[LayoutStyle.GroupBox]);
            {
                EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                {
                    PbConfigData.pbPathType = (PBPathType) EditorGUILayout.EnumPopup("存储路径:",
                        PbConfigData.pbPathType, EditorStyles.popup,
                        GUILayout.ExpandWidth(true));

                    if (PbConfigData.pbPathType == PBPathType.custom)
                    {
                        PbConfigData.customPath = EditorGUILayout.TextField("自定义路径:", PbConfigData.customPath,
                            GUILayout.ExpandWidth(true));
                    }

                    PbConfigData.suffix = EditorGUILayout.TextField("文件后缀:", PbConfigData.suffix,
                        GUILayout.ExpandWidth(true));
                    PbConfigData.pkey =
                        EditorGUILayout.TextField("密钥:", PbConfigData.pkey, GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            #endregion

            if (GUI.changed)
            {
                EditorUtility.SetDirty(PbConfigData);
            }
        }

        #endregion

        #region InitWindowStyle 初始化样式

        /// <summary>
        /// 初始化样式
        /// </summary>
        private static void InitWindowStyle()
        {
            protobufEditorWindow.titleContent.text = "PB编辑器";
            protobufEditorWindow.titleContent.tooltip = "这是一个快捷PB编辑器";
        }

        #endregion

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
using System;
using System.Diagnostics;
using System.IO;
using DKit.ThirdPartyPlugins.LogHelper.Runtime;
using DKit.ThirdPartyPlugins.LogHelper.Runtime.Data;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LogHelper.Editor
{
    public class LogHelperEditorWindow : EditorWindow
    {
        public static LogHelperEditorWindow logHelperEditorWindow;

        private static LogHelperData _logHelperData;

        public static LogHelperData logHelperData
        {
            get
            {
                if (_logHelperData == null)
                {
                    var data = AssetDatabase.LoadAssetAtPath<LogHelperData>(
                        "Assets/DKit/ThirdPartyPlugins/LogHelper/Runtime/Data/LogHelperData.asset");
                    if (data)
                    {
                        _logHelperData = data;
                    }
                    else
                    {
                        LogHelperData n_data = CreateInstance<LogHelperData>();
                        AssetDatabase.CreateAsset(n_data,
                            "Assets/DKit/ThirdPartyPlugins/LogHelper/Runtime/Data/LogHelperData.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        _logHelperData = n_data;
                    }
                }

                return _logHelperData;
            }
        }

        #region 窗口样式变量

        private Vector2 scrollRect;
        private bool isFoldout = true;

        #endregion

        #region 初始化窗口

        [MenuItem("DKit/LogHelper/配置界面", false, 1)]
        public static void Init()
        {
            if (logHelperEditorWindow == null)
            {
                logHelperEditorWindow = CreateInstance<LogHelperEditorWindow>();
            }

            InitWindowStyle();
            logHelperEditorWindow.Show();
        }

        private void OnValidate()
        {
            EditorUtility.SetDirty(this);
        }

        #endregion

        #region 绘制部分

        private void OnGUI()
        {
            #region ScrollView

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect, this[LayoutStyle.GroupBox]);
            {
                EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                {
                    logHelperData.IsEnable = EditorGUILayout.Toggle("启用日志?:", logHelperData.IsEnable,
                        EditorStyles.toggle, GUILayout.ExpandWidth(true));
                    logHelperData.IsSava = EditorGUILayout.Toggle("存储日志?:", logHelperData.IsSava, EditorStyles.toggle,
                        GUILayout.ExpandWidth(true));

                    if (logHelperData.IsSava)
                    {
                        logHelperData.saveLogPathType = (LogPathType) EditorGUILayout.EnumPopup("存储路径:",
                            logHelperData.saveLogPathType, EditorStyles.popup,
                            GUILayout.ExpandWidth(true));
                        if (logHelperData.saveLogPathType == LogPathType.custom)
                        {
                            logHelperData.saveCustomPath = EditorGUILayout.TextField("自定义路径:",
                                logHelperData.saveCustomPath,
                                GUILayout.ExpandWidth(true));
                        }


                        //选择存储日志的类型
                        int c = Enum.GetValues(typeof(LogEnum)).Length;
                        if (logHelperData.isWriteFiles.Length <= 0)
                        {
                            logHelperData.isWriteFiles = new bool[c];
                        }
                        else
                        {
                            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                            {
                                for (int i = 0; i < c - 1; i++)
                                {
                                    logHelperData.isWriteFiles[i] = EditorGUILayout.Toggle(
                                        $"写入{((LogEnum) i).ToString()}?:", logHelperData.isWriteFiles[i],
                                        EditorStyles.toggle, GUILayout.ExpandWidth(true));
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }

                    logHelperData.IsRegularlyCleaned = EditorGUILayout.Toggle("是否定时清理?:",
                        logHelperData.IsRegularlyCleaned, EditorStyles.toggle,
                        GUILayout.ExpandWidth(true));
                    if (logHelperData.IsRegularlyCleaned)
                    {
                        logHelperData.retainedMaxNum = EditorGUILayout.IntField("最多保留条数:", logHelperData.retainedMaxNum,
                            EditorStyles.numberField,
                            GUILayout.ExpandWidth(true));
                    }


                    #region 打开目录

                    if (GUILayout.Button("打开日志目录", GUILayout.ExpandWidth(true), GUILayout.Height(20),
                            GUILayout.ExpandWidth(true)))
                    {
                        LogUtil.OpenLogDictory(GetPath);
                    }

                    #endregion
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            #endregion

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }

        #endregion

        [MenuItem("DKit/LogHelper/打开日志目录", false, 2)]
        public static void OpenLogDictMenu()
        {
            LogUtil.OpenLogDictory(GetPath);
        }


        public static string GetPath
        {
            get
            {
                string strPath = String.Empty;
                switch (logHelperData.saveLogPathType)
                {
                    case LogPathType.persistentDataPath:
                        strPath = Application.persistentDataPath + "/Logs";
                        break;
                    case LogPathType.dataPath:
                        strPath = Application.dataPath + "/Logs";
                        break;
                    case LogPathType.streamingAssetsPath:
                        strPath = Application.streamingAssetsPath + "/Logs";
                        break;
                    case LogPathType.custom:
                        strPath = logHelperData.saveCustomPath + "/Logs";
                        break;
                }

                return strPath;
            }
        }

        #region 初始化样式

        /// <summary>
        /// 初始化样式
        /// </summary>
        private static void InitWindowStyle()
        {
            logHelperEditorWindow.titleContent.text = "LogHelper编辑器";
            logHelperEditorWindow.titleContent.tooltip = "这是一个快捷日志编辑器";
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
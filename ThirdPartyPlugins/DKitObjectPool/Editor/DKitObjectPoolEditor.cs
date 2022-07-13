using System;
using DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.DKitObjectPool.Editor
{
    public class DKitObjectPoolEditor : EditorWindow
    {
        public static DKitObjectPoolEditor DKitObjectPool;
        public static DKitObjectPoolData DKitObjectPoolData;

        #region 窗口样式变量

        private Vector2 scrollRect;
        private bool isFoldout=true;

        #endregion

        [MenuItem("DKit/ObjectPool",false,2)]
        public static void Init()
        {
            if (DKitObjectPoolData == null)
            {
                var data = Resources.Load<DKitObjectPoolData>("DKitObjectPoolData");
                if (data)
                {
                    DKitObjectPoolData = data;
                }
                else
                {
                    DKitObjectPoolData n_data = ScriptableObject.CreateInstance<DKitObjectPoolData>();
                    AssetDatabase.CreateAsset(n_data, "Assets/DKit/ThirdPartyPlugins/DKitObjectPool/Runtime/Core/DKitObjectPoolData.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    DKitObjectPoolData =  ScriptableObject.CreateInstance<DKitObjectPoolData>();
                }
            }
            if (DKitObjectPool == null)
            {
                DKitObjectPool = ScriptableObject.CreateInstance<DKitObjectPoolEditor>();
            }

            InitWindowStyle();
            DKitObjectPool.Show();
        }

        /// <summary>
        /// 初始化样式
        /// </summary>
        private static void InitWindowStyle()
        {
            DKitObjectPool.titleContent.text = "DKitObjectPool编辑器";
            DKitObjectPool.titleContent.tooltip = "这是一个快捷对象池编辑器";
        }

        private void OnGUI()
        {
            if (DKitObjectPoolData == null)return;
            #region ScrollView

            isFoldout = EditorGUILayout.Foldout(isFoldout, $"自定义对象列表[{DKitObjectPoolData.tObjects.Count}]");
            if (isFoldout)
            {
                scrollRect = EditorGUILayout.BeginScrollView(scrollRect, this[LayoutStyle.GroupBox]);
                {
                    for (int i = 0; i < DKitObjectPoolData.tObjects.Count; i++)
                    {
                        EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                        {
                            DKitObjectPoolData.tObjects[i].TId = EditorGUILayout.TextField("TId:", DKitObjectPoolData.tObjects[i].TId,
                                GUILayout.ExpandWidth(true));
                            DKitObjectPoolData.tObjects[i].Prefab = (GameObject) EditorGUILayout.ObjectField("预制体:",
                                DKitObjectPoolData.tObjects[i].Prefab, typeof(GameObject),
                                GUILayout.ExpandWidth(true));
                            DKitObjectPoolData.tObjects[i].InitNum = EditorGUILayout.IntField("初始数量:",
                                DKitObjectPoolData.tObjects[i].InitNum,
                                GUILayout.ExpandWidth(true));
                            DKitObjectPoolData.tObjects[i].RepuishNum = EditorGUILayout.IntField("补充数量:",
                                DKitObjectPoolData.tObjects[i].RepuishNum,
                                GUILayout.ExpandWidth(true));
                            DKitObjectPoolData.tObjects[i].RecycleType = (RecycleType) EditorGUILayout.EnumPopup("回收类型:",
                                DKitObjectPoolData.tObjects[i].RecycleType, EditorStyles.popup,
                                GUILayout.ExpandWidth(true));
                            if (DKitObjectPoolData.tObjects[i].RecycleType == RecycleType.LifeCycle)
                            {
                                DKitObjectPoolData.tObjects[i].ReleaseTimer = EditorGUILayout.IntField("回收生命周期(f):",
                                    DKitObjectPoolData.tObjects[i].ReleaseTimer,
                                    GUILayout.ExpandWidth(true));
                            }

                            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20),
                                    GUILayout.ExpandWidth(true)))
                            {
                                DKitObjectPoolData.tObjects.Remove(DKitObjectPoolData.tObjects[i]);
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }

                    if (GUILayout.Button("+"))
                    {
                        DKitObjectPoolData.tObjects.Add(new DKitObjectData());
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            #endregion

            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
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
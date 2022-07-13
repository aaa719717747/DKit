using System;
using System.Collections.Generic;
using System.Text;
using DKit.ThirdPartyPlugins.LogHelper.Runtime.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime
{
    public static class LogHelper
    {
        public static bool IsInit { get; set; }

        /// <summary>
        /// 日志自定义配置数据
        /// </summary>
        public static LogHelperData LogData { get; set; }


        /// <summary>
        /// 日志更新监听
        /// </summary>
        public delegate void OnLogReceiveCallBack(LogObject lo);

        public static OnLogReceiveCallBack OnLogReceiveCallBackDeletage;


        public delegate void OnInitCall();

        /// <summary>
        /// 初始化日志
        /// </summary>
        public static void Init(OnInitCall call)
        {
            if (IsInit)
            {
                LogWarning(() => "this loghelper already init sucess !");
                return;
            }


            if (LogData == null)
            {
#if UNITY_EDITOR

                var data = AssetDatabase.LoadAssetAtPath<LogHelperData>(
                    "Assets/DKit/ThirdPartyPlugins/LogHelper/Runtime/Data/LogHelperData.asset");
#else
                 var data = Resources.Load<LogHelperData>("Data/LogHelperData");
#endif

                if (data)
                {
                    LogData = data;
                }
                else
                {
#if UNITY_EDITOR
                    LogHelperData n_data = ScriptableObject.CreateInstance<LogHelperData>();
                    AssetDatabase.CreateAsset(n_data,
                        "Assets/DKit/ThirdPartyPlugins/LogHelper/Runtime/Data/LogHelperData.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    LogData = n_data;
#endif
                }
            }


            IsInit = true;
            UpdatePlatfromPath();
            LogUtil.Init();
            LogUtil.LogSystemInfo();
            LogUtil.CleanLogFile();
            Application.logMessageReceived += OnUnitySystemLogCall;
            call?.Invoke();
            
        }

        /// <summary>
        /// 更新平台日志路径
        /// </summary>
        public static void UpdatePlatfromPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    LogData.saveLogPathType = LogPathType.dataPath;
                    break;
                case RuntimePlatform.WindowsPlayer:
                    LogData.saveLogPathType = LogPathType.persistentDataPath;
                    break;
                case RuntimePlatform.WindowsEditor:
                    LogData.saveLogPathType = LogPathType.persistentDataPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    LogData.saveLogPathType = LogPathType.dataPath;
                    break;
            }
        }

        /// <summary>
        /// 监听unityLog
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="stacktrace">调用栈</param>
        /// <param name="type"></param>
        private static void OnUnitySystemLogCall(string condition, string stacktrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    // LogError(() => $"{condition} || {stacktrace}");
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    // LogWarning(() => $"{condition} || {stacktrace}");
                    break;
                case LogType.Log:
                    // Log(() => $"{condition} || {stacktrace}");
                    break;
                case LogType.Exception:
                    LogError(() => $"{condition} || {stacktrace}", LogEnum.ERROR, false);
                    break;
            }
        }

        public static void Log(Func<object> func, LogEnum logType = LogEnum.INFO)
        {
            if (!IsInit) return;
            if (LogData.IsEnable)
            {
                object str = null;
                if (func != null)
                {
                    str = func();
                    var str2 = str.ToString();
                    Debug.Log(str2);
                    LogUtil.SetLogCache(str2, logType, OnLogReceiveCallBackDeletage);
                    if (LogData.IsSava && LogData.isWriteFiles[0])
                    {
                        LogUtil.SaveLog(logType);
                    }
                }
            }
        }

        public static void LogError(Func<object> func, LogEnum logType = LogEnum.ERROR, bool showScreen = true)
        {
            if (!IsInit) return;
            if (LogData.IsEnable)
            {
                object str = null;
                if (func != null)
                {
                    str = func();
                    var str2 = str.ToString();
                    if (showScreen)
                    {
                        Debug.LogError(str2);
                    }

                    LogUtil.SetLogCache(str2, logType, OnLogReceiveCallBackDeletage);
                    if (LogData.IsSava)
                    {
                        LogUtil.SaveLog(logType);
                    }
                }
            }
        }

        public static void LogWarning(Func<string> func, LogEnum logType = LogEnum.WARNING)
        {
            if (!IsInit) return;
            if (LogData.IsEnable)
            {
                object str = null;
                if (func != null)
                {
                    str = func();
                    var str2 = str.ToString();
                    Debug.LogWarning(str);
                    LogUtil.SetLogCache(str2, logType, OnLogReceiveCallBackDeletage);
                    if (LogData.IsSava && LogData.isWriteFiles[1])
                    {
                        LogUtil.SaveLog(logType);
                    }
                }
            }
        }

        public static void LogTrack(Func<string> func, LogEnum logType = LogEnum.TRACK)
        {
            if (!IsInit) return;
            if (LogData.IsEnable)
            {
                object str = null;
                if (func != null)
                {
                    str = func();
                    var str2 = str.ToString();
                    Debug.Log(str2);
                    LogUtil.SetLogCache(str2, logType, OnLogReceiveCallBackDeletage);
                    if (LogData.IsSava && LogData.isWriteFiles[3])
                    {
                        LogUtil.SaveLog(logType);
                    }
                }
            }
        }

        public static void ClearLogView()
        {
            LogUtil.ClearAll();
        }

        /// <summary>
        /// 获取对应日志类型的条目
        /// </summary>
        /// <param name="lt"></param>
        /// <returns></returns>
        public static int LogNum(LogEnum lt = LogEnum.ALL)
        {
            return lt.GetList().Count;
        }

        /// <summary>
        /// 获取本地日志文件的数量
        /// </summary>
        public static int LogFileNum
        {
            get => LogUtil.GetFiles(LogData.SavaPath, "log");
        }
    }
}
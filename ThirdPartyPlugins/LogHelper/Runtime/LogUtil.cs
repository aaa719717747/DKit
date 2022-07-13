using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using DKit.ThirdPartyPlugins.LogHelper.Runtime.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime
{
    public enum LogEnum
    {
        INFO,
        WARNING,
        ERROR,
        TRACK,
        ALL
    }

    public struct SaveThreadStruct
    {
        public ParameterizedThreadStart pts;
        public Thread TempThread;

        public void Init(ParameterizedThreadStart pram)
        {
            pts = new ParameterizedThreadStart(pram);
        }
    }

    public static class LogUtil
    {
        public static IDictionary<int, LogFile> logFiles;

        public static int NowFileId;

        public static Dictionary<LogEnum, List<LogObject>> logEnumDicts;
        private static StreamWriter sw;
        public static string Path;
       
        //存储线程信息
        private static SaveThreadStruct saveThreadStruct;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            logFiles = new Dictionary<int, LogFile>();
            if (!Directory.Exists(LogHelper.LogData.SavaPath + "/Logs"))
            {
                Directory.CreateDirectory(LogHelper.LogData.SavaPath + "/Logs");
            }

            NowFileId = LogHelper.LogFileNum;
            logFiles.Add(NowFileId, new LogFile
            {
                filePath =
                    $"{LogHelper.LogData.SavaPath}/Logs/{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.log",
            });

            logEnumDicts = new Dictionary<LogEnum, List<LogObject>>();

            for (int i = 0; i < Enum.GetValues(typeof(LogEnum)).Length; i++)
            {
                var le = (LogEnum)i;
                if (le != LogEnum.ALL)
                    logEnumDicts.Add(le, new List<LogObject>());
            }

            Path = LogHelper.LogData.SavaPath;
            sw = new StreamWriter(logFiles[NowFileId].filePath);
            saveThreadStruct.Init(SaveLogThread);
        }

        /// <summary>
        /// 打印系统信息(測試)
        /// </summary>
        public static void LogSystemInfo()
        {
            LogHelper.Log(() => "********************系统日志*********************");
            // LogHelper.Log(() => "操作系统:  " + SystemInfo.operatingSystem);
            // LogHelper.Log(() => "系统内存大小:  " + SystemInfo.systemMemorySize);
            // LogHelper.Log(() => "设备模型:  " + SystemInfo.deviceModel);
            // LogHelper.Log(() => "设备唯一标识符:  " + SystemInfo.deviceUniqueIdentifier);
            // LogHelper.Log(() => "处理器数量:  " + SystemInfo.processorCount);
            // LogHelper.Log(() => "处理器类型:  " + SystemInfo.processorType);
            // LogHelper.Log(() => "显卡标识符:  " + SystemInfo.graphicsDeviceID);
            // LogHelper.Log(() => "显卡厂商:  " + SystemInfo.graphicsDeviceVendor);
            // LogHelper.Log(() => "显卡版本:  " + SystemInfo.graphicsDeviceVersion);
            // LogHelper.Log(() => "显存大小:  " + SystemInfo.graphicsMemorySize);
            // LogHelper.Log(() => "显卡着色器级别:  " + SystemInfo.graphicsShaderLevel);
            // LogHelper.Log(() => "是否图像效果:  " + SystemInfo.supportsImageEffects);
            // LogHelper.Log(() => "是否支持内置阴影:  " + SystemInfo.supportsShadows);
            // LogHelper.Log(() => "********************运行日志*********************");
        }

        /// <summary>
        /// 拼装日志信息，并加入缓存
        /// </summary>
        /// <param name="str"></param>
        /// <param name="logType"></param>
        /// <param name="OnLogReceiveCallBackDeletage"></param>
        public static void SetLogCache(string str, LogEnum logType,
            LogHelper.OnLogReceiveCallBack OnLogReceiveCallBackDeletage)
        {
            StackTrace st = new System.Diagnostics.StackTrace(0, true);
            StackFrame[] frames = st.GetFrames();
            StringBuilder bf = new StringBuilder();
            for (int i = frames.Length - 1; i >= 2; i--)
            {
                string fName = frames[i].GetFileName();
                if (!string.IsNullOrEmpty(fName))
                {
                    string[] sp = fName.Split('\\');
                    StringBuilder bfsp = new StringBuilder();
                    for (int j = 3; j < sp.Length; j++)
                    {
                        bfsp.Append(sp[j] + "\\");
                    }

                    string sb = $"# {bfsp}.{frames[i].GetFileLineNumber()} @{frames[i].GetMethod()}";
                    bf.Append(Environment.NewLine);
                    bf.Append(sb);
                }
            }

            LogObject lo = new LogObject
            {
                logId = logFiles[NowFileId].allLogObjects.Count,
                content =
                    $"[{logType.ToString()}] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}=>{str}   {bf.ToString()}]",
                logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                logType = logType
            };

            logType.GetList().Add(lo);
            logFiles[NowFileId].allLogObjects.Add(lo);


            HSLoom.QueueOnMainThread(
                () => { OnLogReceiveCallBackDeletage?.Invoke(lo); }
            );
        }

        /// <summary>
        /// 清除所有日志缓存
        /// </summary>
        public static void ClearAll()
        {
            logEnumDicts[LogEnum.INFO].Clear();
            logEnumDicts[LogEnum.WARNING].Clear();
            logEnumDicts[LogEnum.ERROR].Clear();
            logEnumDicts[LogEnum.TRACK].Clear();
        }

        #region 日志流操作

        /// <summary>
        /// 返回指定类型日志缓存通过日志类型
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        public static List<LogObject> GetList(this LogEnum logType)
        {
            if (logType == LogEnum.ALL) return null;
            return logEnumDicts[logType];
        }

        /// <summary>
        /// 获取所有log条目数量
        /// </summary>
        /// <returns></returns>
        public static int GetListAll()
        {
            return logEnumDicts[LogEnum.INFO].Count + logEnumDicts[LogEnum.ERROR].Count +
                   logEnumDicts[LogEnum.WARNING].Count + logEnumDicts[LogEnum.TRACK].Count;
        }

        /// <summary>
        /// 获取指定日志文件
        /// </summary>
        /// <returns></returns>
        public static string GetOrderLogFile()
        {
            return "";
        }

        /// <summary>
        /// 打开日志目录
        /// </summary>
        /// <exception cref="UnityException"></exception>
        public static void OpenLogDictory(string strPath)
        {
            if (Directory.Exists(strPath))
            {
                Execute(strPath);
            }
            else
            {
                throw new UnityException($"找不到路径:{strPath}");
            }
        }

        /// <summary>
        /// 打开日志目录
        /// </summary>
        /// <exception cref="UnityException"></exception>
        public static void OpenLogDictory()
        {
            string path = LogHelper.LogData.SavaPath + "/Logs/";
            if (Directory.Exists(path))
            {
                Execute(path);
            }
            else
            {
                throw new UnityException($"找不到路径:{path}");
            }
        }

        /// <summary>
        /// 打开指定路径的文件夹。
        /// </summary>
        /// <param name="folder">要打开的文件夹的路径。</param>
        public static void Execute(string folder)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;

                default:
                    throw new FileNotFoundException($"Not support open folder on '{Application.platform}' platform.");
            }
        }

        /// <summary>
        /// 日志清理任务
        /// </summary>
        public static void CleanLogFile()
        {
            if (LogHelper.LogData.IsRegularlyCleaned)
            {
                DirectoryInfo dInfo = new DirectoryInfo(LogHelper.LogData.SavaPath + "/Logs/");
                // 获取 文件夹以及子文件加中所有扩展名为  _extension 的文件
                FileInfo[] fileInfoArr = dInfo.GetFiles("*.log", SearchOption.AllDirectories);
                int deleteCount = fileInfoArr.Length - LogHelper.LogData.retainedMaxNum;
                if (deleteCount > 0)
                {
                    for (int i = 0; i < deleteCount; ++i)
                    {
                        string fullName = fileInfoArr[i].FullName;
                        Debug.Log(fullName);
                        File.Delete(fullName);
                        //2.27s
                        //3.29s
                    }
                }
            }
        }

        /// <summary>
        /// 日志排序
        /// </summary>
        /// <param name="maxRemainNum"></param>
        /// <param name="fileInfoArr"></param>
        private static void DateSort(int maxRemainNum, FileInfo[] fileInfoArr)
        {
            Dictionary<string, long> fileNumber = new Dictionary<string, long>();
            for (int i = 0; i < fileInfoArr.Length; ++i)
            {
                string fullName = fileInfoArr[i].FullName;
                string[] fullNmaes = fullName.Split('-');
                long _year = long.Parse(fullNmaes[0]) * 1000;
                long _mouth = long.Parse(fullNmaes[0]) * 100;
                long _day = long.Parse(fullNmaes[0]) * 50;
                long _hour = long.Parse(fullNmaes[0]) * 20;
                long _min = long.Parse(fullNmaes[0]) * 10;
                long _scends = long.Parse(fullNmaes[0]);
                long a = _year + _mouth + _day + _hour + _min + _scends;
                fileNumber.Add(fullName, a);
            }
        }

        /// <summary>
        /// 获取文件夹中所有指定扩展名的文件信息
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static int GetFiles(string dirPath, string extension)
        {
            DirectoryInfo dInfo = new DirectoryInfo(dirPath);
            // 获取 文件夹以及子文件加中所有扩展名为  _extension 的文件
            return dInfo.GetFiles(extension, SearchOption.AllDirectories).Length;
        }
#if UNITY_EDITOR
        /// <summary>
        /// 清除所有日志
        /// </summary>
        /// <exception cref="Exception"></exception>
        [MenuItem("DKit/LogHelper/清除所有日志", false, 3)]
        private static void ClearLogFiles()
        {
            int a = 0;
            try
            {
                string str = "";
                var data = AssetDatabase.LoadAssetAtPath<LogHelperData>(
                    "Assets/DKit/ThirdPartyPlugins/LogHelper/Runtime/Data/LogHelperData.asset");
                switch (data.saveLogPathType)
                {
                    case LogPathType.persistentDataPath:
                        str = Application.persistentDataPath;
                        break;
                    case LogPathType.dataPath:
                        str = Application.dataPath;
                        break;
                    case LogPathType.streamingAssetsPath:
                        str = Application.streamingAssetsPath;
                        break;
                    case LogPathType.custom:
                        str = data.saveCustomPath;
                        break;
                }

                string[] files = Directory.GetFiles(str + "/Logs"); //得到文件
                foreach (string file in files) //循环文件
                {
                    string exname = file.Substring(file.LastIndexOf(".") + 1); //得到后缀名
                    if ($".log".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1) //如果后缀名为.txt文件
                    {
                        Debug.Log(file);
                        FileInfo fi = new FileInfo(file); //建立FileInfo对象
                        File.Delete(file);
                        a++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            Debug.Log($"清除了{a}个文件！");
        }
#endif
        /// <summary>
        /// 关闭流操作
        /// </summary>
        public static void Dispose()
        {
            sw.Close();
        }

        /// <summary>
        /// 写入信息至Log文件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public static void SaveLog(LogEnum logType)
        {
            saveThreadStruct.TempThread = new Thread(saveThreadStruct.pts);
            saveThreadStruct.TempThread.Start(logType);
        }

        private static void SaveLogThread(object lT)
        {
            try
            {
                var logType = (LogEnum)lT;

                if (!Directory.Exists(Path + "/Logs"))
                {
                    Directory.CreateDirectory(Path + "/Logs");
                }
                
                
                if (!File.Exists(logFiles[NowFileId].filePath))
                {
                    FileStream fc = File.Create(logFiles[NowFileId].filePath);
                    fc.Close();
                }

                var sl = logEnumDicts[logType];

                sw.WriteLine(logEnumDicts[logType][sl.Count - 1].content, true);
                sw.WriteLine(Environment.NewLine, true);
            }
            catch (Exception exception)
            {
                Debug.Log("获取 Application.streamingAssetsPath 报错！" + exception.Message, null);
                return;
            }
            saveThreadStruct.TempThread.Abort();
        }

        #endregion
    }
}
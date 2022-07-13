using System;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.Data
{
    public enum LogPathType
    {
        persistentDataPath,
        dataPath,
        streamingAssetsPath,
        custom
    }
    public class LogHelperData:ScriptableObject
    {
        public bool IsEnable;
        public bool IsSava;
        public LogPathType saveLogPathType;
        public string saveCustomPath;
        public bool IsRegularlyCleaned;
        public int retainedMaxNum;
        public bool[] isWriteFiles;
        
        public string SavaPath
        {
            get
            {
                string str = "";
                switch (saveLogPathType)
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
                        str = saveCustomPath;
                        break;
                }

                return str;
            }
        }
    }
}
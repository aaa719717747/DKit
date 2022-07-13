using System.Collections.Generic;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime
{
    public class LogFile
    {
        public string filePath;
        public List<LogObject> allLogObjects = new List<LogObject>();
    }
}
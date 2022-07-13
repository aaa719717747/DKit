using System.IO;

namespace Im.Unity.protobuf_unity.Tests.Runtime
{
    public class TestFileLoaderHelper
    {
        static string GetCurSourceFileAbsDir
        {
            get
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
                return Path.GetDirectoryName(st.GetFrame(0).GetFileName());
            }
        }

        public static string LoadJsonAtFolderDataJson(string inPathName)
        {
            string tPath = Path.Combine(GetCurSourceFileAbsDir, "DataJson", inPathName);
            StreamReader streamReader = new StreamReader(tPath, System.Text.Encoding.Default);
            return streamReader.ReadToEnd();
        }
    }
}
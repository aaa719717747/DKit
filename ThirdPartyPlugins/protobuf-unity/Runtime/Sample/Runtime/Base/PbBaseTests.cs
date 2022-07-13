using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Im.Unity.protobuf_unity.Tests.Runtime.Base
{
    public class PbBaseTests
    {

        protected PbBaseTests()
        {
            this.TypeNamespace = this.GetType().Namespace;
            this.TypeName = this.GetType().Name;
        }

        internal string TypeName { get; }

        internal string TypeNamespace { get; }

        internal void UnitTestLog(string log, string method = "")
        {
            Debug.Log(
                $"=> UnitTest [{TypeNamespace}.{TypeName}{(string.IsNullOrEmpty(method) ? "" : $".{method}()")}] {log}");
        }

        internal int LineNum()
        {
            StackTrace st = new StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }
    }
}
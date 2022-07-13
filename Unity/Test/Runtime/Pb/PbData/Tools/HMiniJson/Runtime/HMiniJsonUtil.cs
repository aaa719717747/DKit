using System;
using System.Collections.Generic;

namespace DKit.Unity.Tools.HMiniJson.Runtime
{
    public static class HMiniJsonUtil
    {
        /// <summary>
        /// fast get child nodes
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    Dictionary&lt;string,object&gt; node = HMiniJsonUtil.OptObj(dataMap, "node");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <returns></returns>
        public static Dictionary<string, object> OptObj(Dictionary<string, object> o, string key)
        {
            if (o == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return !o.ContainsKey(key) ? null : (Dictionary<string, object>)o[key];
        }


        /// <summary>
        /// fast get int value
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    var val = HMiniJsonUtil.OptInt(dataMap, "history");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <param name="def">def value</param>
        /// <returns></returns>
        public static int OptInt(Dictionary<string, object> o, string key, int def = 0)
        {
            if (o == null)
            {
                return def;
            }

            if (string.IsNullOrEmpty(key))
            {
                return def;
            }

            return !o.ContainsKey(key) ? def : Convert.ToInt32(o[key]);
        }

        /// <summary>
        /// fast get Int16 value
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    var val = HMiniJsonUtil.OptInt16(dataMap, "sid");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <param name="def">def value</param>
        /// <returns></returns>
        public static Int16 OptInt16(Dictionary<string, object> o, string key, Int16 def = 0)
        {
            if (o == null)
            {
                return def;
            }

            if (string.IsNullOrEmpty(key))
            {
                return def;
            }

            return !o.ContainsKey(key) ? def : Convert.ToInt16(o[key]);
        }

        /// <summary>
        /// fast get Int64 value
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    var val = HMiniJsonUtil.OptInt64(dataMap, "timestamp");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <param name="def">def value</param>
        /// <returns></returns>
        public static Int64 OptInt64(Dictionary<string, object> o, string key, Int64 def = 0)
        {
            if (o == null)
            {
                return def;
            }

            if (string.IsNullOrEmpty(key))
            {
                return def;
            }

            return !o.ContainsKey(key) ? def : Convert.ToInt64(o[key]);
        }

        /// <summary>
        /// fast get long value
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    var val = HMiniJsonUtil.OptLong(dataMap, "timestamp");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <param name="def">def value</param>
        /// <returns></returns>
        public static long OptLong(Dictionary<string, object> o, string key, long def = 0)
        {
            return OptInt64(o, key, def);
        }


        /// <summary>
        /// fast get string value
        /// <example>
        /// <code>
        ///    var dataMap = HMiniJson.Deserialize(jsonString) as Dictionary&lt;string,object&gt;;
        ///    var val = HMiniJsonUtil.OptLong(dataMap, "name");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="o">Dictionary&lt;string,object&gt;</param>
        /// <param name="key">json key</param>
        /// <param name="def">def value</param>
        /// <returns></returns>
        public static string OptString(Dictionary<string, object> o, string key, string def = "")
        {
            if (o == null)
            {
                return def;
            }

            if (string.IsNullOrEmpty(key))
            {
                return def;
            }

            return !o.ContainsKey(key) ? def : Convert.ToString(o[key]);
        }
    }
}
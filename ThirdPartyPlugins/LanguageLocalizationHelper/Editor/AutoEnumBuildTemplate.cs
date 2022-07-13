using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Editor
{
    public class AutoEnumBuildTemplate
    {
        public static string UIClass =
            @"using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace DKit.ThirdPartyPlugins.LanguageLocalizationHelper.Runtime
{
//以下代码都是通过脚本自动生成,请勿修改!
    public class #类名# 
    {
   
         #成员#
   
    }
}
";
    }

    public class AutoEnumBuild
    {
        // 判断 字符串是否为数字方法
        public static bool isNumber(string str)
        {
            bool isMatch = Regex.IsMatch(str, @"^\d+$"); // 判断字符串是否为数字 的正则表达式
            return isMatch;
        }

        public static bool isContainsChar(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c.Equals('~') || c.Equals('#') || c.Equals('^') || c.Equals('.') || c.Equals('!') ||
                    c.Equals('@') || c.Equals('*') || c.Equals('~') || c.Equals('~')
                    || c.Equals('&') || c.Equals('~') || c.Equals(']') || c.Equals('[') || c.Equals('=') ||
                    c.Equals('(') || c.Equals(')') || c.Equals('+') || c.Equals('-')
                    || c.Equals('?') || c.Equals('/') || c.Equals('\\') || c.Equals('{') || c.Equals('}') ||
                    c.Equals('"') || c.Equals(';') || c.Equals(']') || c.Equals('[')|| c.Equals(',')|| c.Equals('，')
                    ||c.Equals('<')||c.Equals('>')||c.Equals('$')||c.Equals('%')||c.Equals('“')
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public static void BuildUIScript()
        {
            string enumSettingName = "LLTipEnum";
            //
            LLHelperData Data = Resources.Load<LLHelperData>("LLHelperData");
            List<string> strFarmName = new List<string>();
            foreach (var VARIABLE in Data.llObjects)
            {
                if (string.IsNullOrEmpty(VARIABLE.tips))
                {
                    strFarmName.Add($"_{VARIABLE.uuid}");
                }
                else
                {
                    if (isNumber(VARIABLE.tips.Substring(0, 1)))
                    {
                        strFarmName.Add($"_{VARIABLE.tips}");
                    }
                    else
                    {
                        if (!isContainsChar(VARIABLE.tips))
                        {
                            strFarmName.Add(VARIABLE.tips); 
                        }
                        else
                        {
                            strFarmName.Add($"_{VARIABLE.uuid}");
                        }
                    }
                }
            }

            //
            string scriptPath = Application.dataPath + "/DKit/ThirdPartyPlugins/LanguageLocalizationHelper/Runtime/" +
                                enumSettingName + ".cs";

            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }

            string stringEnumName = "public enum LLTip{ ";

            foreach (var value in strFarmName)
            {
                stringEnumName += value + ",";
            }

            stringEnumName = stringEnumName.Substring(0, stringEnumName.Length - 1);
            stringEnumName += "}";


            //
            string classInfo = AutoEnumBuildTemplate.UIClass;
            classInfo = classInfo.Replace("#类名#", enumSettingName);
            classInfo = classInfo.Replace("#成员#", stringEnumName);
            //

            FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileW.Write(classInfo);
            fileW.Flush();
            fileW.Close();
            file.Close();
            //
            // Debug.Log("创建脚本 " + Application.dataPath + "/Scripts/" + enumSettingName + ".cs 成功!");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
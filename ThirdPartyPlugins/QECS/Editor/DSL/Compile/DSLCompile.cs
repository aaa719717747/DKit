using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DKit.ThirdPartyPlugins.QECS.Runtime.Core.Data;
using ILRuntime.Mono.Cecil;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.QECS.Editor.DSL.Compile
{
    public class DSLCompile
    {
        public static QECSData QecsData;

        public static string UIClass =
            @"
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace DKit.ThirdPartyPlugins.QECS.Runtime.Core.DSL
{
    //以下代码都是通过脚本自动生成,请勿修改!
    public class #类名# 
    {
         #成员#
    }
}
";

        [MenuItem("DKit/QECS/DSLCompile", false, 0)]
        public static void Init()
        {
            QecsData = UnityEngine.Resources.Load<QECSData>("QECSData");
            if (QecsData == null)
            {
                throw new UnityException(
                    "[Init]Build Fail! =>because:not find QECSData(ScriptObject) please check isExist?or path isError?");
                return;
            }

            BuildAllDSLFiles();
        }

        /// <summary>
        /// [编译检查通过才]清理所有现有文件
        /// </summary>
        static void CheckClear()
        {
        }

        static void BuildAllDSLFiles()
        {
            try
            {
                //找到所有的dsl文件
                List<FileInfo> allFileInfos = GetFiles(QecsData.inputDSLPath);
                //对文件内容解析
                foreach (var VARIABLE in allFileInfos)
                {
                    ParseDSL($"{QecsData.inputDSLPath}/{VARIABLE.Name}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public static List<FileInfo> GetFiles(string path)
        {
            List<FileInfo> allFiles = new List<FileInfo>();
            //获取指定路径下面的所有资源文件
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                Debug.Log(files.Length);

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }

                    Debug.Log("Name:" + files[i].Name);
                    Debug.Log("FullName:" + files[i].FullName);
                    Debug.Log("DirectoryName:" + files[i].DirectoryName);
                    allFiles.Add(files[i]);
                }
            }

            return allFiles;
        }

        static void ParseDSL(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open);
            StreamReader sr = new StreamReader(file, System.Text.Encoding.UTF8);

            while (!sr.EndOfStream)
            {
                string lineStr = sr.ReadLine();
                for (int i = 0; i < lineStr.Length; i++)
                {
                    if (i + 2 <= lineStr.Length)
                    {
                        if (lineStr.Substring(i, 2).Equals("//"))
                        {
                            //这里有注释
                            i = lineStr.Length;
                            continue;
                        }
                    }
                }
            }

            sr.Close();
        }

        static void BuildEntity()
        {
            StringBuilder sb_cs = new StringBuilder();
            //代码cs拼接中

            //生成写入
            FileStream file = new FileStream(QecsData.outputDSLPath, FileMode.CreateNew);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileW.Write(sb_cs.ToString());
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
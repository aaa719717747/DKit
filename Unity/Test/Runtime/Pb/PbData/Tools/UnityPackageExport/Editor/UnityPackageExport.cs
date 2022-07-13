using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using DKit.Unity.Tools.HMiniJson.Runtime;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DKit.Unity.Test.Runtime.Pb.PbData.Tools.UnityPackageExport.Editor
{
    public class UnityPackageExport
    {
        /// <summary>
        /// 导出LogHelper.unitypackage
        /// </summary>
        // [MenuItem("EditorTools/Export &E")]
        [MenuItem("DKit/Export Helper/Export LogHelper", false, 1)]
        static void ExportLogHelper()
        {
            string packageJsonPath = LogHelperPackageJsonPath();
            string packageName =
                $"DKit-ThirdPartyPlugins-LogHelper-{FetchUpmVersion(packageJsonPath)}-{DateTime.Now:yyyy-MM-dd-HH-mm}.unitypackage"; //生成包名
            ExportByConfig(packageName, LogHelperdata);
        }
        /// <summary>
        /// 导出LanguageLocalizationHelper.unitypackage
        /// </summary>
        // [MenuItem("EditorTools/Export &E")]
        [MenuItem("DKit/Export Helper/Export LanguageLocalizationHelper", false, 3)]
        static void ExportLanguageLocalizationHelper()
        {
            string packageJsonPath = LanguageLocalizationHelperPackageJsonPath();
            string packageName =
                $"DKit-ThirdPartyPlugins-LanguageLocalizationHelper-{FetchUpmVersion(packageJsonPath)}-{DateTime.Now:yyyy-MM-dd-HH-mm}.unitypackage"; //生成包名
            ExportByConfig(packageName, LanguageLocalizationHelperdata);
        }
        /// <summary>
        /// 导出ProScrollView
        /// </summary>
        // [MenuItem("EditorTools/Export &E")]
        [MenuItem("DKit/Export Helper/Export ProScrollView", false, 2)]
        static void ExportProScrollView()
        {
            string packageJsonPath = ProScrollViewPackageJsonPath();
            string packageName =
                $"DKit-ThirdPartyPlugins-ProScrollView-{FetchUpmVersion(packageJsonPath)}-{DateTime.Now:yyyy-MM-dd-HH-mm}.unitypackage"; //生成包名
            ExportByConfig(packageName, ProScrollViewdata);
        }
        private static string LanguageLocalizationHelperPackageJsonPath()
        {
            return Path.Combine(ProjectRoot(), "Assets", "DKit", "ThirdPartyPlugins", "LanguageLocalizationHelper", "package.json");
        }
        private static string LogHelperPackageJsonPath()
        {
            return Path.Combine(ProjectRoot(), "Assets", "DKit", "ThirdPartyPlugins", "LogHelper", "package.json");
        }
        private static string ProScrollViewPackageJsonPath()
        {
            return Path.Combine(ProjectRoot(), "Assets", "DKit", "ThirdPartyPlugins", "ProScrollView", "package.json");
        }
        private static readonly string[] LogHelperdata =
        {
            "Assets/DKit/ThirdPartyPlugins/LogHelper",
        };
        private static readonly string[] LanguageLocalizationHelperdata =
        {
            "Assets/DKit/ThirdPartyPlugins/LanguageLocalizationHelper",
        };
        private static readonly string[] ProScrollViewdata =
        {
            "Assets/DKit/ThirdPartyPlugins/ProScrollView",
        };
        static string FetchUpmVersion(string path)
        {
            string allText = File.ReadAllText(path);
            var packageJson = Unity.Tools.HMiniJson.Runtime.HMiniJson.Deserialize(allText) as Dictionary<string, object>;
            return HMiniJsonUtil.OptString(packageJson, "version", "0.0.0");
        }
        private static string ArrStringToPrintStr(string[] assetPathNames)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            foreach (string name in assetPathNames)
            {
                sb.Append("\"").Append(name).Append("\",\n");
            }

            sb.Append("}");
            string info = sb.ToString();
            return info;
        }
        private static void ExportByConfig(string packageName, string[] assetPathNames)
        {
            var info = ArrStringToPrintStr(assetPathNames);

            Debug.Log($"assetPathNames config:\n {info}");

            // GUIUtility.systemCopyBuffer = packageName; //将包名复制到剪切板
            AssetDatabase.ExportPackage
            (
                assetPathNames,
                packageName,
                ExportPackageOptions.Recurse
            );
            // Application.OpenURL(System.IO.Directory.GetParent(Application.dataPath)?.ToString());
            // Debug.Log($"Ex");

            string exportFolder = Directory.GetParent(Application.dataPath)?.ToString();
            if (exportFolder != null)
            {
                string exportPkg = Path.Combine(exportFolder, packageName);
                string finalFolder = Directory.GetParent(exportFolder)?.FullName;
                if (finalFolder != null)
                {
                    string finalPath = Path.Combine(finalFolder, packageName);
                    if (File.Exists(exportPkg))
                    {
                        File.Move(exportPkg, finalPath);
                        Execute(finalFolder);
                    }
                }
            }

            Debug.Log($"ExportByConfig success: {packageName}");
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
        private static string ProjectRoot()
        {
            return Directory.GetParent(Application.dataPath)?.ToString();
        }
    }
}
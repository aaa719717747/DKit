using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Google.Protobuf;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DKit.ThirdPartyPlugins.protobuf_unity.Runtime.Core
{
    public enum PBPathType
    {
        persistentDataPath,
        dataPath,
        streamingAssetsPath,
        custom
    }

    public static class PBHelper
    {
        private static PbConfig _pbInitData;


        public static void Init()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        static PbConfig PbInitData
        {
            get
            {
                if (_pbInitData == null)
                {
                    var data = AssetDatabase.LoadAssetAtPath<PbConfig>(
                        "Assets/DKit/ThirdPartyPlugins/protobuf-unity/Runtime/Core/PBConfig.asset");
                    if (data)
                    {
                        _pbInitData = data;
                    }
                    else
                    {
                        PbConfig n_data = ScriptableObject.CreateInstance<PbConfig>();
                        AssetDatabase.CreateAsset(n_data,
                            "Assets/DKit/ThirdPartyPlugins/protobuf-unity/Runtime/Core/PBConfig.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        _pbInitData = n_data;
                    }
                }
                return _pbInitData;
            }
            
        }

        [MenuItem("DKit/PB/打开Pb文件目录", false, 3)]
        public static void OpenPbDirectory()
        {
            if (Directory.Exists(path))
            {
                Execute(path);
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        [MenuItem("DKit/PB/清除Pb目录", false, 4)]
        public static void ClearPbDirectory()
        {
            ClearPBFile();
        }

        [MenuItem("DKit/PB/更新Pb密钥", false, 5)]
        public static void UpdatePbPasswordKey()
        {
            if (string.IsNullOrEmpty(PbInitData.pkey))
            {
                PbInitData.pkey = GUID.Generate().ToString();
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示!", "更新密钥会删除之前的所有PB文件，你确定?", "OK", "Cancel"))
                {
                    PbInitData.pkey = GUID.Generate().ToString();
                    ClearPBFile();
                }
            }
            EditorUtility.SetDirty(PbInitData);
        }

        /*
         * 
         */
        static string path
        {
            get
            {
                string strPath = String.Empty;
                switch (PbInitData.pbPathType)
                {
                    case PBPathType.persistentDataPath:
                        strPath = Application.persistentDataPath;
                        break;
                    case PBPathType.dataPath:
                        strPath = Application.dataPath;
                        break;
                    case PBPathType.streamingAssetsPath:
                        strPath = Application.streamingAssetsPath;
                        break;
                    case PBPathType.custom:
                        strPath = PbInitData.customPath;
                        break;
                }

                return strPath + "/Infos";
            }
        }

        static string suffix
        {
            get { return PbInitData.suffix; }
        }

        static string Key
        {
            get { return PbInitData.pkey; }
        }

        public static void Write<T>(T t) where T : IMessage
        {
            //待修改
            File.WriteAllBytes($"{path}/{t.GetType()}.{suffix}",
                RijndaelEncrypt(t.ToByteArray(), Key));
        }

        public static void Read<T>(T t) where T : IMessage
        {
            if (File.Exists($"{path}/{t.GetType()}.{suffix}"))
            {
                byte[] bs = File.ReadAllBytes($"{path}/{t.GetType()}.{suffix}");
                t.MergeFrom(RijndaelDecrypt(bs, Key));
            }
            else
            {
                Write(t);
            }
        }

        /// <summary>
        /// 清除PB目录
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void ClearPBFile()
        {
            int a = 0;
            try
            {
                string[] files = Directory.GetFiles(path); //得到文件
                foreach (string file in files) //循环文件
                {
                    string exname = file.Substring(file.LastIndexOf(".") + 1); //得到后缀名
                    if ($".{PbInitData.suffix}".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1) //如果后缀名为.txt文件
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
        /// Rijndael加密算法
        /// </summary>
        /// <param name="pString">待加密的明文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        public static byte[] RijndaelEncrypt(byte[] toEncryptArray, string pkey)
        {
            //解密密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pkey);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            //返回加密后的密文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }

        /// <summary>
        /// ijndael解密算法
        /// </summary>
        /// <param name="pString">待解密的密文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        public static byte[] RijndaelDecrypt(byte[] toEncryptArray, string pKey)
        {
            //解密密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            //返回解密后的明文
            byte[] resultArray = new byte[] { };
            try
            {
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception e)
            {
                Debug.LogWarning("pb文件已损坏!,重置文档!");
            }

            return resultArray;
        }
    }
}
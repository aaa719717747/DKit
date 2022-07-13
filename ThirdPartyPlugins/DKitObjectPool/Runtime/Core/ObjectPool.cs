using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.DKitObjectPool.Runtime.Core
{
    public class ObjectPool
    {
        static bool isInit { get; set; }
        static DKitObjectPoolData PoolData { get; set; }

        static IDictionary<string, Queue<DKitObject>> PoolDicts
            = new Dictionary<string, Queue<DKitObject>>();

        static IDictionary<string, Transform> PoolTransfromDicts
            = new Dictionary<string, Transform>();

        /// <summary>
        /// 初始化对象池
        /// </summary>
        static public void Init()
        {
            if (isInit) return;
            isInit = true;
            var data = Resources.Load<DKitObjectPoolData>("DKitObjectPoolData");

            if (data)
            {
                PoolData = data;
            }

            //生成一个父级目录
            GameObject parent = new GameObject($"DKitObjectPool[{PoolData.tObjects.Count}]");
            foreach (var item in PoolData.tObjects)
            {
                Queue<DKitObject> qdobjs = new Queue<DKitObject>();
                GameObject itemParent = new GameObject($"{item.TId}_Groups[{item.InitNum}]");
                itemParent.transform.SetParent(parent.transform);
                for (int i = 0; i < item.InitNum; i++)
                {
                    GameObject go = GameObject.Instantiate(item.Prefab, itemParent.transform);
                    go.transform.localPosition = Vector3.zero;
                    DKitObject dkitObj = go.AddComponent<DKitObject>();
                    dkitObj.Off(item);
                    qdobjs.Enqueue(dkitObj);
                }

                PoolDicts.Add(item.TId, qdobjs);
                PoolTransfromDicts.Add(item.TId, itemParent.transform);
            }
        }

        static public DKitObject Get(string tid)
        {
            if (!PoolDicts.ContainsKey(tid))
            {
                throw new UnityException("tid错误,或者没有配置该对象，请检查对象池配置!");
            }

            Queue<DKitObject> targetPool = PoolDicts[tid];
            foreach (var item in targetPool)
            {
                if (!item.isActive)
                {
                    item.On();
                    return item;
                }
            }

            return RepuishObject(targetPool.Peek());
        }

        static public void Set(DKitObject tObject)
        {
            tObject.Off();
        }

        static private DKitObject RepuishObject(DKitObject target)
        {
            var _transfrom = PoolTransfromDicts[target.data.TId];
            for (int i = 0; i < target.data.RepuishNum; i++)
            {
                GameObject go = GameObject.Instantiate(target.data.Prefab, _transfrom);
                go.transform.localPosition = Vector3.zero;
                DKitObject dkitObj = go.AddComponent<DKitObject>();
                dkitObj.Off(target.data);
                PoolDicts[target.data.TId].Enqueue(dkitObj);
            }

            return Get(target.data.TId);
        }
    }
}
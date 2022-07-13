using System;
using System.Collections.Generic;
using DKit.ThirdPartyPlugins.ProScrollView.Runtime.Scripts;
using UnityEngine;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.Sample
{
    [System.Serializable]
    public struct ExampleItemData
    {
        public string content;
        public string logTime;
        public LogEnum logType;
    }

    public class ExampleController : BaseController<ExampleItem>
    {
        public List<ExampleItemData> allLogItems = new List<ExampleItemData>();

        protected override void Start()
        {
            base.Start();
            // List<ExampleItem> elist = new List<ExampleItem>();
            // for (int i = 0; i < 20; i++)
            // {
            //     elist.Add(new ExampleItem
            //     {
            //         logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //         logType = LogType.Log,
            //         content ="...:  " +i
            //     });
            // }
            // CellData = elist;
            // base.ReloadData();
            // allLogItems = elist;
        }

        private ExampleItemData ToData(ExampleItem item)
        {
            return new ExampleItemData
            {
                content = item.content,
                logTime = item.logTime,
                logType = item.logType
            };
        }

        private List<ExampleItem> ToData(List<ExampleItemData> list)
        {
            List<ExampleItem> _list = new List<ExampleItem>();
            for (int i = 0; i < list.Count; i++)
            {
                _list.Add(new ExampleItem
                {
                    content = list[i].content,
                    logTime = list[i].logTime,
                    logType = list[i].logType
                });
            }

            return _list;
        }

        public void UpdateToAll()
        {
            List<ExampleItem> data= ToData(allLogItems);
            if (data.Count < 20)
            {
                int c = data.Count;
                for (int i = 0; i < 20-c; i++)
                {
                    data.Add(new ExampleItem{isNull = true});
                }
                CellData =data;
                base.ReloadData();
                return;
            }

            CellData = data;
            base.ReloadData();
        }

        public void UpdateLOgType(LogEnum logType)
        {
            List<ExampleItemData> temp  =new List<ExampleItemData>();
           
            for (int i = 0; i < allLogItems.Count; i++)
            {
                if (allLogItems[i].logType == logType)
                {
                    temp.Add(allLogItems[i]);
                }
            }
            List<ExampleItem> data=ToData(temp);
            if (data.Count < 20)
            {
                int c = data.Count;
                for (int i = 0; i < 20-c; i++)
                {
                    data.Add(new ExampleItem{isNull = true});
                }
                CellData =data;
                base.ReloadData();
                return;
            }
            CellData =data;
            base.ReloadData();
        }

        public void ClearAll()
        {
            CellData.Clear();
            allLogItems.Clear();
            base.ReloadData();
        }

        public void AddElement(ExampleItem t)
        {
            allLogItems.Add(ToData(t));
            CellData.Add(t);
           
             base.ReloadData();
        }
    }
}
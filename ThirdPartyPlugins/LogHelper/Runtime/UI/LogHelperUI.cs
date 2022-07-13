using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DKit.ThirdPartyPlugins.LogHelper.Runtime.Sample;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.UI
{
    [System.Serializable]
    public struct RedPointUI
    {
        public LogEnum logEnum;
        public GameObject bgImage;
        public Text redPointText;

        public void Update(int num)
        {
            bgImage.SetActive(num > 0);
            redPointText.text = num.ToString();
        }
    }

    public class LogHelperUI : MonoBehaviour
    {
        public static LogHelperUI Instance { get; set; }
        public Transform content;

        public Button cancelBtn;
        public Text totalLogsTxt;
        public Text[] logLevelNumTxt;

        public Button[] topBtns;
        public Transform contentDetailScrollView;
        public Text contentDetailInfoText;
        public ExampleController SV;

        public Button clearBtn;
        public Text tipText;

        public RedPointUI[] logRedPoints;
        
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this);
                Instance = this;
            }

            cancelBtn.onClick.AddListener(OnClikCancelBtn);
            clearBtn.onClick.AddListener(OnclikClearBtn);
            LogHelper.OnLogReceiveCallBackDeletage += OnLogReceiveCallBackDeletage_Unity;
            Init();
        }


        private void Start()
        {
            LogHelper.Init(() => tipText.gameObject.SetActive(!LogHelper.LogData.IsEnable));
        }

        /// <summary>
        /// 清除当前(视图)log
        /// </summary>
        private void OnclikClearBtn()
        {
            SV.ClearAll();
            LogHelper.ClearLogView();
            UpdateLogRedPointsUI();
        }


        private void UpdateLogRedPointsUI()
        {
            int Total_Count = LogUtil.GetListAll();
            int INFO_Count = LogHelper.LogNum(LogEnum.INFO);
            int WARNING_Count = LogHelper.LogNum(LogEnum.WARNING);
            int ERROR_Count = LogHelper.LogNum(LogEnum.ERROR);
            int TRACK_Count = LogHelper.LogNum(LogEnum.TRACK);
            totalLogsTxt.text = Total_Count.ToString();
            logLevelNumTxt[0].text = INFO_Count.ToString();
            logLevelNumTxt[1].text = WARNING_Count.ToString();
            logLevelNumTxt[2].text = ERROR_Count.ToString();
            logLevelNumTxt[3].text = TRACK_Count.ToString();
            foreach (var redPointTexts in logRedPoints)
            {
                switch (redPointTexts.logEnum)
                {
                    case LogEnum.TRACK:
                        redPointTexts.Update(TRACK_Count);
                        break;
                    case LogEnum.WARNING:
                        redPointTexts.Update(WARNING_Count);
                        break;
                    case LogEnum.ERROR:
                        redPointTexts.Update(ERROR_Count);
                        break;
                }
            }
        }

        public void OnClikTopBtn(int index)
        {
            for (int i = 0; i < topBtns.Length; i++)
            {
                topBtns[i].GetComponent<Image>().color = index == i ? Color.green : Color.white;
            }

            if (index == 0)
            {
                SV.UpdateToAll();
            }
            else if (index == 1)
            {
                SV.UpdateLOgType(LogEnum.TRACK);
            }
            else if (index == 2)
            {
                SV.UpdateLOgType(LogEnum.INFO);
            }
            else if (index == 3)
            {
                SV.UpdateLOgType(LogEnum.WARNING);
            }
            else if (index == 4)
            {
                SV.UpdateLOgType(LogEnum.ERROR);
            }
            else if (index == 5)
            {
                //打开本地当前日志文件
                LogUtil.OpenLogDictory();
            }
            else if (index == 6)
            {
                //发送本地日志文件
            }
        }


        private void Init()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                content.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 打开详细Log信息窗口
        /// </summary>
        public void OpenDetailScrollView(ExampleItem item)
        {
            contentDetailScrollView.gameObject.SetActive(true);
            contentDetailInfoText.text = item.content;
        }


        private void OnDestroy()
        {
            LogHelper.OnLogReceiveCallBackDeletage -= OnLogReceiveCallBackDeletage_Unity;
            LogUtil.Dispose();
        }

        /// <summary>
        /// 日志更新监听
        /// </summary>
        /// <param name="lo"></param>
        private void OnLogReceiveCallBackDeletage_Unity(LogObject lo)
        {
            UpdateLogRedPointsUI();
            SV.AddElement(new ExampleItem
            {
                logType = lo.logType,
                content = lo.content,
                logTime = lo.logTime
            });
        }

        public void OpenLogView()
        {
            transform.Find("View").gameObject.SetActive(true);
        }

        private void OnClikCancelBtn()
        {
            transform.Find("View").gameObject.SetActive(false);
        }
    }
}
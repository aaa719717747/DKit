using UnityEngine;
using UnityEngine.UI;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.UI
{
    [System.Serializable]
    public struct LogObjectData
    {
        public int logId;
        public string content;
        public string logTime;
        public LogEnum logType;
    }

    public class LogUIItem:MonoBehaviour
    {
        public LogEnum logType;
        public Text content;
        public Image selfImage;
        [HideInInspector] public LogObject logObject;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClikLogItem);
        }
        public LogObject SetData(LogObjectData data)
        {
            return new LogObject
            {
                logId = data.logId,
                content = data.content,
                logTime = data.logTime,
                logType = data.logType,
            };
        }
        // public override void UpdateContent(LogObjectData _logObject)
        // {
        //     gameObject.SetActive(true);
        //     logObject = SetData(_logObject);
        //     logType = _logObject.logType;
        //     Color[] logColors = new Color[3] {Color.gray, Color.red, Color.yellow};
        //     content.text = _logObject.content;
        //     selfImage.color = logColors[(int) _logObject.logType];
        // }

        private void OnClikLogItem()
        {
            // LogHelperUI.Instance.OpenDetailScrollView(this);
        }
    }
}
using DKit.ThirdPartyPlugins.LogHelper.Runtime.UI;
using DKit.ThirdPartyPlugins.ProScrollView.Runtime.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.Sample
{
	[System.Serializable]
	public struct ExampleItem {
		public string content;
		public string logTime;
		public LogEnum logType;
		public bool isNull;
	}

	public class ExampleCell : BaseCell<ExampleItem>
	{
		public LogEnum logType;
		public Image selfImage;
		public Text contentLabelTxt;
		public Button onClikBtn;

		protected override void Awake()
		{
			base.Awake();
		}

		public override void UpdateContent(ExampleItem item) {
			contentLabelTxt.text = item.content;
			Color[] logColors = new Color[5]
			{
				Color.white, Color.yellow, Color.red,Color.magenta,Color.white
			};
			
			onClikBtn.onClick.AddListener(() =>
			{
				LogHelperUI.Instance.OpenDetailScrollView(item);
			});
			logType = item.logType;
			if (item.isNull)
			{
				selfImage.color = new Color(0, 0, 0, 0);
			}
			else
			{
				selfImage.color = logColors[(int) item.logType];
			}
		}
	}
}

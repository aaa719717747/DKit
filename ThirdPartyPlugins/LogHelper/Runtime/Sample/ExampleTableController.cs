using DKit.ThirdPartyPlugins.ProScrollView.Runtime.Scripts;

namespace DKit.ThirdPartyPlugins.LogHelper.Runtime.Sample
{
	public class ExampleTableController : BaseController<ExampleItem>
	{
		public ExampleTable table;

		protected override void Start() {
			base.Start();
			CellData = table.items;
		}
	}
}

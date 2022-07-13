using DKit.ThirdPartyPlugins.ProScrollView.Runtime.Scripts;
using UnityEditor;

namespace DKit.ThirdPartyPlugins.ProScrollView.Editor
{
	[CustomEditor(typeof(BaseController<>), true)]
	public class BaseControllerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawPropertiesExcluding(serializedObject, "m_Script");
			serializedObject.ApplyModifiedProperties();
		}
	}
}

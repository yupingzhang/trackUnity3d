using UnityEngine;
using UnityEditor;
using System.Collections;

public class AdvancedListMenu : MonoBehaviour {
	[MenuItem("NGUI/Create/Advanced List", false)]
	static void AddAdvancedList ()
	{
		UIPanel panel = NGUISettings.AddPanel(NGUIMenu.SelectedRoot ());
		if (panel == null) panel = NGUIEditorTools.SelectedRoot(true).GetComponent<UIPanel>();
		panel.clipping = UIDrawCall.Clipping.SoftClip;
		panel.name = "Advanced List";
		UIScrollView scrollview = panel.gameObject.AddComponent<UIScrollView>();
		scrollview.movement = UIScrollView.Movement.Vertical;
		Selection.activeGameObject = panel.gameObject;

		GameObject scrollcontent = new GameObject ();
		scrollcontent.name = "ScrollViewContent";
		scrollcontent.transform.parent = panel.transform;
		scrollcontent.transform.localPosition = Vector3.zero;
		scrollcontent.transform.localScale = Vector3.one;
		scrollcontent.AddComponent<AdvenceListContent> ();
	}
}

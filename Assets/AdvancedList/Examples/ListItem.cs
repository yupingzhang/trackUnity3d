using UnityEngine;
using System.Collections;

public class ListItem : BaseListItem{
	public UILabel indexLabel;
	public UILabel nameLabel;
	public UILabel countLabel;

	private ListItemData data;

	public override void OnSetItemData (int index, object obj)
	{
		base.OnSetItemData (index, obj);
		data = (ListItemData)obj;
		indexLabel.text = index.ToString ();
		nameLabel.text = data.name;
		countLabel.text = "count : " + data.clickCount.ToString ();
	}
	public override void OnStopedList ()
	{
		base.OnStopedList ();
	}

	public void btnClick()
	{
		data.clickCount++;
		countLabel.text = "count : " + data.clickCount.ToString ();
	}
}

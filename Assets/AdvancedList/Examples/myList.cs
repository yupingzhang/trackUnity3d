using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class myList : MonoBehaviour {

	public AdvenceListContent scrollContent;
	public UIButton indexBtn;

	public int Dataleng = 3000;
	private int targetIndex = 0;
	private UILabel btnLabel;
	// Use this for initialization
	void Start () {
		//make listData
		System.Random ran = new System.Random (9999);
		List<object> myItemList = new List<object> ();
		int i = 0;

		for(i = 0; i < Dataleng; i++)
		{

			ListItemData itemData = new ListItemData();
			itemData.name = generateName(7, ran);
			itemData.clickCount = 0;
			myItemList.Add((object)itemData);
		}
		//setList
		scrollContent.setData (myItemList);
		//setBtn;
		targetIndex = Random.Range (0, Dataleng);
		if(btnLabel==null)
		{
			btnLabel = indexBtn.GetComponentInChildren<UILabel> ();
			btnLabel.text = "Move to " + targetIndex;
			UIEventListener.Get (indexBtn.gameObject).onClick = setIndex;
		}
	}

	string generateName(int length, System.Random random) {
		string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,";
		StringBuilder result = new StringBuilder(length);
		for (int i = 0; i < length; i++) {
			result.Append(characters[random.Next(characters.Length)]);
		}
		return result.ToString();
	}
	void setIndex(GameObject go)
	{
		scrollContent.index = targetIndex;
		targetIndex = Random.Range (0, Dataleng);
		btnLabel.text = "Move to " + targetIndex;
	}
}

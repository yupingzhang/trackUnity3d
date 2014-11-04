using UnityEngine;
using System.Collections;

public class BaseListItem:MonoBehaviour{
	public int itemIndex = -1;
	public virtual void OnStopedList()
	{
	}
	public virtual void OnSetItemData(int index, object obj)
	{
		itemIndex = index;
	}
	
}

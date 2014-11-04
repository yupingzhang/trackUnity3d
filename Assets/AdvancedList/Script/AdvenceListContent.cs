using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvenceListContent : MonoBehaviour
{
	private BoxCollider dragArea;
	private UIDragScrollView uiDragScrollView;
	private UIScrollView scrollView;
	private UIPanel panel;
	private UIRoot root;
	[HideInInspector]
	public GameObject area;

	//===============================

	public float cellHeight = 100.0f;

	public int dumpItemN = 2;
	public BaseListItem item;
	public Vector3 offset = new Vector3();
	//public GameObject endDot;
	//===============================

	private BaseListItem fItem;
	private BaseListItem lItem;
	private int itemN;
	private int contentItemN = 0;

	public BaseListItem[] getItemList()
	{
		List<BaseListItem> retList = new List<BaseListItem>(itemList);
		retList.Insert(0,fItem);
		retList.Add(lItem);
		return retList.ToArray();
	}

	private List<BaseListItem> itemList = new List<BaseListItem>();
	private List<object>dataList;

	private float listZeroOffsetY = 0;

	// Use this for initialization
	void Start ()
	{
		init();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (itemList.Count <= 0)return;
		setIndex (checkFirstIndex ());
	}

	private int _index = 0;
	private bool isFirst = false;
	void init()
	{
		if(isFirst)return;
		isFirst = true;
		addBaseComponents();
		setDragArea ();
		setProperty();

		itemList.Clear();
		scrollView.onStoppedMoving += allItemStoped;
	}
	void addBaseComponents()
	{
		dragArea = gameObject.GetComponent<BoxCollider>();
		uiDragScrollView = gameObject.GetComponent<UIDragScrollView>();
		area = new GameObject();
		area.transform.parent = this.transform;
		area.name = "AREA";

		if(dragArea == null)
		{
			dragArea = gameObject.AddComponent<BoxCollider>();
		}
		if(uiDragScrollView == null)
		{
			uiDragScrollView = gameObject.AddComponent<UIDragScrollView>();
		}
		scrollView = transform.parent.gameObject.GetComponent<UIScrollView>();
		if(scrollView == null)Debug.LogError("UIScrollView CONTENT IS WRONG LOCATION!!!!");
		else
		{
			panel = scrollView.gameObject.GetComponent<UIPanel>();
			if(panel == null)Debug.LogError("UIPANEL IS NULL!!!!");
		}
		root = transform.GetComponentInParent<UIRoot> ();
	}
	void setProperty()
	{
		transform.localPosition = Vector3.zero;
		uiDragScrollView.scrollView = scrollView;

		area.transform.localPosition = new Vector3(dragArea.center.x - dragArea.size.x/2.0f, dragArea.center.y + dragArea.size.y/2.0f , 0);
		area.transform.localScale = Vector3.one;
		/*if(endDot!=null)
		{
			endDot.transform.parent = area.transform;
			endDot.transform.localPosition = new Vector3(dragArea.size.x, -dragArea.size.y , 0);
		}
		*/
	}
	void setDragArea()
	{
		dragArea.size = scrollView.bounds.size;
		dragArea.center = scrollView.bounds.center;
	}



	public int index
	{
		get{return _index;}
		set{

			if(_index == value)return;
			if(dataList == null)return;
			if(itemList == null)return;
			if(!isReady)return;
			_index = Mathf.Min (Mathf.Max(0,value),dataList.Count - contentItemN);
			float ty = _index * cellHeight;
			if(value>_index)ty = scrollView.bounds.size.y - panel.clipRange.w;
			scrollView.ResetPosition();
			scrollView.MoveRelative(new Vector3(0,ty));

		}
	}
	private void setIndex(int idx)
	{
		if(dataList == null)return;
		if(itemList == null)return;
		if(!isReady)return;
		_index = Mathf.Min (Mathf.Max(0,idx),dataList.Count - contentItemN);
		setPosition();
	}
	
	private void allItemStoped()
	{
		foreach(BaseListItem _item in itemList)
		{
			_item.OnStopedList();
		}
		if(fItem!=null)
		{
			fItem.OnStopedList();
		}
		if(lItem!=null)
		{
			lItem.OnStopedList();
		}
	}
	private int checkFirstIndex()
	{

		return Mathf.FloorToInt((listZeroOffsetY - panel.clipOffset.y)/cellHeight);
	}
	private bool isReady = false;
	private int itemIdx = 0;
	public void setData(List<object> list)
	{

		init();
		itemClear();
		if(list.Count<=0)return;
		dataList = list;
		if(item==null)
		{
			Debug.LogError("item is Null");
			return;
		}
		//====get itemN
		contentItemN = Mathf.CeilToInt(panel.clipRange.w/cellHeight);
		int itemMax = contentItemN +dumpItemN*2+2;
		itemN = Mathf.Min(itemMax, dataList.Count);
		itemIdx = 0;
		isReady = false;
		if(gameObject.activeInHierarchy)StartCoroutine(setList());

		setDragArea ();
	}
	public void OnEnable()
	{
		if(dataList==null || dataList.Count<=0)return;
		if(!isReady)StartCoroutine(setList());
	}
	private void itemClear()
	{
		foreach(BaseListItem bci in itemList)
		{
			GameObject.Destroy(bci.gameObject);
		}
		if(fItem!=null)
		{
			GameObject.Destroy(fItem.gameObject);
			fItem = null;
		}
		if(lItem!=null)
		{
			GameObject.Destroy(lItem.gameObject);
			lItem = null;
		}
		itemList.Clear();
	}

	IEnumerator setList()
	{
		float size = 1.0f;
		if (root != null) 
		{	
			if (root.activeHeight > 0f )
			{
				size = root.activeHeight / 2.0f;
			}
		}

		//for(int i = 0; i< itemN; i++)
		while(itemIdx<itemN)
		{
			GameObject go = Instantiate(item.gameObject) as GameObject;
			//Debug.Log (go);
			BaseListItem _item = go.GetComponent<BaseListItem>();
			_item.transform.parent = area.transform;
			if(itemIdx == 0)
			{
				_item.transform.localPosition = Vector3.zero + offset;
				fItem = _item;
			}
			else if(itemIdx == itemN-1)
			{
				_item.transform.localPosition = Vector3.zero + new Vector3(0,-(dataList.Count-1)*cellHeight,0) + offset;
				lItem = _item;
			}
			else
			{
				_item.transform.localPosition = Vector3.zero + new Vector3(0,-itemIdx*cellHeight,0) + offset;
				itemList.Add(_item);
			}
			_item.transform.localScale = Vector3.one * size;
			int idx = -(int)(_item.transform.localPosition.y/cellHeight);
			_item.OnSetItemData(idx, dataList[idx]);
			itemIdx++;
			//yield return null;
		}
		if(scrollView == null)scrollView = transform.parent.gameObject.GetComponent<UIScrollView>();
		scrollView.ResetPosition();
		allItemStoped();
		listZeroOffsetY = panel.clipOffset.y;
		isReady = true;
		yield return null;
	}
	private void setPosition()
	{
		float _y = (listZeroOffsetY - panel.clipOffset.y);
		foreach(BaseListItem _item in itemList)
		{
			int idx =0;
			//moveDown!!
			if(_item.transform.localPosition.y + _y > cellHeight+10){
				moveDown(_item, _y);
				idx = -(int)(_item.transform.localPosition.y/cellHeight);
				_item.OnSetItemData(idx, dataList[idx]);
			}
			//moveUP!!
			else if(_item.transform.localPosition.y + _y < -cellHeight*(itemList.Count-2))
			{
				moveUp(_item, _y);
				idx = -(int)(_item.transform.localPosition.y/cellHeight);
				_item.OnSetItemData(idx, dataList[idx]);
			}
		}
	}
	private void moveDown(BaseListItem _item, float _y)
	{
		if(_item.transform.localPosition.y + _y > cellHeight*dumpItemN){
			if(_item.transform.localPosition.y - (cellHeight*itemList.Count) > -(dataList.Count-1)*cellHeight)
			{
				_item.transform.localPosition = new Vector3(0,_item.transform.localPosition.y - (cellHeight*itemList.Count),0) + offset;
				moveDown(_item,_y);
			}
		}
	}
	private void moveUp(BaseListItem _item, float _y)
	{
		if(_item.transform.localPosition.y + _y < -cellHeight*(itemList.Count-2))
		{
			if(_item.transform.localPosition.y + (cellHeight*itemList.Count) <= -cellHeight)
			{
				_item.transform.localPosition = new Vector3(0,_item.transform.localPosition.y + (cellHeight*itemList.Count),0) + offset;
				moveUp(_item,_y);
			}
		}
	}
	public BaseListItem GetItem(int index){
		if (index == 0)
			return fItem;
		else if (index == dataList.Count - 1) {
			return lItem;
		} else if (index - 1 >= itemList.Count) {
			return null;
		} else {
			return itemList [index - 1]; 
		}
	}

	public int GetDataLength(){
		if (dataList == null)
			return 0;

		return dataList.Count;
	}
}


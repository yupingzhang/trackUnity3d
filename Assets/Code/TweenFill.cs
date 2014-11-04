//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the Advanced Anchors values.
/// </summary>

[AddComponentMenu("NGUI/Tween/Fill")]
public class TweenFill : UITweener
{
	public float from;
	public float to;
	
	Transform mTrans;
	UISprite mSprite;
	
	/// <summary>
	/// Current offset.
	/// </summary>
	
	public float Fill
	{
		get
		{
			if (mSprite != null) return mSprite.fillAmount;
			return 0;
		}
		set
		{
			if (mSprite != null) {
				mSprite.fillAmount = value;
			}
		}
	}
	
	/// <summary>
	/// Find all needed components.
	/// </summary>
	
	void Awake ()
	{
		mSprite = GetComponent<UISprite>();
	}
	
	/// <summary>
	/// Interpolate and update the offset.
	/// </summary>
	
	override protected void OnUpdate (float factor, bool isFinished) { Fill = from * (1f - factor) + to * factor;  }
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>
	
	static public TweenFill Begin (GameObject go, float duration, float Fill)
	{
		TweenFill comp = UITweener.Begin<TweenFill>(go, duration);
		comp.from = comp.Fill;
		comp.to = Fill;
		
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
	
	public void SetTo () {
		mSprite = GetComponent<UISprite>();
		mSprite.fillAmount = to;
	}
	
	public void SetFrom () {
		mSprite = GetComponent<UISprite>();
		mSprite.fillAmount = from;
	}
	
	public void GrabTo () {
		mSprite = GetComponent<UISprite>();
		to = mSprite.fillAmount;
		
	}
	
	public void GrabFrom () {
		mSprite = GetComponent<UISprite>();
		from = mSprite.fillAmount;
		
	}
}
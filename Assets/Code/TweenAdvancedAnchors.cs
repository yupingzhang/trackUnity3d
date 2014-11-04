//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the Advanced Anchors values.
/// </summary>

[AddComponentMenu("NGUI/Tween/AdvancedAnchors")]
public class TweenAdvancedAnchors : UITweener
{
	public Vector4 from;
	public Vector4 to;
	
	Transform mTrans;
	UIRect mRect;

	/// <summary>
	/// Current offset.
	/// </summary>
	
	public Vector4 AdvancedAnchors
	{
		get
		{
			if (mRect != null) return new Vector4(mRect.leftAnchor.absolute, mRect.rightAnchor.absolute, mRect.bottomAnchor.absolute, mRect.topAnchor.absolute);
			return Vector2.zero;
		}
		set
		{
			if (mRect != null) {
				mRect.leftAnchor.absolute = (int) value.x;
				mRect.rightAnchor.absolute = (int) value.y;
				mRect.bottomAnchor.absolute = (int) value.z;
				mRect.topAnchor.absolute = (int) value.w;
			}
		}
	}
	
	/// <summary>
	/// Find all needed components.
	/// </summary>
	
	void Awake ()
	{
		mRect = GetComponent<UIRect>();
	}
	
	/// <summary>
	/// Interpolate and update the offset.
	/// </summary>
	
	override protected void OnUpdate (float factor, bool isFinished) { AdvancedAnchors = from * (1f - factor) + to * factor;  }
	
	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAdvancedAnchors Begin (GameObject go, float duration, Vector4 AdvancedAnchors)
	{
		TweenAdvancedAnchors comp = UITweener.Begin<TweenAdvancedAnchors>(go, duration);
		comp.from = comp.AdvancedAnchors;
		comp.to = AdvancedAnchors;
		
		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public void SetTo () {
		mRect = GetComponent<UIRect>();
		mRect.leftAnchor.absolute = (int) to.x;
		mRect.rightAnchor.absolute = (int) to.y;
		mRect.bottomAnchor.absolute = (int) to.z;
		mRect.topAnchor.absolute = (int) to.w;
	}

	public void SetFrom () {
		mRect = GetComponent<UIRect>();
		mRect.leftAnchor.absolute = (int) from.x;
		mRect.rightAnchor.absolute = (int) from.y;
		mRect.bottomAnchor.absolute = (int) from.z;
		mRect.topAnchor.absolute = (int) from.w;
	}

	public void GrabTo () {
		mRect = GetComponent<UIRect>();
		to = new Vector4 (mRect.leftAnchor.absolute, mRect.rightAnchor.absolute, mRect.bottomAnchor.absolute, mRect.topAnchor.absolute);

	}

	public void GrabFrom () {
		mRect = GetComponent<UIRect>();
		from = new Vector4 (mRect.leftAnchor.absolute, mRect.rightAnchor.absolute, mRect.bottomAnchor.absolute, mRect.topAnchor.absolute);

	}
}
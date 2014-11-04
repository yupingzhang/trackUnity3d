using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {


	public float DefaultCardboardMagnet = 0.75f;
	public Vector2 CardboardMagnetMinMax;
	public UISlider CardboardMagnet;
	public UILabel CardboardMagnetLabel;

	void Start () {
		if (PlayerPrefs.HasKey ("CardboardMagnet")) {
			CardboardMagnet.value = DefaultCardboardMagnet;
		} else CardboardMagnet.value = PlayerPrefs.GetFloat ("CardboardMagnet");
		set_GoogleCardboardMagnet ();
	}

	public void set_GoogleCardboardMagnet () {
		CardboardMagnetLabel.text = Mathf.RoundToInt(Mathf.Lerp (CardboardMagnetMinMax.x, CardboardMagnetMinMax.y, CardboardMagnet.value)).ToString();
		PlayerPrefs.SetFloat ("CardboardMagnet", CardboardMagnet.value);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject cameraPhone;
	public GameObject cameraPhoneInterface;
	public GameObject cameraGoogleCardboard;

	public Player thePlayer;
	public int photosLeft = 99;
	public UILabel photoCountLabel;

	public List<BoxCollider> photoMoments = new List<BoxCollider>();
	public List<KodakMoment> kodakMoments = new List<KodakMoment>();

	public List<Texture2D> pics = new List<Texture2D>();
	public int picMagnification = 1;

	public Modal theModal;

	public bool savePics;
	public bool processPics;
	public TweenFill waitSprite;

	public Camera cam;
	private Plane[] planes;

	public UITexture confirmedPhoto;
	public photRating distanceRating;
	public photRating rotationRating;
	public photRating animationRating;

	[HideInInspector] public bool googlecardboard;

	void Awake () {
#if !UNITY_EDITOR
		savePics = true;
#endif
		cameraPhone.SetActive(false);
		cameraPhoneInterface.SetActive(true);
		cameraGoogleCardboard.SetActive(false);

		planes = GeometryUtility.CalculateFrustumPlanes(cam);

		if (PlayerPrefs.HasKey("googlecardboard")) {
			if (PlayerPrefs.GetInt("googlecardboard") == 0) {
				gameSetUp_Phone();
			} else gameSetUp_GoogleCardboard();
		} else gameSetUp_Phone();
		Invoke ("CountDown", 1);
	}

	int Count = 3;
	public TweenScale readyDeviceSprtie;
	public UILabel readyDevice;
	void CountDown () {
		Count -= 1;
		if (Count == 0) {
			readyDevice.text = "Go!";
			Invoke ("StartGame", 1);
		} else {
			readyDevice.text = "Get Your Device Ready!\n" + Count;
			Invoke ("CountDown", 1);
		}
	}

	public void StartGame () {
		readyDeviceSprtie.PlayReverse ();
		readyDevice.GetComponent<TweenScale> ().PlayReverse ();
		thePlayer.GameStart ();
	}

	public void gameSetUp_Phone () {
		googlecardboard = false;
		cameraPhone.SetActive(true);
	}

	public void gameSetUp_GoogleCardboard () {
		googlecardboard = true;
		cameraGoogleCardboard.SetActive(true);
	}
	public void startGame () {
		theModal.closeModal();
		thePlayer.enabled = true;
		if (cameraPhone.activeSelf) cameraPhoneInterface.SetActive(true);
	}

	public void snapPic () {
		photosLeft -= 1;
		photoCountLabel.text = photosLeft.ToString ();
		thePlayer.canSnap = false;
		waitSprite.ResetToBeginning ();
		waitSprite.PlayForward ();
		StartCoroutine ("allowSnap", 1);

		if (!checkCameraCollision ()) return;
		if (savePics) {
			string pathAndName = Application.dataPath + "/Resources/SnapQuest" + (pics.Count + 1) + ".png";
			Application.CaptureScreenshot(pathAndName, picMagnification);
		}
		if (processPics) StartCoroutine(processPhoto());
	}
	
	bool checkCameraCollision () {

		bool somethingfound = false;

		if (kodakMoments.Count == 0) return false;

		for (int i = 0; i < kodakMoments.Count; i++) {
			Vector3 screenPos = cam.WorldToScreenPoint(kodakMoments[i].transform.position);

			//print (kodakMoments[i].transform.name + "is at (" + screenPos.x + ", " + screenPos.y + ") | cam = (" + cam.pixelWidth + ", " + cam.pixelHeight + ")");

			if (screenPos.x < 0) return false;
			if (screenPos.x > cam.pixelWidth) return false;

			if (screenPos.y < 0) return false;
			if (screenPos.y > cam.pixelHeight) return false;
			somethingfound = true;
		}
		if (somethingfound) {
			return true;
		} else return false;
	}


	IEnumerator allowSnap(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		waitSprite.Fill = 0;
		thePlayer.canSnap = true;
	}
	
	IEnumerator processPhoto() {
		cameraPhoneInterface.SetActive(false);
		yield return new WaitForEndOfFrame();
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0, false);
		tex.Apply();
		cameraPhoneInterface.SetActive(true);
		pics.Add(tex);
		confirmedPhoto.mainTexture = tex;
		confirmedPhoto.width = confirmedPhoto.width;
		confirmedPhoto.height = confirmedPhoto.height;

		if (confirmedPhoto.height > confirmedPhoto.width) {
			confirmedPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		} else confirmedPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;

		confirmedPhoto.GetComponent<TweenScale> ().ResetToBeginning ();
		confirmedPhoto.GetComponent<TweenScale> ().PlayForward ();
		confirmedPhoto.GetComponent<TweenAlpha> ().ResetToBeginning ();
		confirmedPhoto.GetComponent<TweenAlpha> ().PlayForward ();
	}

	public void gameOver () {
		thePlayer.enabled = false;
	}

	public void showPics () {

	}

	public void showNextPic () {

	}

	public void showPreviousPic () {

	}

	public void showGameEndScreen () {

	}
}

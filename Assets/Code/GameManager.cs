using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject cameraPhone;
	public GameObject cameraGoogleCardboard;

	public GameObject cameraPhoneSelfie;
	public GameObject cameraGoogleCardboardSelfie;

	public GameObject cameraPhoneUi;
	public GameObject cameraGoogleCardboardUi;

	public GameObject cameraPhoneInterface;

	public List<GameObject> thingsToTurnOffOnGoogleCardboard = new List<GameObject>();

	public Player thePlayer;
	public int photosLeft = 99;
	public UILabel photoCountLabel;

	public List<BoxCollider> photoMoments = new List<BoxCollider>();
	public List<KodakMoment> kodakMoments = new List<KodakMoment>();

	public List<Texture2D> pics = new List<Texture2D>();
	public List<Vector4> picPoints = new List<Vector4>();
	public int picMagnification = 1;

	public Modal theModal;

	public bool savePics;
	public bool processPics;
	public TweenFill waitSprite;

	public Camera cam;
	private Plane[] planes;

	public UITexture confirmedPhoto;
	public UITexture resultdPhoto;

	[HideInInspector] public bool googlecardboard;

	void Awake () {
#if !UNITY_EDITOR
		savePics = true;
#endif
		cameraPhone.SetActive(false);
		cameraGoogleCardboard.SetActive(false);
		cameraPhoneSelfie.SetActive(false);
		cameraGoogleCardboardSelfie.SetActive(false);
		cameraPhoneUi.SetActive(false);
		cameraGoogleCardboardUi.SetActive(false);

		cameraPhoneInterface.SetActive(true);

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
		cameraPhoneSelfie.SetActive(true);
		cameraPhoneUi.SetActive (true);
	}

	public void gameSetUp_GoogleCardboard () {
		googlecardboard = true;
		cameraGoogleCardboard.SetActive(true);
		cameraGoogleCardboardSelfie.SetActive(true);
		cameraGoogleCardboardUi.SetActive (true);

		for (int i = 0; i < thingsToTurnOffOnGoogleCardboard.Count; i++) {
			thingsToTurnOffOnGoogleCardboard[i].SetActive(true);
		}
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

			if (kodakMoments[i].snapped != -1) {
				picPoints.Add(picPoints[kodakMoments[i].snapped]);
			} else {
				picPoints.Add(Vector4.zero);
				kodakMoments[i].snapped = picPoints.Count - 1;
			}
			getPoints(kodakMoments[i].snapped, kodakMoments[i]);

			somethingfound = true;
		}
		if (somethingfound) {
			return true;
		} else return false;
	}

	void getPoints (int targetIndex, KodakMoment picMoment) {
		if (picMoment.checkDistance ()) picPoints [targetIndex] = new Vector4(1,picPoints [targetIndex].y,picPoints [targetIndex].z,picPoints [targetIndex].w);
		if (picMoment.checkRotation ()) picPoints [targetIndex]= new Vector4(picPoints [targetIndex].x,1,picPoints [targetIndex].z,picPoints [targetIndex].w);
		if (picMoment.checkisDoingSpecial ()) picPoints [targetIndex] = new Vector4(picPoints [targetIndex].x,picPoints [targetIndex].y,1,picPoints [targetIndex].w);
		if (thePlayer.selfieMode) picPoints [targetIndex] = new Vector4(picPoints [targetIndex].x,picPoints [targetIndex].y,picPoints [targetIndex].z,1);
	}


	IEnumerator allowSnap(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		waitSprite.Fill = 0;
		thePlayer.canSnap = true;
	}
	
	IEnumerator processPhoto() {
		cameraPhoneInterface.SetActive(false);

		if (googlecardboard) {
			if (thePlayer.selfieMode) {
				thePlayer.selfiePersoneCamLeft.gameObject.SetActive(false);
				thePlayer.selfiePersoneCamRight.gameObject.SetActive(false);
				thePlayer.selfiePersoneCamCam.SetActive(true);
			} else {
				thePlayer.firstPersoneCamLeft.gameObject.SetActive(false);
				thePlayer.firstPersoneCamRight.gameObject.SetActive(false);
				thePlayer.firstPersoneCamCam.SetActive(true);
			}
		}

		yield return new WaitForEndOfFrame();
		Texture2D tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0, false);
		tex.Apply();
		cameraPhoneInterface.SetActive(true);

		if (googlecardboard) {
			if (thePlayer.selfieMode) {
				thePlayer.firstPersoneCamCam.gameObject.SetActive(false);
				thePlayer.selfiePersoneCamLeft.gameObject.SetActive(true);
				thePlayer.selfiePersoneCamRight.gameObject.SetActive(true);
			} else {
				thePlayer.selfiePersoneCamCam.gameObject.SetActive(false);
				thePlayer.firstPersoneCamLeft.gameObject.SetActive(true);
				thePlayer.firstPersoneCamRight.gameObject.SetActive(true);
			}
		}

		pics.Add(tex);
		showPic ();
	}

	void gameOver () {
		thePlayer.enabled = false;
		thePlayer.drive.enabled = false;
		theModal.openResults ();

		if (pics.Count == 0) {
			theModal.NoPhotos.PlayForward ();
			theModal.goToMenu.PlayForward ();
			theModal.restart.PlayForward ();
			return;
		} else {
			if (pics.Count == 1) {
				theModal.goToMenu.PlayForward ();
				theModal.restart.PlayForward ();
			} else theModal.btnNext.PlayForward();
			showResultPic ();
		}
	}

	void showPic () {
		confirmedPhoto.mainTexture = pics[pics.Count - 1];
		confirmedPhoto.width = pics [pics.Count - 1].width;
		confirmedPhoto.height = pics [pics.Count - 1].height;
		
		if (confirmedPhoto.height > confirmedPhoto.width) {
			confirmedPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		} else confirmedPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
		
		confirmedPhoto.GetComponent<TweenScale> ().ResetToBeginning ();
		confirmedPhoto.GetComponent<TweenScale> ().PlayForward ();
		confirmedPhoto.GetComponent<TweenAlpha> ().ResetToBeginning ();
		confirmedPhoto.GetComponent<TweenAlpha> ().PlayForward ();
	}

	int picIndex = 0;
	void showResultPic () {
		resultdPhoto.mainTexture = pics[picIndex];
		resultdPhoto.width = pics[picIndex].width;
		resultdPhoto.height = pics[picIndex].height;
		
		if (resultdPhoto.height > resultdPhoto.width) {
			resultdPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		} else resultdPhoto.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;

		resultdPhoto.leftAnchor.absolute = 200;
		resultdPhoto.rightAnchor.absolute = -200;
		resultdPhoto.bottomAnchor.absolute = 200;
		resultdPhoto.topAnchor.absolute = -200;
		
		resultdPhoto.GetComponent<TweenScale> ().ResetToBeginning ();
		resultdPhoto.GetComponent<TweenScale> ().PlayForward ();
		resultdPhoto.GetComponent<TweenAlpha> ().ResetToBeginning ();
		resultdPhoto.GetComponent<TweenAlpha> ().PlayForward ();

		theModal.distanceRating.TweenIn ();
		theModal.rotationRating.TweenIn ();
		theModal.animationRating.TweenIn ();

		if (picPoints [picIndex].x == 1) theModal.distanceRating.TweenInSuccess ();
		if (picPoints [picIndex].y == 1) theModal.rotationRating.TweenInSuccess ();
		if (picPoints [picIndex].z == 1) theModal.animationRating.TweenInSuccess ();
		if (picPoints [picIndex].w == 1) {
			theModal.selfieRating.TweenInSuccess ();
			theModal.selfieRating.TweenIn ();
		}
	}


	public void showNextPic () {
		if (picIndex + 1 >= pics.Count - 1) {
			theModal.btnNext.PlayReverse();
			theModal.goToMenu.PlayForward ();
			theModal.restart.PlayForward ();
		}

		picIndex += 1;
		theModal.btnPrevious.PlayForward();

		if (picIndex > pics.Count - 1) picIndex = pics.Count - 1;

		resultdPhoto.GetComponent<TweenAdvancedAnchors> ().ResetToBeginning ();
		resultdPhoto.GetComponent<TweenAdvancedAnchors> ().PlayForward ();

		if (!IsInvoking("showResultPic")) Invoke("showResultPic", 0.25f);
	}

	public void showPreviousPic () {
		if (picIndex - 1 <= 0) {
			theModal.btnPrevious.PlayReverse();
		}

		picIndex -= 1;
		theModal.btnNext.PlayForward();

		if (picIndex < 0) picIndex = 0;

		resultdPhoto.GetComponent<TweenAdvancedAnchors> ().ResetToBeginning ();
		resultdPhoto.GetComponent<TweenAdvancedAnchors> ().PlayForward ();
		
		if (!IsInvoking("showResultPic")) Invoke("showResultPic", 0.25f);
	}

	public void loadMainMenu () {
		Application.LoadLevel (0);
	}

	public void reloadLevel () {
		Application.LoadLevel (1);
	}
}

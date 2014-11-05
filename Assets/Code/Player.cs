using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameManager theManager;

	public OpenDiveSensor drive;
	public GameObject firstPersoneCam;
	public GameObject firstPersoneCamCam;
	public Camera firstPersoneCamLeft;
	public Camera firstPersoneCamRight;

	public GameObject selfiePersoneCam;
	public GameObject selfiePersoneCamCam;
	public Camera selfiePersoneCamLeft;
	public Camera selfiePersoneCamRight;

	public TweenScale selfieSprite;
	public TweenScale selfieLabel;

	public GameObject model;

	//public SplineController theSpline;

	public UILabel compassInfo;
	public UILabel gyroscopeInfo;
	public UILabel debugLabel;

	public float magDiffTrig = 30.0f;
	float checkMagInterval = 0.5f;
	public bool canSnap = true;
	public float lastMag;
	public float downTime = 0.0f;
	public bool selfieMode = false;

	public ButtonHold btnHold;

	public TweenScale shutTop;
	public TweenScale shutBottom;

	void Start () {
		magDiffTrig = Mathf.Lerp (10, 50, PlayerPrefs.GetFloat ("CardboardMagnet"));
	}

	public void GameStart () {
		Input.compass.enabled = true;
		Input.gyro.enabled = true;
		drive.enabled = true;
		this.enabled = true;
	}

	void OnEnable () {
		Screen.lockCursor = true;
		//theSpline.enabled = true;
	}
	
	void OnDisable () {
		Screen.lockCursor = false;
		//theSpline.enabled = false;
	}

	public void Toggle () {
		if (this.enabled) {
			this.enabled = false;
		} else this.enabled = true;
	}

	public void CheckSelfieMode () {
		downTime += Time.deltaTime;
		if (downTime > 1.0f && !selfieMode) {
			enableSelfieMode();
		}
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) theManager.SendMessage ("gameOver");

		if (canSnap) {
#if UNITY_EDITOR
			if (Input.GetMouseButton(0)) {
				CheckSelfieMode();
			}

			if (Input.GetMouseButtonUp(0)) {
				Shoot();
			}
		
#else
			if (theManager.googlecardboard) {
				if (Time.time > checkMagInterval) {
					if (Mathf.Abs(Input.compass.rawVector.magnitude - lastMag) > magDiffTrig) {
						Shoot();
						debugLabel.text += "'Ello Chump! " + Time.time;
					}
					lastMag = Input.compass.rawVector.magnitude;
				}

				compassInfo.text =	"Compass = " + Input.compass.enabled + 
					"\nheadingAccuracy = " + Input.compass.headingAccuracy + 
						"\nmagneticHeading = " + Input.compass.magneticHeading + 
						"\nrawVector = " + Input.compass.rawVector + 
						"\nrawVectorMagnitude = " + Input.compass.rawVector.magnitude + 
						"\ntimestamp = " + Input.compass.timestamp  + 
						"\ntrueHeading = " + Input.compass.trueHeading;
				
				gyroscopeInfo.text = "Gyroscope = " + Input.gyro.enabled + 
					"\nattitude + " + Input.gyro.attitude + 
						"\ngravity = " + Input.gyro.gravity + 
						"\nrotationRate = " + Input.gyro.rotationRate + 
						"\nrotationRateMagnitude = " + Input.gyro.rotationRate.magnitude + 
						"\nrotationRateUnbiased = " + Input.gyro.rotationRateUnbiased + 
						"\nupdateInterval = " + Input.gyro.updateInterval + 
						"\nuserAcceleration = " + Input.gyro.userAcceleration;
			}
#endif
		}
	}

	void enableSelfieMode () {

		model.SetActive (true);

		firstPersoneCam.SetActive (false);
		selfiePersoneCam.SetActive (true);

		drive.cameraleft = selfiePersoneCamLeft;
		drive.cameraleft = selfiePersoneCamRight;

		selfieSprite.ResetToBeginning ();
		selfieSprite.PlayForward ();

		selfieLabel.ResetToBeginning ();
		selfieLabel.PlayForward ();

		if (!IsInvoking ("tweenBackSelfies")) Invoke ("tweenBackSelfies", 1);

		selfieMode = true;
	}

	void tweenBackSelfies () {
		selfieSprite.PlayReverse ();

		selfieLabel.PlayReverse ();
		CancelInvoke ("tweenBackSelfies");
	}

	void disableSelfieMode () {

		firstPersoneCam.SetActive (true);
		selfiePersoneCam.SetActive (false);

		model.SetActive (false);

		drive.cameraleft = firstPersoneCamLeft;
		drive.cameraleft = firstPersoneCamRight;

		tweenBackSelfies ();

		selfieMode = false;
	}

	public void Shoot () {
		if (theManager.photosLeft == 0) {
			// play bad sound and shake screen
			return;
		}
		if (selfieMode) Invoke ("disableSelfieMode", 0.75f);
		btnHold.enabled = false;
		//print ("downTime = " + downTime + " | selfieMode = " + selfieMode);
		downTime = 0.0f;
		shutTop.ResetToBeginning ();
		shutBottom.ResetToBeginning ();
		shutTop.PlayForward ();
		shutBottom.PlayForward ();
		audio.Play ();
		theManager.snapPic();
	}
}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameManager theManager;

	public OpenDiveSensor drive;
	public GameObject firstPersoneCam;
	public Camera firstPersoneCamLeft;
	public Camera firstPersoneCamRight;

	public GameObject selfiePersoneCam;
	public Camera selfiePersoneCamLeft;
	public Camera selfiePersoneCamRight;

	public SplineController theSpline;

	public UILabel compassInfo;
	public UILabel gyroscopeInfo;
	public UILabel debugLabel;

	public float magDiffTrig = 30.0f;
	float checkMagInterval = 0.5f;
	public bool canSnap = true;
	public float lastMag;
	public float downTime = 0.0f;
	bool selfieMode = false;

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
		firstPersoneCam.SetActive (false);
		selfiePersoneCam.SetActive (true);

		drive.cameraleft = selfiePersoneCamLeft;
		drive.cameraleft = selfiePersoneCamRight;

		selfieMode = true;
	}

	void disableSelfieMode () {
		firstPersoneCam.SetActive (true);
		selfiePersoneCam.SetActive (false);

		drive.cameraleft = firstPersoneCamLeft;
		drive.cameraleft = firstPersoneCamRight;

		selfieMode = false;
	}

	public void Shoot () {
		if (theManager.photosLeft == 0) {
			// play bad sound and shake screen
			return;
		}
		if (selfieMode) Invoke ("disableSelfieMode", 0.5f);
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

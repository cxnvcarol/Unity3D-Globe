using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NavManager : MonoBehaviour {
	// Use this for initialization

	public bool pathMode;
	private DataVisualizer datavisualizer;
	private int currentYearIndex;
	private int currentCaptureIndex;
	private CameraObrbit maincamScript;


	public Text logTextLabel;

	void Start () {
		pathMode = false;
		datavisualizer=GameObject.Find ("DataVisualizer").GetComponent<DataVisualizer>();
		maincamScript=GameObject.Find ("Main Camera").GetComponent<CameraObrbit>();
		currentYearIndex = 0;
		currentCaptureIndex = 0;
	}
	public void removeCaptureView(int index)
	{
		logTextLabel.text = "Capture removed";
		maincamScript.removeCaptureView (currentCaptureIndex);
		if (currentCaptureIndex == maincamScript.getCountCaptures ()) {
			currentCaptureIndex = maincamScript.getCountCaptures () - 1;
		}
		maincamScript.setCaptureToView (currentCaptureIndex);	

	}
   
	void Update () {
		if (maincamScript.countryMode)
			return;
		if (Input.GetKeyDown ("m")) {
			pathMode = !pathMode;

			logTextLabel.text = (pathMode?"path mode":"year mode");
			Debug.Log ("Changing mode to "+logTextLabel.text);

			if (pathMode) {
				currentCaptureIndex = maincamScript.getCountCaptures () - 1;
				if (currentCaptureIndex > 0) {
					maincamScript.setCaptureToView (currentCaptureIndex);
				}

			} //else?

		}

		if (Input.GetKeyDown ("r")&&maincamScript.getCountCaptures () >0) {
			removeCaptureView (currentCaptureIndex);
				
		}

		if (Input.GetKeyDown ("j")) {
			Debug.Log ("going left");
			if (!pathMode) {
				if (currentYearIndex > 0) {
					currentYearIndex--;
				}
				datavisualizer.ActivateSeries (currentYearIndex);
				int theyear = (currentYearIndex == 0) ? 1990 : ((currentYearIndex == 1) ? 1995 : 2000);
				logTextLabel.text = "Year "+theyear;//burned here same as in GUI
			}
			else {
				

				if (currentCaptureIndex > 0) {
					currentCaptureIndex--;
				}
				maincamScript.setCaptureToView (currentCaptureIndex);
				Debug.Log ("display the previous capture:"+currentCaptureIndex);
			}
			
		}
		else if (Input.GetKeyDown ("l")) {
			Debug.Log ("going right");
			if (!pathMode) {
				if (currentYearIndex <2) {
					currentYearIndex++;
				}
				datavisualizer.ActivateSeries (currentYearIndex);
				int theyear = (currentYearIndex == 0) ? 1990 : ((currentYearIndex == 1) ? 1995 : 2000);
				logTextLabel.text = "Year "+theyear;//burned here same as in GUI
			}
			else {


				if (currentCaptureIndex < maincamScript.getCountCaptures()-1) {
					currentCaptureIndex++;
				}
				maincamScript.setCaptureToView (currentCaptureIndex);

				Debug.Log ("display the next capture:"+currentCaptureIndex);
			}
		}


	}
}

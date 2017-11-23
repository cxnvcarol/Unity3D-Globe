using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NavManager : MonoBehaviour {
	// Use this for initialization

	//TODO: React to: p,j,k,l,c,numbers.
	public bool pathMode;
	private DataVisualizer datavisualizer;
	private int currentYearIndex;
	private int currentCaptureIndex;
	private CameraObrbit maincamScript;

	void Start () {
		pathMode = false;
		datavisualizer=GameObject.Find ("DataVisualizer").GetComponent<DataVisualizer>();
		maincamScript=GameObject.Find ("Main Camera").GetComponent<CameraObrbit>();
		currentYearIndex = 0;
		currentCaptureIndex = 0;
	}
   
	void Update () {
		if (Input.GetKeyDown ("m")) {
			pathMode = !pathMode;
			Debug.Log ("Changing mode to "+(pathMode?"path navigation":"year navigation"));

			if (pathMode) {

				//TODO: render frame on last selected view.

			} else {


			}

		}

		if (Input.GetKeyDown ("j")) {
			Debug.Log ("going left");
			if (!pathMode) {
				if (currentYearIndex > 0) {
					currentYearIndex--;
				}
				datavisualizer.ActivateSeries (currentYearIndex);
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
			}
			else {


				if (currentCaptureIndex < maincamScript.getCountCaptures()-1) {
					currentCaptureIndex++;
				}
				maincamScript.setCaptureToView (currentCaptureIndex);

				Debug.Log ("display the next capture:"+currentCaptureIndex);
			}
		}

		if (Input.GetKeyDown ("i")) {
			Debug.Log ("zoom in");

		}
		else if (Input.GetKeyDown ("k")) {
			Debug.Log ("zoom out");
		}


	}
}

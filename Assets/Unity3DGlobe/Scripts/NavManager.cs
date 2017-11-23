using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NavManager : MonoBehaviour {
	// Use this for initialization

	//TODO: React to: p,j,k,l,c,numbers.
	public bool pathMode;
	private DataVisualizer datavisualizer;
	void Start () {
		pathMode = false;
		datavisualizer=GameObject.Find ("DataVisualizer").GetComponent<DataVisualizer>();
	}
   
	void Update () {
		if (Input.GetKeyDown ("m")) {
			Debug.Log ("Changing mode");
			pathMode = !pathMode;

			if (pathMode) {

				//TODO: render frame on last selected view.

			} else {


			}

		}

		if (Input.GetKeyDown ("j")) {
			Debug.Log ("going left");
			if (!pathMode) {
				datavisualizer.ActivateSeries (0);
			}
			
			
		}
		else if (Input.GetKeyDown ("l")) {
			Debug.Log ("going right");
			if (!pathMode) {
				datavisualizer.ActivateSeries (2);
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

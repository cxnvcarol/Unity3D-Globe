using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class NavManager : MonoBehaviour {
	// Use this for initialization

	//TODO: React to: p,j,k,l,c,numbers.
	public bool pathMode;
	void Start () {
		pathMode = false;
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



	}
}

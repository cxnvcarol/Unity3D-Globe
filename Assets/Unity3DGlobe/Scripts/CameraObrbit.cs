using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Country
{
	public string name;
	public string country;
	public float latitude;
	public float longitude;
}
/**
 * Control main camera postion.
 * **/

public class CameraObrbit : MonoBehaviour {
    public float MinDistance = 1.0f;
    public float MaxDistance = 1.3f;
    float distance= 1000;
    float distanceTarget;
    Vector2 mouse ;
    Vector2 mouseOnDown ;
    public Vector2 rotation;
    public Vector2 target =new Vector2(Mathf.PI* 3 / 2, Mathf.PI / 6 );
	public Vector3 position;
	public Camera preFabCam; 
	public GameObject pathContent;
	public GameObject prefabPanel;

	public GameObject selectorPanel;



	public Text logTextLabel;


	Vector2 targetOnDown ;

	private Country[]  countries;
	private List<Camera> captures;
	private List<GameObject> capturesPanels;

	private int countCaptures;

	public bool countryMode;
	private string inputCountry;

	void loadCountries()
	{
		TextAsset jsonData = Resources.Load<TextAsset>("countries");
		string json = jsonData.text;
		countries = JsonHelper.getJsonArray<Country> (json);
		Debug.Log ("num countries: "+countries.Length);
		inputCountry = "";
	}

    // Use this for initialization
    void Start () {
        distanceTarget = transform.position.magnitude;

		countCaptures = 0;
		loadCountries ();
		captures = new List<Camera> ();
		capturesPanels = new List<GameObject> ();

		preFabCam=Instantiate (preFabCam,transform);
		countryMode = false;


	}
    bool down = false;
    // Update is called once per frame


	Vector2 getCountryCoordinates(string country)
	{
		Vector2 r=new Vector2(0,0);
		foreach (var c in countries) {
			if (string.Equals (country,c.name,System.StringComparison.InvariantCultureIgnoreCase)) {//FIX: Use equals ignorecase.
				
				r.x = c.latitude;
				r.y = c.longitude;
				return r;
			}
		}
		return r;
	}
	void GoTo (string country)
	{
		Vector2 coords = getCountryCoordinates (country);
		if (coords.magnitude==0) {
			Debug.LogWarning ("no country found:"+country);
			logTextLabel.text = country+" not found";
			return;
		}
		logTextLabel.text = "Showing "+country;
		float lat=coords.x;
		float lng = coords.y;

		Vector3 pos;
		pos.x =  Mathf.Cos ((lng) * Mathf.Deg2Rad) * Mathf.Cos (lat * Mathf.Deg2Rad);
		pos.y = Mathf.Sin (lat * Mathf.Deg2Rad);
		pos.z = Mathf.Sin ((lng) * Mathf.Deg2Rad) * Mathf.Cos (lat * Mathf.Deg2Rad);
		Debug.Log (pos);

		target.y = Mathf.Asin (pos.y/pos.magnitude);
		target.x = Mathf.Atan (pos.x / pos.z);
		target.x += pos.z<0?Mathf.PI:0;

		distanceTarget = MinDistance;
	}

    void Update()
    {
		if (countryMode) {
			
			foreach (char c in Input.inputString) {
				if ((c == '#')&&inputCountry.Length>0 ) // enter/return
				{
					inputCountry=inputCountry.Substring (1);
					print("User entered: " + inputCountry);

					GoTo (inputCountry);

					inputCountry = "";
					countryMode = false;
				}
				else
				{
					inputCountry += c;
				}
			}
			return;
		}
		if (Input.GetKeyDown ("q")) {
			countryMode = true;
			return;
		}
		if (Input.GetKeyDown ("p")) {
			//Debug.Log ("Capturing view from camera");
			//logTextLabel.text = ""

			Camera cam = Camera.Instantiate (preFabCam);
			cam.CopyFrom (Camera.main);
			cam.name = "preCam_" + countCaptures;
			cam.rect=new Rect (0, 0, 1,1);
			cam.enabled = true;
			captures.Add (cam);



			//HERE FOR STACKED PANEL..
			GameObject pan = GameObject.Instantiate (prefabPanel);
			//pan.transform.parent = pathContent.transform;
			pan.transform.SetParent(pathContent.transform);
			Vector3 p = pan.transform.position;
			p.x = countCaptures * 200;
			pan.transform.position = p;
			RenderTexture rt = new RenderTexture(256, 256,24, RenderTextureFormat.ARGB32);
			rt.Create();
			cam.targetTexture = rt;
			pan.GetComponent<RawImage> ().texture = rt;
			capturesPanels.Add (pan);
			countCaptures++;

		}

        if (Input.GetMouseButtonDown(0))
        {
            down = true;
            mouseOnDown.x = Input.mousePosition.x;
            mouseOnDown.y = -Input.mousePosition.y;

            targetOnDown.x = target.x;
            targetOnDown.y = target.y; 
        }
        else if(Input.GetMouseButtonUp(0))
        {
            down = false;
        }
        if(down)
        {
            mouse.x = Input.mousePosition.x;
            mouse.y = -Input.mousePosition.y;

            float zoomDamp = distance / 1;

            target.x = targetOnDown.x + (mouse.x - mouseOnDown.x) * 0.005f* zoomDamp;
            target.y = targetOnDown.y + (mouse.y - mouseOnDown.y) * 0.005f* zoomDamp;
            
            target.y = Mathf.Clamp(target.y, -Mathf.PI / 2 + 0.01f, Mathf.PI / 2 - 0.01f);
        }

		distanceTarget -= Input.GetAxis("Mouse ScrollWheel");

		//TODO:: Allow middle values for the zoom!
		if (Input.GetKeyDown ("i")) {
			distanceTarget = MinDistance;
		} 
		else if (Input.GetKeyDown ("k")) {
			distanceTarget = MaxDistance;
		} 
		distanceTarget = Mathf.Clamp(distanceTarget, MinDistance, MaxDistance);


		distance += (distanceTarget - distance) * 0.2f;
		rotation.x += (target.x - rotation.x) * 0.1f;
		rotation.y += (target.y - rotation.y) * 0.1f;

        position.x = distance * Mathf.Sin(rotation.x) * Mathf.Cos(rotation.y);
        position.y = distance * Mathf.Sin(rotation.y);
        position.z = distance * Mathf.Cos(rotation.x) * Mathf.Cos(rotation.y);

        transform.position = position;
        transform.LookAt(Vector3.zero);
    }

	public int getCountCaptures(){
		return countCaptures;
	}

	public void setCaptureToView(int index)
	{
		if (index < countCaptures) {
			Vector3 pos = captures [index].transform.position;

			target.y = Mathf.Asin (pos.y/pos.magnitude);
			target.x = Mathf.Atan (pos.x / pos.z);
			target.x += pos.z<0?Mathf.PI:0;

			distanceTarget = pos.magnitude;



			Vector3 p = selectorPanel.transform.position;
			p.x = index * 200;
			selectorPanel.transform.position = p;
			selectorPanel.SetActive (true);
		}
	}

	public void removeCaptureView(int index)
	{

		if (index < countCaptures) {
			Camera ca = captures [index];
			captures.RemoveAt (index);
			Destroy (ca.gameObject);


			GameObject panel = capturesPanels [index];
			capturesPanels.RemoveAt (index);
			Destroy (panel);

			countCaptures--;

			for (int i = index; i < countCaptures; i++) {
				GameObject pan = capturesPanels [i];
				Vector3 p = pan.transform.position;
				p.x -= 200;
				pan.transform.position = p;
			}
		}

	}
}

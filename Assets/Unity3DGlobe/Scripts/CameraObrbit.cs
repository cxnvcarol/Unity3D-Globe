using UnityEngine;
using System.Collections;


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
	Vector2 targetOnDown ;

	private Country[]  countries;
	//private List<Camera> captures;

	Camera pcam;

	private int countCaptures;


	void loadCountries()
	{
		TextAsset jsonData = Resources.Load<TextAsset>("countries");
		string json = jsonData.text;
		countries = JsonHelper.getJsonArray<Country> (json);
		Debug.Log ("num countries: "+countries.Length);
	}

    // Use this for initialization
    void Start () {
        distanceTarget = transform.position.magnitude;
		pcam = GameObject.Find("preCam").GetComponent<Camera>();
		countCaptures = 0;
		loadCountries ();
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
			return;
		}
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
		if (Input.GetKeyDown ("p")) {
			Debug.Log ("Capturing view from camera");
			Camera cam = Camera.Instantiate (pcam);
			cam.CopyFrom (Camera.main);
			cam.name = "preCam_" + countCaptures;
			cam.rect=new Rect (countCaptures*0.2f, 0, 0.2f,0.2f);
			cam.enabled = true;
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
		else if (Input.GetKeyDown ("k")) {//going to Colombia
			distanceTarget = MaxDistance;
		} 
		distanceTarget = Mathf.Clamp(distanceTarget, MinDistance, MaxDistance);


		//TODO Replace next for real thing:
		if (Input.GetKeyDown ("c")) {//going to Colombia
			GoTo("Colombia");
		} 
		if (Input.GetKeyDown ("n")) {
			GoTo("Nigeria");
		} 
		if (Input.GetKeyDown ("g")) {
			GoTo("Germany");
		} 
		if (Input.GetKeyDown ("a")) {//going to unknown
			GoTo("A");
		} 


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

		//TODO.. Copy camera settings to the main camera!
	}
}

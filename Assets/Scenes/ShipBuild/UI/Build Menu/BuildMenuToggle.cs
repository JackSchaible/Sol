using UnityEngine;
using UnityEngine.UI;

public class BuildMenuToggle : MonoBehaviour
{
    public GameObject Submenu;

    private static Color selected = new Color(0.28f, 0.33f, 0.86f);
    private static Color unselected = new Color(1, 1, 1);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var isOn = gameObject.GetComponent<Toggle>().isOn;

        transform.gameObject.GetComponentInChildren<Text>().color = isOn ? selected : unselected;

	    if (Submenu == null) return;
        SetActive(isOn);
	}

    public void SetActive(bool active)
    {
        Submenu.GetComponent<BuildMenuToggle>().SetActive(active);
        SetActive(active);
    }
}

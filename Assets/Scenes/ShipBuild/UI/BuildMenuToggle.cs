using System.Linq;
using System.Runtime.InteropServices;
using Assets.Scenes.ShipBuild.UI.Build_Menu;
using UnityEngine;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public class BuildMenuToggle : MonoBehaviour
{
    public GameObject Parent;
    public GameObject Child;

    public GameObject Details;

    private static readonly Color Selected = new Color(0.28f, 0.33f, 0.86f);
    private static readonly Color Unselected = new Color(1, 1, 1);

    public bool IsOn
    {
        get { return gameObject.GetComponent<Toggle>().isOn; }
        set { gameObject.GetComponent<Toggle>().isOn = value; }
    }

    private bool _previousOn = false;

	void Start ()
	{
	    if (Child == null) return;
	    Child.GetComponent<Submenu>().Parent = this;
	}
	
	void Update ()
	{
	    transform.gameObject.GetComponentInChildren<Text>().color = IsOn ? Selected : Unselected;

	    if (_previousOn == IsOn) return;

	    if (IsOn && Child != null)
	        Child.SetActive(IsOn);

	    if (Child == null)
	        Details.SetActive(IsOn);

        _previousOn = IsOn;
	}
}

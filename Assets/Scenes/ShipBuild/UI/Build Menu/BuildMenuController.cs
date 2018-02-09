using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
    public GameObject parent;

	void Start ()
	{
	    var submenus = GameObject.FindGameObjectsWithTag("Submenu");

	    foreach (var item in submenus)
	    {
	        var menu = item.GetComponent<BuildMenuToggle>().Submenu;
	        if (menu == null) continue;
            menu.SetActive(false);
	    }
	}

    public void OnChange(Toggle item)
    {
        var toggleScript = item.GetComponent<BuildMenuToggle>();
            
        var submenu = toggleScript.Submenu;

        if (submenu == null) return;

        submenu.SetActive(item.isOn);
    }
}

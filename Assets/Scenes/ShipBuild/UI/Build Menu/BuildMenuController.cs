using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
    public GameObject parent;

    private GameObject[] Options;

    void Start()
    {
        Options = parent.GetComponentsInChildren<Transform>()
            .Where(x => x.tag == "Radial Menu Button")
            .Select(x => x.gameObject).ToArray();

        foreach (var option in Options)
            option.GetComponent<Toggle>().isOn = false;
    }

    public void OnChange(Toggle item)
    {
        foreach (var option in Options)
        {
            if (option.name != item.name)
                option.GetComponent<Toggle>().isOn = false;
        }
    }
}

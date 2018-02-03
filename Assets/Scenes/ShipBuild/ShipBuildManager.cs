using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuildManager : MonoBehaviour
{
    public RadialMenu BuildMenu;
    public Camera mainCamera;

    void Start()
    {
        BuildMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.S))
        {
            var pos = Input.mousePosition;

            BuildMenu.gameObject.transform.position = pos;
            BuildMenu.gameObject.SetActive(true);

            //Constrain to window
            //TODO: broken
            /*
            Rect rect = GameObject.Find("Radial Menu Background").GetComponent<RectTransform>().rect;
            if (mainCamera.WorldToScreenPoint(new Vector3(rect.xMin, rect.y, 0)).x < 0)
                BuildMenu.GetComponent<RectTransform>().position = mainCamera.ScreenToWorldPoint(new Vector3(rect.width / 2, rect.y, 0));
            else if (mainCamera.WorldToScreenPoint(new Vector3(rect.xMax, rect.y, 0)).x > Screen.width)
                BuildMenu.GetComponent<RectTransform>().position = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - (rect.width / 2), rect.y, 0));

            if (mainCamera.WorldToScreenPoint(new Vector3(rect.x, rect.yMin - 88, 0)).y < 0)
                BuildMenu.GetComponent<RectTransform>().position = mainCamera.ScreenToWorldPoint(new Vector3(rect.x, (rect.height / 2) + 88, 0));
            else if (mainCamera.WorldToScreenPoint(new Vector3(rect.x, rect.yMax, 0)).x > Screen.height)
                BuildMenu.GetComponent<RectTransform>().position = mainCamera.ScreenToWorldPoint(new Vector3(rect.x, Screen.height - (rect.height / 2), 0));
                */
        }
    }
}

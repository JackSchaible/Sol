using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class RadialMenu : MonoBehaviour
{
	public Camera MainCamera;
    public Text OptionText;
	public GameObject[][] ButtonObjects;
    public UnityEvent OnClosed;

    private int _currentPage;
	private const int PageSize = 8;
	private int _numPages;

	public void Start()
	{
	    GameObject[] buttons = transform.gameObject.GetComponentsInChildren<Transform>()
            .Where(x => x.tag == "Radial Menu Button")
            .Select(x => x.gameObject).ToArray();
        
        _currentPage = 0;
		_numPages = (buttons.Length / PageSize) + 1;
	    if (buttons.Length == PageSize)
	        _numPages = 1;

		int numItems = buttons.Length;

		ButtonObjects = new GameObject[_numPages][];

		for (int i = 0; i < _numPages; i++)
        {
			var page = i;
			var currentPageSize = Mathf.Clamp (numItems - (PageSize * page), 0, PageSize);
			ButtonObjects [page] = new GameObject[currentPageSize];

			for (int j = 0; j < currentPageSize; j++)
				ButtonObjects [page] [j] = buttons [(i * PageSize) + j];
		}

	    for (int i = 0; i < buttons.Length; i++)
	        buttons[i].SetActive(i < PageSize);

	    if (_numPages == 1)
	    {
	        var fwdBtn = transform.Find("Forward Button").gameObject;
	        var bckBtn = transform.Find("Back Button").gameObject;

            fwdBtn.GetComponent<Image>().color = Color.gray;
	        fwdBtn.GetComponent<Button>().enabled = false;
	        bckBtn.GetComponent<Image>().color = Color.gray;
	        bckBtn.GetComponent<Button>().enabled = false;
	    }
	}

    public void Update() {
		if (Input.GetKeyDown (KeyCode.Escape))
			CloseClicked ();
	}

	public void CloseClicked() {
		gameObject.SetActive (false);
	    OnClosed.Invoke();
	}

	public void NextClicked() {
		_currentPage++;

		if (_currentPage > _numPages - 1)
			_currentPage = 0;

		ChangePages ();
	}

	public void BackClicked() {
		_currentPage--;

		if (_currentPage < 0)
			_currentPage = _numPages - 1;

		ChangePages ();
	}

	public void ChangePages () {
		for (int i = 0; i < ButtonObjects.Length; i++)
			foreach (var button in ButtonObjects[i])
				button.SetActive (i == _currentPage);
	}

    public void MouseOver(Button button)
    {
        OptionText.text = button.name;
    }

    public void MouseExit()
    {
        OptionText.text = "";
    }

    public void MouseClick(GameObject button)
    {
        var submenu = button.transform.Find("Submenu");
        submenu.position = transform.position;
        submenu.gameObject.SetActive(true);
        var fb = transform.Find("Forward Button");
        fb.gameObject.SetActive(false);
        transform.Find("Back Button").gameObject.SetActive(false);
        transform.Find("Close Button").gameObject.SetActive(false);
        transform.Find("Text").gameObject.SetActive(false);

        foreach (var array in ButtonObjects)
            foreach (var b in array)
            {
                b.GetComponent<Button>().interactable = false;
                b.GetComponent<Image>().enabled = false;
                b.transform.Find("Icon").gameObject.GetComponent<Image>().enabled = false;
            }
    }

    public void SubmenuClosed(GameObject submenu)
    {
        submenu.SetActive(false);
        transform.Find("Forward Button").gameObject.SetActive(true);
        transform.Find("Back Button").gameObject.SetActive(true);
        transform.Find("Close Button").gameObject.SetActive(true);
        transform.Find("Text").gameObject.SetActive(true);

        foreach (var array in ButtonObjects)
        foreach (var b in array)
        {
            b.GetComponent<Button>().interactable = true;
            b.GetComponent<Image>().enabled = true;
            b.transform.Find("Icon").gameObject.GetComponent<Image>().enabled = true;
        }
    }
}

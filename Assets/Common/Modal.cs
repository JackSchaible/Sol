using System.Linq;
using Assets.Data;
using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    public GameObject Success;
    public GameObject Info;
    public GameObject Input;
    public GameObject Error;

    private Canvas _canvas;

	// Use this for initialization
	void Start ()
	{
	    _canvas = transform.GetComponentInChildren<Canvas>();
	    _canvas.enabled = false;

        Success.SetActive(false);
	    Info.SetActive(false);
	    Input.SetActive(false);
	    Error.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(ModalData data)
    {
        switch (data.ModalType)
        {
            case ModalTypes.Error:
                Error.SetActive(true);
                break;

            case ModalTypes.Info:
                Info.SetActive(true);
                break;

            case ModalTypes.Input:
                Input.SetActive(true);
                break;

            case ModalTypes.Success:
                Success.SetActive(true);
                break;
        }

        var texts = GetComponentsInChildren<Text>();
        texts.First(x => x.name == "Title").text = data.Title;
        texts.First(x => x.name == "Content").text = data.Text;
    }

    public void ShowModal()
    {
        if (_canvas == null) return;
        _canvas.enabled = true;
    }

    public void OnClose()
    {
        if (_canvas == null) return;
        _canvas.enabled = false;
    }
}

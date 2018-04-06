using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps the attached toggle highlighted
/// </summary>
public class ToggleHighlighter : MonoBehaviour
{
    private Toggle _toggle;
    private Text _text;
    private Color _normal = new Color(1, 1, 1);
    private Color _selected = new Color(197f/255f, 199f/255f, 72f/255f);

	void Start ()
	{
	    _toggle = GetComponent<Toggle>();
	    _text = _toggle.GetComponentInChildren<Text>();
	}
	
	void Update ()
	{
	    _text.color = _toggle.isOn ? _selected : _normal;
	}
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps the attached toggle highlighted
/// </summary>
public class ToggleHighlighter : MonoBehaviour
{
    public float A
    {
        get { return _normal.a; }
        set
        {
            _normal = new Color(_normal.r, _normal.g, _normal.b, value);
            _selected = new Color(_selected.r, _selected.g, _selected.b, value);
        }
    }

    private Toggle _toggle;
    private Text _text;
    private Color _normal;
    private Color _selected;

	void Start ()
	{
	    _toggle = GetComponent<Toggle>();
	    _text = _toggle.GetComponentInChildren<Text>();

	    A = 1;
	    _normal = new Color(1, 1, 1, A);
        _selected = new Color(197f / 255f, 199f / 255f, 72f / 255f);

	}
	
	void Update ()
	{
	    _text.color = _toggle.isOn ? _selected : _normal;
	}
}

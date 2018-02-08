using UnityEngine;
using UnityEngine.UI;

public class BuildMenuButton : MonoBehaviour
{
    private Toggle _toggle;
    private Image _image;

    private static Color highlightColor = new Color(0.3f, 0.4f, 0.9f, 0.1f);
    private static readonly ColorBlock HighlightColorBlock = new ColorBlock()
    {
        normalColor = highlightColor,
        highlightedColor = highlightColor,
        pressedColor = highlightColor,
        disabledColor = highlightColor
    };
    private static readonly ColorBlock NormalColorBlock = new ColorBlock()
    {
        normalColor = Color.white,
        highlightedColor = Color.white,
        pressedColor = Color.white,
        disabledColor = Color.white
    };

    void Start()
    {
        _toggle = GetComponent<Toggle>();
        _image = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (_toggle.isOn)
            _toggle.colors = HighlightColorBlock;
        else
            _toggle.colors = NormalColorBlock;
    }
}

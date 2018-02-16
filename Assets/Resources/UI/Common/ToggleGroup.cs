using UnityEngine;
using UnityEngine.UI;

public class ToggleGroup : MonoBehaviour
{
    private Toggle[] _toggles;
    private bool[] _isOns;

	void Start ()
	{
	    _toggles = GetComponentsInChildren<Toggle>();

	    _isOns = new bool[_toggles.Length];
	    for (var i = 0; i < _isOns.Length; i++)
	        _isOns[i] = false;
	}
	
	void Update ()
	{
	    var hasChanged = false;
	    for (var i = 0; i < _toggles.Length; i++)
	        if (_toggles[i].isOn != _isOns[i])
	        {
	            //Change
	            foreach (var t in _toggles)
	                t.isOn = false;

	            _toggles[i].isOn = true;
                UpdateToggleStatus();
	            hasChanged = true;
	        }

        if (!hasChanged)
	        UpdateToggleStatus();
	}

    private void UpdateToggleStatus()
    {
        for (var i = 0; i < _toggles.Length; i++)
            _isOns[i] = _toggles[i].isOn;
    }
}

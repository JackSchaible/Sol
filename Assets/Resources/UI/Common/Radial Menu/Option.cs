using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Button ButtonRef;

    public bool Hover;
    public bool Down;

    void OnMouseOver()
    {
        
    }

    void OnMouseExit()
    {
        Hover = false;
    }
}

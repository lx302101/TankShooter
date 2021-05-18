using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    // Start is called before the first frame update
    private bool pressed;
    void Start()
    {
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData data)
    {
        pressed = true;
        Debug.Log("Button Pressed");
    }

    public void OnPointerUp(PointerEventData data)
    {
        pressed = false;
        Debug.Log("Button Not Pressed");
    }
    
    public bool isPressed()
    {
        return pressed;
    }
}

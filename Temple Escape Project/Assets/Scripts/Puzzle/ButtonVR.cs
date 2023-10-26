using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public GameObject parent;

    GameObject presser;
    bool isPressed;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!isPressed && other.tag == "PlayerHand")
        {
            print("spain");
            button.transform.Translate(0, -0.03f, 0, parent.transform);
            
            onPress.Invoke();
            isPressed = true;
        }
    }

}

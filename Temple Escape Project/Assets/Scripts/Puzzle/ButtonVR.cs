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
    public AnimationCurve curve;

    GameObject presser;
    bool isPressed;
    void Start()
    {
        isPressed = false;
        curve = GetComponent<AnimationCurve>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.Translate(0, -0.03f, 0, parent.transform);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    public void DescendPillar()
    {
        
    }

}

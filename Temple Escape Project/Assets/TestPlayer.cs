using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public GameObject monsterReference;
    public float howLongUntillMakeSound = 0;

    // Update is called once per frame
    void Update()
    {
        howLongUntillMakeSound += Time.deltaTime;
        if (howLongUntillMakeSound > 20)
        {
            
        }
    }
}

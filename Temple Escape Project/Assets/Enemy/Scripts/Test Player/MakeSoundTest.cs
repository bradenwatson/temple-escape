using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSoundTest : MonoBehaviour
{
    public GameObject monsterReference;
    public float howLongUntillMakeSound = 0;

    // Update is called once per frame
    void Update()
    {
        howLongUntillMakeSound += Time.deltaTime;
        if (howLongUntillMakeSound > 20)
        {
            mBrain_base monster = monsterReference.GetComponent<mBrain_base>();
            Vector3 thisPosition = transform.position;
            if (thisPosition.y == 0 && thisPosition.x == 0 && thisPosition.z == 0)
            {
                Debug.Log("empty");
            }
            monster.SearchSound(thisPosition);           
            howLongUntillMakeSound = 0f;
        }
    }
}

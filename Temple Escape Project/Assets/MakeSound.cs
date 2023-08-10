using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    public bool makeSound = true;
    public bool forceMakeSound = false;         // switch in unity to make sound instantly whereever u want
    public float secondsBetweenEachSound = 10f;
    public float timeSinceLastSound = 0f;
    public GameObject monster;

    private void Update()
    {
        if (timeSinceLastSound > secondsBetweenEachSound && makeSound || forceMakeSound)
        {
            SendSoundToMonster();
            timeSinceLastSound = 0;
            forceMakeSound = false;
        }
        timeSinceLastSound += Time.deltaTime;       
    }

    private void SendSoundToMonster()
    {
        monster.GetComponent<PatrolFindingWalking>().SetSoundTrigger(transform.position);
    }
}

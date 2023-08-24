using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scuttle : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    void Start()
    {
        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        } 
        PlaySound.PlaySoundOnce("Bugs", source);        
    }
}

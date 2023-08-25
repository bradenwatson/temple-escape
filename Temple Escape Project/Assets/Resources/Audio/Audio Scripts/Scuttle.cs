using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        PlaySound.PlaySoundOnce("Bugs", source);
    }
}

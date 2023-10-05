using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterNextLevelControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("NEXT LEVEL!");
        }
    }
}

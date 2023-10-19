using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCollectableScript : MonoBehaviour
{
    Vector3 startingPosition;
    bool moved = false;

    void Start()
    {
        startingPosition = transform.position;
    }

    public void PickUpCollectable()
    {
        AttachCollectableToPlayer();
        moved = true;
    }

    void AttachCollectableToPlayer()
    {

    }

    public void PutCollectableBack()
    {
        transform.position = startingPosition;
        moved = false;
    }
}

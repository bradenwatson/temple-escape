using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInformation : MonoBehaviour
{
    public Vector3 startLocation;
    public bool moved = false;

    private void Start()
    {
        startLocation = transform.position;
    }

    public void SetMovedPiece(bool moved = true)
    {
        this.moved = moved;
    }
}

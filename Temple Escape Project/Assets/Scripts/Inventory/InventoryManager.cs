using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    List<GameObject> slots = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        slots.Add(GameObject.FindGameObjectsWithTag("Slot1")[0]);
        slots.Add(GameObject.FindGameObjectsWithTag("Slot2")[0]);
        slots.Add(GameObject.FindGameObjectsWithTag("Slot3")[0]);
    }

}

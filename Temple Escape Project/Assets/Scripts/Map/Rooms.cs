using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    //Properties
    /*
     * Check if locked     
     * Connected rooms (optional)
     * Check visited by monster
     * Check visited by player
     * Teleportable (based on player position)
     */
    [SerializeField]
    bool hasPlayerVisited { get; set; }
    bool hasEnemyVisisted { get; set; }
    bool isTeleportable { get; set; } 

    private void Start()
    {
        //Initialise room based on tag name
        InitialiseRoom();
    }

    //Methods
    //Initialise room by type
    private void InitialiseRoom()
    {
        hasPlayerVisited = false;
        hasEnemyVisisted = false;
        isTeleportable = false;
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public enum RoomType { Normal, Puzzle, Secret, Safe, Exit, };
    //dropdown
    public RoomType roomType;
    
    
    //PROPERTIES
    //Should it get doors?
    //Should it be able to lock and unlock doors
    //room should have control over doors

    [Header("Debug")]
    bool hasPlayerVisited; //{ get; set; }
    bool hasEnemyVisisted; //{ get; set; }
    bool isTeleportable; //{ get; set; } 

    private void Start()
    {
        //Initialise room based on tag name
        InitialiseRoom();
    }

    //Methods
    //Initialise room by type

    private void InitialiseRoom()
    {
        //Should spawn in the center of each room?
        hasPlayerVisited = false;
        hasEnemyVisisted = false;
        isTeleportable = false;
    }

    public bool HasPlayerVisited { get; set; }
    public bool HasEnemyVisisted { get; set; }
    public bool IsTeleportable { get; set; }
}

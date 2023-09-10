using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public enum RoomType { Normal, Puzzle, Secret, Safe, Exit};
public class Room : MonoBehaviour
{
    
    //dropdown
    public RoomType roomType;
    public readonly int roomID;

    //FUTURE
    //https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
    //https://discussions.unity.com/t/custom-class-reference-not-recognized/243032
    //Automatically create room
    //Create asset


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
    public int RoomID { get; set; } 
    public bool HasPlayerVisited { get; set; }
    public bool HasEnemyVisisted { get; set; }
    public bool IsTeleportable { get; set; }
}

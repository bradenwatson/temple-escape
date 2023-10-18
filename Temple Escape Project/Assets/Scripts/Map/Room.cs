using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public enum RoomType { Normal, Puzzle, Secret, Safe, Exit};
public class Room : MonoBehaviour
{
    
    //Dropdown
    public readonly RoomType roomType;
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
    bool hasPlayerVisited; 
    bool hasEnemyVisisted; 
    bool isTeleportable;
    int passageCount;
    //List<GameObject> passages;      //There can be doors or some without but just a passage

    private void Awake()
    {
        //A node must be within the scene in order to be accessed and referenced
        //In this case, a node represents the room
        CustomNode node = gameObject.AddComponent<CustomNode>();



        //It also possible to initialise its node fields here than from the map since its more direct
        //Check for passages instead  of door (prevent duplication of doors and determine passages without doors)
        //For now use their string object name to determine amount of rooms
        Match roomPassages = Regex.Match(gameObject.name, @"\b\d+\b");
        passageCount = int.Parse(roomPassages.Value);
        node.SetNodeLimit(passageCount);
        //Debug.Log($"Passages = ({passageCount})");
    }

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

    //public int RoomID { get; set; } 
    //public bool HasPlayerVisited { get; set; }
    //public bool HasEnemyVisisted { get; set; }
    //public bool IsTeleportable { get; set; }

    public int PassageCount
    {
        get { return passageCount; }
    }
}

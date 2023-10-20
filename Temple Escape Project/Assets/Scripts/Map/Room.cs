using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.YamlDotNet.Core;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;

public enum RoomType { Normal, Puzzle, Secret, Safe, Exit};
public class Room : MonoBehaviour
{
    
    //Dropdown
    public RoomType roomType;
    

    [Header("Debug")]
    int roomID;
    int passageCount;
    bool hasPlayerVisited; 
    bool hasEnemyVisisted; 
    bool isTeleportable;
    
         

    private void Awake()
    {
        //Label game object as room for backup references
        if(gameObject.tag != "Room")
        {
            gameObject.tag = "Room";
        }

        //A node must be within the scene in order to be accessed and referenced
        CustomNode node = gameObject.AddComponent<CustomNode>();

        //It also possible to initialise its node fields here than from the map class since its more direct
        //Check for passages instead  of door (prevent duplication of doors and determine passages
        //without doors)
        //For now use their string object name to determine amount of rooms
        Match roomPassages = Regex.Match(gameObject.name, @"\b\d+\b");
        passageCount = int.Parse(roomPassages.Value);
        node.SetNodeLimit(4);   //Orientation is indexed based, all rooms have max 4 passages
        node.SetData(gameObject);
        //Debug.Log($"Passages = ({passageCount})");
    }

    private void Start()
    {

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

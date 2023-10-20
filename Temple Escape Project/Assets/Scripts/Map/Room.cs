using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType { Normal=75, Puzzle=90, Secret=97, Safe=100, Exit=-1};
public class Room : MonoBehaviour
{
    //Dropdown
    [Header("Customisation")]
    public RoomType roomType = RoomType.Normal;
    public Boolean randomiserEnabled = false;

    [Header("Room")]
    int roomID = -1;     //Low = Closer to center, High = Away from center
    int passageCount = -1;
    bool isTeleportable = false;
    GameObject[] doors;     //PLACEHOLDER

    [Header("Player")]
    bool hasPlayerVisited = false;
    bool isPlayerPresent = false;

    [Header("Room")]
    bool hasEnemyVisisted = false;
    bool isEnemyPresent = false;




    private void Awake()
    {
        //Label game object as room for backup references
        if (gameObject.tag != "Room") { gameObject.tag = "Room"; }

        //Randomly chose a room (except exit) if enabled
        Randomiser(randomiserEnabled);

        //A node must be within the scene in order to be accessed and referenced
        //CustomNode node = gameObject.AddComponent<CustomNode>();
        

        //Check for passages instead of door (prevent duplication and find passages without doors)
        //For now use their string object name to determine amount of passages
        Match roomPassages = Regex.Match(gameObject.name, @"\b\d+\b");
        passageCount = int.Parse(roomPassages.Value);

        
        //node.SetNodeLimit(4);   //Orientation indexed based, all rooms have max 4 passages
        //node.SetData(gameObject);
    }

    private void Randomiser(bool randomiser)
    {
        if(randomiser)
        {
            int chance = Random.Range(0, 100);
            if((chance >= 0) && (chance <= (int)RoomType.Normal))
            {
                roomType = RoomType.Normal; 
            }
            else if((chance > (int)RoomType.Normal) && (chance <= (int)RoomType.Puzzle)) 
            {
                roomType = RoomType.Puzzle;
            }
            else if ((chance > (int)RoomType.Puzzle) && (chance <= (int)RoomType.Secret))
            {
                roomType = RoomType.Secret;
            }
            else if ((chance > (int)RoomType.Secret) && (chance <= (int)RoomType.Safe))
            {
                roomType = RoomType.Safe;
            }
            else
            {
                throw new Exception("Error set in RoomType Enum.");
            }
        }
    }



    //Accessors
    //Rooms
    public int RoomID { get { return roomID; } set { roomID = value; } }
    public int PassageCount { get { return passageCount; } }
    public bool IsTeleportable { get { return isTeleportable; } set { isTeleportable = value; } }
    //PLACEHOLDER
    public GameObject[] Doors { get { return doors; } }

    //Player
    public bool HasPlayerVisited { get { return hasPlayerVisited; } }
    public bool IsPlayerPresent 
    {
        get { return isPlayerPresent; }
        set 
        { 
            isPlayerPresent = value; 
            if(isPlayerPresent) { hasPlayerVisited = true; }
        } 
    }

    //Enemy
    public bool HasEnemyVisited { get { return hasEnemyVisisted; } }
    public bool IsEnemyPresent
    {
        get { return isEnemyPresent; }
        set
        {
            isEnemyPresent = value;
            if (isEnemyPresent) { hasEnemyVisisted = true; }
        }
    }

}

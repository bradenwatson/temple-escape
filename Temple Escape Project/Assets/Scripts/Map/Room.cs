using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public enum RoomType { Normal=75, Puzzle=90, Secret=97, Safe=100, Exit=-1};     //Random range (0,100)
public class Room : MonoBehaviour
{
    //Dropdown
    [Header("Customisation")]
    [SerializeField]
    public RoomType roomType = RoomType.Normal;
    public Boolean randomiserEnabled = false;

    [Header("Room")]
    [SerializeField]
    int roomID = -1;     //Low = Closer to center, High = Away from center
    [SerializeField]
    int passageCount = -1;
    [SerializeField]
    bool isTeleportable = false;
    [SerializeField]
    GameObject[] doors;     //PLACEHOLDER
    [SerializeField]
    CustomNode node = null;

    [Header("Player")]
    [SerializeField]
    bool hasPlayerVisited = false;
    [SerializeField]
    bool isPlayerPresent = false;

    [Header("Room")]
    [SerializeField]
    bool hasEnemyVisisted = false;
    [SerializeField]
    bool isEnemyPresent = false;

    private void Awake()
    {
        //Label game object as room for backup references
        if (gameObject.CompareTag("Room")) { gameObject.tag = "Room"; }

        //Randomly chose a room (except exit) if enabled
        Randomiser(randomiserEnabled);
        
        //Check for passages instead of door (prevent duplication and find passages without doors)
        //For now use their string object name to determine amount of passages
        Match roomPassages = Regex.Match(gameObject.name, @"\b\d+\b");
        int test = int.Parse(roomPassages.Value);
        PassageCount = test;
    }

    //Accessors
    //Rooms
    public int RoomID { get { return roomID; } set { roomID = value; } }
    public int PassageCount { get { return passageCount; } private set { passageCount = value; } }
    public bool IsTeleportable { get { return isTeleportable; } set { isTeleportable = value; } }
    //PLACEHOLDER
    public GameObject[] Doors { get { return doors; } }

    public CustomNode Node 
    { 
        get { return node; } 
        set 
        { 
            node = value;
            roomID = node.GetIndex();
        } 
    }

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

    
    
    
    //METHODS
    /*********************************************************************************************************
    * Method: Randomiser 
    * Input: randomiserEnabled(bool)
    * Output: N/A
    * Purpose: Determine room type based on the current probability values the RoomType enum if turned on
    *********************************************************************************************************/
    private void Randomiser(bool randomiserEnabled)
    {
        if (randomiserEnabled)
        {
            int chance = Random.Range(0, 100);
            if ((chance >= 0) && (chance <= (int)RoomType.Normal))
            {
                roomType = RoomType.Normal;
            }
            else if ((chance > (int)RoomType.Normal) && (chance <= (int)RoomType.Puzzle))
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
        Debug.Log($"{gameObject.name} randomly selected as {roomType}");
    }


    /*************************************************************************************************************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}

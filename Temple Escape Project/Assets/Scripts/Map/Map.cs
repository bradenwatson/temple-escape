/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: User friendly map tool without needing to understand trees. Once a central room is 
 *          defined as the root, the class will connect all the rooms as a tree which can be 
 *          used to traverse and navigate and perform calculations. The map is designed to be 
 *          shared amongst the enemy and the player and interacting with the game manager.
 *  Last updated: 31/08/23
 *  Notes: 
    * Uses NTree and Room class
    * Uses enum called Compass which is used to indicate how list of child rooms are connected by index
/************************************************************************************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum Compass { N, S, E, W};      //Direction assigned to respective index
public class Map : MonoBehaviour
{
    //PROPERTIES
    [SerializeField]
    public NTree map;
    public GameObject Enemy;
    public GameObject Player;

    [Header("Pick central room")]
    public GameObject centralRoom;      //Manually set

    [Header("Room Count")]
    int totalRooms = 0;
    int normalRooms = 0;
    int puzzleRooms = 0;
    int secretRooms = 0;
    int safeRooms = 0;

    [Header("Other options")]
    public bool isFinalLvl;

    //Dropdown room to inspect and debug????


    //CONSTRUCTORS
    //Awake()?
    //https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    //https://gamedevbeginner.com/start-vs-awake-in-unity/
    public Map() 
    {
        try
        {
            map = new NTree(centralRoom);
            map.InsertTracker(Enemy);
            map.InsertTracker(Player);
            this.totalRooms = map.GetCount();
            //totalRooms = normalRooms = puzzleRooms = secretRooms = safeRooms = 0;
        }
        catch(NullReferenceException e)
        {
            Debug.LogWarning("Caught in Map(): " + e.Message);
        }
        catch(Exception e) 
        {
            Debug.LogError(e.Message);
        }
    }


    //GETTERS
    public NTree GetMap() { return map; }
    public int GetTotalRooms() { return totalRooms; }
    public int GetNormalRooms() { return normalRooms; }
    public int GetPuzzleRooms() { return puzzleRooms; }
    public int GetSecretRooms() { return secretRooms; }
    public int GetSafeRooms() { return safeRooms; }
    public bool CheckIsFinalLevel() { return isFinalLvl; }

    //SETTERS

    //METHODS
    /* define collision detection direction
     * insert room by collider overlap based on door direction
     */
    //Room type at the node
    public Room.RoomType GetRoomType(NTree.CustomNode node)
    {
        return node.GetData().GetComponent<Room.RoomType>();
    }

    //get room at?
    public Room GetRoom(int idx)
    {
         return this.map.FindNode(idx).GetData().GetComponent<Room>();
    }

     /* add tracker if enemy and player exist == marker (dynamic)
     * get tracker location
     */
    //add marker (fixed)
    public void AddMarker(GameObject gameObject)
    {
        this.map.InsertTracker(gameObject);
    }


     // update tracker if moved => you need to pass in direction
     //                             reference from player or enemy

     // update room status => passby reference if current overlap with player/enemy
    public void ChangeTeleport(int idx, bool state)
    {
        Room tmp = this.map.FindNode(idx).GetData().GetComponent<Room>();
        tmp.IsTeleportable = state;
    }
     




}

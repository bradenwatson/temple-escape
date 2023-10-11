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
using System.Linq;
using System.Net.Security;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public enum Compass { N, S, E, W};      //Global orientation assigned to respective index
public class Map : MonoBehaviour
{
    //PROPERTIES
    [SerializeField]
    private static NTree tree;
    //private BoxCollider Area;

    //Use FindObjectType<Enemy>
    [Header("Enemy Details")]
    public GameObject Enemy;
    public NTree.CustomNode SetEnemyAt;
    int EnemyInRoomID;
    RoomType EnemyAtType;

    NTree.CustomNode TargetRoom;
    int TargetRoomID;
    RoomType TargetRoomType;

    //Use FindObjectType<PLayer>
    [Header("Player Details")]
    public GameObject Player;
    public NTree.CustomNode SetPLayerAt;
    int PlayerInRoomID;
    RoomType PlayerAtType;

    [Header("Central room")]
    private GameObject centralRoom;      

    [Header("Room Count")]
    int totalRooms = 0;
    int normalRooms = 0;
    int puzzleRooms = 0;
    int secretRooms = 0;
    int safeRooms = 0;

    [Header("Other options")]
    public bool isFinalLvl;

    //CONSTRUCTORS
    public void Awake()
    {
        try
        {

            //GameObject[] test = FindObjectsOfType<GameObject>();
            //foreach (GameObject testObj in test) 
            //{
            //    if(testObj.CompareTag("Room"))
            //    {
            //        this.totalRooms++;
            //    }
            //}

            //Debug.Log("Rooms present = " + this.totalRooms);            //Gameobjects in awake method

            DetectRooms();

            //this.totalRooms = tree.GetCount();
            //totalRooms = normalRooms = puzzleRooms = secretRooms = safeRooms = 0;
            //Debug.Log("Map made in awake.");
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Caught in Map(): " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    //https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    //https://gamedevbeginner.com/start-vs-awake-in-unity/


    //GETTERS
    public NTree GetMap() { return tree; }
    public int GetTotalRooms() { return totalRooms; }
    public int GetNormalRooms() { return normalRooms; }
    public int GetPuzzleRooms() { return puzzleRooms; }
    public int GetSecretRooms() { return secretRooms; }
    public int GetSafeRooms() { return safeRooms; }
    public bool CheckIsFinalLevel() { return isFinalLvl; }

    //SETTERS

    //METHODS
    // define collision detection direction
    // if box collider overlaps with door > take center of box and center of door and calc vector
    // if vector matches direction respective to enum : assign door's room to current room
    // https://docs.unity3d.com/ScriptReference/RectInt.Overlaps.html
    // https://discussions.unity.com/t/detecting-overlapping-room-in-a-dungeon-generator/201543
    // https://forum.unity.com/threads/check-for-overlaps-among-many-simultaneously-spawned-gameobjects-solved.874141/


    



    //METHODS
    /***************************************************************************************/
    /* Method: TestOverlap (WIP)
     * Input: N/A
     * Output: isOverlapping(bool)
     * Purpose: Check if central node walls overlaps with other acdcessible walls within delta range
    /***************************************************************************************/

   
    private void DetectRooms()
    {
        //Get all rooms
        List<GameObject> rooms = GameObject.FindGameObjectsWithTag("Room").ToList<GameObject>();
        
        //https://discussions.unity.com/t/sorting-an-array-of-gameobjects-by-their-position/86640
        //Sort rooms from closest to center on x, then shortest distance from center
        rooms = rooms.OrderBy(rooms => Math.Abs(rooms.transform.position.x)).ThenBy(rooms => rooms.transform.position.sqrMagnitude).ToList();
        foreach (GameObject pos in rooms)
        {
            Debug.Log(pos.name + " = " + pos.transform.position);
        }
        

        //Set field total rooms
        this.totalRooms = rooms.Count;
        Debug.Log("Rooms present = " + this.totalRooms);



        //Sorted by x then magnitude => Check the following:
        // For every x, check shortest vector from to origin OR last inserted vector. Otherwise tmp store it away in next pass
        // Check overlapping walls 

        GameObject lastInserted = null;
        List<GameObject> unattached = new List<GameObject>();
        foreach (GameObject room in rooms)
        {
            if(tree == null)
            {
                //Assign central room
                this.centralRoom = rooms.First();
                lastInserted = this.centralRoom;
                tree = new NTree(this.centralRoom);
                Debug.Log("Root node set to central room at " + rooms.First().name + " = " + rooms.First().transform.position);
            }
            // Rooms after the root node
            else
            {
                //Get all containing same x element and insert up to max of 4 directions (based on doors)
                List<GameObject> roomsAtX = rooms.FindAll(i => Math.Abs(i.transform.position.x) == lastInserted.transform.position.x);
                roomsAtX = (List<GameObject>)roomsAtX.OrderBy(rooms => rooms.transform.position.sqrMagnitude);
                int maxDoors = 4;
                int count = 0;
                //Loop through roomsAtX and get up to max of 4 with shortest distance and 
                for(int i = 0; i < roomsAtX.Count && count <= maxDoors; i++) 
                { 

                }

            }
        }
        



        
        for (int i = 1; i < this.totalRooms; i++)
        {
            GameObject currRoom = rooms[i];
            noLinks.Add(currRoom);
            //Check if angle and distance matches from queue list
            float dist = Vector3.Distance(this.centralRoom.transform.position, currRoom.transform.position);


            float angle = Vector3.SignedAngle(Vector3.right, rooms[i].transform.position, Vector3.down);
            //Note: By default max angle [-180,180]
            bool north = angle >= 45 && angle <= 135;
            bool south = angle <= -45 && angle >= -135;
            bool east = angle < 45 && angle > -45;
            bool west = angle > 135 && angle < -135;

            if(north)
            {

            }

            else if(south)
            {

            }

            else if(east)
            { 
            }

            else if(west)
            {

            }

            else
            {
                throw new Exception("Something strange happened!");
            }

        }



        //Shift each direction based on room's scene rotation

    }


    

    

    //Room type at the node
    public string GetRoomType(int idx)
    {
        GameObject room = tree.FindNode(idx).GetData();
        return room.GetComponent<Room>().roomType.ToString();
    }

    //get room at?
    public Room GetRoom(int idx)
    {
        GameObject room = tree.FindNode(idx).GetData();
        return room.GetComponent<Room>();
    }

     /* add tracker if enemy and player exist == marker (dynamic)
     * get tracker location
     */
    //add marker (fixed)
    public void AddMarker(GameObject gameObject)
    {
        tree.InsertTracker(gameObject);
    }
    //Pass by ref https://forum.unity.com/threads/what-is-out-syntax-of-c-and-what-does-it-actually-do.404585/
    //Use ref in parameter to input variable and change it simultaneously 

    //Triggers https://www.codinblack.com/colliders-and-triggers-in-unity3d/
    /* Key trigger
     * https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
     * https://forum.unity.com/threads/key-press-on-trigger-enter.793650/
     * https://discussions.unity.com/t/event-trigger-for-key-pressed/153761/3
     * https://discussions.unity.com/t/passing-the-object-as-event-argument/60796
     * https://discussions.unity.com/t/event-trigger-component-how-to-access-input-data/127895
     */

    // update tracker if moved => you need to pass in direction
    //                             reference from player or enemy

    // update room status => passby reference if current overlap with player/enemy
    public void ChangeTeleport(int idx, bool state)
    {
        GameObject room = tree.FindNode(idx).GetData();
        room.GetComponent<Room>().IsTeleportable = state;
    }
     




}

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
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public enum Compass { N, S, E, W};      //Global orientation assigned to respective index
public class Map : MonoBehaviour
{
    //PROPERTIES
    [SerializeField]
    private static NTree map;
    private BoxCollider Area;

    //Use FindObjectType<Enemy>
    [Header("Enemy Details")]
    public GameObject Enemy;
    public NTree.CustomNode SetEnemyAt;
    private int EnemyInRoomID;
    private RoomType EnemyAtType;

    private NTree.CustomNode TargetRoom;
    private int TargetRoomID;
    private RoomType TargetRoomType;

    //Use FindObjectType<PLayer>
    [Header("Player Details")]
    public GameObject Player;
    public NTree.CustomNode SetPLayerAt;
    private int PlayerInRoomID;
    private RoomType PlayerAtType;

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

    //CONSTRUCTORS
    public void Awake()
    {
        try
        {
            map = new NTree(centralRoom);
            //map.InsertTracker(Enemy);
            //map.InsertTracker(Player);
            Area = centralRoom.GetComponent<BoxCollider>();
            Debug.Log(Area);
            this.totalRooms = map.GetCount();
            //totalRooms = normalRooms = puzzleRooms = secretRooms = safeRooms = 0;
            Debug.Log("Map made in awake.");
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
    /*
    public Map() 
    {
        
        try
        {
            map = new NTree(centralRoom);
            //map.InsertTracker(Enemy);
            //map.InsertTracker(Player);
            Area = centralRoom.GetComponent<BoxCollider>();
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
    */


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
    // define collision detection direction
    // if box collider overlaps with door > take center of box and center of door and calc vector
    // if vector matches direction respective to enum : assign door's room to current room
    
    
    
    public void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            Debug.Log("Collided with " + other.name);
        }

        //If the map's central room collider touches a room 
        if(other.tag == "Room") 
        {
            Debug.Log("Collided with room");


            //Get walls in the central room that are accessible
            GameObject[] walls = centralRoom.GetComponentsInChildren<GameObject>();
            foreach(GameObject wall in walls)
            {
                if (wall.tag == "Accessible")
                {
                    //Pass the wall's position and the collider to the 
                    //insert room in respective direction and index
                    Vector3 position = wall.transform.localPosition;
                    InsertRoom(centralRoom, other, position);
                }
            }
            //Get Collision vector when triggered with central room's collider
            
        }
    }

    //Wrapper RotationChanged()

    
    private void SetToGlobalOrientation(GameObject obj)
    {
        int maxDirections = 4;
        if(obj.GetComponent<Room>() == null)
        {
            throw new NullReferenceException("Room component missing from object.");
        }

        NTree.CustomNode objNode = map.FindNode(obj.GetComponent<Room>().RoomID);
        //Check node capacity has been set
        if (objNode.GetNodeLimit() <= 0 || objNode.GetNodeLimit() > maxDirections) 
        {
            throw new NotSupportedException("List capacity needs to be set correctly.");
        }
        else
        {
            objNode.SetNodeLimit(maxDirections);
            int angle = 90;
            int turn = (int)obj.GetComponent<Quaternion>().y / angle;
            var queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
            switch (turn)
            {
                case -2:case 2: //Shift all child nodes by 2
                    //queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
                    queue.Enqueue(queue.Dequeue());
                    queue.Enqueue(queue.Dequeue());
                    objNode.SetChildren(queue.ToList<NTree.CustomNode>());
                    Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
                    break;
                case -1:
                    //var queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
                    queue.Enqueue(queue.Dequeue());
                    objNode.SetChildren(queue.ToList<NTree.CustomNode>());
                    Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
                    break;
                case 0:
                    Debug.Log("No change to Room" + objNode.GetIndex().ToString());
                    break;
                case 1:
                    queue.Enqueue(queue.Dequeue());
                    queue.Enqueue(queue.Dequeue());
                    queue.Enqueue(queue.Dequeue());
                    objNode.SetChildren(queue.ToList<NTree.CustomNode>());
                    Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
                    break;
                default:
                    throw new NotSupportedException("Unable to set rotation.");
            }
        }
        objNode.GetChildren().TrimExcess();
    }
    

    // insert room by collider overlap based on door direction
    private void InsertRoom(GameObject currObj, Collider other, Vector3 location)
    {
        //Check both objects rotation and assign to global orientation
        //queue in the respective order

        int index = -1;
        //Reduce to vector to magnitude of 1
        location.Normalize();
        //Get node key that contains current object
        //Insert the room or the door's room (children) to the central room (node)
        //if it has not been occupied
        if (location == Vector3.forward) //N
        {
            index = (int)Compass.N;
            //Insert the other at the node's key
            Debug.Log(index);
        }
        else if (location == Vector3.back) //S
        {
            index = (int)Compass.S;
            Debug.Log(index);
        }
        else if (location == Vector3.right) //E
        {
            index = (int)Compass.E;
            Debug.Log(index);
        }
        else if (location == Vector3.left) //W
        {
            index = (int)Compass.W;
            Debug.Log(index);
        }
        else
        {
            throw new NotSupportedException("Vector out of line.");
        }

        SetToGlobalOrientation(currObj);
        SetToGlobalOrientation(other.gameObject);
    }

    //Room type at the node
    public string GetRoomType(int idx)
    {
        GameObject room = map.FindNode(idx).GetData();
        return room.GetComponent<Room>().roomType.ToString();
    }

    //get room at?
    public Room GetRoom(int idx)
    {
        GameObject room = map.FindNode(idx).GetData();
        return room.GetComponent<Room>();
    }

     /* add tracker if enemy and player exist == marker (dynamic)
     * get tracker location
     */
    //add marker (fixed)
    public void AddMarker(GameObject gameObject)
    {
        map.InsertTracker(gameObject);
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
        GameObject room = map.FindNode(idx).GetData();
        room.GetComponent<Room>().IsTeleportable = state;
    }
     




}

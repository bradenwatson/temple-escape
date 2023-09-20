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


    //Version 2
    /*
     * Get central room and wall positions
     * Since room center are snapped in the cartesian plane, this means neighbouring rooms will share same axis
     * Can determine the the next room, by comparing the axis that has the smallest difference to the centre
     * So if we project a line in steps in 4 directions and if that line intersects with another rooms center, add to current room
     * then we start 1 direction (in the queue) with the next node and repeat prev steps using projectionasnd add to the remaning queue for 
     */



    //METHODS
    /***************************************************************************************/
    /* Method: TestOverlap (WIP)
     * Input: N/A
     * Output: isOverlapping(bool)
     * Purpose: Check if central node walls overlaps with other acdcessible walls within delta range
    /***************************************************************************************/

    //https://www.reddit.com/r/roguelikedev/comments/7xmxt2/resources_for_creating_a_map_of_randomly/

        //Consecutive detection-too slow
        //Get alls the walls 
        //Get get their center
        //Project line perpendicular to the wall center for all 
        //Queue accessible rooms in order of orientation (code set up in order)
        //assign rooms to central room and each of the detected rooms add central room on prev in the correct global orientation (not local rotation)
        //Get next room in queue
        //push new rooms at front of queue in order of orientation
        //repeat until queue is empty


        //By per axis per room detection-tricky
        //Project vertical and horizontal lines from center
        //Store new all accessible rooms into respective list of queues based on orientation
            //if new room has no room perpendicular to the current axis ignore
            //otherwise enqueue room to the front in the respective list of queues (Test enqueue at start and enqueue at end)
            //(Test try simultaneously all at the same time)
            //test until all the queues are empty



        //By per axis detection-tricky
        //Project vertical and horizontal lines lines from center from 2 axis and count number of time triggers in each room center
        //Count which direction is least from the 2 axis
        //Using the chosen axis, create 2 queues as positive or negative side
        //each element stored (steps in the axis) is a list with all the rooms stored from negative to positive (similar to index based but includes negatives)
            //optional sort by the absolute displacement from the central axis (but do not change vector)
        //do the same with the other axis
            //for every element in the new axis check each element in the queue of the other axis
                //if they share common axis on the first element on the new axis it means the previous rooms (at start is center room) is connected
                    //add rooms that are common together
                //otherwise located away from center
                    //create list of queues these rooms in 2 queues positive and negative
                        //then run a pass on each of these once and check if on of the walls in the direciton of axis istouching and accessible
                
                //on the next step (away from the first step)
                //repeat above for any connected through the central room and check if they are connected to other outside rooms by comparing axis from any order front or back
                    //test connected if another line from the other list on the axis beam if first intersect of centre or wall matches the current central
                //otherwise for all the other rooms found assignt o the list of queues respectively
                //compare the last queue with its previous and if compare the room axis perpendicular to the current for both 
                    //if they match, then those rooms are connected (and then pop the queue) 
                    //(NOTE: there is a case where the axis through the center contains nodes which is shared by both sides of list of queues(pos and neg)
                        //to fix this both must have the central rooms in the list of queues
            
            
            //when reached end 
                //check number of rooms
        
        //NOTE:might need to track order found, or found respective to their orientation 
            //Question how to reassemble into tree now that each room has its connection?   Maybe a queue in the order of orientation for every room
                

            //Note if there are multiple detected on the new axis and close together, do not assume they are



    /* TLDR:
     * Project 2 lines parallel to wall from center of central room along same axis and stop at every new point of contact/centers
     * For each step loo through 2 other parallel lines and queue list of room when both lines intersect at one's center
     * 
     */
     

    //Version3 - simpler
    //what if sort distance from centre and re-arrange and angle and nearest neighbour by shortest distance
    //split by semi circle, get room with vector and angle from list sorted, then use a ref point as that so next one get shortest distance
    private void DetectRooms()
    {
        //Get all rooms
        List<GameObject> rooms = GameObject.FindGameObjectsWithTag("Room").ToList<GameObject>();
        //https://discussions.unity.com/t/sorting-an-array-of-gameobjects-by-their-position/86640
        //rooms = rooms.OrderBy(rooms => rooms.transform.position.sqrMagnitude).ToList();     //sort rooms by shortest distance from origin
        rooms = rooms.OrderBy(rooms => Math.Abs(rooms.transform.position.x)).ToList();
        //foreach(GameObject pos in rooms) 
        //{
        //    Debug.Log(pos.name + " = " + pos.transform.position);
        //}
        this.centralRoom = rooms.First();
        tree = new NTree(this.centralRoom);
        if(tree.GetRoot() != null)
        {
            Debug.Log("Root node set to central room at " + rooms.First().name + " = " + rooms.First().transform.position);
        }
        this.totalRooms = rooms.Count;
        Debug.Log("Rooms present = " + this.totalRooms);

        List<GameObject> noLinks = new List<GameObject>();
        for (int i = 1; i < this.totalRooms; i++)
        {
            GameObject currRoom = rooms[i];
            noLinks.Add(currRoom);
            //Check if angle and distance matches from queue list
            float dist = Vector3.Distance(this.centralRoom.transform.position, currRoom.transform.position);


            float angle = Vector3.SignedAngle(Vector3.right, rooms[i].transform.position, Vector3.down);
            bool north = angle >= 45 && angle <= 135;
            bool south = angle <= -45 && angle >= -135;
            bool east = angle < 45 && angle > -45;
            bool west = angle > 135 || angle < -135;

            if(north)
            {

            }

            if(south)
            {

            }

            if(east)
            { 
            }

            if(west)
            {

            }

        }



        //Switch directions via NE, NW etc rounding eg >45 or <135 is N to assign direction, then match distance too
        //otherwise rotate with magnitude to the angle and then calc its rooms shortest distance from current placement to previous rooms and then assign

    }


    
    private void SetToGlobalOrientation(GameObject obj)
    {
        //Count accessible walls and change their orientation

        //int maxDirections = 4;
        //if(obj.GetComponent<Room>() == null)
        //{
        //    throw new NullReferenceException("Room component missing from object.");
        //}

        //NTree.CustomNode objNode = tree.FindNode(obj.GetComponent<Room>().RoomID);
        ////Check node capacity has been set
        //if (objNode.GetNodeLimit() <= 0 || objNode.GetNodeLimit() > maxDirections) 
        //{
        //    throw new NotSupportedException("List capacity needs to be set correctly.");
        //}
       
        //objNode.SetNodeLimit(maxDirections);
        //int angle = 90;
        //int turn = (int)obj.GetComponent<Quaternion>().y / angle;
        //Queue<NTree.CustomNode> queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
        //switch (turn)
        //{
        //    case -2:case 2: //Shift all child nodes by 2
        //        //queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
        //        queue.Enqueue(queue.Dequeue());
        //        queue.Enqueue(queue.Dequeue());
        //        objNode.SetChildren(queue.ToList<NTree.CustomNode>());
        //        Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
        //        break;
        //    case -1:
        //        //var queue = new Queue<NTree.CustomNode>(objNode.GetChildren());
        //        queue.Enqueue(queue.Dequeue());
        //        objNode.SetChildren(queue.ToList<NTree.CustomNode>());
        //        Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
        //        break;
        //    case 0:
        //        Debug.Log("No change to Room" + objNode.GetIndex().ToString());
        //        break;
        //    case 1:
        //        queue.Enqueue(queue.Dequeue());
        //        queue.Enqueue(queue.Dequeue());
        //        queue.Enqueue(queue.Dequeue());
        //        objNode.SetChildren(queue.ToList<NTree.CustomNode>());
        //        Debug.Log("Room " + objNode.GetIndex().ToString() + "adjusted.");
        //        break;
        //    default:
        //        throw new NotSupportedException("Unable to set rotation.");
        //}
        //objNode.GetChildren().TrimExcess();
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

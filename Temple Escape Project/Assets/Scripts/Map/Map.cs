/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: User friendly map tool without needing to understand trees. Once a central room is 
 *          defined as the root, the class will connect all the rooms as a tree which can be 
 *          used to traverse and navigate and perform calculations. The map is designed to be 
 *          shared amongst the enemy and the player and interacting with the game manager.
 *  Last updated: 17/10/23
 *  Notes: 
    * Uses NTree and Room class
    * Uses enum called Compass which is used to indicate how list of child rooms are connected by index
/************************************************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Compass { N, S, E, W};      //Global orientation assigned to respective index
public class Map : MonoBehaviour
{
    //PROPERTIES
    [SerializeField]
    private static NTree tree;

    //Use FindObjectType<Enemy>
    [Header("Enemy Details")]
    public GameObject Enemy;
    public CustomNode SetEnemyAt;
    int EnemyInRoomID;
    RoomType EnemyAtType;

    CustomNode TargetRoom;
    int TargetRoomID;
    RoomType TargetRoomType;

    //Use FindObjectType<PLayer>
    [Header("Player Details")]
    public GameObject Player;
    public CustomNode SetPLayerAt;
    int PlayerInRoomID;
    RoomType PlayerAtType;

    [Header("Central room")]
    private CustomNode centralRoom;      

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
            //tree = gameObject.AddComponent<NTree>();
            DetectRooms();
            //Debug.Log("Map made in awake.");
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Caught in Map(): " + e);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
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
        //Get all rooms by node. This list is temporary, remove elements from list (not destroy object) when room is linked
        List<CustomNode> rooms = GameObject.FindObjectsOfType<CustomNode>().ToList<CustomNode>();
        
        //https://discussions.unity.com/t/sorting-an-array-of-gameobjects-by-their-position/86640
        //Sort rooms from closest to center on x, then shortest distance from center
        rooms = rooms.OrderBy(rooms => Math.Abs(rooms.transform.position.x)).ThenBy(rooms => rooms.transform.position.sqrMagnitude).ToList();
        //Tranform all rooms fast: https://docs.unity3d.com/ScriptReference/Transform.TransformDirections.html


        //Set field total rooms
        this.totalRooms = rooms.Count;
        Debug.Log("Rooms present = " + this.totalRooms);
        
        //Assign central room
        this.centralRoom = rooms.First();

        //Create tree with central room
        if (tree == null)
        {
            tree = gameObject.AddComponent<NTree>();
        }

        tree.SetRoot(this.centralRoom);
        Debug.Log("Root node set to central room at (" + rooms.First().name + ") @ " + rooms.First().transform.position);

        for (int i = 1; i < this.totalRooms; i++) 
        {
            //Transform direction of every room individually : https://docs.unity3d.com/ScriptReference/Transform.TransformDirection.html
            CustomNode currRoom = rooms[i];
            //Check if rooms contain number of passages

            //Check if room currently connects to any in the list. If disconnected room becomes fully connected, remove from list.
            //Add current to disconnected if still has connections remaining
            for (int j = 0; j < i; j++) 
            {
                CustomNode prevRoom = rooms[j];

                //If that room's children does not contain null then remove from the list
                if (prevRoom.GetChildren().Contains(null))
                { 

                    //Check the following: angle & contact between current and prev room//Regardless of left or right side of the centre and world rotation
                    //Only insertion in the correct following order N,S,E,W
                    // https://docs.unity3d.com/ScriptReference/Vector3.Dot.html
                    //do dot product of 4 directions (Change later once know how many doors but assume 4 for now)
                    //Do dot product of direction upon displacement vector between current and previous room
                    //Some will have direction, but some will not contact
                    //Dot product test-if truly connected then they are alteat beside each other so the dot product is 0
                    //Obtain dot of 1 mean in same direction

                    
                    
                    
                    //Debug.Log($"C:{currRoom.name} ({currRoom.transform.position})\tP:{prevRoom.name} ({prevRoom.transform.position})");



                    //Conditions of Inserting node leaf:
                    //(1)If the mid point magnitude ~ magnitude of half the rooms size (centre to edge = 1/2 perpendicular distance from center >> Use box collider size)
                    //(2)Share the same door == Check the door object's relationship
                    //(3)Wall Contact

                    Vector3 displacement = prevRoom.transform.position - currRoom.transform.position;
                    Debug.Log($"Displacement = ({displacement})");
                    //Intersections do work but may need more than 1 condition
                    bool adjacent = currRoom.GetData().GetComponent<BoxCollider>().bounds.Intersects(prevRoom.GetData().GetComponent<BoxCollider>().bounds);    

                    Debug.Log($"(i:{i},j:{j}) Adjacent rooms [{adjacent}]: C({currRoom.name}[{currRoom.transform.position}]) <=> P({prevRoom.name}[{prevRoom.transform.position}])");
                    if (adjacent)
                    {
                        //Debug.Log($"Adjacent rooms: C({currRoom.name}) <=> P({prevRoom.name})");

                        bool north = Vector3.Dot(displacement, Vector3.forward) > 0;
                        bool south = Vector3.Dot(displacement, Vector3.back) > 0;
                        bool east = Vector3.Dot(displacement, Vector3.right) > 0;
                        bool west = Vector3.Dot(displacement, Vector3.left) > 0;

                        //Because capacity has been set, it is possible to select index
                        if (north)
                        {
                            Debug.Log("North");
                            //Connect forwarda and backwards
                            //Backward
                            tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.N);
                            //Forward 
                            tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.S);
                        }

                        else if (south)
                        {
                            Debug.Log("South");
                            //Connect forwarda and backwards
                            //Backward
                            tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.S);
                            //Forward 
                            tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.N);
                        }

                        else if (east)
                        {
                            Debug.Log("East");
                            //Connect forwarda and backwards
                            //Backward
                            tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.E);
                            //Forward 
                            tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.W);
                        }

                        else if (west)
                        {
                            Debug.Log("West");
                            //Connect forwarda and backwards
                            //Backward
                            tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.W);
                            //Forward 
                            tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.E);
                        }

                        else
                        {
                            throw new Exception("Something strange happened!");
                        }
                    }

                }

            }

        }
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
        tree.InsertTracker();
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
        //room.GetComponent<Room>().IsTeleportable = state;
    }


}

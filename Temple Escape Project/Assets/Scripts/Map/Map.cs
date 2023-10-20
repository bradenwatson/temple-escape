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
        CreateRooms();
    }


    


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

    /*********************************************************************************************************
    * Method: TestOverlap 
    * Input: N/A
    * Output: N/A
    * Purpose: Connects rooms beginning from the center and spanning outward horizontally
    *********************************************************************************************************/

    /************************
     * CORE METHOD
     ***********************/
    private void CreateRooms()
    {
        try
        {
            List<CustomNode> rooms = null;
            //Get all rooms by node. This list is temporary for inserting nodes.
            if (GameObject.FindObjectOfType<CustomNode>() != null)
            {
                rooms = GameObject.FindObjectsOfType<CustomNode>().ToList<CustomNode>();
            }
            else if (GameObject.FindGameObjectWithTag("Room") != null)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
                {
                    CustomNode node = obj.GetComponent<CustomNode>();
                    if (node != null)
                    {
                        rooms.Add(node);
                    }
                }
            }
            else
            {
                throw new NullReferenceException("Rooms is not present in the scene. Ensure the gameObject has 'Room' script.");
            }


            //https://discussions.unity.com/t/sorting-an-array-of-gameobjects-by-their-position/86640
            //Sort rooms from closest to center on x, then shortest distance from center
            rooms = rooms.OrderBy(rooms => Math.Abs(rooms.transform.position.x)).ThenBy(rooms => rooms.transform.position.sqrMagnitude).ToList();
            //Tranform all rooms fast: https://docs.unity3d.com/ScriptReference/Transform.TransformDirections.html


            //Set Map field total rooms
            this.totalRooms = rooms.Count;
            Debug.Log("Rooms present = " + this.totalRooms);        //MODIFY FOR FUNCTION BELOW

            /**** -SUB METHOD- ****/
            CountTotalRoomType(rooms);
            /**********************/


            //Assign central room
            this.centralRoom = rooms.First();

            //Create tree with central room
            if (tree == null)
            {
                tree = gameObject.AddComponent<NTree>();
            }

            tree.SetRoot(this.centralRoom);
            Debug.Log("Root node set to central room at (" + rooms.First().name + ") @ " + rooms.First().transform.position);


            /**** -SUB METHOD- ****/
            IterateRooms(rooms);
            /**********************/


            /**** -SUB METHOD- ****/
            //(DEBUGGING ONLY) : Do one last check if all rooms are full
            VerifyConnections(rooms);
            /**********************/
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning($"Caught in Map [CreateRooms()]:  {e}.");
        }
        catch (Exception e) 
        {
            Debug.LogWarning($"Caught in Map [CreateRooms()]:  {e}.");
        }
    }


    /************************
    * SUB METHOD
    ***********************/
    private void CountTotalRoomType(List<CustomNode> rooms)          //WIP
    {

    }

    /************************
     * SUB METHOD
     ***********************/
    private void IterateRooms(List<CustomNode> rooms)
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            //Transform direction of every room individually : https://docs.unity3d.com/ScriptReference/Transform.TransformDirection.html
            CustomNode currRoom = rooms[i];
            //Check if rooms contain number of passages

            //Check if room currently connects to any in the list.
            //Add current to disconnected if still has connections remaining
            for (int j = 0; j < i; j++)
            {
                CustomNode prevRoom = rooms[j];

                //If that room's children does not contain null ignore but do not delete reference.
                if (prevRoom.GetChildren().Count(x => x != null) <= prevRoom.GetData().GetComponent<Room>().PassageCount)
                {
                    /****************************** ------SUB METHODS------ ****************************/
                    if (IsAdjacentRoom(prevRoom, currRoom)) {   InsertByOrientation(prevRoom, currRoom);    }
                    /***********************************************************************************/
                }
            }
        }
    }
        /************************
         * SUB METHOD
         ***********************/
        //FUTURE ADD DOORS TOO
        private bool IsAdjacentRoom(CustomNode prevRoom, CustomNode currRoom, float percentMin=0.9f)
        {
            //Conditions of Inserting node leaf:
            //(1)Wall Contact
            //(2)If the mid point magnitude ~ magnitude of half the rooms size (centre to edge = 1/2 perpendicular distance from center >> Use box collider size)
            //(Optional) Share the same door == Check the door object's relationship
            //(3) Check perpendicular direction within error limit


            //CURRENTLY ISSUES WITH CORNERS (EG ROOM2WAY + ROOM4WAY) >> AT THE CORNER IT CAN INTERSECT IN 2 DIRECTIONS TECHNICALLY
            ////Intersections do work but may need more than 1 condition
            bool state1 = currRoom.GetData().GetComponent<BoxCollider>().bounds.Intersects(prevRoom.GetData().GetComponent<BoxCollider>().bounds);


            //Test if the extends distance of currRoom is ~90 of the half of the displacement length
            /****************************** ------SUB METHOD------ ****************************/
            Vector3 displacement = DisplacementFromCurrentRoom(prevRoom, currRoom);
            /***********************************************************************************/

            Vector3 toCurrCenter = currRoom.GetData().GetComponent<BoxCollider>().bounds.extents;
            
            bool adjacent = state1;
            //Check if half of room size is within the half of the displacement vector which can be done by comparing lengths based on their position as well
            if (!adjacent)
            {
                bool state2 = (toCurrCenter.magnitude > (percentMin * displacement.magnitude)) && (toCurrCenter.magnitude <= displacement.magnitude);
                adjacent = state1 || state2;
                Debug.Log($"Exact = {state1}     Approx = {state2}       disp = ({displacement}) |{displacement.magnitude}      extends = ({toCurrCenter}) |{toCurrCenter.magnitude}");
            }

            //Debug.Log($"(i:{i},j:{j}) Adjacent rooms [{adjacent}]: C({currRoom.name}[{currRoom.transform.position}]) <=> P({prevRoom.name}[{prevRoom.transform.position}])");
            return adjacent;
        }

        /************************
         * SUB METHOD
         ***********************/
        //FUTURE ADD DOORS TOO
        private void InsertByOrientation(CustomNode prevRoom, CustomNode currRoom, int error=5)
        {
            //Use angles to differentiate between adjacent rooms including contact with corners (reference from current room center, anglemeasured from horizontal)
            /****************************** ------SUB METHOD------ ****************************/
            Vector3 displacement = DisplacementFromCurrentRoom(prevRoom, currRoom);
            /***********************************************************************************/

            float angle = Vector3.SignedAngle(Vector3.right, displacement, Vector3.down);
            //Debug.Log($"D=({displacement}):     C({currRoom.name}[{currRoom.transform.position}]) <{angle} -> P({prevRoom.name}[{prevRoom.transform.position}])");

            bool north = (angle >= (90 - error)) && (angle <= (90 + error));
            bool south = (angle >= (-90 - error)) && (angle <= (-90 + error));
            bool east = (angle >= (-error)) && (angle <= (error));
            bool west = (angle >= (180 - error) && angle <= 180) || (angle >= -180 && angle <= (-180 + error));


            //Its is possible the tree does not have the currNode inserted yet so insert from previous, THEN the currNode (WORKING)
            if (north)
            {
                Debug.Log($"D=({displacement}):   P({currRoom.name}[{currRoom.transform.position}])   >>  (North) >>  C({prevRoom.name}[{prevRoom.transform.position}])");
                tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.S);
                tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.N);
            }
            else if (south)
            {

                Debug.Log($"D=({displacement}):   P({currRoom.name}[{currRoom.transform.position}])   >>  (South) >>  C({prevRoom.name}[{prevRoom.transform.position}])");
                tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.N);
                tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.S);
            }
            else if (east)
            {

                Debug.Log($"D=({displacement}):   P({currRoom.name}[{currRoom.transform.position}])   >>  (East) >>  C({prevRoom.name}[{prevRoom.transform.position}])");
                tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.W);
                tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.E);
            }
            else if (west)
            {

                Debug.Log($"D=({displacement}):   P({currRoom.name} [ {currRoom.transform.position} ])   >>  (West) >>  C( {prevRoom.name} [ {prevRoom.transform.position}])");
                tree.InsertNodeAt(prevRoom, currRoom, (int)Compass.E);
                tree.InsertNodeAt(currRoom, prevRoom, (int)Compass.W);
            }
            else
            {
                //Debug.Log($"D=({displacement}):   P({prevRoom.name}[{prevRoom.transform.position}])   --None--  C({currRoom.name}[{currRoom.transform.position}])");
                Debug.Log($"D=({displacement}):   P({currRoom.name}[{currRoom.transform.position}])   --None--  C({prevRoom.name}[{prevRoom.transform.position}])");
            }
        }

            /************************
             * SUB METHOD
             ***********************/
            private Vector3 DisplacementFromCurrentRoom(CustomNode prevRoom, CustomNode currRoom)
            {
                return prevRoom.transform.position - currRoom.transform.position; 
            }


    /************************
     * SUB METHOD
     ***********************/
    private void VerifyConnections(List<CustomNode> rooms)
    {
        Debug.Log($"Rooms all connected = {rooms.All(x => x.GetChildren().Count(x => x != null) == x.GetData().GetComponent<Room>().PassageCount)}");

        //(DEBUGGING ONLY) : ITERATE EVERY ELEMENT 
        /*
        foreach (CustomNode room in rooms)
        {
            int count = room.GetChildren().Count(x => x != null);
            int max = room.GetData().GetComponent<Room>().PassageCount;
            //if (count == max)
            //{
            //    Debug.Log($"{room.name} Counted = {count}   Max = {max}");
            //}
            //else
            //{

            //}
            Debug.Log($"{room.name} ({count == max}): Counted = {count}   Max = {max}");
        }
        */
    }


    /*********************************************************************************************************
    ************************************     END     OF      METHOD     **************************************
    *********************************************************************************************************/
















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

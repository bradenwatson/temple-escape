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
    CustomNode centralRoom;      

    [Header("Room Count")]
    int totalRooms = 0;
    int normalRooms = 0;
    int puzzleRooms = 0;
    int secretRooms = 0;
    int safeRooms = 0;
    int exitRooms = 0;

    [Header("Other options")]
    public bool isFinalLvl;




    //CONSTRUCTORS
    public void Awake()
    {
        CreateRooms();
        foreach(CustomNode node in centralRoom.GetChildren())
        {
            if (node != null) 
            {
                Debug.Log($"Root children: {node.GetData().name}");
            }
        }

        //for (int i = 0; i < this.totalRooms; i++)
        //{
        //    Debug.Log($"Node({i}):\t{tree.FindNode(i).GetData().name}");
        //}
        string test = tree.OutputString();
        Debug.Log(test);
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
            //Get all rooms by node. This list is temporary for inserting nodes.
            List<GameObject> rooms = GameObject.FindGameObjectsWithTag("Room").ToList();
            if(rooms == null)
            {
                throw new NullReferenceException("Rooms is not present in the scene. Ensure the gameObject has 'Room' script.");
            }


            //https://discussions.unity.com/t/sorting-an-array-of-gameobjects-by-their-position/86640
            //Sort rooms from closest to center on x, then shortest distance from center
            rooms = rooms.OrderBy(rooms => Math.Abs(rooms.transform.position.x)).ThenBy(rooms => rooms.transform.position.sqrMagnitude).ToList();
            //Tranform all rooms fast: https://docs.unity3d.com/ScriptReference/Transform.TransformDirections.html


            /**** -SUB METHOD- ****/
            CountTotalRoomType(rooms);
            /**********************/


            

            //Create tree with central room
            if (tree == null)
            {
                tree = new NTree(rooms.First());
                //Assign central room
                this.centralRoom = tree.GetRoot();
                Debug.Log("Root node set to central room at (" + rooms.First().name + ") @ " + rooms.First().transform.position);
            }


            /**** -SUB METHOD- ****/
            IterateRooms(rooms);
            /**********************/


            /**** -SUB METHOD- ****/
            //(DEBUGGING ONLY) : Do one last check if all rooms are full
            //VerifyConnections();
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
    private void CountTotalRoomType(List<GameObject> rooms)          //WIP
    {
        this.totalRooms += rooms.Count;
        foreach(GameObject obj in rooms) 
        {
            RoomType roomType = obj.GetComponent<Room>().roomType;
            switch(roomType)
            {
                case RoomType.Normal:
                    this.normalRooms += 1;
                    break;

                case RoomType.Puzzle: 
                    this.puzzleRooms += 1;
                    break;

                case RoomType.Safe: 
                    this.safeRooms += 1;
                    break;

                case RoomType.Secret: 
                    this.secretRooms += 1;
                    break;

                case RoomType.Exit: 
                    this.exitRooms += 1;
                    break;
                default:
                    throw new Exception("Something strange happened.");      
            }
        }
    }

    /************************
     * SUB METHOD
     ***********************/
    private void IterateRooms(List<GameObject> rooms)
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            //Transform direction of every room individually : https://docs.unity3d.com/ScriptReference/Transform.TransformDirection.html
            int maxRooms = 4;
            GameObject currRoom = rooms[i];
            CustomNode currNode = new CustomNode(currRoom, maxRooms);
            //Check if rooms contain number of passages

            //Check if room currently connects to any in the list.
            //Add current to disconnected if still has connections remaining
            for (int j = 0; j < i; j++)
            {
                GameObject prevRoom = rooms[j];
                CustomNode prevNode = new CustomNode(prevRoom, maxRooms);
                if (prevNode.GetChildren().Count(x => x != null) <= prevRoom.GetComponent<Room>().PassageCount)
                {
                    /****************************** ------SUB METHODS------ ****************************/
                    if (IsAdjacentRoom(prevNode, currNode)) { InsertByOrientation(prevNode, currNode); }
                    /***********************************************************************************/
                }
                else
                {
                    rooms.RemoveAt(j);
                }
            }
        }
    }
        /************************
         * SUB METHOD
         ***********************/
        //FUTURE ADD DOORS TOO
        private bool IsAdjacentRoom(CustomNode prevNode, CustomNode currNode, float percentMin=0.9f)
        {
            //Conditions of Inserting node leaf:
            //(1)Wall Contact
            //(2)If the mid point magnitude ~ magnitude of half the rooms size (centre to edge = 1/2 perpendicular distance from center >> Use box collider size)
            //(Optional) Share the same door == Check the door object's relationship
            //(3) Check perpendicular direction within error limit


            //CURRENTLY ISSUES WITH CORNERS (EG ROOM2WAY + ROOM4WAY) >> AT THE CORNER IT CAN INTERSECT IN 2 DIRECTIONS TECHNICALLY
            ////Intersections do work but may need more than 1 condition
            bool state1 = currNode.GetData().GetComponent<BoxCollider>().bounds.Intersects(prevNode.GetData().GetComponent<BoxCollider>().bounds);


            //Test if the extends distance of currRoom is ~90 of the half of the displacement length
            /****************************** ------SUB METHOD------ ****************************/
            Vector3 displacement = DisplacementFromCurrentRoom(prevNode, currNode);
            /***********************************************************************************/

            Vector3 toCurrCenter = currNode.GetData().GetComponent<BoxCollider>().bounds.extents;
            
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
        private void InsertByOrientation(CustomNode prevNode, CustomNode currNode, int error=5)
        {
            //Use angles to differentiate between adjacent rooms including contact with corners (reference from current room center, anglemeasured from horizontal)
            /****************************** ------SUB METHOD------ ****************************/
            Vector3 displacement = DisplacementFromCurrentRoom(prevNode, currNode);
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
                Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (North) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
                CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.S);
                CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.N);
                Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
            }
            else if (south)
            {

                Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (South) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
                CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.N);
                CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.S);
                Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
            }
            else if (east)
            {

                Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (East) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
                CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.W);
                CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.E);
                Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
            }
            else if (west)
            {

                Debug.Log($"D=({displacement}):   P({currNode.GetData().name} [ {currNode.GetData().transform.position} ])   >>  (West) >>  C( {prevNode.GetData().name} [ {prevNode.GetData().transform.position}])");
                CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.E);
                CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.W); 
                Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
            }
            else
            {
                //Debug.Log($"D=({displacement}):   P({prevRoom.name}[{prevRoom.transform.position}])   --None--  C({currRoom.name}[{currRoom.transform.position}])");
                Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   --None--  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
            }
        }

            /************************
             * SUB METHOD
             ***********************/
            private Vector3 DisplacementFromCurrentRoom(CustomNode prevNode, CustomNode currNode)
            {
                return prevNode.GetData().transform.position - currNode.GetData().transform.position; 
            }


    /************************
     * SUB METHOD
     ***********************/
    private void VerifyConnections()
    {
        for(int i = 0; i < this.totalRooms; i+=2) 
        {
            //Debug.Log($"Node({i}):\t{tree.CheckNodeExists(i)}");
            Debug.Log($"Node({i}):\t{tree.FindNode(i).GetData().name}");
        }
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

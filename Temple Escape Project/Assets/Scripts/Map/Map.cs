/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: User friendly map tool without needing to understand trees. Once a central room is 
 *          defined as the root, the class will connect all the rooms as a tree which can be 
 *          used to traverse and navigate and perform calculations. The map is designed to be 
 *          shared amongst the enemy and the player and interacting with the game manager.
 *  Last updated: 22/10/23
 *  Notes: 
    * Uses NTree and Room class
    * Uses enum called Compass which is used to indicate how list of child rooms are connected by index
/************************************************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Core.Editor;
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
    [SerializeField]
    public CustomNode SetEnemyAt;
    int EnemyInRoomID;
    RoomType EnemyAtType;

    CustomNode TargetRoom;
    int TargetRoomID;
    RoomType TargetRoomType;

    //Use FindObjectType<PLayer>
    [Header("Player Details")]
    public GameObject Player;
    [SerializeField]
    public CustomNode SetPLayerAt;
    int PlayerInRoomID;
    RoomType PlayerAtType;

    
    [Header("Central room")]
    [SerializeField]
    CustomNode centralRoom;      

    [Header("Room Count")]
    [SerializeField]
    int totalRooms = 0;
    [SerializeField]
    int normalRooms = 0;
    [SerializeField]
    int puzzleRooms = 0;
    [SerializeField]
    int secretRooms = 0;
    [SerializeField]
    int safeRooms = 0;
    [SerializeField]
    int exitRooms = 0;

    [Header("Other options")]
    [SerializeField]
    public bool isFinalLvl;


    void Awake()
    {
        //Check if Map is present in the scene
        Map mapComponent = GetComponent<Map>();
        if (mapComponent == null)
        {
            throw new NullReferenceException("Map has not been instantiated in Awake().");
        }

        //Connect all the rooms
        CreateMap();
    }



    //GETTERS
    public NTree GetMap() { return tree; }
    public int GetTotalRooms() { return totalRooms; }
    public int GetNormalRooms() { return normalRooms; }
    public int GetPuzzleRooms() { return puzzleRooms; }
    public int GetSecretRooms() { return secretRooms; }
    public int GetSafeRooms() { return safeRooms; }

    public int GetExitRooms() { return exitRooms; }
    public bool CheckIsFinalLevel() { return isFinalLvl; }





    //METHODS

    /*********************************************************************************************************
    * Method: CreateMap
    * Input: N/A
    * Output: N/A
    * Purpose: Connects rooms beginning from the center and spanning outward horizontally 
    * Notes:
        * Any GameObject in the scene that is a room must contain the tag "Room"
        * All rooms need to be the same size and contain a box collider matching the mesh size
        * Based on fixed global orientation in the following order (N, S, E, W)
    *********************************************************************************************************/
    /************************
     * CORE METHOD 1
     ***********************/
    private void CreateMap()
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

            /**** -SUB METHOD-1.1 ****/
            CountTotalRoomType(rooms);
            /*************************/

            //Create tree with central room
            if (tree == null)
            {
                int maxRooms = 4;
                tree = new NTree(rooms.First(), maxRooms);
                
                //Assign central room
                centralRoom = tree.GetRoot();
                /**** -SUB METHOD-1.2 ****/
                SetRoomID(centralRoom);
                /*************************/
            }


            /**** -SUB METHOD-1.3 ****/
            IterateRooms(rooms);
            /*************************/

            /**** -SUB METHOD-1.7 ****/
            VerifyConnections(rooms);
            /*************************/
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
    * SUB METHOD 1.1
    ***********************/
    private void CountTotalRoomType(List<GameObject> rooms)    
    {
        totalRooms = rooms.Count;
        foreach(GameObject obj in rooms) 
        {
            RoomType roomType = obj.GetComponent<Room>().roomType;
            switch(roomType)
            {
                case RoomType.Normal:
                    normalRooms += 1;
                    break;

                case RoomType.Puzzle: 
                    puzzleRooms += 1;
                    break;

                case RoomType.Safe: 
                    safeRooms += 1;
                    break;

                case RoomType.Secret: 
                    secretRooms += 1;
                    break;

                case RoomType.Exit: 
                    exitRooms += 1;
                    break;
                default:
                    throw new Exception("Something strange happened.");      
            }
        }
        Debug.Log($"Total: {totalRooms}\nNormal:{normalRooms}, Puzzle:{puzzleRooms}, Safe:{safeRooms}, Secret:{secretRooms}, Exit:{exitRooms}");
    }

    /************************
    * SUB METHOD 1.2
    ***********************/
    private void SetRoomID(CustomNode node)
    {
        if (node.GetData() == null)
        {
            throw new NullReferenceException("Node contains no GameObject.");
        }
        node.GetData().GetComponent<Room>().Node = node;
        //Debug.Log($"{node.GetData().name} id({node.GetIndex()}) roomID({node.GetData().GetComponent<Room>().RoomID})");
    }

    /************************
     * SUB METHOD 1.3
     ***********************/
    private void IterateRooms(List<GameObject> rooms)
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            int maxRooms = 4;
            GameObject currRoom = rooms[i];
            CustomNode currNode = NTree.CreateCustomNode(currRoom, maxRooms);       //THIS IS FOR NEW DETECTED ROOMS. CREATED OUTSIDE OF THE TREE OBJECT.
            SetRoomID(currNode);


            //Check if room currently connects to any in the list.
            //Add current to disconnected if still has connections remaining
            for (int j = 0; j < i; j++)
            {
                GameObject prevRoom = rooms[j];
                //Debug.Log($"prevRoom ID = {prevRoom.GetComponent<Room>().RoomID}");
                CustomNode prevNode = prevRoom.GetComponent<Room>().Node;       //PREVIOUS NODES HAVE BEEN INITIALISED ALREADY. ACCESS EASILY THROUGH ROOM OBJECT REFERENCE.
                

                if(prevNode == null)
                {
                    throw new NullReferenceException("Prev node is currently null");
                }

                if (prevNode.GetChildren().Count(x => x != null) < prevRoom.GetComponent<Room>().PassageCount)        
                {
                    /****************************** ------SUB METHODS------ ****************************/
                    //SUB METHOD 1.4
                    if (IsAdjacentRoom(prevNode, currNode)) 
                    {
                        //SUB METHOD 1.5
                        InsertByOrientation(prevNode, currNode);     
                    }
                    /***********************************************************************************/
                }
            }
        }
    }

    /************************
    * SUB METHOD  1.4
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

        
        /****************************** ------SUB METHOD------ ****************************/
        //SUB METHOD 1.6
        Vector3 displacement = DisplacementFromCurrentRoom(prevNode, currNode);
        /***********************************************************************************/

            
        bool adjacent = state1;
        //Check if half of room size is within the half of the displacement vector which can be done by comparing lengths based on their position as well
        if (!adjacent)
        {
            //Test if the extends distance of currRoom is ~90 of the half of the displacement length
            Vector3 toCurrCenter = currNode.GetData().GetComponent<BoxCollider>().bounds.extents;
            bool state2 = (toCurrCenter.magnitude > (percentMin * displacement.magnitude)) && (toCurrCenter.magnitude <= displacement.magnitude);
            adjacent = state1 || state2;
            //Debug.Log($"Exact = {state1}     Approx = {state2}       disp = ({displacement}) |{displacement.magnitude}      extends = ({toCurrCenter}) |{toCurrCenter.magnitude}");
        }

        //Debug.Log($"(i:{i},j:{j}) Adjacent rooms [{adjacent}]: C({currRoom.name}[{currRoom.transform.position}]) <=> P({prevRoom.name}[{prevRoom.transform.position}])");
        return adjacent;
    }

    /************************
    * SUB METHOD 1.5
    ***********************/
    //FUTURE ADD DOORS TOO
    private void InsertByOrientation(CustomNode prevNode, CustomNode currNode, int error=5)
    {
        //Use angles to differentiate between adjacent rooms including contact with corners (reference from current room center, anglemeasured from horizontal)
        /****************************** ------SUB METHOD------ ****************************/
        //SUB METHOD 1.6
        Vector3 displacement = DisplacementFromCurrentRoom(prevNode, currNode);
        /***********************************************************************************/


        //Angle between centroids (Assuming Room size is the same)
        
        float angle = Vector3.SignedAngle(Vector3.right, displacement, Vector3.down);
        //Debug.Log($"D=({displacement}):     C({currNode.GetData().name}[{currNode.GetData().transform.position}]) <{angle} -> P({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");

        bool north = (angle >= (90 - error)) && (angle <= (90 + error));
        bool south = (angle >= (-90 - error)) && (angle <= (-90 + error));
        bool east = (angle >= (-error)) && (angle <= (error));
        bool west = (angle >= (180 - error) && angle <= 180) || (angle >= -180 && angle <= (-180 + error));

        //Debug.Log($"N({north}) S({south}) E({east}) W({west})");
        

        if (north)
        {
            //Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (North) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
            CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.S);
            CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.N);
            //Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
        }
        else if (south)
        {

            //Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (South) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
            CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.N);
            CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.S);
            //Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
        }
        else if (east)
        {

            //Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   >>  (East) >>  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
            CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.W);
            CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.E);
            //Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
        }
        else if (west)
        {

            //Debug.Log($"D=({displacement}):   P({currNode.GetData().name} [ {currNode.GetData().transform.position} ])   >>  (West) >>  C( {prevNode.GetData().name} [ {prevNode.GetData().transform.position}])");
            CustomNode tmp1 = tree.InsertNodeAt(prevNode, currNode, (int)Compass.E);
            CustomNode tmp2 = tree.InsertNodeAt(currNode, prevNode, (int)Compass.W); 
            //Debug.Log($"Insert to prev = {tmp1.GetData().name} id({tmp1.GetIndex()})\tInsert to current = {tmp2.GetData().name} id({tmp1.GetIndex()})");
        }
        else
        {
            /****************************************************************
             * 
             * 
             *          FUTURE: ADD EXTRA CASE FOR LARGER ROOMS
             * 
             ***************************************************************/

            //Debug.Log($"D=({displacement}):   P({prevRoom.name}[{prevRoom.transform.position}])   --None--  C({currRoom.name}[{currRoom.transform.position}])");
            //Debug.Log($"D=({displacement}):   P({currNode.GetData().name}[{currNode.GetData().transform.position}])   --None--  C({prevNode.GetData().name}[{prevNode.GetData().transform.position}])");
        }
        
    }

    /************************
    * SUB METHOD 1.6
    ***********************/
    private Vector3 DisplacementFromCurrentRoom(CustomNode prevNode, CustomNode currNode)
    {
        return prevNode.GetData().transform.position - currNode.GetData().transform.position; 
    }

    /************************
    * SUB METHOD 1.7
    ***********************/
    private void VerifyConnections(List<GameObject> rooms)
    {
        bool result = rooms.All(x => (x.GetComponent<Room>().RoomID != -1) && (x.GetComponent<Room>().Node != null));
        Debug.Log($"Fully Connected = {result}");
    }

    /*********************************************************************************************************
    ************************************     END     OF      METHOD     **************************************
    *********************************************************************************************************/


    /*********************************************************************************************************
    * Method: DisplayAllRooms 
    * Input: N/A
    * Output: N/A
    * Purpose: Displays all possible nodes that are connected to the tree.
    *********************************************************************************************************/
    public void DisplayAllRooms()
    {
        for (int i = 0; i < totalRooms; i++)
        {
            //Debug.Log($"Node({i}):\t{tree.CheckNodeExists(i)}");
            Debug.Log($"Room ID({i}):\t{tree.FindNode(i).GetData().name}");
        }
    }

    /*********************************************************************************************************
    * Method: GetRoomType 
    * Input: idx(int)
    * Output: RoomType(Room.Enum)
    * Purpose: Get the enum element from the Room class
    *********************************************************************************************************/
    public RoomType GetRoomType(int idx)
    {
        GameObject room = tree.FindNode(idx).GetData();
        return room.GetComponent<Room>().roomType;
    }

    /*********************************************************************************************************
    * Method: GetRoom
    * Input: idx(int)
    * Output: Room
    * Purpose: Return the Room component it is currently attached to
    *********************************************************************************************************/
    public Room GetRoom(int idx)
    {
        GameObject room = tree.FindNode(idx).GetData();
        return room.GetComponent<Room>();
    }

    /*********************************************************************************************************
    * Method: AddMarker 
    * Input: N/A
    * Output: N/A
    * Purpose: Connects rooms beginning from the center and spanning outward horizontally
    *********************************************************************************************************/
    public void AddMarker(GameObject gameObject)
    {
        tree.InsertTracker();
    }

    /*********************************************************************************************************
    * Method: TestOverlap 
    * Input: N/A
    * Output: N/A
    * Purpose: Connects rooms beginning from the center and spanning outward horizontally
    *********************************************************************************************************/
    public void ChangeTeleport(int idx, bool state)
    {
        GameObject room = tree.FindNode(idx).GetData();
        room.GetComponent<Room>().IsTeleportable = state;
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





    /*************************************************************************************************************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum Compass { N, S, E, W};      //Direction assigned to respective index
public class Map : MonoBehaviour
{
    //PROPERTIES
    [SerializeField]
    NTree map;

    [Header("Pick central room")]
    public GameObject centralRoom;      //Manually set

    [Header("Room Count")]
    int totalRooms;
    int normalRooms;
    int puzzleRooms;
    int secretRooms;
    int safeRooms;

    [Header("Other options")]
    public bool isFinalLevel;

    //Dropdown room to inspect and debug????


    //CONSTRUCTORS
    public Map() 
    {
        map = new NTree();
        totalRooms = normalRooms = puzzleRooms = secretRooms = safeRooms = 0;
        isFinalLevel = false;
    }



    //METHODS
    /* define collision detection direction
     * insert room by collider overlap based on door direction
     * Room type at the node
     * get room at?
     * add tracker if enemy and player exist == marker (dynamic)
     * get tracker location
     * add marker (fixed)
     * update tracker if moved
     * update room status
     */




}

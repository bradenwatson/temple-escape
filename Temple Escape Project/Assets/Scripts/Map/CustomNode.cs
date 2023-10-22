/*************************************************************************************************************************************************************************************
 *  Name: Tony Bui
 *  Purpose: A Custom Node to contain Unity GameObjects
 *  Last updated: 22/10/23
 *  Notes: Intended for use with NTree
*************************************************************************************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*************************************************************************************************************************************************************************************
 *  Class: CustomNode 
    *  Properties:
        *   index (int)
        *   data (GameObject)
        *   children (List<CustomNode>)
    *   
    *   NOTES: 
            * NODE ATTACHED DO NOT HAVE TO BE UNIQUE, THEY CAN BE PREVIOUS EXISTING NODES
            * CANNOT ATTACH CUSTOM CLASSES DIRECTLY. IT IS RECOMMENDED TO PASS GAMEOBJECT REFERENCE FROM THE SCENE
*************************************************************************************************************************************************************************************/
public class CustomNode 
{
    // PROPERTIES
    [SerializeField]
    int index;

    [SerializeField]
    GameObject data;

    [SerializeField]
    CustomNode[] children; // Use an array instead of List

    [SerializeField]
    int numOfChildrenSpawned;

    // CONSTRUCTORS
    public CustomNode()
    {
        index = -1; // Defaults to -1 if the node is not connected to the tree
        data = null;
        children = null;
        numOfChildrenSpawned = 0;
    }
    public CustomNode(GameObject _data, int limit)
    {
        this.index = -1;
        this.data = _data;
        this.children = new CustomNode[limit];
        this.numOfChildrenSpawned = 0;
    }
    public CustomNode(int _index, GameObject _data, int limit)
    {
        this.index = _index;
        this.data = _data;
        this.children = new CustomNode[limit];
        this.numOfChildrenSpawned = 0;
    }

    public CustomNode(CustomNode _node)
    {
        this.index = _node.index;
        this.data = _node.data;
        this.children = _node.children;
        this.numOfChildrenSpawned = _node.numOfChildrenSpawned;
    }

    // GETTERS
    public int GetIndex() { return index; }
    public GameObject GetData() { return data; }
    public CustomNode[] GetChildren() { return children; }
    public int GetNodeLimit() { return this.children.Length; }
    public int GetNumberOfChildren() { return this.numOfChildrenSpawned; }

    // SETTERS
    public void SetIndex(int _index) { index = _index; }
    public void SetData(GameObject _data) { this.data = _data; }
    public void SetChildren(CustomNode[] _children) 
    { 
        this.children = _children; 
        this.numOfChildrenSpawned = this.children.Count(x=> x != null);
    }
    public void SetNodeLimit(int limit)
    {
        if (limit < 0)
        {
            throw new ArgumentException("Capacity cannot be negative.");
        }

        else if(this.children != null)
        {
            throw new ArgumentOutOfRangeException("Cannot resize the existing capacity.");
        }

        else
        {
            this.children = new CustomNode[limit];
        }
    }

    // METHODS
    public void InsertChildren(CustomNode node)
    {
        if (this.children == null)
        {
            throw new NullReferenceException("Array has not been initialised.");
        }
        else
        {
            for (int i = 0; i < this.children.Length; i++)
            {
                if (this.children[i] == null)
                {
                    this.children[i] = node;
                    this.numOfChildrenSpawned++;
                    break;
                }
            }
        }
    }

    public void InsertChildrenAt(int childIdx, CustomNode node)
    {
        if (this.children == null)
        {
            throw new NullReferenceException("Array has not been initialised.");
        }

        else if (this.children[childIdx] != null)
        {
            throw new NotSupportedException($"Element at {childIdx} already occupied.");
        }

        else
        {
            this.children[childIdx] = node;
            this.numOfChildrenSpawned++;
        }
    }


    /*************************************************************************************************************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}




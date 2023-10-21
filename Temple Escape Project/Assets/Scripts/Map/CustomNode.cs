/*************************************************************************************************************************************************************************************
 *  Name: Tony Bui
 *  Purpose: A Custom Node to contain Unity GameObjects
 *  Last updated: 17/10/23
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
    *   parent (CustomNode)
    *   children (List<CustomNode>)
    *   
    *   NOTE: NODE ATTACHED DO NOT HAVE TO BE UNIQUE, THEY CAN BE PREVIOUS EXISTING NODES
*************************************************************************************************************************************************************************************/
public class CustomNode 
{
    // PROPERTIES
    int index;
    GameObject data;
    CustomNode[] parent; // Use an array instead of List
    CustomNode[] children; // Use an array instead of List
    int numOfChildrenSpawned;
    int numOfParentsSpawnsed;

    // CONSTRUCTORS
    public CustomNode()
    {
        index = -1; // Defaults to -1 if the node is not connected to the tree
        data = null;
        parent = null;
        children = null;
        numOfChildrenSpawned = 0;
        numOfParentsSpawnsed = 0;       //Note root will not have any
    }

    public CustomNode(GameObject _data)
    {
        this.data = _data;
    }

    public CustomNode(GameObject _data, int limit)
    {
        this.data = _data;
        this.children = new CustomNode[limit];
    }

    public CustomNode(int _index, GameObject _data)
    {
        this.index = _index;
        this.data = _data;
    }

    public CustomNode(int _index, GameObject _data, int limit)
    {
        this.index = _index;
        this.data = _data;
        this.children = new CustomNode[limit];
    }

    public CustomNode(CustomNode _node)
    {
        this.index = _node.index;
        this.data = _node.data;
        this.parent = _node.parent;
        this.children = _node.children;
        this.numOfChildrenSpawned = _node.numOfChildrenSpawned;
        this.numOfParentsSpawnsed = _node.numOfParentsSpawnsed;
    }

    // GETTERS
    public int GetIndex() { return index; }
    public GameObject GetData() { return data; }
    public CustomNode[] GetParent() { return parent; }
    public CustomNode[] GetChildren() { return children; }
    public int GetNodeLimit() { return this.children.Length; }
    public int GetParentLimit() { return this.parent.Length; }
    public int GetNumberOfChildren() { return this.numOfChildrenSpawned; }
    public int GetNumberOfParents() { return this.numOfParentsSpawnsed; }


    // SETTERS
    public void SetIndex(int _index) { index = _index; }
    public void SetData(GameObject _data) { this.data = _data; }
    public void SetParent(CustomNode[] _parent) 
    { 
        this.parent = _parent; 
        this.numOfParentsSpawnsed = this.parent.Count(x=> x != null);
    }
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
                    //this.children[i].parent = new CustomNode[] { this }; // Assign parent to child node as an array with a single parent
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

        else if (this.children[childIdx] == null)
        {
            this.children[childIdx] = node;
            
        }      
    }

    public void InsertParentAt(int parentIdx, CustomNode node)
    {
        if (this.parent == null)
        {
            throw new NullReferenceException("Array has not been initialised.");
        }

        else if (this.parent[parentIdx] == null)
        {
            this.parent[parentIdx] = node;

        }
    }

    /****************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}




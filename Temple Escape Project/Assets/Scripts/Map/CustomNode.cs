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
*************************************************************************************************************************************************************************************/
public class CustomNode
{
    //PROPERTIES
    int index;
    GameObject data;
    CustomNode parent;      // For traversal
    List<CustomNode> children { get; set; }


    //CONSTRUCTORS
    public CustomNode()
    {
        index = -1;     //Defaults to 1 if node is not connected to the tree
        data = null;
        parent = null;
        children = null;
    }
    public CustomNode(GameObject _data)
    {
        this.data = _data;
    }

    public CustomNode(GameObject _data, int limit)
    {
        this.data = _data;
        this.children = new List<CustomNode>(limit);
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
        this.children = new List<CustomNode>(limit);
    }

    public CustomNode(CustomNode _node)
    {
        this.index = _node.index;
        this.data = _node.data;
        this.parent = _node.parent;
        this.children = _node.children;
    }


    //GETTERS
    public int GetIndex() { return index; }
    public GameObject GetData() { return data; }
    public CustomNode GetParent() { return parent; }
    public List<CustomNode> GetChildren() { return children; }
    public int GetNodeLimit() { return this.children.Capacity; }


    //SETTERS
    public void SetIndex(int _index) { index = _index; }
    public void SetData(GameObject _data) { this.data = _data; }
    public void SetParent(CustomNode _parent) { this.parent = _parent; }
    public void SetChildren(List<CustomNode> _children) { this.children = _children; }
    public void SetNodeLimit(int limit)
    {
        if (limit < 0)
        {
            throw new ArgumentException("Capacity cannot be negative.");
        }

        if (this.children == null)
        {
            this.children = new List<CustomNode>();
        }

        if (limit == 0 || this.children.Count <= limit)
        {
            this.children.Capacity = limit;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Cannot reduce existing capacity.");
        }
    }



    //METHODS
    public void InsertChildren(CustomNode node)
    {
        if (this.children == null)
        {
            this.children = new List<CustomNode>();
        }

        if (this.children.Capacity == 0 || this.children.Count < this.children.Capacity)
        {

            this.children.Add(node);
            this.children.Last().parent = new CustomNode(this);         //Assign child to parent node
        }
        else
        {
            throw new ArgumentOutOfRangeException("List has reached its maximum already.");
        }

        //Should not return node for ease of access because it must only exists
        //if conditions are met.
    }


    /****************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}




/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: A generic n-list tree made for Unity
 *  Last updated: 31/08/23
 *  Notes: 
/************************************************************************************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NTree
{
/************************************************************************************************************************************************************************************/
/*  Class: NTree 
 *  Purpose: Manages the creation and search for nodes of any Unity _data type within the network.
 *  Properties:
    *   root (CustomNode)
    *   counter (int)
    *   Tracker (List<CustomNode>): Referrence points within the network
 *  Notes:
    * Assumes nodes are fixed and will not be deleted
    * Assumes existence of central node and is not optimised for unbalanced trees
/************************************************************************************************************************************************************************************/

    //PROPERTIES
    CustomNode root;
    int counter;
    List<CustomNode> tracker;
    

    //CONSTRUCTORS
    private NTree()
    {
        root = null;        //Root node should be central to all the _data
        counter = 0;
        tracker = null;
    }

    //Create Tree with root
    public NTree(GameObject data)
    {
        if(this.root != null)
        {
            throw new NotSupportedException("Tree instantiated with root already exists.");
        }
        else
        {
            this.root = new CustomNode(this.counter++, data);     //Counter increments after method runs 
        }
            
    }

    //Create Tree with root and trackers
    public NTree(GameObject data, int amount)
    {
        if (this.root != null)
        {
            throw new NotSupportedException("Tree instantiated with root already exists.");
        }
        else
        {
            this.root = new CustomNode(this.counter++, data);     //Counter increments after method runs
            this.tracker = new List<CustomNode>();
            for(int i = 0; i < amount; i++)
            {
                this.tracker.Add(new CustomNode());
            }
        }
    }


    //GETTERS
    public  CustomNode GetRoot() { return this.root; }
    public int GetCount() { return counter; }
    public List<CustomNode> GetTrackers() {  return tracker; }
    

    //SETTERS
    public void SetRoot(CustomNode root) {  this.root = root; }



    //METHODS
    
    /***************************************************************************************/
    /* Method: InsertTracker
     * Input: amount(int)
     * Output: N/A
     * Purpose: Checks whether tracker(Node) variables exist and creates them based on the 
     *          amount desired.
    /***************************************************************************************/
    public void InsertTracker(int amount)
    {
        if(this.tracker == null) { this.tracker = new List<CustomNode>(); }

        for (int i = 0; i < amount; i++)
        {
            this.tracker.Add(new CustomNode());
        }
    }
    
    /***************************************************************************************/
    /* Method: SelectTracker
     * Input: index(int)
     * Output: targetNode(Node)
     * Purpose: Checks whether tracker(Node) variables exist and returns with same index
    /***************************************************************************************/
    public CustomNode SelectTracker(int index) 
    {
        CustomNode targetNode = null; 
        if(this.tracker == null)
        {
            throw new NoNullAllowedException("Tracker node has not been instantiated.");
        }
        else 
        { 
            if (index < 0 || index >= tracker.Count)
            {
                throw new IndexOutOfRangeException("Tracker does not exist.");
            }
            else
            {
                targetNode = tracker[index];
            } 
        }
        return targetNode;
    }

    /***************************************************************************************/
    /* Method: CheckNodeExists
     * Input: key(int)
     * Output: isFound(bool)
     * Purpose: This filters nodes that were created within the tree class as their index 
     *          correlates with the counter. External nodes do not connect or contribute to 
     *          the tree counter and always has an index of -1. 
    /***************************************************************************************/
    public bool CheckNodeExists(int key)
    {
        bool isFound = false;
        if (key >= 0 && key <= this.counter) 
        { 
            isFound = true; 
        }    
        return isFound;
    }

    /***************************************************************************************/
    /* Method: FindNode
     * Input: key(int)
     * Output: node(Node)
     * Purpose: Finds a node within the tree by traversing through a queue of node indexes 
     *          starting from the root node and its children (by row) until their index 
     *          matches with the key
    /***************************************************************************************/
    public CustomNode FindNode(int key)
    {
        CustomNode node = null;

        if(key < 0)
        {
            throw new ArgumentOutOfRangeException("Key must be a positive integer.");
        }

        if (this.root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else
        {
            if (!CheckNodeExists(key))
            {
                throw new NullReferenceException("Node does not exist.");
            }
            else
            {
                Queue<CustomNode> queue = new Queue<CustomNode>();
                queue.Enqueue(this.root);
                while (queue.Count > 0 && node == null)    //Queue children at end, pop parent until either queue
                                                         //is empty or node has been found
                {
                    if (queue.Peek().GetIndex() == key)   //If the first node in the queue matches
                    {
                        node = queue.Peek();              //Copy the matching node in the queue
                    }
                    else                                   //If keys dont match
                    {
                        foreach (CustomNode n in queue.Peek().GetChildren())     //Add child nodes to the back of the queue
                        {
                            queue.Enqueue((CustomNode)n);     //Queue children
                        }
                        queue.Dequeue();      //Pop parent to restart the process
                    }
                }

                if (node == null)        //If node could not be found throw exception
                {
                    throw new NoNullAllowedException("The node could not be found.");
                }
            }
        }
        return node;
    }


    /***************************************************************************************/
    /* Method: InsertNode
     * Input: key(int), _data (GameObject)  
     * Output: N/A
     * Purpose: Inserts node within the tree at a specific key
    /***************************************************************************************/
    public void InsertNode(int key, GameObject data) 
    {
        if(root == null) 
        {
            this.root = new CustomNode(this.counter++, data);     //Counter increments after method runs    
        }
        else
        {
            //Add child node at index
            CustomNode node = new CustomNode(this.counter++, data);
            FindNode(key).InsertChildren(node);
        }
    }


    /***************************************************************************************/
    /* Method: SetTrackerTo
     * Input: index(int), node (Node)
     * Output: N/A
     * Purpose: Select a tracker(node) and assign to a specific node within the tree
    /***************************************************************************************/
    public void SetTrackerTo(int index, CustomNode node)    
    {
        if(index < 0 || index >= this.counter) 
        {
            throw new IndexOutOfRangeException("Tracker node does not exist.");  
        }
        else
        {
            if (!CheckNodeExists(node.GetIndex()))
            {
                throw new NullReferenceException("Node does not belong within tree.");
            }
            else
            {
                CustomNode copy = new CustomNode(node);
                this.tracker[index] = copy;
            }
        }
    }
/***************************************************************************************/
/*
*                      END     OF      CLASS!
*/
/************************************************************************************************************************************************************************************/







/***************************************************************************************/
/*
*                  NEW     CLASS   BEGINS  HERE!
*/
/************************************************************************************************************************************************************************************/
/*  Class: CustomNode 
    *  Properties:
    *   index (int)
    *   _data (GameObject)
    *   children (List<CustomNode>)
/************************************************************************************************************************************************************************************/
    public class CustomNode
    {
        //PROPERTIES
        int index;
        GameObject data;
        List<CustomNode> children { get; set; }


        //CONSTRUCTORS
        public CustomNode()
        {
            index = -1;     //Defaults to 1 if node is not connected to the tree
            data = null;
            children = null;
        }
        public CustomNode(GameObject _data)
        {
            this.data = _data;
            this.children = new List<CustomNode>();
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
            this.children = new List<CustomNode>();
        }

        public CustomNode(int _index, GameObject _data, int limit)
        {
            this.index = _index;
            this.data = _data;
            this.children = new List<CustomNode>(limit);
        }
        public CustomNode(GameObject _data, List<CustomNode> _children)
        {
            this.data = _data;
            this.children = _children;
        }
        public CustomNode(CustomNode _node)
        {
            this.index = _node.index;
            this.data = _node.data;
            this.children = _node.children;
        }


        //GETTERS
        public int GetIndex() { return index; } 
        public GameObject GetData() { return data; } 
        public List<CustomNode> GetChildren() { return children; }
        public int GetNodeLimit() {  return this.children.Capacity; }

        
        //SETTERS
        public void SetIndex(int _index) { index = _index; }
        public void SetData(GameObject data) { this.data = data; }
        public void SetNodeLimit(int limit)
        {
            if(limit < 0)
            {
                throw new ArgumentException("Capacity cannot be negative.");
            }
            else
            {
                if(this.children == null)
                {
                    this.children= new List<CustomNode>();
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
        }



        //METHODS
        public void InsertChildren(CustomNode node) 
        {
            if(this.children == null)
            {
                this.children = new List<CustomNode>();
            }

           if(this.children.Count == 0 || this.children.Count < this.children.Capacity) 
            {
                this.children.Add(node);
            }
            else
            {
                throw new ArgumentOutOfRangeException("List has reached its maximum already.");
            }
        }

    }

/***************************************************************************************/
/*
*                   END     OF      CLASS!
*/
/************************************************************************************************************************************************************************************/

}

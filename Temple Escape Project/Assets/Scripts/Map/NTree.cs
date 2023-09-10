/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: A generic n-list tree made for Unity
 *  Last updated: 31/08/23
 *  Notes: 
/************************************************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public NTree()
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

    //GETTERS
    public  CustomNode GetRoot() { return this.root; }
    public int GetCount() { return counter; }
    public List<CustomNode> GetTrackers() {  return tracker; }
    

    //SETTERS
    public void SetRoot(CustomNode root) {  this.root = root; }



    //METHODS
    /***************************************************************************************/
    /* Method: InsertTracker
     * Input: data (GameObject)
     * Output: N/A
     * Purpose: Creates a unique node separate to the tree containing GameObject data
    /***************************************************************************************/
    public void InsertTracker(GameObject data)
    {
        if(this.tracker == null) { this.tracker = new List<CustomNode>(); }

        CustomNode tmp = new CustomNode(data);
        this.tracker.Add(tmp);
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
     *          
    /***************************************************************************************/
    public bool CheckNodeExists(int key)
    {
        bool isFound = false;
        if (key >= 0 && key < this.counter) 
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
                        if (queue.Peek().GetChildren() != null)
                        {
                            foreach (CustomNode n in queue.Peek().GetChildren())     //Add child nodes to the back of the queue
                            {
                                queue.Enqueue((CustomNode)n);     //Queue children
                            }
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
    public void InsertNodeAt(int key, GameObject data) 
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
    /* Method: InsertNode
     * Input: node(CustomNode), _data (GameObject)  
     * Output: N/A
     * Purpose: Inserts node within the tree at a specific node
    /***************************************************************************************/
    public void InsertNodeAt(CustomNode node, GameObject data)
    {
        if (root == null)
        {
            this.root = new CustomNode(this.counter++, data);     //Counter increments after method runs    
        }
        else
        {
            //Add child node at index
            CustomNode child = new CustomNode(this.counter++, data);
            node.InsertChildren(child);
        }
    }

    /***************************************************************************************/
    /* Method: SetTrackerTo
     * Input: trackerIdx(int), nodeIdx (int)
     * Output: N/A
     * Purpose: Select a tracker(node) and assign to a specific node within the tree
    /***************************************************************************************/
    public void SetTrackerTo(int trackerIdx, int nodeIdx)    
    {
        if(trackerIdx < 0 || trackerIdx >= this.counter) 
        {
            throw new IndexOutOfRangeException("Tracker node does not exist.");  
        }
        else
        {
            if (!CheckNodeExists(nodeIdx))
            {
                throw new NullReferenceException("Node does not belong within tree.");
            }
            else
            { 
                //Copy node details except for data as the tracker's data is its own entity
                CustomNode tmp = this.FindNode(nodeIdx);
                this.tracker[trackerIdx].SetIndex(tmp.GetIndex());
                this.tracker[trackerIdx].SetParent(tmp.GetParent());
                this.tracker[trackerIdx].SetChildren(tmp.GetChildren());
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
    *   data (GameObject)
    *   parent (CustomNode)
    *   children (List<CustomNode>)
/************************************************************************************************************************************************************************************/
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
        public int GetNodeLimit() {  return this.children.Capacity; }

        
        //SETTERS
        public void SetIndex(int _index) { index = _index; }
        public void SetData(GameObject _data) { this.data = _data; }
        public void SetParent(CustomNode _parent) {  this.parent = _parent; }
        public void SetChildren(List<CustomNode> _children) {  this.children = _children; }
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

           if(this.children.Capacity == 0 || this.children.Count < this.children.Capacity) 
            {

                this.children.Add(node);
                this.children.Last().parent = new CustomNode(this);         //Assign child to parent node
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

/************************************************************************************************************************************************************************************/
/*  Name: Tony Bui
 *  Purpose: A generic n-list tree made for Unity
 *  Last updated: 31/08/23
 *  Notes: 
/************************************************************************************************************************************************************************************/
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.SceneManagement;

public class NTree : MonoBehaviour
{
/************************************************************************************************************************************************************************************/
/*  Class: NTree 
 *  Purpose: Manages the creation and search for nodes of any Unity data type within the network.
 *  Properties:
    *   root (Node)
    *   counter (int)
    *   Tracker (List<Node>): Referrence points within the network
 *  Notes:
    * Assumes nodes are fixed and will not be deleted
    * Assumes existence of central node and is not optimised for unbalanced trees
/************************************************************************************************************************************************************************************/

    //PROPERTIES
    Node root;
    int counter;
    List<Node> tracker;
    

    //CONSTRUCTORS
    public NTree()
    {
        root = null;        //Root node should be central to all the data
        counter = 0;
        tracker = null;
    }

    public NTree(GameObject data)
    {
        this.root = new Node(this.counter++, data);     //Counter increments after method runs     
    }

    public NTree(GameObject data, int amount)
    {
        this.root = new Node(this.counter++, data);     //Counter increments after method runs
        this.tracker = new List<Node>();
        for(int i = 0; i < amount; i++)
        {
            this.tracker.Add(new Node());
        }
    }


    //GETTERS
    public Node GetRoot() { return this.root; }
    public int GetCount() { return counter; }
    

    //SETTERS
    public void SetRoot(Node root) {  this.root = root; }



    //METHODS
    /***************************************************************************************/
    /* Method: SelectTracker
     * Input: index(int)
     * Output: targetNode(Node)
     * Purpose: Checks whether tracker(Node) variables exist and returns with same index
    /***************************************************************************************/
    public Node SelectTracker(int index) 
    {
        Node targetNode = null; 
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
    /* Method: InsertTracker
     * Input: amount(int)
     * Output: N/A
     * Purpose: Checks whether tracker(Node) variables exist and creates them based on the 
     *          amount desired.
    /***************************************************************************************/
    public void InsertTracker(int amount)
    {
        if(this.tracker == null) { this.tracker = new List<Node>(); }

        for (int i = 0; i < amount; i++)
        {
            this.tracker.Add(new Node());
        }
    }

    /***************************************************************************************/
    /* Method: InsertNode
     * Input: key(int), data (GameObject)  
     * Output: N/A
     * Purpose: Inserts node within the tree at a specific key
    /***************************************************************************************/
    public void InsertNode(int key, GameObject data) 
    {
        if(root == null) 
        {
            this.root = new Node(this.counter++, data);     //Counter increments after method runs    
        }
        else
        {
            //Add child node at index
            Node node = new Node(this.counter++, data);
            FindNode(key).InsertChildren(node);
        }
    }


    /***************************************************************************************/
    /* Method: CheckNodeExists
     * Input: key(int)
     * Output: isFound(bool)
     * Purpose: This filters nodes that were created within the tree class as their index 
     *          correlates with the counter. External nodes do not connect or contribute to 
     *          the tree counter and always has an index of -1. 
    /***************************************************************************************/
    private bool CheckNodeExists(int key)
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
    public Node FindNode(int key)    
    {
        Node node = null;
        if(root == null)
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
                List<Node> queue = new List<Node>();
                queue.Add(root);
                while (queue != null || node != null)    //Queue children at end, pop parent until either queue
                                                         //is empty or node has been found
                {
                    if (queue.First().GetIndex() == key)   //If the first node in the queue matches
                    {
                        node = queue.First();              //Select the first node in the queue
                    }
                    else                                   //If keys dont match
                    {
                        foreach (Node n in queue.First().GetChildren())     //Add child nodes to the back of the queue
                        {
                            queue.Add((Node)n);     //Queue children
                        }
                        queue.RemoveAt(0);      //Pop parent to restart the process
                    }
                }

                if(node == null)        //If node could not be found throw exception
                {
                    throw new NoNullAllowedException("The node could not be found.");
                }
            }
        }
        return node;
    }


    /***************************************************************************************/
    /* Method: SetTrackerTo
     * Input: index(int), node (Node)
     * Output: N/A
     * Purpose: Select a tracker(node) and assign to a specific node within the tree
    /***************************************************************************************/
    public void SetTrackerTo(int index, Node node)    
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
                Node copy = new Node(node);
                this.tracker[index] = copy;
            }
        }
    }
/************************************************************************************************************************************************************************************/




/************************************************************************************************************************************************************************************/
/*  Class: Node 
 *  Properties:
    *   index (int)
    *   data (GameObject)
    *   children (List<Node>)
/************************************************************************************************************************************************************************************/
    public class Node
    {
        //PROPERTIES
        int index;
        GameObject data;
        List<Node> children { get; set; }


        //CONSTRUCTORS
        public Node()
        {
            index = -1;     //Defaults to 1 if node is not connected to the tree
            data = null;
            children = null;
        }
        public Node(GameObject data)
        {
            this.data = data;
            this.children = new List<Node>();
        }
        public Node(int index, GameObject data)
        {
            this.index = index;
            this.data = data;
            this.children = new List<Node>();
        }
        public Node(GameObject data, List<Node> children)
        {
            this.data = data;
            this.children = children;
        }
        public Node(Node _node)
        {
            this.index = _node.index;
            this.data = _node.data;
            this.children = _node.children;
        }


        //GETTERS
        public int GetIndex() { return index; } 
        public GameObject GetData() { return data; } 
        public List<Node> GetChildren() { return children; }

        
        //SETTERS
        public void SetIndex(int _index) { index = _index; }
        public void SetData(GameObject data) { this.data = data; }


        //METHODS
        public void InsertChildren(Node node) { this.children.Add(node); }

    }
/************************************************************************************************************************************************************************************/

}


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
using UObject = UnityEngine.Object;

public class NTree : MonoBehaviour
{
    public class Node
    {
        int index;
        UObject data;
        List<Node> children { get; set; }

        public Node()
        {
            index = -1;
            data = null;
            children = null;
        }

        public Node(UObject data)
        {
            this.data = data;
            this.children = new List<Node>();
        }

        public Node(int index, UObject data)
        {
            this.index = index;
            this.data = data;
            this.children = new List<Node>();
        }

        public Node(UObject data, List<Node> children) 
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

        //Getters & Setters
        public int GetIndex()   { return index; }
        public void SetIndex(int _index) { index = _index; } 

        public UObject GetData() { return data; }
        public void SetData(UObject data) { this.data = data; }
        public List<Node> GetChildren() { return children; }
        
        
        //Methods
        public void InsertChildren(Node node) { this.children.Add(node); }
        
    }



    Node root;
    int counter;
    List<Node> tracker;
    
    public NTree()
    {
        root = null;    //should be the center OR the saferoom in order for the tree/map to be balanced
        counter = 0;
        tracker = null;
    }

    public NTree(UObject data)
    {
        this.root = new Node(this.counter++, data);     //Counter increments after method runs     
    }

    public NTree(UObject data, int amount)
    {
        this.root = new Node(this.counter++, data);     //Counter increments after method runs
        this.tracker = new List<Node>();
        for(int i = 0; i < amount; i++)
        {
            this.tracker.Add(new Node());
        }
    }

    //Getters & Setters
    public Node GetRoot() { return this.root; }
    public void SetRoot(Node root) {  this.root = root; }

    public int GetCount() { return counter; }

    public Node SelectTracker(int index) 
    {
        Node targetNode = null; 
        if(tracker != null)
        {
            if (index < tracker.Count)
            {
                targetNode = tracker[index];
            }
            else
            {
                Debug.Log("Tracker does not exist.");
            }
        }
        else { Debug.Log("Tracker node has not been instantiated."); }
        return targetNode;
    }

    public void InsertTracker(int amount)
    {
        if(this.tracker == null) { this.tracker = new List<Node>(); }


        for (int i = 0; i < amount; i++)
        {
            this.tracker.Add(new Node());
        }

    }


    //Methods
    public void Insert(int key, UObject data) 
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


    //This filters nodes that were created within the tree class as their index correlates with the counter.                        
    //External nodes do not connect or contribute to the tree counter and always has an index of -1. 
    private bool CheckNodeWithinTree(int key)
    {
        bool isFound = false;
        if (key >= 0 && key <= this.counter) 
        { 
            isFound = true; 
        }    
        return isFound;
    }

    //Finds a node within the tree by traversing through a queue of node indexes starting from the root node and its children until their index matches with the key
    public Node FindNode(int key)    //Search by row
    {
        Node node = null;
        if(root != null)
        {
            if (CheckNodeWithinTree(key))
            {
                List<Node> queue = new List<Node>();
                queue.Add(root);
                while (queue != null || node != null)    //Queue children at end, pop parent
                {
                    if (queue.First().GetIndex() == key)
                    {
                        node = queue.First();
                    }
                    else
                    {
                        foreach (Node n in queue.First().GetChildren())
                        {
                            queue.Add((Node)n);     //Queue children
                        }
                        queue.RemoveAt(0);      //Pop parent
                    }
                }
            }
            {
                Debug.Log("Node does not exist.");
            }
        }
        else
        {
            Debug.Log("Tree has not been built.");
        }
        return node;
    }

    public void SetTrackerTo(int index, Node node)    
    {
        if (CheckNodeWithinTree(node.GetIndex()))
        {
            Node copy = new Node(node);
            this.tracker[index] = copy;  
        }
        else { Debug.Log("Node does not belong within tree.");  } 
    }

}

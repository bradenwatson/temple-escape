
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class NTree : MonoBehaviour
{
    //https://stackoverflow.com/questions/61374720/build-tree-from-edge-pairs-and-root
    public class Node
    {
        int index;
        Object data;
        List<Node> children { get; set; }

        Node()
        {
            index = 0;
            data = null;
            children = null;
        }

        public Node(Object data)
        {
            this.data = data;
            this.children = new List<Node>();
        }

        public Node(int index, Object data)
        {
            this.index = index;
            this.data = data;
            this.children = new List<Node>();
        }

        public Node(Object data, List<Node> children) 
        {
            this.data = data;
            this.children = children;
        }

        public int GetIndex()   { return index; }
        public void SetIndex(int _index) { index = _index; } 

        public Object GetData() { return data; }
        public void SetData(Object data) { this.data = data; }
        public List<Node> GetChildren() { return children; }
        public void InsertChildren(Node node) { this.children.Add(node); }
        
    }

    Node root { get; set; }
    int counter; 
    
    public NTree()
    {
        root = null;    //should be the center OR the saferoom in order for the tree/map to be balanced
        counter = 0;
        //List of heads (2) for player and enemy to 
    }

    public NTree(Object data)
    {
        this.root = new Node(this.counter++, data);     //Counter increments after method runs     
    }

    public int GetCount() { return counter; }

    public void Insert(int key, Object data) 
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

    public Node FindNode(int key)    //Search by row
    {
        Node node = null;
        if(root != null)
        {
            if (key <= this.counter)
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
        }
        return node;
    }

    public void SetHeadTo(Node node)    //Wrapper method
    {
        //calls A* function which returns indexes
        //move the head until the index returns none
    }

}

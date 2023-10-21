/************************************************************************************************************************************************************************************
*  Name: Tony Bui
 *  Purpose: A generic n-list tree made for Unity
 *  Last updated: 17/10/23
 *  Notes: Requires a CustomNode class
************************************************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static PlasticGui.LaunchDiffParameters;



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
public class NTree
{

    //PROPERTIES
    CustomNode root;
    CustomNode lastInserted;
    int counter;
    List<CustomNode> tracker;
    

    //CONSTRUCTORS
    private NTree()
    {
        root = null;        //Root node should be central to all the _data
        lastInserted = null;
        counter = 0;
        tracker = null;
        Debug.Log($"NTree instantiated to default.");
    }

    //Create Tree using GameObject to create root node
    public NTree(GameObject data)
    {
        if(this.root != null) { throw new NotSupportedException("Tree instantiated with root already exists."); }
        else
        {
            this.root = new CustomNode(this.counter++, data);
            this.lastInserted = this.root;
            Debug.Log($"NTree instantiated with root as ({data.name}) "
                + $"@ [{this.root.GetIndex()}].");
        }
    }

    //Create Tree with a node containing GameObject
    public NTree(CustomNode node)
    {
        if (this.root != null) { throw new NotSupportedException("Tree instantiated with root already exists."); }
        else
        {
            node.SetIndex(counter++);     //Counter increments after method runs 
            //counter += 1;
            this.root = node;
            this.lastInserted = this.root;
            Debug.Log($"NTree instantiated with root as ({node.GetData().name}) " 
                    + $"@ [{this.root.GetIndex()}].");
        }
    }

    //GETTERS
    public  CustomNode GetRoot() { return this.root; }
    public int GetCount() { return counter; }
    public List<CustomNode> GetTrackers() {  return tracker; }

    //Accessor
    public CustomNode LastInserted 
    { 
        get { return lastInserted; } 
        set 
        { 
            if(!(this.CheckNodeExists(value.GetIndex())))
            {
                throw new ArgumentNullException("Node does not exist within the tree.");
            }
            this.lastInserted = value;
        } 
    }


    /***************************************************************************************
     * Method: InsertTacker
     * Input: N/A
     * Output: insertedTracker (CustomNode)
     * Purpose: Checks whether tracker(Node) variables exist and returns with same index
     ***************************************************************************************/
    public CustomNode InsertTracker()
    {
        if (this.tracker == null) { this.tracker = new List<CustomNode>(); }

        CustomNode tmp = new CustomNode();
        this.tracker.Add(tmp);
        return this.tracker.Last();
    }

    /***************************************************************************************
     * Method: SelectTracker
     * Input: index(int)
     * Output: targetNode(Node)
     * Purpose: Checks whether tracker(Node) variables exist and returns with same index
     ***************************************************************************************/
    public CustomNode SelectTracker(int index) 
    {
        CustomNode targetNode = null; 
        if(this.tracker == null)
        {
            throw new NoNullAllowedException("Tracker node has not been instantiated.");
        }

        if (index < 0 || index >= tracker.Count)
        {
            throw new IndexOutOfRangeException("Tracker does not exist.");
        }

        targetNode = tracker[index];
        return targetNode;
    }

    /***************************************************************************************
     * Method: CheckNodeExists
     * Input: key(int)
     * Output: isFound(bool)
     * Purpose: This filters nodes that were created within the tree class as their index 
     *          correlates with the counter. External nodes do not connect or contribute to 
     *          the tree counter and always has an index of -1. 
     *          
     ***************************************************************************************/
    public bool CheckNodeExists(int key)
    {
        bool isFound = false;
        if (key >= 0 && key < counter) 
        { 
            isFound = true; 
        }    
        return isFound;
    }

    /***************************************************************************************
     * Method: FindNode
     * Input: key(int)
     * Output: node(Node)
     * Purpose: Finds a node within the tree by traversing through a queue of node indexes 
     *          starting from the root node and its children (by row) until their index 
     *          matches with the key
     ***************************************************************************************/
    
    //THIS MAY NEVER FINISH DUE TO REPEATED CONNECTIONS-WIP
    public CustomNode FindNode(int key)
    {
        /*
        CustomNode node = null;

        if(key < 0)
        {
            throw new ArgumentOutOfRangeException("Key must be a positive integer.");
        }

        if (this.root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        
        if (!CheckNodeExists(key))
        {
            throw new NullReferenceException("Node does not exist.");
        }

        Queue<CustomNode> queue = new Queue<CustomNode>();
        Queue<CustomNode> allPrevNodes  = new Queue<CustomNode>();
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
                        if(!allPrevNodes.Contains(n))       //Prevent duplicates from being re-added
                        {
                            queue.Enqueue(n);     //Queue children
                        }  
                    }
                }
                allPrevNodes.Enqueue(queue.Dequeue());      //Pop parent to restart the process & store its history
            }
        }

        if (node == null)        //If node could not be found throw exception
        {
            throw new NoNullAllowedException("The node could not be found.");
        }
       
        return node;
        */
        if (this.root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else if (!CheckNodeExists(key))
        {
            throw new NullReferenceException("Node does not exist.");
        }
        else if(this.lastInserted.GetIndex() == key)
        {
            return this.LastInserted;
        }
        else
        {

            return null;
        }
    }

    /***************************************************************************************
     * Method: InsertNode
     * Input: key(int), data (GameObject)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific key
     ***************************************************************************************/
    public CustomNode InsertNodeAt(int key, GameObject data)
    {
        if (root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }

        //Add child node at index
        CustomNode nodeToFind = FindNode(key);
        if (nodeToFind == null)
        {
            throw new NullReferenceException("Node does not exist.");
        }
        CustomNode child = new CustomNode(counter++, data);
        nodeToFind.InsertChildren(child);
        Debug.Log($"Node inserted at Node({key} = {child.Equals(nodeToFind.GetChildren().Last())})");
        return child;
    }

    /***************************************************************************************
     * Method: InsertNode
     * Input: node(CustomNode), data (GameObject)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific node
     ***************************************************************************************/
    public CustomNode InsertNodeAt(CustomNode node, GameObject data)
    {
        if (root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else if(!(this.CheckNodeExists(node.GetIndex())))
        {
            throw new NullReferenceException("Node does not exist.");
        }
        else
        {
            //Add child node at index
            CustomNode child = new CustomNode(counter++, data);
            node.InsertChildren(child);
            Debug.Log($"Node inserted at Node({node.GetIndex()} = {child.Equals(node.GetChildren().Last())})");
            return child;
        }
        
    }

    /***************************************************************************************
     * Method: InsertNode
     * Input: node1(CustomNode), node2(CustomNode)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific node
     ***************************************************************************************/
    public CustomNode InsertNodeAt(CustomNode selectedNode, CustomNode inputNode)
    {
        if (root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else if (!(this.CheckNodeExists(selectedNode.GetIndex())))
        {
            throw new NullReferenceException("Node does not exist.");
        }
        else
        {
            inputNode.SetIndex(counter++);

            //Add child node at index
            selectedNode.InsertChildren(inputNode);

            Debug.Log($"Node inserted at Node({selectedNode.GetIndex()} "
                        + $"= {inputNode.Equals(selectedNode.GetChildren().Last())})");
            return inputNode;
        }
    }

    /***************************************************************************************
     * Method: InsertNode
     * Input: node1(CustomNode), node2(CustomNode)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific node
     ***************************************************************************************/
    public CustomNode InsertNodeAt(CustomNode selectedNode, CustomNode inputNode, int index)
    {
        if (root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else if (!(this.CheckNodeExists(selectedNode.GetIndex())))
        {
            throw new NullReferenceException("Node does not exist.");
        }
        else
        {
            inputNode.SetIndex(counter++);

            //Add child node at index
            selectedNode.InsertChildrenAt(index, inputNode);

            Debug.Log($"Node inserted at Node({selectedNode.GetIndex()} "
                        + $"= {inputNode.Equals(selectedNode.GetChildren()[index])})");
            return inputNode;
        }
    }
    /***************************************************************************************
     * Method: SetTrackerTo
     * Input: trackerIdx(int), nodeIdx (int)
     * Output: N/A
     * Purpose: Retain a reference to desired node
     ***************************************************************************************/
    public void SetTrackerTo(int trackerIdx, int nodeIdx)    
    {
        if(trackerIdx < 0 || trackerIdx >= counter - 1) 
        {
            throw new IndexOutOfRangeException("Tracker node does not exist.");  
        }

        if (!CheckNodeExists(nodeIdx))
        {
            throw new NullReferenceException("Node does not belong within tree.");
        }

        //Copy node details except for data as the tracker's data is its own entity
        this.tracker[trackerIdx] = this.FindNode(nodeIdx);
    }


    /***************************************************************************************
     * Method: Display()
     * Input: N/A
     * Output: output(string)
     * Purpose: Displays all nodes and its children
     ***************************************************************************************/

    //Needs testing

    public string OutputString()
    {

        if (root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }

        CustomNode currNode = null;
        Queue<CustomNode> currLvl = new Queue<CustomNode>();
        Queue<CustomNode> nextLvl = new Queue<CustomNode>();
        Queue<CustomNode> allPrevNodes = new Queue<CustomNode>();
        //List<string> output = new List<string>();
        string data = string.Empty;
        currLvl.Enqueue(this.GetRoot());
        allPrevNodes.Enqueue(this.GetRoot());

        //Loop until last node contains no children
        while (currLvl.Count > 0 && nextLvl.Count > 0)
        {
            //Get front of currLvl
            currNode = currLvl.Dequeue();
            //Enqueue all currLvl children onto the nextLvl for next cycle
            foreach (CustomNode node in currNode.GetChildren())
            {
                if(node !=null)
                {
                    nextLvl.Enqueue(node);
                    allPrevNodes.Enqueue(node);
                    Debug.Log($"{currNode.GetIndex()}: Child ({node.GetIndex()})");
                }
                
            }


            data.Concat($"{currNode.GetIndex()}(");
            foreach (CustomNode node in currNode.GetChildren())
            {
                if (node.Equals(currNode.GetChildren().Last()))
                {
                    data.Concat($"{node.GetIndex()})\t");
                }
                else
                {
                    data.Concat($"{node.GetIndex()},");
                }
            }

            //If currLvl has none left, move onto next one and add data string
            if (currLvl.Count == 0)
            {
                //Add data string to output to re-order later
                //output.Add(data);
                //Reset string OR add new line
                data.Concat("\n");
  

                //When currLvl is empty, transfer from nextLvl
                while (nextLvl.Count != 0)
                {
                    CustomNode tmp = nextLvl.Dequeue();
                    //Prevents repeated nodes from reoccurring and lock loop
                    if (!allPrevNodes.Contains(tmp))     
                    {
                        currLvl.Enqueue(tmp);
                    }
                }
            }
        }

        return data;
    }

    /****************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}

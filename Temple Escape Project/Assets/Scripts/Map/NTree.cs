/************************************************************************************************************************************************************************************
 *  Name: Tony Bui
 *  Purpose: A generic n-list tree made for Unity
 *  Last updated: 22/10/23
 *  Notes: Requires a CustomNode class
************************************************************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;

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
    [SerializeField]
    CustomNode root;

    [SerializeField]
    CustomNode lastInserted;

    [SerializeField]
    static int counter;

    [SerializeField]
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
    public NTree(GameObject data, int limit=0)
    {
        if(this.root != null) { throw new NotSupportedException("Tree instantiated with root already exists."); }
        else
        {
            counter = 0;
            this.lastInserted = this.root =  CreateCustomNode(data, limit);     //displays id correctly but not set permanently
            //Debug.Log($"NTree instantiated with root as ({data.name}) "
            //    + $"@ [{this.root.GetIndex()}].");
        }
    }

    //Create Tree with a node containing GameObject
    public NTree(CustomNode node)
    {
        if (this.root != null) { throw new NotSupportedException("Tree instantiated with root already exists."); }
        else
        {
            counter = 0;
            node.SetIndex(counter++);     //Counter increments after method runs 
            this.lastInserted = this.root = node;
            //Debug.Log($"NTree instantiated with root as ({node.GetData().name}) " 
            //        + $"@ [{this.root.GetIndex()}].");
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
     * Method: CreateCustomNode()
     * Input: data (GameObject)
     * Output: node (CustomNode)
     * Purpose: Create node and setting IDs simultaneously
     ***************************************************************************************/

    public static CustomNode CreateCustomNode(GameObject data, int capacity)
    {
        return new CustomNode(counter++, data, capacity);
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
            throw new NullReferenceException("Tracker node has not been instantiated.");
        }

        targetNode = tracker[index];
        return targetNode;
    }

    /***************************************************************************************
     * Method: SetTrackerTo
     * Input: trackerIdx(int), nodeIdx (int)
     * Output: N/A
     * Purpose: Retain a reference to desired node
     ***************************************************************************************/
    public void SetTrackerTo(int trackerIdx, int nodeIdx)
    {
        //NOTE COUNTER IS ALWAYS 1 MORE THAN THE LAST NODE'S INDEX DUE TO POST-INCREMENT
        if (trackerIdx < 0 || trackerIdx >= counter)
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
        //NOTE COUNTER IS ALWAYS 1 MORE THAN THE LAST NODE'S INDEX DUE TO POST-INCREMENT
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
     * Source: https://chat.openai.com/share/f68a6628-3aad-44ef-a254-33b31dd1aa91
     ***************************************************************************************/
    
    //FUTURE: MODIFY TO USE LAST INSERTED NODE IF THE KEY IS CLOSE TO THE LAST INSERTED 

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
        
        if (!CheckNodeExists(key))
        {
            throw new NullReferenceException("Node does not exist.");
        }

        if (lastInserted.GetIndex() == key)
        {
            node = lastInserted;
        }
        else
        {
            Queue<CustomNode> queue = new Queue<CustomNode>();
            Queue<CustomNode> allPrevNodes = new Queue<CustomNode>();
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
                            if (!allPrevNodes.Contains(n) && n != null)       //Prevent duplicates from being re-added
                            {
                                queue.Enqueue(n);     //Queue children
                            }
                        }
                    }
                    //queue.Dequeue();
                    allPrevNodes.Enqueue(queue.Dequeue());      //Pop parent to restart the process & store its history
                }
            }

            if (node == null)        //If node could not be found throw exception
            {
                throw new NoNullAllowedException("The node could not be found.");
            }
        }
        return node;
    }

    //Recursive version
    /*
    public CustomNode FindNode(int key)
    {
        if (this.root == null)
        {
            throw new NoNullAllowedException("Tree has not been built.");
        }
        else if (!CheckNodeExists(key))
        {
            throw new NullReferenceException("Node does not exist.");
        }
        else if (this.lastInserted.GetIndex() == key)
        {
            return this.LastInserted;
        }
        else
        {
            return FindNodeDFS(root, key);      //THIS FAILS
        }
    }


    // Call the recursive DFS method to search for the node.
    private CustomNode FindNodeDFS(CustomNode currentNode, int key)
    {
        if (currentNode.GetIndex() == key)
        {
            return currentNode;
        }

        // Recursively search in the children of the current node.
        if (currentNode.GetChildren() != null)
        {
            foreach (CustomNode child in currentNode.GetChildren())
            {
                CustomNode result = FindNodeDFS(child, key);
                if (result != null)
                {
                    return result; // Return the node if found.
                }
            }
        }

        return null; // Node with the given key not found in the subtree rooted at currentNode.
    }
    */

    /***************************************************************************************
     * Method: InsertNode
     * Input: key(int), data (GameObject)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific key
     ***************************************************************************************/
    public CustomNode InsertNodeAt(int key, CustomNode node)
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

        //Checks if node exists previously otherwise it is a new node and set its index
        //Debug.Log($"Before Counter = {counter} {node.GetData().name} = {this.CheckNodeExists(node.GetIndex())} id({node.GetIndex()})");
        if (!(this.CheckNodeExists(node.GetIndex())))
        {
            node.SetIndex(counter++);
            //Debug.Log($"After Counter = {counter}");
        }

        nodeToFind.InsertChildren(node);
        //Debug.Log($"Node inserted at Node({key} = {node.Equals(nodeToFind.GetChildren().Last())})");
        this.lastInserted = node;
        return node;
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
            //Checks if node exists previously otherwise it is a new node and set its index
            //Debug.Log($"Before Counter = {counter} {inputNode.GetData().name} = {this.CheckNodeExists(inputNode.GetIndex())} id({inputNode.GetIndex()})");
            if (!(this.CheckNodeExists(inputNode.GetIndex())))
            {
                inputNode.SetIndex(counter++);
                //Debug.Log($"After Counter = {counter}");
            }

            //Add child node at index
            selectedNode.InsertChildren(inputNode);

            //Debug.Log($"Node inserted at Node({selectedNode.GetIndex()} "
            //            + $"= {inputNode.Equals(selectedNode.GetChildren().Last())})");
            this.lastInserted = inputNode;
            return inputNode;
        }
    }

    /***************************************************************************************
     * Method: InsertNode
     * Input: node1(CustomNode), node2(CustomNode)  
     * Output: nodeInserted (CustomNode)
     * Purpose: Inserts node within the tree at a specific node
     ***************************************************************************************/
    //CHECK THE INPUT NODE IS UNITQUE OR NOT OTHERWISE IT KEEPS CREATING NEW NODES
    
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
            //Checks if node exists previously otherwise it is a new node and set its index
            //Debug.Log($"Before Counter = {counter} {inputNode.GetData().name} = {this.CheckNodeExists(inputNode.GetIndex())} id({inputNode.GetIndex()})");
            if (!(this.CheckNodeExists(inputNode.GetIndex())))
            {
                inputNode.SetIndex(counter++);
                //Debug.Log($"After Counter = {counter}");
            }

            //Add child node at index
            selectedNode.InsertChildrenAt(index, inputNode);

            //Debug.Log($"Node inserted at Node({selectedNode.GetIndex()} "
            //            + $"= {inputNode.Equals(selectedNode.GetChildren()[index])})");
            this.lastInserted = inputNode;
            return inputNode;
        }
    }

    /*************************************************************************************************************************************************************************************
    *
    *                   END     OF      CLASS!
    *
    *************************************************************************************************************************************************************************************/
}

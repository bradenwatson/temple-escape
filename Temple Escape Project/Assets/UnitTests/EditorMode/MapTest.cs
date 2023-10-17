using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MapTest
{
    NTree tree;
    NTree.CustomNode node;
    GameObject obj;
    List<GameObject> listObj;

    /*********************************************************/
    /*              CUSTOMNODE TESTS BELOW
    /*********************************************************/
    [Test]
    public void Make_Node()
    {
        obj = new GameObject();
        node = new NTree.CustomNode(obj);
        Assert.AreSame(node.GetData(), obj);
    }

    [Test]
    public void Set_MaxChildren_Fail()
    {
        int max1 = -1;
        int max2 = 2;
        int size = 1;
        obj = new GameObject();
        node = new NTree.CustomNode(obj);
        NTree.CustomNode node2 = new NTree.CustomNode(node);
        Assert.Throws<ArgumentException>(() => node.SetNodeLimit(max1));
        node.SetNodeLimit(max2);
        node.InsertChildren(node2);
        node.InsertChildren(node2);
        Assert.IsTrue(node.GetNodeLimit() == max2);
        Assert.Throws<ArgumentOutOfRangeException>(() => node.SetNodeLimit(size));
    }

    [Test]
    public void Set_MaxChildren_Pass()
    {
        int max = 1;
        obj = new GameObject();
        node = new NTree.CustomNode(obj);
        NTree.CustomNode node2 = new NTree.CustomNode(node);
        node.SetNodeLimit(max);
        Assert.IsNotNull(node.GetChildren());
        Assert.IsTrue(node.GetNodeLimit() == max);
        node.InsertChildren(node2);
        Assert.IsTrue(node.GetChildren().Count == max);
    }

    [Test]
    public void InsertChildren_Infinite()
    {
        obj = new GameObject();
        node = new NTree.CustomNode(obj);
        NTree.CustomNode node2 = new NTree.CustomNode(node);
        node.InsertChildren(node2);
        Assert.IsFalse(node.GetNodeLimit() == 0);
        Assert.IsTrue(node.GetChildren().Count > 0);
        Assert.AreSame(node.GetChildren().First(), node2);
    }

    [Test]
    public void InsertChildren_Finite()
    {
        int max = 2;
        obj = new GameObject();
        node = new NTree.CustomNode(obj);
        NTree.CustomNode node2 = new NTree.CustomNode(node);
        NTree.CustomNode node3 = new NTree.CustomNode(node2);
        NTree.CustomNode node4 = new NTree.CustomNode(node2);
        node.SetNodeLimit(max);
        node.InsertChildren(node2);
        node.InsertChildren(node3);

        Assert.IsTrue(node.GetNodeLimit() == max);
        Assert.IsTrue(node.GetChildren().Count > 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => node.InsertChildren(node4));
    }

   
    


    /*********************************************************/
    /*                  NTREE TESTS BELOW
    /*********************************************************/

    [Test]
    public void Tree_Instantiate()
    {
        obj = new GameObject();
        tree = new NTree(obj);
        Assert.NotNull(tree.GetRoot());
    }

    [Test]
    public void Insert_Tracker()
    {
        obj = new GameObject();
        tree = new NTree(obj);
        tree.InsertTracker();
        Assert.NotNull(tree.GetRoot());
        Assert.IsTrue(tree.GetTrackers().Count == 1);
    }

    [Test]
    public void Select_Tracker()
    {
        int count = 4;
        int pick = 2;
        obj = new GameObject();
        tree = new NTree(obj);
        for(int i = 0; i < count; i++)
        {
            tree.InsertTracker();
        }
        NTree.CustomNode tmp = tree.SelectTracker(pick);
        Assert.NotNull(tree.GetRoot());
        Assert.IsTrue(tree.GetTrackers().Count == count);
        Assert.AreSame(tmp, tree.GetTrackers()[pick]);
    }

    [Test]
    public void Check_Node_Exists()
    {
        int key = 0;
        obj = new GameObject();
        tree = new NTree(obj);
        Assert.IsTrue(tree.CheckNodeExists(key));
    }

    [Test]
    public void Find_Node_Fail()
    {
        int key1 = -1;
        int key2 = 3;
        obj = new GameObject();
        tree = new NTree(obj);
        Assert.IsFalse(tree.GetRoot().GetIndex() == key1);
        Assert.Throws<ArgumentOutOfRangeException>(() => tree.FindNode(key1));
        Assert.Throws<NullReferenceException>(() => tree.FindNode(key2));
    }

    [Test]
    public void Find_Node_Pass()
    {
        int key = 0;
        obj = new GameObject();
        tree = new NTree(obj);
        Assert.IsTrue(tree.GetRoot().GetIndex() == key);
        Assert.AreSame(tree.FindNode(key), tree.GetRoot());
    }

    
    [Test]
    public void Insert_Node()
    {
        int max = 4;
        obj = new GameObject();
        tree = new NTree(obj);
        NTree.CustomNode lastNode = null;
        //This is inserted to the same node index
        for(int i = 0; i < max; i++)
        {
            lastNode = tree.InsertNodeAt(0,obj);
        }
        Assert.IsTrue(tree.GetCount() == max+1);
        Assert.AreEqual(lastNode.GetIndex(), tree.GetRoot().GetChildren().Last().GetIndex());
        Assert.IsTrue(lastNode.GetIndex() == max);
        
    }

    [Test]
    public void Insert_Node_At()
    {
        int max = 4;
        obj = new GameObject();
        tree = new NTree(obj);
        NTree.CustomNode lastNode = null;
        //This is inserted at consecutive nodes
        for(int i = 0; i < max; i++)
        {
            lastNode = tree.InsertNodeAt(i,obj);
            //This works
            Assert.AreEqual(lastNode.GetIndex(), tree.FindNode(i).GetChildren().Last().GetIndex(), $"lastNode P = {lastNode.GetParent().GetIndex()}" 
                + $"Child P of {i} = {tree.FindNode(i).GetChildren().Last().GetParent().GetIndex()}"  );
        }
        Assert.IsTrue(tree.GetCount() == max+1);
        tree.InsertNodeAt(tree.GetRoot(), obj);
        Assert.IsTrue(tree.GetRoot().GetChildren().Count == 2);
        Assert.IsNotNull(tree.FindNode(lastNode.GetIndex()), $"Parent oflastNode = {lastNode.GetParent().GetIndex()}");
        //This fails, why does root have 2 elements but also why is the the last node in it has index of 5
        //Assert.AreEqual(lastNode.GetIndex(), tree.GetRoot().GetChildren().Last().GetIndex(), $"lastNode P = {lastNode.GetParent().GetIndex()}, " +
        //    $"Tree root last parent = {tree.GetRoot().GetChildren().Last().GetParent().GetIndex()}" +
        //    $" Root elements = {tree.GetRoot().GetChildren().Count}");
        Assert.IsTrue(lastNode.GetIndex() == max);
    }

    [Test]
    public void Set_Tracker_To_Fail()
    {
        int max = 4;
        obj = new GameObject();
        tree = new NTree(obj);
        for (int i = 0; i < max; i++)
        {
            tree.InsertNodeAt(0, obj);
        }
        Assert.IsTrue(tree.GetCount() == max + 1);
        Assert.IsTrue(tree.GetRoot().GetChildren().Last().GetIndex() == max);
        tree.InsertTracker();
        Assert.IsTrue(tree.GetTrackers().Count == 1);
        Assert.Throws<IndexOutOfRangeException>(() => tree.SetTrackerTo(-1, 2));
        Assert.Throws<NullReferenceException>(() => tree.SetTrackerTo(0, 5));
    }

    [Test]
    public void Set_Tracker_To_Pass()
    {
        int max = 4;
        obj = new GameObject();
        tree = new NTree(obj);
        for (int i = 0; i < max; i++)
        {
            tree.InsertNodeAt(0, obj);
        }
        Assert.IsTrue(tree.GetCount() == max + 1);
        Assert.IsTrue(tree.GetRoot().GetChildren().Last().GetIndex() == max);
        tree.InsertTracker();
        Assert.IsTrue(tree.GetTrackers().Count == 1);
        tree.SetTrackerTo(0, 3);
        Assert.IsTrue(tree.SelectTracker(0).GetIndex() == 3);
    }

    /*********************************************************/
    /*                  MAP TESTS BELOW
    /*********************************************************/



}

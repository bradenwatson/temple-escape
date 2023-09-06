using System;
using System.Collections;
using System.Collections.Generic;
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
    /*                  NTREE TESTS BELOW
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
        Assert.IsTrue(node.GetNodeLimit() == 0);
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
    /*              CUSTOMNODE TESTS BELOW
    /*********************************************************/
    [Test]
    public void Tree_Insert()
    {
        obj = new GameObject();
        tree = new NTree(obj);
        Assert.NotNull(tree.GetRoot());
    }


    [Test]
    public void Tree_Insert_Trackers()
    {
        int count = 2;
        obj = new GameObject();
        tree = new NTree(obj, count);
        Assert.NotNull(tree.GetRoot());
        Assert.IsTrue(tree.GetTrackers().Count == count);
    }

    [Test]
    public void Insert_Tracker()
    {
        int count = 3;
        obj = new GameObject();
        tree = new NTree(obj);
        tree.InsertTracker(count);
        Assert.NotNull(tree.GetRoot());
        Assert.IsTrue(tree.GetTrackers().Count == count);
    }


    [Test]
    public void Select_Tracker()
    {
        int count = 4;
        int pick = 2;
        obj = new GameObject();
        tree = new NTree(obj, count);
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
        for(int i = 0; i < max; i++)
        {
            tree.InsertNode(0,obj);
        }
        Assert.IsTrue(tree.GetCount() == max+1);
        Assert.IsTrue(tree.GetRoot().GetChildren().Last().GetIndex() == max);
    }
    
    
}

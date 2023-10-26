using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Destructable : MonoBehaviour
{
    [SerializeField] GameObject Normal;
    [SerializeField] GameObject Destroyed;
    [SerializeField] GameObject SpawnPoint;

    List<string> CollisionDestructableCollection;

    private void Awake()
    {
        // List of XR Grab items that can break the item
        CollisionDestructableCollection = new()
        {
            //"Ground",
            "Wall", "Brazier", "Vase",
            "LHS Wall", "LHS Entry Wall",
            "RHS Wall", "RHS Entry Wall",
            "Front Wall", "Front Entry Wall",
            "Back Wall", "Back Entry Wall",
            "Door", "Stairs", "Table", "Enemy Anubis"
        };
    }

    private void OnCollisionEnter(Collision collider)
    {
        // one of the destructable items in the list above
        foreach(var name in CollisionDestructableCollection)
        {
            if (collider.gameObject.name == name)
            {
                Instantiate(Destroyed, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                Destroy(Normal);
            }
        }
    }

    // NOTE: You won't need this part of the script for your game, it was a part required for an event in my game.
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "XR Origin")
        {
            Instantiate(Destroyed, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
            Destroy(Normal);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class something
{
    public int x; public int y;
    public List<string> directionsToGoIN = new List<string>();


    public something()
    {

    }
}
public class NewBehaviourScript : MonoBehaviour
{

    public LayerMask LookMask;
    public Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit)) 
        {
           
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

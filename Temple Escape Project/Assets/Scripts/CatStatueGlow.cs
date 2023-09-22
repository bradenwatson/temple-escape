using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatStatueGlow : MonoBehaviour
{
    public Material originalMaterial;
    public Material glowMaterial;
    public GameObject parent;

    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        parent.gameObject.tag = "Untagged";
        
    }

    void Update()
    {
        if (parent.gameObject.tag == "Glow")
        {
            meshRenderer.material = glowMaterial;
        }
        else
        {
            meshRenderer.material = originalMaterial;
        }
    }
}

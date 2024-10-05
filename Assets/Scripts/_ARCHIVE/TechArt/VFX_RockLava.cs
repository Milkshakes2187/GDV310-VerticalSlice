using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_RockLava : MonoBehaviour
{
    public MeshRenderer lavaMeshRend;
    public Material lavaMaterial;
    [Range(0f, 1f)] public float lavaAmount;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            Material newMat = new Material(lavaMaterial);
            lavaMaterial = newMat;
            lavaMeshRend.materials[1] = lavaMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //lavaMaterial.SetFloat("_LavaAmount", lavaAmount);
        lavaMeshRend.materials[1].SetFloat("_LavaAmount", lavaAmount);
    }
}

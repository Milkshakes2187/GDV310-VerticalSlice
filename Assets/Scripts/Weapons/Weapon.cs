using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    List<Collider> colliders = new List<Collider>();
    List<Character> hitChars = new List<Character>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Collider col in GetComponentsInChildren<Collider>())
        {
            colliders.Add(col);
        }
        DeactivateHitbox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateHitbox()
    {
        foreach(Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    public void DeactivateHitbox()
    {
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }

}

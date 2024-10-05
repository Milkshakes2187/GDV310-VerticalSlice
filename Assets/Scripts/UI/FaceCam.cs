using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCam : MonoBehaviour
{
    void Update()
    {
        Debug.Log("rotating");
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
    }
}

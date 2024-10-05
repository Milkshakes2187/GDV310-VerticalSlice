using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    public List<GameObject> Threads;
    public List<GameObject> ThreadPoints;

    public float fInnerRadius = 20;
    public float fOuterRadius = 40;


    public List<GameObject> ReaverSpawns;
    public List<GameObject> ReaverOppositeSpawns;

    public GameObject ReaverPF;
    public GameObject ThreadPointPF;

    private void Start()
    {
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            float fStep = 360f / 6f;
            Vector3 fCenter = Vector3.zero;

            for (int i = 0; i < 6; i++)
            {
                float angle = Mathf.Deg2Rad * (fStep * i);

                float x = fCenter.x + fInnerRadius * Mathf.Cos(angle);
                float y = fCenter.y + fInnerRadius * Mathf.Sin(angle);

                Instantiate(ThreadPointPF, new Vector3(x, y, fCenter.z), Quaternion.identity);
            }
        }

       //// pick a random reaver spawn position
       //int randomReaver = Random.Range(0, ReaverSpawns.Count);
       //
       //// Spawn both the reaver and another reaver on the opposite side of the map
       //Instantiate(ReaverPF, ReaverSpawns[randomReaver].transform.position, Quaternion.identity);
       //Instantiate(ReaverPF, ReaverOppositeSpawns[randomReaver].transform.position, Quaternion.identity);
    }
}

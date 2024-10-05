using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    public List<GameObject> Threads;
    public List<GameObject> ThreadInnerPoints;
    public List<GameObject> ThreadOuterPoints;

    public float fInnerRadius = 20;
    public float fOuterRadius = 40;


    public List<GameObject> ReaverSpawns;
    public List<GameObject> ReaverOppositeSpawns;

    public GameObject ReaverPF;
    public GameObject ThreadPF;
    public GameObject ThreadPointPF;

    private void Start()
    {
        float fStep = 360f / 6f;
        Vector3 fCenter = Vector3.zero;

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (fStep * i);

            float fInnerX = fCenter.x + fInnerRadius * Mathf.Cos(angle);
            float fInnerZ = fCenter.z + fInnerRadius * Mathf.Sin(angle);

            float fOuterX = fCenter.x + fOuterRadius * Mathf.Cos(angle);
            float fOuterZ = fCenter.z + fOuterRadius * Mathf.Sin(angle);

            var inner = Instantiate(ThreadPointPF, new Vector3(fInnerX, fCenter.y, fInnerZ), Quaternion.identity);
            ThreadInnerPoints.Add(inner);

            var outer = Instantiate(ThreadPointPF, new Vector3(fOuterX, fCenter.y, fOuterZ), Quaternion.identity);
            ThreadOuterPoints.Add(outer);
        }

        for (int i = 0; i < 6; i++)
        {
            var innerThread = Instantiate(ThreadPF, ThreadInnerPoints[i].transform.position, Quaternion.identity);
            var outerThread = Instantiate(ThreadPF, ThreadOuterPoints[i].transform.position, Quaternion.identity);

            if (i == 5)
            {
                innerThread.transform.LookAt(ThreadInnerPoints[0].transform);
                outerThread.transform.LookAt(ThreadOuterPoints[0].transform);
            }
            else
            {
                innerThread.transform.LookAt(ThreadInnerPoints[i + 1].transform);
                outerThread.transform.LookAt(ThreadOuterPoints[i + 1].transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            
        }

       //// pick a random reaver spawn position
       //int randomReaver = Random.Range(0, ReaverSpawns.Count);
       //
       //// Spawn both the reaver and another reaver on the opposite side of the map
       //Instantiate(ReaverPF, ReaverSpawns[randomReaver].transform.position, Quaternion.identity);
       //Instantiate(ReaverPF, ReaverOppositeSpawns[randomReaver].transform.position, Quaternion.identity);
    }
}

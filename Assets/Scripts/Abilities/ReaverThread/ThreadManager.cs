using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    public List<GameObject> Threads;
    public List<GameObject> ReaverSpawns;
    public List<GameObject> ReaverOppositeSpawns;

    public GameObject ReaverPF;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // pick a random reaver spawn position
            int randomReaver = Random.Range(0, ReaverSpawns.Count);

            // Spawn both the reaver and another reaver on the opposite side of the map
            Instantiate(ReaverPF, ReaverSpawns[randomReaver].transform.position, Quaternion.identity);
            Instantiate(ReaverPF, ReaverOppositeSpawns[randomReaver].transform.position, Quaternion.identity);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ThreadManager : Ability
{
    [Tab("Values")]
    public float fInnerRadius = 20;
    public float fOuterRadius = 40;

    public int iTotalPoints = 6; // set the amount of points on the circle to place a thread point (e.g. 6 = Hexagon)

    //public List<GameObject> ReaverSpawns;
    //public List<GameObject> ReaverOppositeSpawns;

    [Tab("Data")]
    [Header("Thread Points")]
    public List<GameObject> ThreadInnerPoints;
    public List<GameObject> ThreadOuterPoints;

    [Header("Threads")]
    public List<GameObject> edgeThreads;
    public List<GameObject> inbetweenThreads;

    [Tab("References")]
    [Header("Abilities")]
    public AbilitySO interwovenThreads;
    public AbilitySO wovenReality;

    [Header("Prefabs")]
    public GameObject ThreadPF;
    public GameObject ThreadPointPF;
    //public GameObject ReaverPF;

    private void Start()
    {
        InitializeArenaThreads();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InterwovenThreads();
        }
    }

    /***********************************************
    * UseSpellEffect: Overriden spell effect, creates a shadow assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        InitializeArenaThreads();
    }

    /***********************************************
    * InitializeArenaThreads: Initializes the threads for the thread weaver arena.
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void InitializeArenaThreads()
    {
        float fStep = 360f / iTotalPoints;
        Vector3 fCenter = Vector3.zero;

        // iterate through the points and place down a threadpoint forming a hexagon
        for (int i = 0; i < iTotalPoints; i++)
        {
            /* 
             Create a hexagon by placing 6 points at even spaces around a circle
             --- Placement always starts on the right side going counter-clockwise
            
                    3   2
                    *   *
                4           1
                *           *
                    5   6
                    *   *
            */

            // Set the current angle step
            float angle = Mathf.Deg2Rad * (fStep * i);

            // Calculate the world position for the inner and outer radius thread points using the angle step
            float fInnerX = fCenter.x + fInnerRadius * Mathf.Cos(angle);
            float fInnerZ = fCenter.z + fInnerRadius * Mathf.Sin(angle);

            float fOuterX = fCenter.x + fOuterRadius * Mathf.Cos(angle);
            float fOuterZ = fCenter.z + fOuterRadius * Mathf.Sin(angle);

            // Instantiate an inner and outer thread point
            var inner = Instantiate(ThreadPointPF, new Vector3(fInnerX, fCenter.y, fInnerZ), Quaternion.identity, transform);
            ThreadInnerPoints.Add(inner);

            var outer = Instantiate(ThreadPointPF, new Vector3(fOuterX, fCenter.y, fOuterZ), Quaternion.identity, transform);
            ThreadOuterPoints.Add(outer);
        }

        // Iterate through each thread point and place a thread facing towards the next thread point
        for (int i = 0; i < iTotalPoints; i++)
        {
            int nextPoint = (i == iTotalPoints - 1) ? 0 : i + 1; // Determine the next point index

            // Instantiate an inner and outer thread
            InstantiateThread(ThreadInnerPoints[i].transform.position, ThreadInnerPoints[nextPoint].transform.position, edgeThreads);
            InstantiateThread(ThreadOuterPoints[i].transform.position, ThreadOuterPoints[nextPoint].transform.position,  edgeThreads);

            // Instantiate in-between threads (Threads going from the outer points to the inner points as well as the inner points to the center of arena)
            InstantiateThread(ThreadInnerPoints[i].transform.position, fCenter, inbetweenThreads);
            InstantiateThread(ThreadOuterPoints[i].transform.position, ThreadInnerPoints[i].transform.position, inbetweenThreads);
        }
    }

    /***********************************************
    * InstantiateThread: Used within the Initialize InitializeArenaThreads to instantiate the threads, face them the correct direction and add them to a list
    * @author: Juan Le Roux
    * @parameter: GameObject, Vector3, Vector3, List
    * @return: void
    ************************************************/
    GameObject InstantiateThread(Vector3 _v3Pos, Vector3 _v3LookDir, List<GameObject> _threadList)
    {
        // create the thread and add it to its designated list
        var thread = Instantiate(ThreadPF, _v3Pos, Quaternion.identity, transform);
        float fDistBetweenPoints = Vector3.Distance(_v3Pos, _v3LookDir);

        // Get the mesh component and set its size and scale
        var threadMesh = thread.transform.GetChild(0);
        threadMesh.transform.localScale = new Vector3(0.1f, 1, fDistBetweenPoints);
        threadMesh.transform.localPosition = new Vector3(0, 0, fDistBetweenPoints/2);

        // face thread towards next point
        thread.transform.LookAt(_v3LookDir);

        _threadList.Add(thread);

        return thread;
    }

    /***********************************************
    * ResetThreads: Resets all threads to inactive
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public void ResetThreads()
    {
        // Search through all edge threads and deactivate them
        foreach (var thread in edgeThreads)
        {
            thread.GetComponent<Thread>().ChangeThreadState(false);
        }
    }

    /***********************************************
    * InterwovenThreads: casts Interwoven threads
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public void InterwovenThreads()
    {
        var ability = interwovenThreads.InitialiseAbility(owner, target, Vector3.zero);
        ability.GetComponent<Ability>().CastSpell();
    }

    /***********************************************
    * WovenReality: Casts Woven Reality which starts the intermission
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    void WovenReality() // TODO: Efficientize this + comments
    {
        wovenReality.InitialiseAbility(owner, target, Vector3.zero);
    }

}

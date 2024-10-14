using UnityEngine;

public class InterwovenThreads : Ability
{
    public int interwovenThreadsCount = 5;

    ThreadManager threadManager;

    /***********************************************
    * UseSpellEffect: Overriden spell effect, creates a shadow assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        threadManager = FindFirstObjectByType<ThreadManager>();

        threadManager.ResetThreads();

        int threadsActive = 0;

        while (threadsActive < interwovenThreadsCount)
        {
            int randomThread = Random.Range(0, threadManager.edgeThreads.Count);

            if (!threadManager.edgeThreads[randomThread].GetComponent<Thread>().isThreadActive)
            {
                threadManager.edgeThreads[randomThread].GetComponent<Thread>().ChangeThreadState(true);

                threadsActive++;
            }
        }

        Destroy(gameObject);
    }
}

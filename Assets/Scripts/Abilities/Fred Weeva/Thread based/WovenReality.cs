using UnityEngine;

public class WovenReality : Ability
{
    ThreadManager threadManager;

    public int wovenRealityCount = 3;

    /***********************************************
    * UseSpellEffect: Overriden spell effect, creates a shadow assassin on targets position
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void UseSpellEffect()
    {
        threadManager = FindFirstObjectByType<ThreadManager>();

        int threadsActive = 0;

        while (threadsActive < wovenRealityCount)
        {
            int randomThread = Random.Range(0, threadManager.inbetweenThreads.Count);

            if (!threadManager.edgeThreads[randomThread].GetComponent<Thread>().isThreadActive)
            {
                threadManager.edgeThreads[randomThread].GetComponent<Thread>().ChangeThreadState(true);
                threadManager.edgeThreads[randomThread + 1].GetComponent<Thread>().ChangeThreadState(true);

                threadsActive++;
            }
        }
    }
}

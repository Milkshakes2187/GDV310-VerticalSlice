using UnityEngine;

public class ThreadCollision : MonoBehaviour
{
    public Thread parentThread;

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var character = other.gameObject.GetComponent<Character>();

        if (character && parentThread.isThreadActive)
        {
            parentThread.CharacterCollided(character);
        }
    }
}

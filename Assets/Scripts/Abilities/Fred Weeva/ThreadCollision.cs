using UnityEngine;

public class ThreadCollision : MonoBehaviour
{
    Thread parentThread;

    private void Start()
    {
        parentThread = transform.root.GetComponent<Thread>();
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

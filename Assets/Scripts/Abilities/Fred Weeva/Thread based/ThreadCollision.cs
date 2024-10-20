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

        // Check if there is a collision with a character
        if (character && parentThread.isThreadActive)
        {
            // ensure the player is in the scene
            if (Player.instance)
            {
                // if the character is a player then call the PlayerCollided() function
                if (character == Player.instance)
                {
                    parentThread.PlayerCollided(character);
                }
            }
            else
            {
                Debug.LogWarning("No player in scene");
            } 
        }
    }
}

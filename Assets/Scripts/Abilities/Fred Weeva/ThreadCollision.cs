using UnityEngine;

public class ThreadCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var character = other.gameObject.GetComponent<Character>();

        if (character)
        {
            character.TakeDamage(50);
        }
    }
}

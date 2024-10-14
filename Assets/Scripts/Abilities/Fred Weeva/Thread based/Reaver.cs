using Unity.VisualScripting;
using UnityEngine;

public class Reaver : Ability
{
    private void Update()
    {
        // make reaver face player
        // wait cast duration
        // travel towards position player was at when cast went off
    }
    private void OnTriggerEnter(Collider other)
    {
        // if the reaver hits the thread disable the thread
        if (other.tag == "Thread")
        {
            other.gameObject.SetActive(false);
        }
    }

    public override void UseSpellEffect()
    {

    }
}

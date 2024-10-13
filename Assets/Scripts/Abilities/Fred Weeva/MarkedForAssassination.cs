using UnityEngine;

public class MarkedForAssassination : Ability
{
    // TODO: make sure to rework this ability to work with georges ability system
    // TODO: Comment once reworked

    public GameObject indicatorPF;
    public GameObject assassinPF;

    GameObject indicator;

    public override void UseSpellEffect()
    {
        Instantiate(assassinPF, target.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

   /***********************************************
   * UseSpellEffect: Abstract function to perform inituial setup. Overridden by children
   * @author: George White
   * @parameter:
   * @return: abstract void
   ************************************************/
    public override void InitialSetup()
    {
        indicator = Instantiate(indicatorPF, target.transform.position, Quaternion.identity);
        Destroy(indicator, timeToCast);
    }

    private void Update()
    {
        if (indicator)
        {
            indicator.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 4, target.transform.position.z);
        }
    }
}

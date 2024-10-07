using UnityEngine;

public class AbilityTest1 : Ability
{
   

    public override void CastSpell()
    {
        base.CastSpell();


        Debug.Log("spell!");
    }
}

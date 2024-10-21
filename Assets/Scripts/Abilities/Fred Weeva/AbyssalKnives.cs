using System.Collections.Generic;
using UnityEngine;

public class AbyssalKnives : Ability
{
    Vector3 targetPosShifted;
    public GameObject projectilePF;

    List<GameObject> projectiles = new List<GameObject>();
    public List<GameObject> indicators = new List<GameObject>();

    public float projectileSpeed = 5f;
    public float range = 20f;
    public float angleOffset = 15f;

    private void Update()
    {
        // Check if projectiles exist
        if (projectiles.Count > 0)
        {
            int projectilesAlive = 0;

            // for each projectile that exists move it forward at a designated speed
            foreach (var projectile in projectiles)
            {
                if (projectile)
                {
                    projectile.transform.position += projectile.transform.forward * projectileSpeed * Time.deltaTime;
                    projectilesAlive++;
                }
            }

            // if all projectiles have reached the end of their life cycle destroy this parent object
            if (projectilesAlive <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    /***********************************************
   * UseSpellEffect: Overriden spell effect, creates projectiles and positions them to face the target
   * @author: Juan Le Roux
   * @parameter:
   * @return: void
   ************************************************/
    public override void UseSpellEffect()
    {
        // Disable the indicators when the ability activates
        foreach (var indicator in indicators)
        {
            indicator.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            var projectile = Instantiate(projectilePF, transform.position, Quaternion.identity, transform);
            Destroy(projectile, range / projectileSpeed);

            projectiles.Add(projectile);
        }

        projectiles[0].transform.LookAt(targetPosShifted);

        Quaternion leftRotation = Quaternion.Euler(0, -angleOffset, 0);
        projectiles[1].transform.rotation = Quaternion.LookRotation(leftRotation * (targetPosShifted - projectiles[1].transform.position));

        Quaternion rightRotation = Quaternion.Euler(0, angleOffset, 0);
        projectiles[2].transform.rotation = Quaternion.LookRotation(rightRotation * (targetPosShifted - projectiles[2].transform.position));
    }

    /***********************************************
    * UseSpellEffect: Overriden inituial setup. 
    * @author: Juan Le Roux
    * @parameter:
    * @return: void
    ************************************************/
    public override void InitialSetup()
    {
        // Enable the indicators and position in the correct look directions
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].SetActive(true);
        }

        // shift the Y position slightly higher than the ground
        targetPosShifted = target.transform.position;
        targetPosShifted.y = transform.position.y + 1;

        // Set the first indicator to look straight forward from the owning character
        indicators[0].transform.LookAt(targetPosShifted);

        // Shift the second indicator to the left by the angleOffset variable
        Quaternion leftRotation = Quaternion.Euler(0, -angleOffset, 0);
        indicators[1].transform.rotation = Quaternion.LookRotation(leftRotation * (targetPosShifted - indicators[1].transform.position));

        // Shift the third indicator to the right by the angleOffset variable
        Quaternion rightRotation = Quaternion.Euler(0, angleOffset, 0);
        indicators[2].transform.rotation = Quaternion.LookRotation(rightRotation * (targetPosShifted - indicators[2].transform.position));
    }
}

using System.Collections.Generic;
using UnityEngine;

public class AbyssalKnives : Ability
{
    Vector3 targetPosShifted;
    public GameObject projectilePF;

    List<GameObject> projectiles = new List<GameObject>();

    public float projectileSpeed = 5f;
    public float range = 10f;

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
        // Get the target position and shift its Y to the same height as the position it was fired from
        targetPosShifted = target.transform.position;
        targetPosShifted.y = transform.position.y;

        for (int i = 0; i < 3; i++)
        {
            var projectile = Instantiate(projectilePF, transform.position, Quaternion.identity, transform);
            Destroy(projectile, range / projectileSpeed);

            projectiles.Add(projectile);
        }

        float angleOffset = 10;

        projectiles[0].transform.LookAt(targetPosShifted);

        Quaternion leftRotation = Quaternion.Euler(0, -angleOffset, 0);
        projectiles[1].transform.rotation = Quaternion.LookRotation(leftRotation * (targetPosShifted - projectiles[1].transform.position));

        Quaternion rightRotation = Quaternion.Euler(0, angleOffset, 0);
        projectiles[2].transform.rotation = Quaternion.LookRotation(rightRotation * (targetPosShifted - projectiles[2].transform.position));
    }
}

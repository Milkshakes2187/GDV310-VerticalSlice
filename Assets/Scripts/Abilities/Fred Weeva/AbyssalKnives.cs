using System.Collections.Generic;
using UnityEngine;

public class AbyssalKnives : Ability
{
    // TODO: make sure to rework this ability to work with georges ability system
    // TODO: Comment once reworked

    public Vector3 targetPosShifted;
    public GameObject projectilePF;

    List<GameObject> projectiles = new List<GameObject>();

    public float projectileSpeed = 5f;
    public float range = 10f;

    public override void UseSpellEffect()
    {
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

    private void Update()
    {
        if (projectiles.Count > 0)
        {
            int projectilesAlive = 0;

            foreach (var projectile in projectiles)
            {
                if (projectile)
                {
                    projectile.transform.position += projectile.transform.forward * projectileSpeed * Time.deltaTime;
                    projectilesAlive++;
                }
            }

            if (projectilesAlive <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

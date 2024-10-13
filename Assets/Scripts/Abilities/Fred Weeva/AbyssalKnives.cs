using System.Collections.Generic;
using UnityEngine;

public class AbyssalKnives : MonoBehaviour
{
    // TODO: make sure to rework this ability to work with georges ability system
    // TODO: Comment once reworked

    public Vector3 targetDirection;
    public GameObject projectilePF;

    List<GameObject> projectiles = new List<GameObject>();

    public float projectileSpeed = 5f;
    public float range = 10f;


    private void Start()
    {
        targetDirection.y = transform.position.y;

        ActivateAbility();
    }

    private void Update()
    {
        if (projectiles.Count > 0)
        {
            foreach (var projectile in projectiles)
            {
                if (projectile)
                {
                    projectile.transform.position += projectile.transform.forward * projectileSpeed * Time.deltaTime;
                }
            }
        }
    }

    void ActivateAbility()
    {
        for (int i = 0; i < 3; i++)
        {
            var projectile = Instantiate(projectilePF, transform.position, Quaternion.identity);
            Destroy(projectile, range / projectileSpeed);

            projectiles.Add(projectile);
        }

        float angleOffset = 10;
        

        projectiles[0].transform.LookAt(targetDirection);

        Quaternion leftRotation = Quaternion.Euler(0, -angleOffset, 0);
        projectiles[1].transform.rotation = Quaternion.LookRotation(leftRotation * (targetDirection - projectiles[1].transform.position));

        Quaternion rightRotation = Quaternion.Euler(0, angleOffset, 0);
        projectiles[2].transform.rotation = Quaternion.LookRotation(rightRotation * (targetDirection - projectiles[2].transform.position));
    }
}

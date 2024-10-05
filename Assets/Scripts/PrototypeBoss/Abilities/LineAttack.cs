using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class LineAttack : MonoBehaviour
{
    public GameObject projectilePF;
    public GameObject lineIndicator;
    [HideInInspector] public int damage;
    [HideInInspector] public bool dealingDamage;
    public Timer castTimer;

    private void Start()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 newPosition = new Vector3(Mathf.Cos(angle) * PrototypeBoss.instance.fLineArenaRadius, 0f, Mathf.Sin(angle) * PrototypeBoss.instance.fLineArenaRadius);

        transform.position = newPosition;
        transform.LookAt(Player.instance.transform.position);
    }

    private void Update()
    {
        castTimer.elapsedtime += Time.deltaTime;

        if (castTimer.elapsedtime > castTimer.timeRequired)
        {
            var newProjectile = Instantiate(projectilePF, transform.position, Quaternion.identity);

            newProjectile.transform.rotation = transform.rotation;
            newProjectile.transform.position += new Vector3(0, 2, 0);
                    
            Destroy(gameObject);
        }
    }

    public float CalculatePercentage(float elapsedTime, float requiredTime)
    {
        // Ensure requiredTime is not zero to avoid division by zero
        if (requiredTime == 0)
        {
            return 0;
        }

        float percentage = (elapsedTime / requiredTime);
        return Mathf.Clamp(percentage, 0f, 1f);
    }
}

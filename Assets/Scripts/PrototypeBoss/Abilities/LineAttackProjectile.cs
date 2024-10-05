using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttackProjectile : MonoBehaviour
{
    PrototypeBoss boss;
    public float fMoveSpeed = 5f;
    public float fDespawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        boss = PrototypeBoss.instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * fMoveSpeed * Time.deltaTime;

        fDespawnTime -= Time.deltaTime;

        if (fDespawnTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.instance.gameObject)
        {
            Player.instance.TakeDamage(PrototypeBoss.instance.iLineDamage);
        }

        List<GameObject> bouldersToRemove = new List<GameObject>();

        foreach (var boulder in boss.spawnedBoulders)
        {
            if (other.gameObject == boulder)
            {
                bouldersToRemove.Add(boulder);
            }
        }

        boss.ActivateBoulders(bouldersToRemove);
    }
}

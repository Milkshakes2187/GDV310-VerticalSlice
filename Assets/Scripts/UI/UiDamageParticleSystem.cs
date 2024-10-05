using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UiDamageParticleSystem : MonoBehaviour
{
    [HideInInspector] public GameObject prefabDamageNumber;
    [HideInInspector] public GameObject prefabDamageParticleSystem;

    [HideInInspector] public float numberLifetime;
    [HideInInspector] public float damage;
    [HideInInspector] public float scale;
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        StartCoroutine(HandleNumbers());
    }

    IEnumerator HandleNumbers()
    {
        float currentLifetime = 0;
        GameObject createdNumberSystem = gameObject.GetComponentInChildren<ParticleSystem>().gameObject;

        ParticleSystem particleSys = createdNumberSystem.GetComponent<ParticleSystem>();
        AttachGameObjectsToParticles particleAttach = createdNumberSystem.GetComponent<AttachGameObjectsToParticles>();

        particleAttach.m_Prefab = prefabDamageNumber;
        particleSys.startLifetime = numberLifetime;

        while (currentLifetime < numberLifetime)
        {
            foreach (GameObject _inst in particleAttach.m_Instances)
            {
                _inst.GetComponent<UIDamageNumber>().SetNumber(damage);
                _inst.transform.localScale = Vector3.one * scale;
            }
            currentLifetime += Time.deltaTime;
            yield return null;
        }

        Destroy(createdNumberSystem.transform.root.gameObject);
    }
}

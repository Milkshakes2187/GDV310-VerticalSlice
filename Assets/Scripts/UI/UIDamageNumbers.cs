using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIDamageNumbers : MonoBehaviour
{
    private PrototypeBoss attachedDamagable;

    public GameObject prefabYieldNumber;
    public GameObject prefabYieldParticleSystem;
    public float numberLifetime;
    public float particleScale;
    public float delayTimer = 0.5f;
    private float currentHealth;
    private float elapsedTime;
    public float minScale;
    public float healthScaling;
    private void Awake()
    {
        attachedDamagable = GetComponentInParent<PrototypeBoss>();
        if (attachedDamagable == null)
        {
            Debug.LogError("ERROR: root object does not have a Mining Unit");
        }
    }
    private void Start()
    {
        currentHealth = attachedDamagable.fHealth;
    }

    private void LateUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayTimer)
        {
            if (attachedDamagable.fHealth < currentHealth)
            {
                ShowDamageNumber(attachedDamagable.fHealth - currentHealth);
            }
            currentHealth = attachedDamagable.fHealth;
            elapsedTime = 0;
        }
    }

    void ShowDamageNumber(float amount)
    {
        if (attachedDamagable == null) { return; }

        GameObject createdParticleSystem = Instantiate(prefabYieldParticleSystem, transform.position, Quaternion.identity);

        UiDamageParticleSystem partSystem = createdParticleSystem.GetComponent<UiDamageParticleSystem>();

        partSystem.numberLifetime = numberLifetime;
        partSystem.prefabDamageNumber = prefabYieldNumber;
        partSystem.damage = amount;
        partSystem.scale = minScale + (Mathf.Clamp(MathF.Abs(amount / attachedDamagable.fMaxHealth), 0f, 1f) * healthScaling);
        partSystem.transform.localScale = Vector3.one * particleScale;

    }
}
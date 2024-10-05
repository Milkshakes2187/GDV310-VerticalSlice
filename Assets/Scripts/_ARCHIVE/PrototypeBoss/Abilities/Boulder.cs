using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using VInspector;

public class Boulder : MonoBehaviour
{
    public GameObject rock;
    public GameObject indicator;
    public GameObject detonationIndicator;
    public Timer spawnTimer;
    public Timer detonateTimer;
    public int Damage = 0;
    public bool detonationTriggered = false;
    Collider damageCollider;
    public Material indicatorFill;
    public VFX_RockLava VFX_RockLava;
    public GameObject ExplosionVFX;
    public GameObject CoasterVFX;
    public bool bSpawnCoaster = false;
    public GameObject ExplodeSoundPF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.enabled = false;

        spawnTimer.timeRequired = 3;
        detonateTimer.timeRequired = 3;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer.elapsedtime += Time.deltaTime;
        
        if (spawnTimer.elapsedtime > spawnTimer.timeRequired)
        {
            damageCollider.enabled = true;
            indicator.SetActive(false);
            rock.SetActive(true);

            if (detonationTriggered)
            {
                DetonateRock();

                if (!bSpawnCoaster)
                {
                    bSpawnCoaster = true;
                    var effect = Instantiate(CoasterVFX, transform.position + Vector3.up * 0.01f, transform.rotation);
                    effect.GetComponentInChildren<VisualEffect>().SetFloat(Shader.PropertyToID("AnimationTime"), (Single)detonateTimer.timeRequired);
                    Destroy(effect, 5);
                }
            }
        }

        indicator.GetComponent<IndicatorBase>().BackgroundFillAmount = CalculatePercentage(spawnTimer.elapsedtime, spawnTimer.timeRequired);
    }

    public void DetonateRock()
    {
        detonationIndicator.SetActive(true);

        detonateTimer.elapsedtime += Time.deltaTime;

        VFX_RockLava.lavaAmount = CalculatePercentage(detonateTimer.elapsedtime, detonateTimer.timeRequired);

        if (detonateTimer.elapsedtime > detonateTimer.timeRequired)
        {
            detonationIndicator.SetActive(false);

            var effect = Instantiate(ExplosionVFX, transform.position, transform.rotation);
            Destroy(effect, 5);

            var sound = Instantiate(ExplodeSoundPF, transform.position, transform.rotation);
            Destroy(sound, 5);

            if (Vector3.Distance(Player.instance.transform.position, transform.position) <= 25f)
            {
                Player.instance.TakeDamage(PrototypeBoss.instance.iBoulderDamage);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.instance.gameObject)
        {
            Player.instance.TakeDamage(Damage);
        }
    }

    public float CalculatePercentage(float elapsedTime, float requiredTime)
    {
        // Ensure requiredTime is not zero to avoid division by zero
        if (requiredTime == 0)
        {
            Debug.LogWarning("Required time should not be zero.");
            return 0;
        }

        float percentage = (elapsedTime / requiredTime);
        return Mathf.Clamp(percentage, 0f, 1f); // Ensure the percentage is between 0 and 100
    }
}

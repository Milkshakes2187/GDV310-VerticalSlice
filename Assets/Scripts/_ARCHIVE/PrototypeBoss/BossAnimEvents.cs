using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static PB_FSM;

public class BossAnimEvents : MonoBehaviour
{
    PrototypeBoss boss;
    Player player;
    public GameObject SpinPF;
    public GameObject ConePF;
    public GameObject DashPF;
    public GameObject ExplosionPF;
    Collider dashCollider;

    private void Start()
    {
        boss = PrototypeBoss.instance;
        player = Player.instance;
        dashCollider = GetComponent<Collider>();
    }

    public void TriggerConeDamage()
    {
        var effect = Instantiate(ConePF, transform.root.position, transform.root.rotation, transform.root);
        Destroy(effect, 5);

        // Sound
        if (AudioLibrary.instance.audioConeAttack.clip)
        {
            AudioLibrary.instance.audioConeAttack.PlayOneShot(AudioLibrary.instance.audioConeAttack.clip, 1);
        }

        // If the player is in the cone when the attack goes off, deal damage
        if (CheckForObjectInCone(player.gameObject, boss.fConeRange, boss.fConeHalfFov))
        {
            Player.instance.TakeDamage(boss.iConeDamage);
        }

        List<GameObject> bouldersToRemove = new List<GameObject>();

        foreach (var boulder in boss.spawnedBoulders)
        {
            if (CheckForObjectInCone(boulder, boss.fConeRange, boss.fConeHalfFov))
            {
                bouldersToRemove.Add(boulder);
                boulder.GetComponent<Boulder>().DetonateRock();
            }
        }
        boss.ActivateBoulders(bouldersToRemove);
    }

    public void TriggerStopRotation()
    {
        boss.bStopRotation = true;
    }

    public void TriggerSpinningStart()
    {
        boss.bSpinning = true;

        var effect = Instantiate(SpinPF, transform.root.position, transform.root.rotation, transform.root);
        effect.GetComponentInChildren<VisualEffect>().SetFloat("AnimationTime", 3f);
        Destroy(effect, 5);

        if (boss.fHealth / boss.fMaxHealth < 0.25)
        {
            boss.agent.isStopped = false;
            boss.spinDuration.timeRequired = 4;
            effect.GetComponentInChildren<VisualEffect>().SetFloat("AnimationTime", 5f);
        }

        if (AudioLibrary.instance.audioSpinAttack.clip)
        {
            AudioLibrary.instance.audioSpinAttack.Play();
        }

    }

    public void TriggerSpinningStop()
    {
        boss.agent.isStopped = true;
        boss.bSpinning = false;

        if (AudioLibrary.instance.audioSpinAttack.clip)
        {
            AudioLibrary.instance.audioSpinAttack.Stop();
        }
    }

    public void TriggerDashStart()
    {
        dashCollider.enabled = true;

        // Sound
        if (AudioLibrary.instance.audioBossDash.clip)
        {
            AudioLibrary.instance.audioBossDash.PlayOneShot(AudioLibrary.instance.audioBossDash.clip, 1);
        }

        var effect = Instantiate(DashPF, transform.root.position, transform.root.rotation, transform.root);
        Destroy(effect, 5);

        boss.bDashActive = true;
        StartCoroutine(boss.Dash(boss.fDistToDash));
    }

    public void TriggerDashFinished()
    {
        // Sound
        if (AudioLibrary.instance.audioDashSwipe.clip)
        {
            AudioLibrary.instance.audioDashSwipe.PlayOneShot(AudioLibrary.instance.audioDashSwipe.clip, 1);
        }

        boss.bDashActive = false;
        dashCollider.enabled = false;
    }

    public void TriggerTimedExplosion()
    {
        boss.roomWideIndicator.SetActive(false);

        // Sound
        if (AudioLibrary.instance.audioBossExplosion.clip)
        {
            AudioLibrary.instance.audioBossExplosion.PlayOneShot(AudioLibrary.instance.audioBossExplosion.clip, 1);
        }

        var effect = Instantiate(ExplosionPF, transform.root.position, transform.root.rotation, transform.root);
        Destroy(effect, 5);

        if (boss.HasLOS())
        {
            Player.instance.TakeDamage(boss.iExplosionDamage);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 9999f);

        List<GameObject> bouldersToDetonate = new List<GameObject>();

        foreach (Collider hitCollider in hitColliders)
        {
            Boulder boulder = hitCollider.GetComponent<Boulder>();
            if (boulder != null)
            {
                bouldersToDetonate.Add(boulder.gameObject);
            }
        }

        boss.ActivateBoulders(bouldersToDetonate);

        StartCoroutine(boss.LowerLava());
    }

    public void TriggerExplosionDone()
    {
        boss.bExplosionDone = true;
    }

    public void TriggerAttackDone()
    {
        boss.bIsAttacking = false;
        boss.bStopRotation = false;
    }

    public bool CheckForObjectInCone(GameObject _objectToCheck, float _fVisionDist, float _fHalfFOV)
    {
        Vector3 v3Dir = _objectToCheck.transform.position - boss.transform.position;
        float fAngle = Vector3.Angle(v3Dir, boss.transform.forward);

        Debug.DrawRay(boss.transform.position, boss.transform.forward * _fVisionDist, Color.cyan);

        Quaternion leftRayRotation = Quaternion.AngleAxis(-_fHalfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(_fHalfFOV, Vector3.up);

        // Cast rays to represent the FOV
        Vector3 leftRayDirection = leftRayRotation * boss.transform.forward;
        Vector3 rightRayDirection = rightRayRotation * boss.transform.forward;

        Debug.DrawRay(boss.transform.position, leftRayDirection * _fVisionDist, Color.red);
        Debug.DrawRay(boss.transform.position, rightRayDirection * _fVisionDist, Color.red);

        // if object in cone, return true
        if (v3Dir.magnitude < _fVisionDist && fAngle < _fHalfFOV)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        dashCollider.enabled = false;

        if (other.gameObject == Player.instance.gameObject)
        {
            player.TakeDamage(boss.fdashDamage);
        }

        // Check if boulders are hit
        List<GameObject> bouldersToRemove = new List<GameObject>();
        foreach (var boulder in boss.spawnedBoulders)
        {
            if (other.gameObject == boulder.gameObject)
            {
                bouldersToRemove.Add(boulder);
                boulder.GetComponent<Boulder>().DetonateRock();
            }
        }
        boss.ActivateBoulders(bouldersToRemove);
    }
}

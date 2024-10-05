using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class AudioLibrary : MonoBehaviour
{
    public static AudioLibrary instance;


    [Tab("Player")]
    [Header("Abilities")]
    public AudioSource audioStep;
    public AudioSource audioDash;
    public AudioSource audioAbility1;


    [Tab("PlayerCombat")]
    [Header("Light")]
    public List<AudioSource> audioLightAttacks;
    

    [Header("Heavy")]
    public AudioSource audioChargedSpinAttack;
    public AudioSource audioChargedSlashAttack;
    public AudioSource audioChargedStage1;
    public AudioSource audioChargedStage2;
    public AudioSource audioChargedStage3;

    [Header("Impact")]
    public AudioSource audioSwordImpact;
    public AudioSource audioHeavyImpact;
    public AudioSource audioSwordLifestealImpact;

    [Tab("Boss")]
    public AudioSource audioConeAttack;
    public AudioSource audioSpinAttack;
    public AudioSource audioBossDash;
    public AudioSource audioDashSwipe;
    public AudioSource audioBossExplosion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

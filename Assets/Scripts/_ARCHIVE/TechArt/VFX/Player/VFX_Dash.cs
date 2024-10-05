using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Dash : MonoBehaviour
{
    private Player player;
    private VisualEffect dashVFX;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        dashVFX = GetComponent<VisualEffect>();
    }
    void Start()
    {
        player.OnDash += ActivateDash;
    }

    private void ActivateDash(float dashLength)
    {
        StartCoroutine(HandleDash(dashLength));
    }

    IEnumerator HandleDash(float dashLength)
    {
        dashVFX.SendEvent("Activate");
        yield return new WaitForSeconds(dashLength);
        dashVFX.SendEvent("Deactivate");
    }
}

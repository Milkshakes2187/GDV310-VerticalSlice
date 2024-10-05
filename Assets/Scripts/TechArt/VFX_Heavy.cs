using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Heavy : MonoBehaviour
{
    VisualEffect vfx;
    float currentProg;
    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            currentProg += Time.deltaTime / 1.5f;
            currentProg = Mathf.Clamp01(currentProg);
        }
        vfx.SetFloat("Progression", currentProg);
    }
}

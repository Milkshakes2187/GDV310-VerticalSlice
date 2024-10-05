using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VInspector;

[ExecuteInEditMode]
[RequireComponent(typeof(IndicatorBase))]
public class IndicatorCircle : MonoBehaviour
{
    private IndicatorBase baseIndicator;
    private bool clone;

    [HideIf("clone")]
    [Foldout("Assigned Indicators")]
        public DecalProjector sideRIndicator;
        public DecalProjector sideLIndicator;

    [Tab("Shape")]
        public float size;
        [Range(0f, 1f)] public float fillAmount;

    private void Awake()
    {
        baseIndicator = GetComponent<IndicatorBase>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        clone = baseIndicator.clone;
        if (baseIndicator.allProjectors.Count == 0 || baseIndicator.allProjectors[0].enabled == false 
            || sideRIndicator == null
            || sideLIndicator == null)
        {
            return;
        }

        foreach (Material mat in baseIndicator.allMaterials)
        {
            if (mat != null)
            {
                mat.SetFloat("_FillAmount", fillAmount);
            }
        }
        sideRIndicator.material.SetInt("_FlipTexture", 1);

        foreach (DecalProjector proj in baseIndicator.allProjectors)
        {
            proj.size = new Vector3(size, size, baseIndicator.projectionHeight);
            proj.pivot = new Vector3(proj.pivot.x, proj.pivot.y, 0);
        }

        //////////////////////////////////////////////////////////
        // Rotate side indicators to match fill

        sideRIndicator.transform.localRotation = Quaternion.Euler(0, 0, fillAmount * 180f);
        sideLIndicator.transform.localRotation = Quaternion.Euler(0, 0, -fillAmount * 180f);

        sideRIndicator.enabled = fillAmount < 1 && fillAmount != 0;
        sideLIndicator.enabled = fillAmount < 1 && fillAmount != 0;

        //////////////////////////////////////////////////////////
    }
}

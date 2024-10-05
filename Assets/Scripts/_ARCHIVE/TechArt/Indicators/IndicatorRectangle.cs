using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VInspector;

[ExecuteInEditMode]
[RequireComponent(typeof(IndicatorBase))]
public class IndicatorRectangle : MonoBehaviour
{
    private IndicatorBase baseIndicator;
    private bool clone;

    [Header("Rectangle settings are work in progress")]
    [HideIf("clone")]
    [Space]
    [Tab("Shape")]
        public Vector2 size = new Vector2(1,1);
        public Vector3 pivot;

    private void Awake()
    {
        baseIndicator = GetComponent<IndicatorBase>();
    }

    private void Update()
    {
        clone = baseIndicator.clone;
        if (baseIndicator.allProjectors.Count == 0) return;

        foreach (DecalProjector proj in baseIndicator.allProjectors)
        {
            proj.size = new Vector3(size.x, size.y, baseIndicator.projectionHeight);
            proj.pivot = new Vector3(pivot.x, pivot.z, pivot.y);
        }
    }
}

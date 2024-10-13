using MPUIKIT;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MPUIGradientController : MonoBehaviour
{
    public List<MPUIKIT.MPImage> gradientImages;
    public GradientEffect gradient;

    // Update is called once per frame
    void Update()
    {
        if (gradientImages == null) return;

        foreach (MPImage _mpImg in gradientImages)
        {
            _mpImg.GradientEffect = gradient;
        }
    }
}

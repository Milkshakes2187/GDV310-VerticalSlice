using MPUIKIT;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

[ExecuteInEditMode]
public class MPUIGradientController : MonoBehaviour
{
    public bool baseColorControls = false;

    [ShowIf("baseColorControls")] 
        public Color baseColor;
    [EndIf]

    public List<MPUIKIT.MPImage> gradientImages;
    public GradientEffect gradient;

    // Update is called once per frame
    //

    [OnValueChanged("gradient", "baseColorControls", "baseColor", "gradientImages")]
    void UpdateGradient()
    {
        if (gradientImages == null) return;

        foreach (MPImage _mpImg in gradientImages)
        {
            if (gradient.Enabled)
            {
                _mpImg.GradientEffect = new GradientEffect
                {
                    Enabled = true,
                    Gradient = gradient.Gradient,
                    GradientType = gradient.GradientType,
                    Rotation = gradient.Rotation
                };
            }

            if(baseColorControls)
            {
                _mpImg.color = baseColor;
            }
        }
    }
}

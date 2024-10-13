using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIFillController : MonoBehaviour
{
    public List<Image> fillImages;
    [Range(0f,1f)] public float fillAmount = 1;

    // Update is called once per frame
    void Update()
    {
        if (fillImages == null) return;

        foreach (Image _img in fillImages) 
        {
            _img.fillAmount = fillAmount;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

[ExecuteInEditMode]
public class UIFillController : MonoBehaviour
{
    public List<Image> fillImages;
    [Range(0f,1f)] public float fillAmount = 1;


    [Button]
    public void AddChildren()
    {
        fillImages = new List<Image>();
        foreach (Image _iChild in transform.GetComponentsInChildren<Image>())
        {
            fillImages.Add(_iChild);
        }
    }

    //
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

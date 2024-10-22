using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

[ExecuteInEditMode]
public class UIFillController : MonoBehaviour
{
    public List<Image> fillImages;
    [Range(0f,1f)] public float fillAmount = 1;


    /***********************************************
    * AddChildren: Tool to allow the developer to add all children under a UI element for easy access
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    [Button]
    public void AddChildren()
    {
        fillImages = new List<Image>();
        foreach (Image _iChild in transform.GetComponentsInChildren<Image>())
        {
            fillImages.Add(_iChild);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fillImages == null) return;

        foreach (Image _img in fillImages) 
        {
            if (_img == null) { Debug.LogError("Fill controllers image no longer exists on UI component: " + transform.name); continue; }
            _img.fillAmount = fillAmount;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

[ExecuteInEditMode]
public class UIGRPSpacingController : MonoBehaviour
{
    public List<RectTransform> spacingTransforms = new List<RectTransform>();
    private RectTransform rectTransform;

    public GRPSpacingSettings spacingSettings;


    [Serializable]
    public class GRPSpacingSettings
    {
        public bool vertical = false;
        public float spacing = 0;

        public bool overrideSize = false;

        [ShowIf("overrideSize")]
        public Vector2 size = new Vector2();

    }

    private void Start()
    {
        UpdateSpacing();
    }

    [Button]
    public void AddChildren()
    {
        spacingTransforms = new List<RectTransform>();
        foreach (RectTransform _tChild in transform) 
        {
            spacingTransforms.Add(_tChild);
        }
    }

    [Button]
    [OnValueChanged("spacingSettings")]
    public void UpdateSpacing()
    {
        rectTransform = GetComponent<RectTransform>();
        Vector2 dirVector = Vector2.right;
        if(spacingSettings.vertical) dirVector = Vector2.up;

        int postionIndex = 0;
        foreach (RectTransform _sTransform in spacingTransforms)
        {
            _sTransform.anchoredPosition = Vector2.zero
                + (dirVector * spacingSettings.spacing * postionIndex)
                - (dirVector * spacingSettings.spacing * spacingTransforms.Count) /2f
                + (dirVector * spacingSettings.spacing / 2f);

            _sTransform.anchoredPosition = dirVector * _sTransform.anchoredPosition;
            postionIndex++;

            if(spacingSettings.overrideSize)
            {
                _sTransform.sizeDelta = spacingSettings.size;
            }
        }
    }
}

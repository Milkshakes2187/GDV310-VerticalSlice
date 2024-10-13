using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

[ExecuteInEditMode]
public class UIGRPSpacingController : MonoBehaviour
{
    public List<RectTransform> spacingTransforms = new List<RectTransform>();
    public bool vertical = false;
    public float spacing = 0;
    private RectTransform rectTransform;

    private void Start()
    {
        SpacingChanged();
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

    [OnValueChanged("spacing", "vertical")]
    public void SpacingChanged()
    {
        rectTransform = GetComponent<RectTransform>();
        Vector2 dirVector = Vector2.right;
        if(vertical) dirVector = Vector2.up;

        int postionIndex = 0;
        foreach (RectTransform _sTransform in spacingTransforms)
        {
            _sTransform.anchoredPosition = rectTransform.anchoredPosition
                + (dirVector * spacing * postionIndex)
                - (dirVector * spacing * spacingTransforms.Count) /2f
                + (dirVector * spacing/2f);

            _sTransform.anchoredPosition = dirVector * _sTransform.anchoredPosition;
            postionIndex++;
        }
    }
}

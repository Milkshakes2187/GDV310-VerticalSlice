using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VInspector;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif

public class IndicatorBase : MonoBehaviour
{
    [ShowIf("clone")]
        [Header("Warning\n\nIndicator is a duplicate of an existing indicator in the scene.\nDuplicating indictors in edit mode will break all varients\n\n" +
            "Either destroy this asset or create a new material\n\nBy unchecking 'clone' below u will regain access,\nonly do this if you believe this popup is bugged\n\nIf in doubt ask Nathan")]

    [Space]
        public bool clone = false;
    [EndIf]

    [HideIf("clone")]
        [Header("Currently Editing Material: ")]
        [ReadOnly] public Material editingMaterial;
        public SerializedDictionary<GameObject, IndicatorLibrary> projectorShaders = new SerializedDictionary<GameObject, IndicatorLibrary>();

    [EndFoldout]
        [Header("General Indicator Settings")]
        [Tab("Settings")]
            [Space]
            public LightLayerEnum lightDecalLayers = LightLayerEnum.LightLayerDefault;
            public float projectionHeight = 10;

        [Tab("Color")]
            [Space]
            [ColorUsage(true, true)] public Color outlineColor = Color.white;
            [ColorUsage(true, true)] public Color backgroundColor = Color.white;

            [Header("Background Fill")]
            [Range(0f, 1f)] public float BackgroundFillAmount;
            public Vector2 BackgroundFillRange = new Vector2(0f,1f);
    [EndIf]

    //////////////////////////////////////////////////////////
    // Private Variables

    [Serializable]
    public enum IndicatorLibrary
    {
        Indicator_Noise,
        IndicatorCircle_Noise,
    }

    [HideInInspector] public List<Material> allMaterials;
    [HideInInspector] public List<DecalProjector> allProjectors;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            clone = false;
            foreach (DecalProjector projector in allProjectors) 
            {
                projector.material = new Material(projector.material);
            }
        }

        allMaterials = new List<Material>();
        allProjectors = GetComponentsInChildren<DecalProjector>().ToList();
        foreach (DecalProjector projector in allProjectors)
        {
            allMaterials.Add(projector.material);
        }
    }

    void Update()
    {
        //////////////////////////////////////////////////////////
        // Editor Help Settings

        if (!Application.isPlaying)
        {
            foreach (IndicatorBase indicator in FindObjectsByType<IndicatorBase>(FindObjectsSortMode.None))
            {
                clone = indicator.editingMaterial == editingMaterial && indicator != this;
                if (clone) break;
            }
        }

        if (allProjectors.Count == 0 || allProjectors[0].enabled == false)
        {
            editingMaterial = null;
            return;
        }
        else
        {
            editingMaterial = allProjectors[0].material;
        }

        foreach (DecalProjector decalProj in GetComponentsInChildren<DecalProjector>())
        {
            if (!projectorShaders.ContainsKey(decalProj.gameObject))
            {
                Debug.LogWarning("Warning: Decal projector on indicator is not assigned a shader\nAsk Nathan if confused. "+ decalProj.gameObject.name);
            }
        }

        //////////////////////////////////////////////////////////
        // Change setting applicable to all materials

        backgroundColor.a = Mathf.Lerp(BackgroundFillRange.x, BackgroundFillRange.y, BackgroundFillAmount);
        foreach (Material mat in allMaterials) 
        {
            mat.SetColor("_OutlineColor", outlineColor);
            mat.SetColor("_BackgroundColor", backgroundColor);
        }

        foreach (DecalProjector proj in allProjectors)
        {
            proj.renderingLayerMask = (uint)lightDecalLayers;
        }

    }

    #if UNITY_EDITOR
    [Button]
    void CreateNewMaterial()
    {
        clone = false;

        string savePath = "Assets\\Art\\Materials\\Indicators\\IndicatorVarients\\" + gameObject.name + "\\";

        if (AssetDatabase.FindAssets("", new string[1] { savePath }).Length > 0)
        {
            EditorUtility.DisplayDialog("Error", "Can't create new indicator:\nIndicator with name already exists\nRename the object to create a new material", "I understand and wont do it again");
            return;
        }

        AssetDatabase.CreateFolder("Assets\\Art\\Materials\\Indicators\\IndicatorVarients", gameObject.name);

        int index = 0;
        foreach (KeyValuePair<GameObject, IndicatorLibrary> projectorShader in projectorShaders)
        {
            DecalProjector projector = projectorShader.Key.GetComponent<DecalProjector>();

            Material newMat = new Material(Shader.Find("Shader Graphs/" + ((IndicatorLibrary)(int)projectorShader.Value).ToString()));
            AssetDatabase.CreateAsset(newMat, savePath + gameObject.name + "_" + index + ".mat");

            projector.material = newMat;
            projector.enabled = true;
            index++;
        }

        OnEnable();

        AssetDatabase.SaveAssets();
    }

    [Button]
    void DeleteMaterial()
    {
        if (allProjectors.Count == 0 || allProjectors[0].material == null)
        {
            return;
        }

        if (!EditorUtility.DisplayDialog("Delete Indicator?", "This will delete the assets for the current indicator " + allMaterials[0].name + "\nOnly do this if you are very sure", "Delete Materials", "Cancel"))
        {
            return;
        }

        AssetDatabase.DeleteAsset("Assets/Art/Materials/Indicators/IndicatorVarients/" + gameObject.name);

        foreach(DecalProjector proj in allProjectors)
        {
            if (!proj) continue;
            proj.material = null;
            proj.enabled = false;
            proj.material = null;
        }
        allMaterials = new List<Material>();
    }

    #endif
}

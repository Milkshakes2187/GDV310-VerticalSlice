using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIUnitFrameController : MonoBehaviour
{
    private RectTransform rTransform;
    public Character attachedCharacter;

    private Vector3 characterOffset;
    private bool initialized = false;

    public Vector3 generalOffset;

    private void Awake()
    {
        rTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (!initialized || attachedCharacter == null)
        {
            if (!initialized) Debug.LogError("Unit frame was not initialized before creation");
            else Debug.LogError("Unit frame was not disposed of properly, please ask nathan so we can fix it"); 

            enabled = false;
            Destroy(gameObject);
            return;
        }

        TickUnitFrame();
    }

    /***********************************************
    * Initialize: Attaches Unit Frame to character. Assigned by UIUnitFrameGroupManager.cs
    * @author: Nathan Hunt
    * @parameter: Character, UnitFrameSettings
    * @return: void
    ************************************************/
    public void Initialize(Character character, UIUnitFrameGroupManager.UnitFrameSettings unitFrameSettings)
    {
        if (character == null) return;
        initialized = true;


        // Setup Variables
        attachedCharacter = character;
        generalOffset = unitFrameSettings.generalOffset;
        transform.localScale = Vector3.one * unitFrameSettings.frameScale;

        // Generate Average bounds of character
        characterOffset = GenerateCharacterOffset();
    }

    /***********************************************
    * TickUnitFrame: Updates the Unit Frames values every frame
    * @author: Nathan Hunt
    * @return: void
    ************************************************/

    public void TickUnitFrame()
    {
        if (!Camera.main) { Debug.LogError("Main camera not set, Unit frame wont update"); return; }
        rTransform.position = Camera.main.WorldToScreenPoint(attachedCharacter.transform.position + characterOffset + generalOffset);
    }

    /***********************************************
    * GenerateCharacterOffset: Finds the average point of the attached characters mesh bounds and creates an offset for the Unit Frame
    * @author: Nathan Hunt
    * @return: Vector3
    ************************************************/

    public Vector3 GenerateCharacterOffset()
    {
        if (!attachedCharacter) return Vector3.zero;

        float _charHeight = 0;
        Bounds _charAverageBound = attachedCharacter.GetComponentInChildren<Renderer>().bounds;

        foreach (Renderer _renderer in attachedCharacter.GetComponentsInChildren<Renderer>())
        {
            if (_renderer.bounds.size.y > _charHeight) _charHeight = _renderer.bounds.size.y;
            _charAverageBound.Encapsulate(_renderer.bounds);
        }
        return _charAverageBound.center - attachedCharacter.transform.position + Vector3.up * _charHeight / 2f;
    }
}

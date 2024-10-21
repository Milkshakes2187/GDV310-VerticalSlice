using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using VInspector;
using UnityEngine.TextCore.Text;
public class UIUnitFrameGroupManager : MonoBehaviour
{
    private WorldManager worldManager;
    private Dictionary<Character, UIUnitFrameController> unitFrames = new Dictionary<Character, UIUnitFrameController>();

    [Serializable]
    public class UnitFrameSettings
    {
        public GameObject unitFramePrefab;
        public Vector3 generalOffset = Vector3.zero;
        public float frameScale = 1;
    }

    public UnitFrameSettings unitFrameSettings;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        worldManager = WorldManager.instance;
        if(worldManager == null)
        {
            enabled = false;
            Debug.LogError("World Manager does not exist for unit frames");
            return;
        }

        worldManager.onEnemyListChange += UpdateUnitFrames;

        StartCoroutine(UpdateInitalFrames());
    }

    /***********************************************
    * UpdateInitalFrames: Updates all unit frames when first loaded to ensure every character has a frame
    * @author: Nathan Hunt
    * @return: IEnumerator
    ************************************************/
    IEnumerator UpdateInitalFrames()
    {
        yield return new WaitForEndOfFrame();

        foreach(Character character in WorldManager.instance.allEnemies)
        {
            UpdateUnitFrames(character);
        }
    }

    /***********************************************
    * UpdateUnitFrames: Updates the assigned characters unit frame, adding or deleting if character exists
    * @author: Nathan Hunt
    * @parameter: Character
    * @return: void
    ************************************************/
    void UpdateUnitFrames(Character unit)
    {
        bool containsNullKey = false;

        if(unit == null)
        {
            foreach(KeyValuePair<Character, UIUnitFrameController> _unitPair in unitFrames)
            {
                if(_unitPair.Key == null)
                {
                    containsNullKey = true;
                    Destroy(_unitPair.Value.gameObject);
                }
            }
        }
        else if(!unitFrames.ContainsKey(unit))
        {
            CreateNewUnitFrame(unit);
        }
        else
        {
            Debug.LogError("Unit frame already exists in dictionary\nAsk nathan about this, its a problem");
        }

        if(containsNullKey) EmptyNullKeys();
    }

    /***********************************************
    * EmptyNullKeys: Iterates through Unit Frame dictionary and removes all null keys
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    private void EmptyNullKeys()
    {
        foreach (var key in unitFrames.Keys.ToArray())
        {
            if (key == null)
            {
                unitFrames.Remove(key);
            }
        }
    }

    #region Frame Management

    /***********************************************
    * CreateNewUnitFrame: Creates new unit frame for character
    * @author: Nathan Hunt
    * @parameter: Character
    * @return: void
    ************************************************/
    private void CreateNewUnitFrame(Character character)
    {
        UIUnitFrameController newUnitFrame = Instantiate(unitFrameSettings.unitFramePrefab, transform).GetComponent<UIUnitFrameController>();
        unitFrames.Add(character, newUnitFrame);

        InitializeFrame(newUnitFrame, character);
    }

    /***********************************************
    * InitializeFrame: Initializes the characters Unit Frame UI
    * @author: Nathan Hunt
    * @parameter: UIUnitFrameController, Character
    * @return: void
    ************************************************/
    private void InitializeFrame(UIUnitFrameController frame, Character character) => frame.Initialize(character, unitFrameSettings);


    /***********************************************
    * ReinitializeFrames: If any Unit Frame settings have been changed, reinitizaing all the Unit Frames with new settings
    * @author: Nathan Hunt
    * @return: void
    ************************************************/
    [OnValueChanged("unitFrameSettings")]
    private void ReinitializeFrames()
    {
        foreach (KeyValuePair<Character, UIUnitFrameController> _unitPair in unitFrames)
        {
            if (_unitPair.Key != null && _unitPair.Value != null)
            {
                InitializeFrame(_unitPair.Value, _unitPair.Key);
            }
        }
    }

    #endregion
}

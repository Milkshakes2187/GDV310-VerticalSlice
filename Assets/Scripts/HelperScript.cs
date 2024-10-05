using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using VInspector;

[System.Serializable]
public struct Timer
{
    public float timeRequired;
    public float elapsedtime;

    public Timer(float timeRequired)
    {
        this.timeRequired = timeRequired;
        this.elapsedtime = 0;
    }
}

[System.Serializable]
public struct ComboStruct
{
    public PB_FSM.ATTACKS attacks;
    public float delay;
}

[System.Serializable]
public struct BossCombo
{
    public float exhaustionTimer;
    public List<ComboStruct> combo;
}

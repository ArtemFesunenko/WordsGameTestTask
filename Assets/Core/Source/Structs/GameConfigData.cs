using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameConfigData
{
    public bool ErrorSoundEnabled;
    public bool ErrorScreenShakeEnabled;
    public float ErrorScreenShakeDuration;
    public float ErrorScreenShakeStrength;
    public int ErrorScreenShakeShakesAmount;
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Swap", menuName = "VPL/Functions/Swap")]
public class Swap : FuncBlockDefinition
{
    public override List<VPLArg> Args => new()
    {
        new() { Name = "LFreq", Type = "num"},
        new() { Name = "RFreq", Type = "num"}
    };
    public override void Execute(params object[] input)
    {
        int LeftFreq = int.Parse(input[0].ToString()) -1;
        int RightFreq = int.Parse(input[1].ToString()) -1;
        AudioClip leftAudio = CalibrationManager.Instance.Modules[LeftFreq].GetComponent<AudioSource>().clip;
        AudioClip rightAudio = CalibrationManager.Instance.Modules[RightFreq].GetComponent<AudioSource>().clip;
        var tmp = CalibrationManager.Instance.Frequencies[LeftFreq];
        var tmpAudio = leftAudio;
        CalibrationManager.Instance.Frequencies[LeftFreq] = CalibrationManager.Instance.Frequencies[RightFreq];
        CalibrationManager.Instance.Frequencies[RightFreq] = tmp;
        CalibrationManager.Instance.SwitchAudio(LeftFreq, rightAudio);
        CalibrationManager.Instance.SwitchAudio(RightFreq, tmpAudio);
        CalibrationManager.Instance.UpdateVisuals();
        CalibrationManager.Instance.IsSorted(LeftFreq, RightFreq);
    }
}

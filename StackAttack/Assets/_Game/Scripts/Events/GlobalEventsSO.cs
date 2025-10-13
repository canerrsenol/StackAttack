using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEventsSO", menuName = "ScriptableObjects/Events/GlobalEventsSO")]
public class GlobalEventsSO : ScriptableObject
{
    public UIEvents UIEvents = new UIEvents();
}

public class UIEvents
{
    public Action<int> RemainingTime;
}
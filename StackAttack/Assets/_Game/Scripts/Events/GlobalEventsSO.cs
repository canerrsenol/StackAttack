using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEventsSO", menuName = "ScriptableObjects/Events/GlobalEventsSO")]
public class GlobalEventsSO : ScriptableObject
{
    public UIEvents UIEvents = new UIEvents();
    public PlayerEvents PlayerEvents = new PlayerEvents();
}

public class UIEvents
{
    public Action<int> RemainingTime;
}

public class PlayerEvents
{
    public Action<int> HealthChanged;
    public Action<float> zPositionChanged;
}
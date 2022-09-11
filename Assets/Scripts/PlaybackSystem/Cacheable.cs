using System.Collections.Generic;
using UnityEngine;

public abstract class Cacheable : MonoBehaviour
{
    protected List<string> history = new();
    protected bool isCaching = false;

    public bool IsCaching
    {
        get => isCaching;
        set => isCaching = value;
    }

    public abstract void SaveState();
    
    public abstract void LoadState(int index);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour, Interactuable
{
    [SerializeField] private ColdSystem coldSystem;

    public void Interactuar()
    {
        coldSystem.ColdTime += 10;
        if (coldSystem.ColdTime > coldSystem.MaxTimeCold)
        {
            coldSystem.ColdTime = coldSystem.MaxTimeCold;
        }
    }
}

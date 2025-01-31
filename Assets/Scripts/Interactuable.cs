using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactuable
{
    public Transform transform { get; }
    public void Interactuar();
}

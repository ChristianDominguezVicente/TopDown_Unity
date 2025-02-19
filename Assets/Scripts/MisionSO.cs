using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptable Objects/Mision")]
public class MisionSO : ScriptableObject
{
    public string nombre;
    public int lugarEnCanvas;
    public int numeroFases;
    public string secundaria;
    public bool secundariaOk;
    public Sprite secundariaConseguida;
    public Sprite secundariaNoConseguida;
}


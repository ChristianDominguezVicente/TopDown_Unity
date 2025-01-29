using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public float danho;
    public string nombre;
    public int nivelNecesario;
    public Sprite icono;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, Interactuable
{
    [SerializeField] private GameObject doorLabel;

    public void Interactuar()
    {
        doorLabel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour, Interactuable
{
    [SerializeField] private TorchSystem torchSystem;

    public void Interactuar()
    {
        torchSystem.StartTorchTimer();
        Destroy(this.gameObject);
    }
}

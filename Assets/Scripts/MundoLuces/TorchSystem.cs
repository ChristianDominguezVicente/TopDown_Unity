using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchSystem : MonoBehaviour
{
    [SerializeField] private float torchDuration;
    [SerializeField] private Light2D torchPlayer;

    public void StartTorchTimer() 
    {
        torchPlayer.pointLightOuterRadius = 4.5f;
        StartCoroutine(TorchTimer());
    }

    IEnumerator TorchTimer() 
    {
        yield return new WaitForSeconds(torchDuration);
        torchPlayer.pointLightOuterRadius = 1f;
    }
}

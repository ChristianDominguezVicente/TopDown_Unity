using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override void Interactuar()
    {
        base.Interactuar();
        gameManager.CollectedCoins++;
    }
}

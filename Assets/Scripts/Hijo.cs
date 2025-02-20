using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hijo : MonoBehaviour
{
    public string parentObjectName = "Canvas";

    void Start()
    {
        GameObject parentObject = GameObject.Find(parentObjectName);

        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);
        }
    }

    void Update()
    {
        GameObject parentObject = GameObject.Find(parentObjectName);
        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);
            enabled = false;
        }
    }
}

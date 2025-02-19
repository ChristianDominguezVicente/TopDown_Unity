using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColdSystem : MonoBehaviour
{
    [SerializeField] private float maxTimeCold;

    private float coldTime;

    public float ColdTime { get => coldTime; set => coldTime = value; }
    public float MaxTimeCold { get => maxTimeCold; set => maxTimeCold = value; }

    // Start is called before the first frame update
    void Start()
    {
        coldTime = maxTimeCold;
    }

    // Update is called once per frame
    void Update()
    {
        if (coldTime > 0)
        {
            coldTime -= Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        Debug.Log(coldTime);
    }
}

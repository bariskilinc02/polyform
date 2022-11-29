using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private void Init()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
        }
    }
    private void Awake()
    {
        Init();
    }
}

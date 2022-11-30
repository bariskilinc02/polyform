using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo : MonoBehaviour
{
    private LevelController LevelController;
    private ObjectPool ObjectPool;

    private void Init()
    {
        LevelController = FindObjectOfType<LevelController>();
        ObjectPool = FindObjectOfType<ObjectPool>();
    }
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UndoMove();
        }
    }

    void UndoMove()
    {
        if (LevelController.Boxs.Count == 0) return;

        ObjectPool.AddObjectToPool(LevelController.Boxs[LevelController.Boxs.Count - 1].gameObject);
        LevelController.Boxs.RemoveAt(LevelController.Boxs.Count - 1);
        
    }
}

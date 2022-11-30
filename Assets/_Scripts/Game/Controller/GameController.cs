using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private LevelController LevelController;
    private UIInteractions UIInteractions;
    private Bools Bools;

    private void Init()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
        }

        LevelController = FindObjectOfType<LevelController>();
        UIInteractions = FindObjectOfType<UIInteractions>();
        Bools = FindObjectOfType<Bools>();
    }
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Bools.isOnCreateMode) return;

        if (LevelController.IsLevelFinished() && Bools.isLevelTransition == false)
        {
            StartCoroutine(ToNextLevel());
        }
    }

    private IEnumerator ToNextLevel()
    {
        Bools.isLevelTransition = true;
        LevelController.GetNextLevel();
        LevelController.DestroyAllLevelObjects();
        LevelController.CreateLevel();
        yield return new WaitForSeconds(1);
        Bools.isLevelTransition = false;
    }
}

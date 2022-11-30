using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelCreator : MonoBehaviour
{
    private LevelController LevelController;
    private ClickEvents ClickEvents;
    private Bools Bools;

    private Level createdLevel;
    private int _nextLevelIndex;

    [Range(1, 40)]
    public int GridLength;

    public string SolutionBinary;


    private void Awake()
    {
        LevelController = FindObjectOfType<LevelController>();
        ClickEvents = FindObjectOfType<ClickEvents>();
        Bools = FindObjectOfType<Bools>();
    }

    private void Start()
    {
        if (Bools.isOnCreateMode)
        {
            _nextLevelIndex = LevelController.Levels.Count + 1;
            //RecreateGrids();
        }

    }

    private void Update()
    {

        if (Bools.isOnCreateMode){
            LevelController.UpdateCounters();
            UpdateCreatingLevelIndexes();

            if (Input.GetKeyDown(KeyCode.T))
            {
                createdLevel = new Level();
                ScriptableObjectCreator.CreateLevelScriptableObject(createdLevel, _nextLevelIndex);
                _nextLevelIndex += 1;
            }
        }

        

    }

    [Button]
    private void WriteLevelFile()
    {
        ScriptableObjectCreator.CreateLevelScriptableObject(createdLevel, _nextLevelIndex);
        _nextLevelIndex += 1;
        RecreateGrids();

    }

    private void UpdateCreatingLevelIndexes()
    {
        if (createdLevel == null)
        {
            Debug.Log("Oluþturulacak Level Boþ");
            return;
        }

        if (GridLength != LevelController.CurrentGridLength)
        {
            Debug.Log("Level Grid Uzunluðunu Doðrula");
            return;
        }

        createdLevel.GridLength = GridLength;

        for (int i = 0; i < createdLevel.Grids.Count; i++)
        {
            if (LevelController.RaycasterUnitsX[i].BoxsOnRow.Count == GridLength)
            {
                createdLevel.Grids[i] = new Vector2(LevelController.RaycasterUnitsX[i].HitCount, createdLevel.Grids[i].y);
            }

            if (LevelController.RaycasterUnitsY[i].BoxsOnRow.Count == GridLength)
            {
                createdLevel.Grids[i] = new Vector2(createdLevel.Grids[i].x, LevelController.RaycasterUnitsY[i].HitCount);
            }

        }

        SolutionBinary = "";
        for (int i = 0; i < GridLength; i++)
        {
            for (int a = 0; a < GridLength; a++)
            {
                Box box = LevelController.Boxs.Find(x => x.X == i && x.Y == a);
                if (box == null)
                {
                    SolutionBinary += 0;
                }
                else
                {
                    SolutionBinary += (box.transform.GetComponent<Box>().Length + 1).ToString();
                }
            }
        }

        createdLevel.SolutionBinary = SolutionBinary;



    }

    [Button]
    public void RecreateGrids()
    {
        ClickEvents.SelectedBox = null;

        createdLevel = new Level();
        createdLevel.GridLength = GridLength;

        createdLevel.Grids.Clear();
        for (int i = 0; i < GridLength; i++)
        {
            createdLevel.Grids.Add(new Vector2());
        }

        LevelController.RecreateGrids(GridLength, createdLevel);

    }

}

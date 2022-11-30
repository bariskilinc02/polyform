using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMechanics : MonoBehaviour
{
    private LevelController LevelController;
    private ObjectPool ObjectPool;
    private Bools Bools;

    private void Init()
    {
        LevelController = FindObjectOfType<LevelController>();
        ObjectPool = FindObjectOfType<ObjectPool>();
        Bools = FindObjectOfType<Bools>();
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            ResetLevel();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchDeleteMode();
        }

        if (Input.GetMouseButtonDown(0))
            DeleteLine();
    }

    #region Undo
    private void UndoMove()
    {
        if (LevelController.Boxs.Count == 0) return;

        ObjectPool.AddObjectToPool(LevelController.Boxs[LevelController.Boxs.Count - 1].gameObject);
        LevelController.Boxs.RemoveAt(LevelController.Boxs.Count - 1);
    }
    #endregion



    #region Reset
    private void ResetLevel()
    {
        if (LevelController.Boxs.Count == 0) return;

        for (int i = LevelController.Boxs.Count - 1; i>= 0; i--)
        {
            ObjectPool.AddObjectToPool(LevelController.Boxs[i].gameObject);
            LevelController.Boxs.Remove(LevelController.Boxs[i]);
        }
     
    }
    #endregion

    #region LineDelete

    private void SwitchDeleteMode()
    {
        Bools.isDeleteLineMode = !Bools.isDeleteLineMode;

        if (Bools.isDeleteLineMode) ShowLineDeleters();
        else HideLineDeleters();
    }

    private void ShowLineDeleters()
    {
        foreach(GameObject counter in LevelController.CountersX)
        {
            counter.transform.GetChild(0).gameObject.SetActive(false);
            counter.transform.GetChild(1).gameObject.SetActive(true);
        }

        foreach (GameObject counter in LevelController.CountersY)
        {
            counter.transform.GetChild(0).gameObject.SetActive(false);
            counter.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void HideLineDeleters()
    {
        foreach (GameObject counter in LevelController.CountersX)
        {
            counter.transform.GetChild(0).gameObject.SetActive(true);
            counter.transform.GetChild(1).gameObject.SetActive(false);
        }

        foreach (GameObject counter in LevelController.CountersY)
        {
            counter.transform.GetChild(0).gameObject.SetActive(true);
            counter.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void DeleteLine()
    {
      
        if (!Bools.isDeleteLineMode) return;


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (!hit.transform.CompareTag("Counter")) return;

            LineDeleterUnit lineDeleteUnit = hit.transform.GetComponent<LineDeleterUnit>();

            foreach (GameObject gObject in lineDeleteUnit.Raycaster.BoxsOnRow)
            {
                Box box = LevelController.Boxs.Find(x => x.gameObject == gObject);

                if (box != null)
                {
                    ObjectPool.AddObjectToPool(box.gameObject);
                    LevelController.Boxs.Remove(LevelController.Boxs.Find(x => x == box));
                }

            }
        }
    }



    #endregion

    #region Hint

    #endregion
}

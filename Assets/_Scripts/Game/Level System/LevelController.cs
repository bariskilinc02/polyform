using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class LevelController : MonoBehaviour
{
    private LevelCreator levelCreator;
    private ObjectPool ObjectPool;

    public List<Level> Levels;
    public Level currentLevel;

    #region Level Components
    public List<GroundUnit> GroundUnits;
    public List<Box> Boxs;

    public List<RaycasterUnit> RaycasterUnitsX, RaycasterUnitsY;
    public List<GameObject> CountersX, CountersY;
    public List<LineDeleterUnit> LineDeletersX, LineDeletersY;
    #endregion

    #region Prefabs
    [Space]
    [Header("Prefabs")]
    public GameObject GroundUnitPrefab;
    public GameObject RaycasterPrefab;
    public GameObject CounterPrefab;

    #endregion
    
    #region Parents
    [Space][Header("Parents")]
    public GameObject LevelMainParent;
    public GameObject BoxsParent;
    public GameObject GroundUnitParent;
    public GameObject RaycastersParent;
    public GameObject CounterParent;
    #endregion

    #region Fields
    public int CurrentLevelIndex;
    public int CurrentGridLength;

    public Vector3 CreationCenterPosition;
    #endregion

    #region Unity Functions
    private void Init()
    {
        Levels = Resources.LoadAll<Level>("Levels").ToList();

        levelCreator = FindObjectOfType<LevelCreator>();
        ObjectPool = FindObjectOfType<ObjectPool>();
    }
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        CreateLevel();
    }
    private void Update()
    {
    }
    #endregion

    private void GetActiveLevel()
    {
        currentLevel = Levels[CurrentLevelIndex];
        CurrentGridLength = currentLevel.GridLength;
    }

    public void GetNextLevel()
    {
        CurrentLevelIndex += 1;
        currentLevel = Levels[CurrentLevelIndex];
        CurrentGridLength = currentLevel.GridLength;
    }

    private void ClearAllComponents()
    {
        GroundUnits.Clear();
        Boxs.Clear();
        RaycasterUnitsX.Clear();
        RaycasterUnitsY.Clear();
        CountersX.Clear();
        CountersY.Clear();
        LineDeletersX.Clear();
        LineDeletersY.Clear();
    }


    #region Create State
    public void CreateLevel()
    {
        GetActiveLevel();
        ClearAllComponents();

        SetCreationCenterPosition();

        CreateGroundUnits();
        CreateRaycasterUnits();
        CreateCounterUnits();

    }

    private void CreateGroundUnits()
    {
        Vector3 placementVector = CreationCenterPosition;

        for (int a = 0; a < CurrentGridLength; a++)
        {
            for (int b = 0; b < CurrentGridLength; b++)
            {
                GameObject newGroundUnit = InstantiateBehaviour.InstantiateGameObject(GroundUnitPrefab, GroundUnitParent.transform, placementVector);
                newGroundUnit.GetComponent<GroundUnit>().SetPositionIndex(a, b);
                GroundUnits.Add(newGroundUnit.GetComponent<GroundUnit>());

                placementVector += new Vector3(1f, 0, 0);
            }

            placementVector = new Vector3(CreationCenterPosition.x, 0, placementVector.z - 1);
        }
    }

    private void CreateRaycasterUnits()
    {


        //Create Raycasters On X
        Vector3 placementVectorX = new Vector3(CreationCenterPosition.x - 1, 0, CreationCenterPosition.z);
        for (int i = 0; i < CurrentGridLength; i++)
        {
            GameObject newRaycasterUnit = InstantiateBehaviour.InstantiateGameObject(RaycasterPrefab, RaycastersParent.transform, placementVectorX);
            RaycasterUnit raycasterUnit = newRaycasterUnit.GetComponent<RaycasterUnit>();

            raycasterUnit.CreateRaycasts(CurrentGridLength);
            raycasterUnit.SetRaycastProperties(false, i, true);
            raycasterUnit.StartSendingRays();

            if (currentLevel.Grids[i].x == 0)
                raycasterUnit.isActive = false;

            RaycasterUnitsX.Add(raycasterUnit);

            placementVectorX = new Vector3(CreationCenterPosition.x - 1, 0, placementVectorX.z - 1);
        }

        //Create Raycasters On Y
        Vector3 placementVectorY = new Vector3(CreationCenterPosition.x, 0, CreationCenterPosition.z + 1);
        for (int i = 0; i < CurrentGridLength; i++)
        {
            GameObject newRaycasterUnit = InstantiateBehaviour.InstantiateGameObject(RaycasterPrefab, RaycastersParent.transform, placementVectorY);
            RaycasterUnit raycasterUnit = newRaycasterUnit.GetComponent<RaycasterUnit>();

            raycasterUnit.CreateRaycasts(CurrentGridLength);
            raycasterUnit.SetRaycastProperties(true, i, true);
            raycasterUnit.StartSendingRays();

            if (currentLevel.Grids[i].y == 0)
                raycasterUnit.isActive = false;

            RaycasterUnitsY.Add(raycasterUnit);

            placementVectorY = new Vector3(placementVectorY.x + 1, 0, CreationCenterPosition.z + 1);
        }
    }

    private void CreateCounterUnits()
    {
        //Create Counters On X
        Vector3 placementVectorX = new Vector3(CreationCenterPosition.x - 1, 0, CreationCenterPosition.z);
        for (int i = 0; i < CurrentGridLength; i++)
        {
            GameObject newCounter = InstantiateBehaviour.InstantiateGameObject(CounterPrefab, CounterParent.transform, placementVectorX);

            LineDeleterUnit lineDeleter = newCounter.GetComponent<LineDeleterUnit>();
            lineDeleter.Raycaster = RaycasterUnitsX[i];
            lineDeleter.X_or_Y = false;
            lineDeleter.Id = i;

            newCounter.transform.GetChild(0).transform.GetComponent<TextMeshPro>().text = currentLevel.Grids[i].x.ToString();

            CountersX.Add(newCounter);

            placementVectorX = new Vector3(CreationCenterPosition.x - 1, 0, placementVectorX.z - 1);
        }

        //Create Counters On Y
        Vector3 placementVectorY = new Vector3(CreationCenterPosition.x, 0, CreationCenterPosition.z + 1);
        for (int i = 0; i < CurrentGridLength; i++)
        {
            GameObject newCounter = InstantiateBehaviour.InstantiateGameObject(CounterPrefab, CounterParent.transform, placementVectorY);

            LineDeleterUnit lineDeleter = newCounter.GetComponent<LineDeleterUnit>();
            lineDeleter.Raycaster = RaycasterUnitsY[i];
            lineDeleter.X_or_Y = true;
            lineDeleter.Id = i;

            newCounter.transform.GetChild(0).transform.GetComponent<TextMeshPro>().text = currentLevel.Grids[i].y.ToString();

            CountersY.Add(newCounter);

            placementVectorY = new Vector3(placementVectorY.x + 1, 0, CreationCenterPosition.z + 1);
        }
    }
    private void SetCreationCenterPosition()
    {
        int gridLength = currentLevel.GridLength;

        CreationCenterPosition = new Vector3(-((gridLength / 2) - 0.5f), 0, ((gridLength / 2) - 0.5f));
    }

    #endregion

    #region Destroy State
    public void DestroyAllLevelObjects()
    {
        foreach(GroundUnit Unit in GroundUnits)
        {
            Destroy(Unit.gameObject);
        }

        foreach (Box Unit in Boxs)
        {
            ObjectPool.AddObjectToPool(Unit.gameObject);
        }

        foreach (RaycasterUnit Unit in RaycasterUnitsX)
        {
            Destroy(Unit.gameObject);
        }


        foreach (RaycasterUnit Unit in RaycasterUnitsY)
        {
            Destroy(Unit.gameObject);
        }

        foreach (GameObject Unit in CountersX)
        {
            Destroy(Unit);
        }

        foreach (GameObject Unit in CountersY)
        {
            Destroy(Unit);
        }


        ClearAllComponents();
    }
    #endregion

    #region GamePlay State
    public bool IsLevelFinished()
    {
        bool isLevelFinished = true;
        for (int i = 0; i < CurrentGridLength; i++)
        {
            if (currentLevel.Grids[i].x != 0)
            {
                if (currentLevel.Grids[i].x != RaycasterUnitsX[i].HitCount || RaycasterUnitsX[i].HeightOfBoxsOnRow.Count != CurrentGridLength)
                {
                    isLevelFinished = false;
                    break;
                }
            }

            if (currentLevel.Grids[i].y != 0)
            {
                if (currentLevel.Grids[i].y != RaycasterUnitsY[i].HitCount || RaycasterUnitsY[i].HeightOfBoxsOnRow.Count != CurrentGridLength)
                {
                    isLevelFinished = false;
                    break;

                }
            }

            
        }
        return isLevelFinished;
    }
    #endregion
}

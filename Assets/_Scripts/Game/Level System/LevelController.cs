using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelController : MonoBehaviour
{
    private LevelCreator levelCreator;

    public List<Level> Levels;
    private Level currentLevel;

    #region Level Components
    public List<GameObject> GroundUnits;
    public List<GameObject> Boxs;


    public List<GroundUnit> Units, GetRays;
    public List<RaycasterUnit> RaycasterUnitsX, RaycasterUnitsY;
    public List<GameObject> BoxCounterParentsX, BoxCounterParentsY, BoxCounterTextsX, BoxCounterTextsY;
    public List<LineDeleterUnit> LineDeletersX, LineDeletersY;
    #endregion

    #region Prefabs
    [Space]
    [Header("Prefabs")]
    public GameObject GroundUnitPrefab;
    public GameObject RaycasterPrefab;

    #endregion
    
    #region Parents
    [Space][Header("Parents")]
    public GameObject LevelMainParent;
    public GameObject BoxsParent;
    public GameObject GroundUnitParent;
    public GameObject RaycastersParent;
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
    }
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateLevel();
        }
        
    }
    #endregion

    private void GetActiveLevel()
    {
        currentLevel = Levels[CurrentLevelIndex];
        CurrentGridLength = currentLevel.GridLength;
    }

    private void ClearAllComponents()
    {
        Boxs.Clear();
        BoxCounterParentsX.Clear();
        BoxCounterParentsY.Clear();
        BoxCounterTextsX.Clear();
        BoxCounterTextsY.Clear();
        LineDeletersX.Clear();
        LineDeletersY.Clear();
    }


    #region Create State
   [ContextMenu("Create Level")]
    private void CreateLevel()
    {
        GetActiveLevel();
        ClearAllComponents();

        SetCreationCenterPosition();

        CreateGroundUnits();
        CreateRaycasterUnits();

    }

    private void CreateGroundUnits()
    {
        Vector3 placementVector = CreationCenterPosition;

        for (int a = 0; a < CurrentGridLength; a++)
        {
            for (int b = 0; b < CurrentGridLength; b++)
            {
                GameObject newGroundUnit = InstantiateBehaviour.InstantiateGameObject(GroundUnitPrefab, GroundUnitParent.transform, placementVector);
                GroundUnits.Add(newGroundUnit);

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

            BoxCounterParentsX.Add(newRaycasterUnit);
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

            BoxCounterParentsX.Add(newRaycasterUnit);
            RaycasterUnitsX.Add(raycasterUnit);

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
    private void DestroyAllLevelObjects()
    {

    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickEvents : MonoBehaviour
{
    #region Components
    private SliderInteractions SliderInteractions;
    private ObjectPool ObjectPool;
    private LevelController LevelController;

    private GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
    public RectTransform canvasRect;

    public GameObject MovingBox;
    public GameObject SelectedBox; private Box SelectedBoxUnit;

    public GameObject BoxsParent;
    #endregion

    #region Fields
    private const float _movingBoxYLimit = 0.5f;

    #endregion

    #region Unity Functions

    private void Init()
    {
        SliderInteractions = FindObjectOfType<SliderInteractions>();
        ObjectPool = FindObjectOfType<ObjectPool>();
        LevelController = FindObjectOfType<LevelController>();

        m_EventSystem = EventSystem.current;
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }
    void Start()
    {
        Init();
    }


    void Update()
    {
        GetMovingBoxFromPool();

        ShowBoxOnGrid();
        ReplaceBoxOnGrid();
        SelectBoxOnGrid();

        SelectedBoxConfiguration();

    }


    #endregion

    #region Functions
    public void ShowBoxOnGrid()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Vector3 BoxPosition = new Vector3(hit.transform.position.x, _movingBoxYLimit, hit.transform.position.z);

            if (hit.transform.CompareTag("GroundUnit"))
            {
                GroundUnit groundUnit = hit.transform.GetComponent<GroundUnit>();
                Box box = MovingBox.GetComponent<Box>();

                int accesableHeight = groundUnit.GetMinAccesableHeight(LevelController, groundUnit);
                box.Effect(accesableHeight);

                MovingBox.SetActive(true);
                MovingBox.transform.position = Vector3.Lerp(MovingBox.transform.position, BoxPosition, 0.2f);

                if (accesableHeight >= LevelController.CurrentGridLength)
                    MovingBox.SetActive(false);
            }
            else
            {
                MovingBox.SetActive(false);
                BoxPosition = new Vector3(hit.point.x, _movingBoxYLimit, hit.point.z);
                MovingBox.transform.position = Vector3.Lerp(MovingBox.transform.position, BoxPosition, 0.3f); //BoxLocation;
            }

        }
        else
        {
            MovingBox.SetActive(false);
        }



    }

    public void ReplaceBoxOnGrid()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.CompareTag("GroundUnit"))
                {
                    GroundUnit groundUnit = hit.transform.GetComponent<GroundUnit>();
                    Box box = MovingBox.GetComponent<Box>();

                    int accesableHeight = groundUnit.GetMinAccesableHeight(LevelController, groundUnit);
                    box.Effect(accesableHeight);

                    box.X = groundUnit.X;
                    box.Y = groundUnit.Y;

                    if (accesableHeight >= LevelController.CurrentGridLength) 
                        return;

                    LevelController.Boxs.Add(MovingBox.GetComponent<Box>());

                    Vector3 BoxPosition = new Vector3(hit.transform.position.x, _movingBoxYLimit, hit.transform.position.z);
                    MovingBox.transform.position = BoxPosition;
                    MovingBox.transform.parent = BoxsParent.transform;
                    MovingBox.layer = 6;
                    SelectedBox = MovingBox;
                    SelectedBoxUnit = MovingBox.GetComponent<Box>();
                    MovingBox = null;

                    //Reverser.RV.MoveList.Add(Moving_Gameobject.gameObject);
                }
                else
                {
                }

            }

        }
    }

    public void SelectBoxOnGrid()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.transform.CompareTag("Box"))
                {
                    Box box = hit.transform.GetComponent<Box>();

                    SelectedBox = hit.transform.gameObject;
                    SelectedBoxUnit = hit.transform.GetComponent<Box>();

                    SliderInteractions.SetSliderValue(box.Length, LevelController.CurrentGridLength - 1);
                }
            }
        }

       
    }

    private void SelectedBoxConfiguration()
    {
        if (SelectedBox == null) return;

        GroundUnit groundUnit =  LevelController.GroundUnits.Find(x => x.X == SelectedBoxUnit.X && x.Y == SelectedBoxUnit.Y);

        SliderInteractions.GetSliderValue(groundUnit,LevelController, SelectedBox.GetComponent<Box>());
    }
    public void GetMovingBoxFromPool()
    {
        if (MovingBox == null)
            MovingBox = ObjectPool.RemoveObjectFromPool();
    }
    #endregion

}

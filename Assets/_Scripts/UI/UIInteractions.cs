using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIInteractions : MonoBehaviour
{
    private CanvasGroups CanvasGroups;
    private AnimCurvesUI AnimationCurves;
    private Bools Bools;
    private CameraController CameraController;

    [Header("Main Screen Buttons")]
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button MarketButton;

    [Header("Game Screen Buttons")]
    [SerializeField] private Button BackToMenuButton;


    #region Unity Functions
    private void Init()
    {
        CanvasGroups = FindObjectOfType<CanvasGroups>();
        Bools = FindObjectOfType<Bools>();
        AnimationCurves = FindObjectOfType<AnimCurvesUI>();
        CameraController = FindObjectOfType<CameraController>();

        PlayButton.onClick.AddListener(btn_Menu_to_Play);
        SettingsButton.onClick.AddListener(btn_Menu_to_Play);
        MarketButton.onClick.AddListener(btn_Menu_to_Play);

        BackToMenuButton.onClick.AddListener(btn_Play_to_Menu);
    }

    private void Awake()
    {
        Init();
    }
    #endregion

    #region Button Interactions

    public void btn_Play_to_Menu()
    {
        if (Bools.is_OnTransition == true) return;

        StartCoroutine(UIBehaviours.TransitionBetweenScreens(CanvasGroups.MainScreen, CanvasGroups.PlayScreen, 0.5f, AnimationCurves.Menu_to_Game_Curve));
        StartCoroutine(Play_to_Menu_Routine(Bools, AnimationCurves.Menu_to_Game_Curve));
    }

    public void btn_Menu_to_Play()
    {
        if (Bools.is_OnTransition == true) return;

        StartCoroutine(UIBehaviours.TransitionBetweenScreens(CanvasGroups.PlayScreen, CanvasGroups.MainScreen, 0.5f, AnimationCurves.Menu_to_Game_Curve));
        StartCoroutine(Menu_to_Play_Routine(Bools, AnimationCurves.Menu_to_Game_Curve));
    }

    /*
    public void btn_ResetLevel()
    {
        for (int i = Parents.BoxParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Parents.BoxParent.transform.GetChild(i).gameObject);
        }
    }

    public void btn_Menu_to_Market()
    {
        StartCoroutine(InteractionUI.OpenCloseScreens(Groups.Market, Groups.MainScreen, 0.5f, AnimationCurves.Menu_to_Game_Curve));
    }

    public void btn_Market_to_Menu()
    {
        StartCoroutine(InteractionUI.OpenCloseScreens(Groups.MainScreen, Groups.Market, 0.5f, AnimationCurves.Menu_to_Game_Curve));
    }

    public void btn_Menu_to_Credits()
    {
        StartCoroutine(GameManager.OpenScreens_Routine(Groups.Credits, 0.5f, AnimationCurves.Menu_to_Game_Curve));
        StartCoroutine(GameManager.ScaleScreens_Routine(Groups.Credits.gameObject, 0.4f, new Vector3(1, 0, 0), new Vector3(1, 1, 1), AnimationCurves.Bubble_Curve));
    }

    public void btn_Credits_to_Menu()
    {
        StartCoroutine(GameManager.CloseScreens_Routine(Groups.Credits, 0.5f, AnimationCurves.Menu_to_Game_Curve));
        StartCoroutine(GameManager.ScaleScreens_Routine(Groups.Credits.gameObject, 0.4f, new Vector3(1, 1, 1), new Vector3(1, 0, 0), AnimationCurves.r_Bubble_Curve));
    }

    public void btn_Menu_to_Settings()
    {
        StartCoroutine(InteractionUI.OpenCloseScreens(Groups.Settings, Groups.MainScreen, 0.5f, AnimationCurves.Menu_to_Game_Curve));
    }
    

    public void btn_Settings_to_Menu()
    {
        StartCoroutine(InteractionUI.OpenCloseScreens(Groups.MainScreen, Groups.Settings, 0.5f, AnimationCurves.Menu_to_Game_Curve));
    }
    */
    #endregion

    #region Routines
    public IEnumerator OpenCloseScreens(CanvasGroup toOpen, CanvasGroup toClose, float MaxTime, AnimationCurve TransitionCurve)
    {
        float time = 0;
        toOpen.alpha = 0;
        toClose.alpha = 1;
        toOpen.gameObject.SetActive(true);
        while (time < MaxTime)
        {
            toOpen.alpha = Mathf.Lerp(0, 1, TransitionCurve.Evaluate(time / MaxTime));
            toClose.alpha = Mathf.Lerp(1, 0, TransitionCurve.Evaluate(time / MaxTime));
            time += Time.deltaTime;
            yield return null;
        }

        toClose.gameObject.SetActive(false);
    }


    public IEnumerator Menu_to_Play_Routine(Bools Bools, AnimationCurve Curve)
    {
        Bools.is_InMenu = false;
        Bools.is_OnTransition = true;

        //CameraController.SetCameraTransformToGame();
        CameraController.CameraTransitionToGame();

        yield return new WaitForSeconds(1f);

        Bools.is_OnTransition = false;
        Bools.is_Playing = true;
    }

    public IEnumerator Play_to_Menu_Routine(Bools Bools, AnimationCurve Curve)
    {
        Bools.is_InMenu = true;
        Bools.is_OnTransition = true;

        //CameraController.SetCameraTransformToGame();

        CameraController.CameraTransitionToMenu();

        yield return new WaitForSeconds(1f);

        Bools.is_OnTransition = false;
        Bools.is_Playing = false;
    }

    public IEnumerator OpenCloseScreens_Routine(CanvasGroup toOpen, CanvasGroup toClose, float MaxTime, AnimationCurve TransitionCurve)
    {
        float time = 0;
        toOpen.alpha = 0;
        toClose.alpha = 1;
        toOpen.gameObject.SetActive(true);
        while (time < MaxTime)
        {
            toOpen.alpha = Mathf.Lerp(0, 1, TransitionCurve.Evaluate(time / MaxTime));
            toClose.alpha = Mathf.Lerp(1, 0, TransitionCurve.Evaluate(time / MaxTime));
            time += Time.deltaTime;
            yield return null;
        }

        toClose.gameObject.SetActive(false);
    }

    public IEnumerator OpenScreens_Routine(CanvasGroup toOpen, float MaxTime, AnimationCurve TransitionCurve)
    {
        float time = 0;
        toOpen.alpha = 0;
        toOpen.gameObject.SetActive(true);
        while (time < MaxTime)
        {
            toOpen.alpha = Mathf.Lerp(0, 1, TransitionCurve.Evaluate(time / MaxTime));
            time += Time.deltaTime;
            yield return null;
        }

    }

    public IEnumerator CloseScreens_Routine(CanvasGroup toClose, float MaxTime, AnimationCurve TransitionCurve)
    {
        float time = 0;
        toClose.alpha = 1;
        while (time < MaxTime)
        {
            toClose.alpha = Mathf.Lerp(1, 0, TransitionCurve.Evaluate(time / MaxTime));
            time += Time.deltaTime;
            yield return null;
        }

        toClose.gameObject.SetActive(false);
    }

    public IEnumerator ScaleScreens_Routine(GameObject Screen, float MaxTime, Vector3 StartScale, Vector3 EndScale, AnimationCurve TransitionCurve)
    {
        float time = 0;
        while (time < MaxTime)
        {
            Screen.transform.localScale = Vector3.LerpUnclamped(StartScale, EndScale, TransitionCurve.Evaluate(time / MaxTime));
            time += Time.deltaTime;
            yield return null;
        }
    }
    #endregion



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIBehaviours
{
    public static IEnumerator TransitionBetweenScreens(CanvasGroup toOpen, CanvasGroup toClose, float MaxTime, AnimationCurve TransitionCurve)
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
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class QuizUiManager : MonoBehaviour
{ 
    //Time (in seconds) that the panel takes to fade in/out
    public float fadeTime = 1.0f;
    //Controls the panel’s visibility and interaction
    public CanvasGroup canvasGroup;
    //Handles the panel’s positioning for animation
    public RectTransform rectTransform;
    //List of UI items that will animate in
    public List<GameObject> items = new List<GameObject>();

    //Method to animate the panel and items in
    public void PanelFadeIn()
    {
        //Checks that required components are not null
        if (canvasGroup == null || rectTransform == null)
        {
            Debug.LogError("[QuizUiManager] CanvasGroup or RectTransform is not assigned.");
            return;
        }

        try
        {
            //Set panel to be invisible and off-screen
            canvasGroup.alpha = 0f;
            rectTransform.localPosition = new Vector3(0f, -1000f, 0f);

            //Animate panel moving into position and fading in
            rectTransform.DOAnchorPos(Vector2.zero, fadeTime, false).SetEase(Ease.OutElastic);
            canvasGroup.DOFade(1, fadeTime);

            //Start animating individual items
            StartCoroutine("ItemsAnimation");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[QuizUiManager] Error during PanelFadeIn: {ex.Message}");
        }
    }

    //Method to animate the panel and items out
    public void PanelFadeOut()
    {
        //Checks that required components are not null
        if (canvasGroup == null || rectTransform == null)
        {
            Debug.LogError("[QuizUiManager] CanvasGroup or RectTransform is not assigned.");
            return;
        }

        try
        {
            //Set initial position and fade out
            canvasGroup.alpha = 0f;
            rectTransform.localPosition = new Vector3(0f, 0f, 0f);

            //Animate panel moving off-screen and fading out
            rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
            canvasGroup.DOFade(0, fadeTime);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[QuizUiManager] Error during PanelFadeOut: {ex.Message}");
        }
    }

    //Coroutine to animate each item in the list with a bounce effect
    IEnumerator ItemsAnimation()
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("[QuizUiManager] Items list is empty or null.");
            yield break;
        }

        //Set all item scales to 0 (hidden)
        foreach (var item in items)
        {
            if (item != null)
                item.transform.localScale = Vector3.zero;
        }

        //Scale each item up with a bounce effect
        foreach (var item in items)
        {
            if (item != null)
            {
                try
                {
                    item.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
                   
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[QuizUiManager] Error animating item: {ex.Message}");
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
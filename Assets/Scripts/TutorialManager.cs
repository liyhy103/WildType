using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> steps;
    public GameObject darkOverlay;
    public Material holeMaskMaterial;
    public TMP_Text guideTextDisplay;

    private int currentStepIndex = 0;

    void Start() => ShowStep(0);

    public void ShowStep(int index)
    {
        if (index >= steps.Count) { EndTutorial(); return; }

        currentStepIndex = index;
        var step = steps[index];
        Debug.Log($"[Tutorial] Step {index}: {step.stepName}");

        // Find targetUI if no onClick event
        if (step.targetUI == null && !string.IsNullOrEmpty(step.targetUIName))
        {
            GameObject found = GameObject.Find(step.targetUIName);
            if (found != null)
                step.targetUI = found.GetComponent<RectTransform>();
        }

        if (step.targetUI == null)
        {
            Debug.LogWarning($"[Tutorial] Step {index} has no targetUI.");
            return;
        }

        // Set on the Mask and Guide text
        darkOverlay.SetActive(true);
        guideTextDisplay.text = step.guideText;

        // Calculate Mask Hole
        SetMaskHole(step.targetUI);

        // Automatically go next step if the target with no onClick event
        if (step.autoAdvanceAfterSeconds > 0f)
        {
            StartCoroutine(AutoAdvanceToNextStep(step.autoAdvanceAfterSeconds));
        }
        else
        {
            // Click and go to the next step
            Button btn = step.targetUI.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    btn.onClick.RemoveAllListeners();
                    ShowStep(currentStepIndex + 1);
                });
            }
            else
            {
                Debug.LogWarning($"[Tutorial] Step {index} targetUI has no Button component.");
            }
        }
    }

    void SetMaskHole(RectTransform targetUI)
    {
        Vector3[] corners = new Vector3[4];
        targetUI.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        Vector2 screenBL = RectTransformUtility.WorldToScreenPoint(Camera.main, bottomLeft);
        Vector2 screenTR = RectTransformUtility.WorldToScreenPoint(Camera.main, topRight);
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        Vector2 uvBL = new Vector2(screenBL.x / screenSize.x, screenBL.y / screenSize.y);
        Vector2 uvTR = new Vector2(screenTR.x / screenSize.x, screenTR.y / screenSize.y);
        Vector2 uvCenter = (uvBL + uvTR) * 0.5f;
        Vector2 uvSize = new Vector2(uvTR.x - uvBL.x, uvTR.y - uvBL.y);

        holeMaskMaterial.SetVector("_HoleCenter", new Vector4(uvCenter.x, uvCenter.y, 0, 0));
        holeMaskMaterial.SetVector("_HoleSize", new Vector4(uvSize.x, uvSize.y, 0, 0));
    }

    private IEnumerator AutoAdvanceToNextStep(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowStep(currentStepIndex + 1);
    }

    public void EndTutorial()
    {
        darkOverlay.SetActive(false);
        guideTextDisplay.text = "";
        holeMaskMaterial.SetVector("_HoleSize", Vector4.zero);
        Debug.Log("[Tutorial] Finished.");
    }
}
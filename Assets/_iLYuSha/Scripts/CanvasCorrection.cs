using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasCorrection : MonoBehaviour
{
    public RectTransform background;
    private float backgroundRatio;

    void Start()
    {
        UpdateBackground();
    }

    void Update()
    {
        //UpdateBackground();
    }

    public void UpdateBackground()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();

        float screenWidthScale = Screen.width / canvasScaler.referenceResolution.x;
        float screenHeightScale = Screen.height / canvasScaler.referenceResolution.y;

        canvasScaler.matchWidthOrHeight = screenWidthScale > screenHeightScale ? 1 : 0;

        if (background != null)
        {
            backgroundRatio = screenHeightScale / screenWidthScale;
            if (backgroundRatio > 1)
                background.sizeDelta *= backgroundRatio;

            else
                background.sizeDelta /= backgroundRatio;
        }
    }
}

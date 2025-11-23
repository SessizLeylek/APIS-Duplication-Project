using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverChargeIndicator : MonoBehaviour
{
    [HideInInspector] public Transform leverTransform;
    public float chargeDuration = 1;
    private float remainingTime = 1;
    private Image uiImage;

    void Start()
    {
        uiImage= GetComponent<Image>();
        remainingTime = chargeDuration;
    }

    void Update()
    {
        Camera activeCam = FindObjectOfType<AudioListener>().GetComponent<Camera>();
        Vector3 viewportVec = activeCam.WorldToViewportPoint(leverTransform.position);
        uiImage.enabled = (viewportVec.z > 0) && (Time.timeScale > 0);
        uiImage.rectTransform.anchoredPosition = new Vector2(viewportVec.x * Screen.width, viewportVec.y * Screen.height);

        remainingTime -= Time.deltaTime;
        uiImage.fillAmount = remainingTime / chargeDuration;

        if(remainingTime < 0)
            Destroy(gameObject);
    }
}

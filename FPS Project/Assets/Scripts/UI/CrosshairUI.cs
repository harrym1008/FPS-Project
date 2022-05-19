using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public bool renderCrosshair;
    [Range(30, 300)] public float distance = 50f;
    public float smoothness = 2f;
    float targetDistance;

    [SerializeField] Image[] images;
    [SerializeField] RectTransform upImage;
    [SerializeField] RectTransform downImage;
    [SerializeField] RectTransform leftImage;
    [SerializeField] RectTransform rightImage;


    private void Start()
    {
        targetDistance = distance;
    }

    private void Update()
    {
        targetDistance = Mathf.Lerp(targetDistance, distance, smoothness * Time.deltaTime);
        UpdatePositions();

        if (!renderCrosshair)
        {
            foreach (Image image in images)
            {
                image.enabled = false;
            }
        }
        else
        {
            foreach (Image image in images)
            {
                image.enabled = true;
            }
        }
    }

    private void UpdatePositions()
    {
        upImage.localPosition = new Vector3(0f, targetDistance, 0f);
        downImage.localPosition = new Vector3(0f, -targetDistance, 0f);
        leftImage.localPosition = new Vector3(-targetDistance, 0f, 0f);
        rightImage.localPosition = new Vector3(targetDistance, 0f, 0f);
    }
}

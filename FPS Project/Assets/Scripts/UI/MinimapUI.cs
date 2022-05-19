using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUI : MonoBehaviour
{
    public GameObject minimapCameraObject;
    public RenderTexture renderTexture;

    Camera minimapCamera;
    Transform player;

    private void Start()
    {
        minimapCamera = Instantiate(minimapCameraObject, null).GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Update();

        minimapCamera.targetTexture = renderTexture;
    }

    private void Update()
    {
        minimapCamera.transform.position = new Vector3(player.position.x, 500, player.position.z);
        minimapCamera.transform.eulerAngles = new Vector3(90f, 0f, -player.eulerAngles.y);
    }
}

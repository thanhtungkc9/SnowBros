using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;

    public Vector2 margin = new Vector2(1, 1); // If the player stays inside this margin, the camera won't move.
    public Vector2 smoothing = new Vector2(3, 3); // The bigger the value, the faster is the camera.

    public BoxCollider2D cameraBounds;

    private Vector3 min, max;

    public bool isFollowing;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    void Start()
    {
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;
        isFollowing = true;
        mainCamera = GetComponent<Camera>();

        // Camera background Colour.
        if (Application.loadedLevelName != "Town")
            mainCamera.backgroundColor = Color.black;
    }

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;

        if (isFollowing)
        {
            if (Mathf.Abs(x - player.position.x) > margin.x)
                x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);

            if (Mathf.Abs(y - player.position.y) > margin.y)
                y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
        }

        // ortographicSize is the haldf of the height of the Camera.
        var cameraHalfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height);

        x = Mathf.Clamp(x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, min.y + mainCamera.orthographicSize, max.y - mainCamera.orthographicSize);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    // PixelPerfectScript.
    public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
    {
        float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
        return adjustedUnityUnits;
    }

    void LateUpdate()
    {
        Vector3 newPos = transform.position;
        Vector3 roundPos = new Vector3(RoundToNearestPixel(newPos.x, mainCamera), RoundToNearestPixel(newPos.y, mainCamera), newPos.z);
        transform.position = roundPos;
    }

    public void UpdateBounds()
    {
        min = cameraBounds.bounds.min;
        max = cameraBounds.bounds.max;
    }
}
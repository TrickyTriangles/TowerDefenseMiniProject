
using UnityEngine;

public class CameraController: MonoBehaviour
{
    Transform swivel, stick;

    //public HexGrid grid;

    float zoom = 1f;

    static CameraController instance;

    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float rotationSpeed;

    void OnEnable() { instance = this; }

    void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }

    void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }

        float rotationDelta = Input.GetAxisRaw("Rotation");
        if (Mathf.Abs(rotationDelta) >= 0.1f)
        {
            if (rotationDelta > 0) { Mathf.FloorToInt(rotationDelta); }
            if (rotationDelta < 0) { Mathf.CeilToInt(rotationDelta); }
            AdjustRotation(rotationDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (Mathf.Abs(xDelta) >= 0.1f || Mathf.Abs(zDelta) >= 0.1f) //added >= abs to prevent drifting camera
        {
            AdjustPosition(xDelta, zDelta);
        }
    }

    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    public float moveSpeedMinZoom, moveSpeedMaxZoom;

    void AdjustPosition(float xDelta, float zDelta)
    {
        // direction prevents moving faster on the diagonals
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        // damping prevents the controls from drifting after WASD released
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom)
                        * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = position;//ClampPosition(position);
        
        /*
        Vector3 ClampPosition(Vector3 position)
        {
            // limits movement to grid
            float xMax = 30f;
            position.x = Mathf.Clamp(position.x, 0f, xMax);

            float zMax = 30f;
            position.z = Mathf.Clamp(position.z, 0f, zMax);
            return position;
        }
        */
        
    }

    float rotationAngle;

    void AdjustRotation(float delta)
    {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;

        if (rotationAngle < 0f) { rotationAngle += 360f; }
        else if (rotationAngle >= 360f) { rotationAngle -= 360f; }

        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    public static bool Locked
    {
        set { instance.enabled = !value; }
    }

    public static void ValidatePosition()
    {
        instance.AdjustPosition(0f, 0f);
    }
}

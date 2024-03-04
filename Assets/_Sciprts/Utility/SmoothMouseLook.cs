using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public float sensitivity = 4.0f;
    [HideInInspector]
    public float sensitivityAmt = 4.0f; // Actual sensitivity modified by IronSights Script

    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -85f;
    private float maximumY = 85f;
    [HideInInspector]
    public float rotationX = 0.0f;
    [HideInInspector]
    public float rotationY = 0.0f;
    [HideInInspector]
    public float inputY = 0.0f;

    public float smoothSpeed = 0.35f;

    private Quaternion originalRotation;
    private Transform myTransform;
    [HideInInspector]
    public float recoilX; // Non-recovering recoil amount managed by WeaponKick function of WeaponBehavior.cs
    [HideInInspector]
    public float recoilY; // Non-recovering recoil amount managed by WeaponKick function of WeaponBehavior.cs

    void Start()
    {
        if (GetComponent<Rigidbody>()) { GetComponent<Rigidbody>().freezeRotation = true; }

        myTransform = transform; // Cache transform for efficiency

        originalRotation = myTransform.localRotation;
        // Sync the initial rotation of the main camera to the y rotation set in the editor
        Vector3 tempRotation = new Vector3(0, transform.eulerAngles.y, 0);
        originalRotation.eulerAngles = tempRotation;

        sensitivityAmt = sensitivity; // Initialize sensitivity amount from var set by player

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            rotationX += Input.GetAxisRaw("Mouse X") * sensitivityAmt * Time.timeScale; // Lower sensitivity at slower time settings
            rotationY += Input.GetAxisRaw("Mouse Y") * sensitivityAmt * Time.timeScale;

            // Reset vertical recoilY value if it would exceed maximumY amount 
            if (maximumY - Input.GetAxisRaw("Mouse Y") * sensitivityAmt * Time.timeScale < recoilY)
            {
                rotationY += recoilY;
                recoilY = 0.0f;
            }
            // Reset horizontal recoilX value if it would exceed maximumX amount 
            if (maximumX - Input.GetAxisRaw("Mouse X") * sensitivityAmt * Time.timeScale < recoilX)
            {
                rotationX += recoilX;
                recoilX = 0.0f;
            }

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY - recoilY, maximumY - recoilY);

            inputY = rotationY + recoilY; // Set public inputY value for use in other scripts

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX + recoilX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY + recoilY, -Vector3.right);

            // Smooth the mouse input
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, originalRotation * xQuaternion * yQuaternion, smoothSpeed * Time.smoothDeltaTime * 60 / Time.timeScale);
            // Lock mouselook roll to prevent gun rotating with fast mouse movements
            myTransform.rotation = Quaternion.Euler(myTransform.rotation.eulerAngles.x, myTransform.rotation.eulerAngles.y, 0.0f);
        }
    }

    // Function used to limit angles
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}

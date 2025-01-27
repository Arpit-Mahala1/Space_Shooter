
using UnityEngine;

public class Follow_player : MonoBehaviour
{
    public Transform Player;
    public Camera Camera;
    public Camera OverLay_Cam;

    //private int cam_fov = 75;
    private Vector3 offset;

    private Vector3 currentVelocity;
    private float rotationVelocity;

    void Start()
    {
        // Set the initial offset relative to the car
        offset = new Vector3(-15, 10, 0);
        Camera.transform.position = Player.position + Player.transform.TransformDirection(offset);
        Camera.transform.LookAt(Player);
        OverLay_Cam.transform.position = Player.position + Player.transform.TransformDirection(offset);
        OverLay_Cam.transform.LookAt(Player);
    }

    void Update()
    {
        // Calculate the target position with the offset
        Vector3 targetPosition = Player.position + Player.transform.TransformDirection(offset);

        // Smoothly move the camera to the target position
        Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, targetPosition, ref currentVelocity, 0.1f);
        OverLay_Cam.transform.position = Vector3.SmoothDamp(Camera.transform.position, targetPosition, ref currentVelocity, 0.1f);


        // Calculate the target rotation to look at the car
        Vector3 direction = Player.position - Camera.transform.position;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float smoothedAngle = Mathf.SmoothDampAngle(Camera.transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);

        // Apply the smoothed rotation to the camera
        Camera.transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);
        OverLay_Cam.transform.rotation = Quaternion.Euler(0, smoothedAngle, 0);



    }
}


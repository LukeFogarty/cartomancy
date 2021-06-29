using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    public Camera cam;
    readonly float smoothSpeed = 0.125f;
    float camRotation;

    private void Update()
    {
        if (player.GetComponent<PlayerMovement>().canMove == true)
        {
            float h = Input.GetAxis("CameraHorizontal");

            if (Input.GetKey(KeyCode.E) || h >0.5f)
            {
                camRotation++;
            }
            if (Input.GetKey(KeyCode.Q) || h <- 0.5f)
            {
                camRotation--;
            }
            if (camRotation > 360) camRotation = 0;
            if (camRotation < 0) camRotation = 360;
        }

        if (player != null)
        {
            Vector3 fwd = transform.TransformDirection(-Vector3.forward + Vector3.up * 0.5f);
            fwd.y = 0;
            Debug.DrawRay(player.transform.position, (fwd + Vector3.up * -0.1f) * 5f, Color.red);
            Debug.DrawRay(player.transform.position, fwd * 5f, Color.white);
        }

        Vector3 desiredPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.eulerAngles = new Vector3(0, camRotation, 0);
    }

}


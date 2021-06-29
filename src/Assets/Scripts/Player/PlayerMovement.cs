using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float SpeedMax;
    public bool canMove = true;
    public bool hurt = false;
    public bool useMap = true;
    public bool gotCrystal = false;
    public Animator animator;
    public Transform spawn;
    public Vector3 spawnPoint;
    public GameObject burst;
    public ParticleSystem walker;
    public CharacterController controller;
    public Camera cam;
    public AudioSource audioSource;
    public AudioClip[] clips;

    private float h;
    private float v;
    private readonly float rotateSpeed = 8f;
    private Vector3 moveDirection;
    private GameObject camTarget;
    public int respawn = 0;
    public int coinsCollected = 0;
    int step = 3;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camTarget = GameObject.FindWithTag("cameraTarget");
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        transform.position = spawnPoint;
        respawn = 50;
    }

    void Update()
    {
        if (gotCrystal == true)
        {
            canMove = false;
            useMap = false;
            animator.SetBool("isMoving", false);

        }
        if (canMove == false)
        {
            animator.SetBool("isMoving", false);
        }
        if (respawn > 0)
        {
            PlayerHurt();
        }

        if (canMove == true) { 
        // Bit shift the index of the layer (8) to get floor layer
        int layerMask = 1 << 8;

        Ray ray = new Ray(new Vector3(transform.position.x, -5, transform.position.z), new Vector3(0, 5, 0));
            if (!Physics.Raycast(ray, out _, Mathf.Infinity, layerMask))
            {
                if (respawn == 0)
                {
                    PlayerHurt();
                }
            }
        }
        if (canMove == true && respawn == 0)
        {
            //get input from joystick/arrows/WASD
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // base movement on camera
            if (camTarget != null)
            {
                Vector3 correctedVertical = v * camTarget.transform.forward;
                Vector3 correctedHorizontal = h * camTarget.transform.right;

                Vector3 combinedInput = correctedHorizontal + correctedVertical;

                // only update on the current direction being input, else travel in the last direction

                if ((h != 0 || v != 0) && canMove == true)
                {
                    moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z); moveSpeed = SpeedMax;
                    animator.SetBool("isMoving", true);

                    if (h > 0) { }

                    if (step == 0)
                    {
                        audioSource.clip = clips[Random.Range(0, 6)]; step = 15;
                        audioSource.Play();
                    }
                    step--;

                }

                if ((h == 0 && v == 0) || canMove == false) { moveDirection = transform.forward; moveSpeed = 0; step = 3; animator.SetBool("isMoving", false); }
            }

            //look in movement direction
            Quaternion rot = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
            transform.rotation = targetRotation;
            transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
            controller.Move((moveDirection * moveSpeed) * Time.deltaTime);
        }
    }


    public void PlayerHurt()
    {
        respawn++;
        canMove = false;
        useMap = false;
        hurt = true;
        walker.Stop();
        gameObject.GetComponent<LockY>().enabled = false;
        if (respawn == 5)
        {
            animator.SetBool("isHurt", true);
            animator.SetBool("isMoving", false);
            audioSource.clip = clips[Random.Range(6, 9)];
            audioSource.Play();
        }

        if (respawn > 10 && respawn<=15)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        }
        if (respawn > 15 && respawn <= 20)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        }
        if (respawn == 25) Instantiate(burst, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        if (respawn > 30)
        {
           gameObject.GetComponent<LockY>().enabled = true;

        }
        if(respawn > 60)
        {
            transform.position = spawnPoint;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, spawn.eulerAngles.y, transform.eulerAngles.z);
            canMove = true;
            walker.Play();
            animator.SetBool("isHurt", false);
            animator.SetBool("isMoving", false);
            hurt = false;
        }
        if (respawn > 70)
        {
            animator.SetBool("isHurt", false);
            respawn = 0;
            useMap = true;
        }
    }
}

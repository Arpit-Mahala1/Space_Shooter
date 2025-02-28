using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent (typeof(CharacterController))]
public class car_movement : MonoBehaviour
{
    [SerializeField] private AudioSource jetSound;
    [SerializeField] private AudioSource HitSound;
    [SerializeField] private Image HealthBar;
    public float forwardSpeed = 0f;
    public bool isSlowed = false;
    public float maxSpeed = 75f;
    private float acceleration = 50f;
    public float brakeForce = 10f;
    public float lateralSpeed = 45f;
    public float reverseSpeed = 30f;
    public float max_health;
    public float Current_health;
    public Camera Main_Camera;
    public GameObject MESH;
    public GameObject Player1;
    //public GameObject GameEndUI;
    private UI_Manager GameendUI;
    private Vector2 InputMovement;
    private bool Break;
    public string PlayerName;
    public bool GameFinished=false;
    private float PreviousHealth;

    public CharacterController carController;

    // Tilt angles
    private float maxTiltAngle = 30f; // Maximum tilt in degrees
    private float tiltSpeed = 30f;     // Speed of tilting

    private float currentTiltZ = 0f;  // Current forward-backward tilt (W/S)
    private float currentTiltX = 0f;  // Current left-right tilt (A/D)
    private GameObject enemy;


  
    void Start()
    {
        //if (Player1.tag == "Player1")
        //{
        //    enemy = GameObject.FindWithTag("Player2");
        //}
        //else if (Player1.tag == "Player2")
        //{
        //    enemy = GameObject.FindWithTag("Player1");
        //}

        //Transform targetTransform = enemy.transform.GetChild(4).GetChild(1);
        //LookAtConstraint lookAtConstraint = targetTransform.GetComponent<LookAtConstraint>();

        //ConstraintSource constraintSource = new ConstraintSource
        //{
        //    sourceTransform = targetTransform,
        //    weight = 1f
        //};

        //lookAtConstraint.AddSource(constraintSource);
        //lookAtConstraint.constraintActive = true;

        //// Add rotation constraint
        //StartCoroutine(LimitRotation(targetTransform));



        //if (Player1.tag == "Player1")
        //{
        //    enemy = GameObject.FindWithTag("Player2");
        //}
        //else if (Player1.tag == "Player2")
        //{
        //    enemy = GameObject.FindWithTag("Player1");
        //}


        //ConstraintSource constraintSource = new ConstraintSource
        //{
        //    sourceTransform = enemy.transform.GetChild(4).transform.GetChild(1).transform,
        //    weight = 1f
        //};
        //enemy.transform.GetChild(4).transform.GetChild(1).GetComponent<LookAtConstraint>().AddSource(constraintSource);
        //enemy.transform.GetChild(4).transform.GetChild(1).GetComponent<LookAtConstraint>().constraintActive = true;

        InvokeRepeating("Accelerate", 0f, 0.1f); // Call Accelerate at intervals
        carController = GetComponent<CharacterController>();
        max_health = 100f;
        GameendUI = FindObjectOfType<UI_Manager>();
        PreviousHealth = max_health;
        Current_health=max_health;
    }

    private void FixedUpdate()
    {
        jetSound.volume = forwardSpeed;
        jetSound.pitch = forwardSpeed/100;
    }
    private void LateUpdate()
    {
        PreviousHealth=Current_health;
        HealthBar.fillAmount= Current_health/max_health;
        
        
        
        
    }
    void disableAudio()
    {
        HitSound.enabled = false;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        InputMovement= context.ReadValue<Vector2>();
    }

    public void OnBreak(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        Break = value > 0f; // Convert float to bool
    }


    void Accelerate()
    {
        // Increase speed gradually up to maxSpeed
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += acceleration * Time.deltaTime;
            forwardSpeed = Mathf.Min(forwardSpeed, maxSpeed); // Clamp speed to maxSpeed
        }
    }

    void Update()
    {

        if (max_health <= 0f)
        {
            if (GameendUI != null)
            {
                GameendUI.OnGameEnd();
            }
            PlayerName = Player1.tag;
            
            GameendUI.OnGameEnd();
            GameFinished = true;
            Destroy(Player1);

            //GameTimer gameTimer = FindObjectOfType<GameTimer>();
            //if (gameTimer != null)
            //{
            //    if (gameObject.name == "Player1") gameTimer.player1 = null;
            //    if (gameObject.name == "Player2") gameTimer.player2 = null;
            //}
        }
        if (Current_health != PreviousHealth)
        {
            HitSound.enabled = true;
            Invoke("disableAudio", 1f);
        }
        Vector3 currentPosition = carController.transform.position;

        // Initialize upwardSpeed to 0 (no vertical movement by default)
        float upwardSpeed = 0f;

        // Handle Y-axis movement
        if (InputMovement.y>0 && currentPosition.y < 1500)
        {
            upwardSpeed = 0.2f * forwardSpeed; // Move upward
            currentTiltZ = Mathf.MoveTowards(currentTiltZ, maxTiltAngle/2.5f, tiltSpeed * (upwardSpeed / 9) * Time.deltaTime); // Tilt forward (Z-axis rotation)
        }
        else if (InputMovement.y < 0 && currentPosition.y > -500)
        {
            upwardSpeed = -0.25f * forwardSpeed; // Move downward
            if (currentPosition.y > -450) { 
            currentTiltZ = Mathf.MoveTowards(currentTiltZ, -maxTiltAngle/2.5f, tiltSpeed * (-upwardSpeed / 9) * Time.deltaTime); // Tilt backward (Z-axis rotation)
            }
        }
        else
        {
            // Gradually return to neutral tilt for forward-backward
            currentTiltZ = Mathf.MoveTowards(currentTiltZ, 0f, (tiltSpeed/1.5f) * Time.deltaTime);
        }

        // Handle X-axis tilt for left-right movement
        if (InputMovement.x < 0)
        {
            currentTiltX = Mathf.MoveTowards(currentTiltX, maxTiltAngle, tiltSpeed * Time.deltaTime); // Tilt left (X-axis rotation)
            transform.Rotate(Vector3.up, -lateralSpeed * Time.deltaTime);
        }
        else if (InputMovement.x > 0)
        {
            currentTiltX = Mathf.MoveTowards(currentTiltX, -maxTiltAngle, tiltSpeed * Time.deltaTime); // Tilt right (X-axis rotation)
            transform.Rotate(Vector3.up, lateralSpeed * Time.deltaTime);
        }
        else
        {
            // Gradually return to neutral tilt for left-right
            currentTiltX = Mathf.MoveTowards(currentTiltX, 0f, (tiltSpeed/3) * Time.deltaTime);
        }

        // Calculate move direction (forward and vertical movement combined)
        Vector3 moveDirection = (transform.right * forwardSpeed + Vector3.up * upwardSpeed) * Time.deltaTime;

        // Apply braking
        if (Break && forwardSpeed > 40)
        {
            forwardSpeed = Mathf.Max(forwardSpeed - brakeForce * Time.deltaTime, 0f);
        }

        // Adjust acceleration based on speed
        if (forwardSpeed < 50)
        {
            acceleration = 150f;
        }
        else
        {
            acceleration = 50f;
        }

        // Smoothly apply tilt rotation
        Quaternion targetRotation = Quaternion.Euler(currentTiltX, transform.rotation.eulerAngles.y, currentTiltZ);
        Quaternion MESHRotation = Quaternion.Euler(-currentTiltZ, 90f, currentTiltX);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
        MESH.transform.localRotation = Quaternion.Lerp(MESH.transform.localRotation, MESHRotation, Time.deltaTime * tiltSpeed);

        // Move the car according to the calculated moveDirection
        carController.Move(moveDirection);
    }
}


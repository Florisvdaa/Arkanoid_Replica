using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform ballStartPos;
    [SerializeField] private float rotationSpeed = 90f; // degrees per second
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;
    [SerializeField] private Vector2 launchDirection = new Vector2(1, 4);
    [SerializeField] private Transform launchArrow;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player extendFeedback;
    [SerializeField] private MMF_Player backToNormalFeedback;
    [SerializeField] private MMF_Player extendEndingFeedback;
    [SerializeField] private MMF_Player enableMagnetFeedback;
    [SerializeField] private MMF_Player disableMagnetFeedback;
    [SerializeField] private float extendDuration = 5f;
    [SerializeField] private float magnetDuration = 5f;

    private float currentAngle = 0f;
    private bool rotatingforward = true;
    private bool launch = false;
    private PlayerInputActions inputActions;
    private BoxCollider2D boxCol2D;
    private int childCount;

    // Powerups
    private bool isExtended = false;
    private bool endingFeedbackPlayed = false;
    private float extendTimer = 0f;
    private float maxExtendTime = 30f;
    private bool magnetEnabled = false;
    private float magnetTimer = 0f;
    private float maxMagnetTime = 30f;

    private Coroutine magnetRoutine;
    private void Awake()
    {
        inputActions = new PlayerInputActions(); // create instance
        inputActions.Player.Lauch.performed += ctx => Launch();

        boxCol2D = GetComponent<BoxCollider2D>();

        childCount = transform.childCount;
    }

    public void SetupBall(GameObject ball)
    {
        GameObject currentBall = Instantiate(ball, ballStartPos.position, Quaternion.identity, transform);
        //PowerUpManager.Instance.SetCurrentBall(currentBall.GetComponent<Ball>());

        //ball.transform.position = ballStartPos.position;
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            EnableMagnet();
        }

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(moveInput.x, 0, 0);
        transform.Translate(move * speed * Time.deltaTime);

        if(transform.childCount > childCount) // Ball attached
        {
            launchArrow.gameObject.SetActive(true);
            // Move angle
            if (rotatingforward)
                currentAngle += rotationSpeed * Time.deltaTime;
            else
                currentAngle -= rotationSpeed * Time.deltaTime;

            // Clamp & reverse direction
            if (currentAngle >= maxAngle)
                rotatingforward = false;
            else if (currentAngle <= minAngle)
                rotatingforward = true;

            // Convert angel to direction vector
            float rad = (currentAngle + 90f) * Mathf.Deg2Rad;
            launchDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            // Rotate arrow visually
            if (launchArrow != null)
                launchArrow.localRotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else
        {
            launchArrow.gameObject.SetActive(false);
        }
    }

    private void Launch()
    {
        // only launch if there are childs attached
        if(transform.childCount > childCount)
        {
            // Collect all attached balls
            List<Ball> ballsToLaunch = new List<Ball>();

            foreach (Transform child in transform)
            {
                Ball ball = child.GetComponent<Ball>();
                if (ball != null)
                    ballsToLaunch.Add(ball);
            }

            // Launch each ball with a slight angle offset
            float angleStep = 10f;
            float startAngle = -(angleStep * (ballsToLaunch.Count - 1) / 2f);

            for (int i = 0; i < ballsToLaunch.Count; i++)
            {
                Ball ball = ballsToLaunch[i];

                // Calculate unique angle
                float angleOffset = startAngle + (i * angleStep);
                Vector2 launchDir = Quaternion.Euler(0, 0, angleOffset) * launchDirection;

                // Detach from paddle
                ball.transform.SetParent(null);

                // Launch
                ball.Launch(launchDir.normalized);
            }

            //Ball ball = GetComponentInChildren<Ball>();
            //ball.Launch(launchDirection);
            ////magnetEnabled = false;
        }
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    // Upgrades
    public void Extend()
    {
        extendTimer = Mathf.Min(extendTimer + extendDuration, maxExtendTime); // Prevent infinate Stacking

        if (!isExtended)
            StartCoroutine(ExtendCO());
    }

    private IEnumerator ExtendCO()
    {
        isExtended = true;
        endingFeedbackPlayed = false;

        extendFeedback.PlayFeedbacks();

        Vector2 startSize = boxCol2D.size;
        Vector2 targetSize = new Vector2(5f, startSize.y);

        float t = 0f;
        float speed = 5f;

        // Smoothly grows collider
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            boxCol2D.size = Vector2.Lerp(startSize, targetSize, t);
            yield return null;
        }
        
        // Stay extended while timer > 0
        while(extendTimer > 0f)
        {
            extendTimer -= Time.deltaTime;

            if (!endingFeedbackPlayed && extendTimer < 1f)
            {
                extendEndingFeedback.PlayFeedbacks();
                endingFeedbackPlayed = true;
            }

            yield return null;
        }    

        // Smoothly shrinks collider
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            boxCol2D.size = Vector2.Lerp(targetSize, startSize, t);
            yield return null;
        }    

        backToNormalFeedback.PlayFeedbacks();
        
        isExtended = false;
        endingFeedbackPlayed = false;
    }
    public void EnableMagnet()
    {
        Debug.Log("Paddle Magnet");

        // Add time to the magnet timer
        magnetTimer = Mathf.Min(magnetTimer + magnetDuration, maxMagnetTime);

        // Start coroutine if not already running
        if (!magnetEnabled)
            magnetRoutine = StartCoroutine(MagnetCO());
    }
    private IEnumerator MagnetCO()
    {
        magnetEnabled = true;

        // Optional: FEEL feedback for activation
        // magnetStartFeedback.PlayFeedbacks();
        enableMagnetFeedback.PlayFeedbacks();

        while (magnetTimer > 0f)
        {
            magnetTimer -= Time.deltaTime;
            yield return null;
        }

        // Optional: FEEL feedback for ending
        // magnetEndFeedback.PlayFeedbacks();
        disableMagnetFeedback.PlayFeedbacks();

        magnetEnabled = false;
        magnetRoutine = null;

        Launch();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(magnetEnabled && collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ball.Catch(transform);
        }
    }
}

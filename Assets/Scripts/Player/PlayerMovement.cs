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

    private float currentAngle = 0f;
    private bool rotatingforward = true;
    private bool launch = false;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions(); // create instance
        inputActions.Player.Lauch.performed += ctx => Launch();
    }

    public void SetupBall(GameObject ball)
    {
        GameObject currentBall = Instantiate(ball, ballStartPos.position, Quaternion.identity, transform);
        PowerUpManager.Instance.SetCurrentBall(currentBall.GetComponent<Ball>());

        //ball.transform.position = ballStartPos.position;
    }

    private void Update()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(moveInput.x, 0, 0);
        transform.Translate(move * speed * Time.deltaTime);

        if(transform.childCount > 3) // Ball attached
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
        if(transform.childCount > 3)
        {
            Ball ball = GetComponentInChildren<Ball>();
            ball.Launch(launchDirection);
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
        Debug.Log("Paddle extend");
    }

    public void EnableMagnet()
    {
        Debug.Log("Paddle Magnet");
    }
}

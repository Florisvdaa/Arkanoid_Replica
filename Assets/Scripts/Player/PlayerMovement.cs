using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform ballStartPos;
    [SerializeField] private Vector2 launchDirection = new Vector2(1, 4);

    private bool launch = false;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions(); // create instance
        inputActions.Player.Lauch.performed += ctx => Launch();
    }

    private void Start()
    {
        //// Setup ball on start position
        //Ball ball = GetComponentInChildren<Ball>();
        //ball.transform.position = ballStartPos.position;
        
    }

    public void SetupBall(GameObject ball)
    {
        Instantiate(ball, ballStartPos.position, Quaternion.identity, transform);
        //ball.transform.position = ballStartPos.position;
    }

    private void Update()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(moveInput.x, 0, 0);
        transform.Translate(move * speed * Time.deltaTime);
    }

    private void Launch()
    {
        if(transform.childCount > 1)
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
}

using System.Collections;
using UnityEngine;

public class CharacterMovements : MonoBehaviour
{
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public InputActions InputActions { get; private set; }

    [Header("Movement Setup")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    float _currSpeed;
    bool _canMove;
    int _walk;
    int _run;
    int _animSpeed;

    [Header("Punch Setup")]
    [SerializeField] float impulseSpeed;
    [SerializeField] float impulseDistance;
    [SerializeField] float impulseDelay;
    int _punch;

    public bool CanMove
    {
        get => _canMove;
        set => _canMove = value;
    }

    private void OnDestroy()
    {
        InputActions.Gameplay.Run.performed -= ctx => _currSpeed = runSpeed;
        InputActions.Gameplay.Run.canceled -= ctx => _currSpeed = walkSpeed;
        InputActions.Gameplay.Punch.performed -= ctx => StartCoroutine(PunchCouroutine());
        InputActions.Disable();
    }

    protected void Awake()
    {
        if (!Controller)
            Controller = GetComponent<CharacterController>();

        if (!Animator)
            Animator = GetComponentInChildren<Animator>();

        InputActions = new InputActions();
        InputActions.Enable();

        _walk = Animator.StringToHash("Walk");
        _run = Animator.StringToHash("Run");
        _punch = Animator.StringToHash("Punch");
        _animSpeed = Animator.StringToHash("AnimSpeed");
    }

    private void Start()
    {
        // Trigger the run
        InputActions.Gameplay.Run.performed += ctx => _currSpeed = runSpeed;
        InputActions.Gameplay.Run.canceled += ctx => _currSpeed = walkSpeed;
        InputActions.Gameplay.Punch.performed += ctx => StartCoroutine(PunchCouroutine());

        //Debug
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove)
            return;

        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Set walk speed as default speed
        if (_currSpeed != runSpeed)
            _currSpeed = walkSpeed;

        // Read input values
        Vector2 input = InputActions.Gameplay.Movement.ReadValue<Vector2>();

        // Rotate
        var rotateVector = input.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotateVector, 0);

        // Movement
        var moveVector = _currSpeed * input.y * Time.deltaTime * transform.forward;
        Controller.Move(moveVector);

        // Set animator state
        Animator.SetFloat(_animSpeed, input.y);
        Animator.SetBool(_walk, input != Vector2.zero);
        Animator.SetBool(_run, input != Vector2.zero && _currSpeed == runSpeed);
    }

    IEnumerator PunchCouroutine()
    {
        var target = transform.position + (impulseDistance * transform.forward);
        var movement = impulseSpeed * Time.deltaTime * transform.forward;

        Animator.SetTrigger(_punch);

        yield return new WaitForSeconds(impulseDelay);

        while (transform.position.z < target.z)
        {
            Controller.Move(movement);
            yield return new WaitForEndOfFrame();
        }
    }
}

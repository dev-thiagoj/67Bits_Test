using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] PlayerStats _stats;
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    public InputActions InputActions { get; private set; }

    [Header("Movement Setup")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    float _currSpeed;

    int _walk;
    int _run;
    int _animSpeed;

    public float MovementX { get; private set; }

    private void OnDestroy()
    {
        InputActions.Gameplay.Run.performed -= ctx => _currSpeed = runSpeed;
        InputActions.Gameplay.Run.canceled -= ctx => _currSpeed = walkSpeed;
        InputActions.Disable();
    }

    protected void Awake()
    {
        if(!_stats)
            _stats = GetComponent<PlayerStats>();

        if (!Controller)
            Controller = GetComponent<CharacterController>();

        if (!Animator)
            Animator = GetComponentInChildren<Animator>();

        InputActions = new InputActions();
        InputActions.Enable();

        _walk = Animator.StringToHash("Walk");
        _run = Animator.StringToHash("Run");
        _animSpeed = Animator.StringToHash("AnimSpeed");
    }

    private void Start()
    {
        // Trigger the run
        InputActions.Gameplay.Run.performed += ctx => _currSpeed = runSpeed;
        InputActions.Gameplay.Run.canceled += ctx => _currSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stats.CanMove)
        {
            Animator.Rebind();
            return;
        }

        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Set walk speed as default speed
        if (_currSpeed != runSpeed)
            _currSpeed = walkSpeed;

        // Read input values
        Vector2 input = InputActions.Gameplay.Movement.ReadValue<Vector2>();
        MovementX = input.x;

        // Rotate
        var rotateVector = input.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotateVector, 0);

        // Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        var movement = input.y * _currSpeed;
        Controller.SimpleMove(movement * forward);

        //TODO carrybodies que se mexeu

        // Set animator state
        Animator.SetFloat(_animSpeed, input.y);
        Animator.SetBool(_walk, input != Vector2.zero);
        Animator.SetBool(_run, input != Vector2.zero && _currSpeed == runSpeed);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField]
    private float _movementForce = 1f;
    [SerializeField]
    private float _jumpForce = 5f;
    [SerializeField]
    private float _maxSpeed = 5f;
    private Vector3 _forceDirection = Vector3.zero;
    private float _gravity = 1f;

    [Header("Camera")]
    [SerializeField]
    private Camera _playerCamera;
    private Animator _animator;

    // [Header("Animation")]
    //private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        //_animator = GetComponent<Animator>();
        //_playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _inputManager = InputManager.Instance;

        _inputManager.inputControl.Player.Jump.started += ctx => DoJump();
        _inputManager.inputControl.Player.Jump.performed += ctx => DoJump();
        _inputManager.inputControl.Player.Jump.canceled += ctx => DoJump();
    }

    private void Update()
    {
        Movement();
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void LookAt()
    {
        Vector3 direction = _rb.velocity;
        direction.y = 0;

        if (_inputManager.inputControl.Player.Move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void Movement()
    {
        _forceDirection += _inputManager.inputControl.Player.Move.ReadValue<Vector2>().x * _movementForce * GetCameraRight(_playerCamera);
        _forceDirection += _inputManager.inputControl.Player.Move.ReadValue<Vector2>().y * _movementForce * GetCameraForward(_playerCamera);

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        if (_rb.velocity.y < 0f)
        {
            _rb.velocity -= Physics.gravity.y * Time.fixedDeltaTime * Vector3.down;
        }

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * _maxSpeed + Vector3.up * _rb.velocity.y;
        }

        LookAt();
    }

    private void DoJump()
    {
        if (IsGrounded())
        {
            //_animator.SetTrigger("isJump");
            AddJumpForce();
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddJumpForce()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}

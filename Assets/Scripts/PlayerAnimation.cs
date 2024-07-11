using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private float _maxSpeed = 5f;
    private bool _isAttacking = false;
    private Animator _animator;
    private Rigidbody _rb;

    private InputManager _inputManager;

    void Start()
    {
        _inputManager = InputManager.Instance;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _inputManager.inputControl.Player.Attack.performed += ctx => SimpleAttack();
    }

    private void SimpleAttack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
        }
    }

    public void DisableAttack()
    {
        _isAttacking = false;
    }

    void Update()
    {
        if (_inputManager.inputControl.Player.Move.ReadValue<Vector2>() != Vector2.zero)
            _animator.SetFloat("speed", _rb.velocity.magnitude / _maxSpeed);
        else
            _animator.SetFloat("speed", 0f);
    }

}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _joystickDotOffset;
    [SerializeField] private float _afkStartTimeDelay;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _maxJumpVelocity;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Transform _joystickDot;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;

    private Rigidbody _rigidbody;

    private Animator _animator;
    private int _runHash = Animator.StringToHash("Run");
    private int _jumpHash = Animator.StringToHash("Jump");
    private int _isGroundedHash = Animator.StringToHash("isGrounded");
    private int _idleIndexHash = Animator.StringToHash("IdleIndex");
    private int _jumpIndexHash = Animator.StringToHash("JumpIndex");

    private float _inputX;
    private float _inputY;

    private float _currentAfkTime;

    [SerializeField] private bool _isGrounded = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(AfkRoutine());
    }

    private void Update()
    {
        _inputX = _joystick.Horizontal + Input.GetAxis("Horizontal");
        _inputY = _joystick.Vertical + Input.GetAxis("Vertical");

        _joystickDot.position = new Vector3(_inputX * _joystickDotOffset + transform.position.x, _joystickDot.position.y, _inputY * _joystickDotOffset + transform.position.z);

        var canRun = Mathf.Abs(_inputX) > 0.1f || Mathf.Abs(_inputY) > 0.1f;

        if (canRun)
        {
            transform.LookAt(_joystickDot);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            ResetCurrentAfkTime();

            /*            if (_animator.GetBool(_runHash) == false && _isGrounded)
                        {
                            _animator.SetBool(_runHash, true);
                        }*/
        }
        /*        else
                {
                    if (_animator.GetBool(_runHash))
                    {
                        _animator.SetBool(_runHash, false);
                    }
                }*/

        if (Input.GetButtonDown("Jump"))
            Jump();

        _animator.SetBool(_isGroundedHash, _isGrounded);
        _animator.SetBool(_runHash, _isGrounded && canRun);
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_inputX) > 0.1f || Mathf.Abs(_inputY) > 0.1f)
        {
            var movedirection = new Vector3(_inputX, 0, _inputY).normalized;

            _rigidbody.AddForce(movedirection * _moveSpeed, ForceMode.Impulse);
        }

        GroundCheck();
        SetGravity();
    }

    private IEnumerator AfkRoutine()
    {
        ResetCurrentAfkTime();
        int animationsCount = 11;

        while (true)
        {
            _currentAfkTime -= Time.deltaTime;

            if (_currentAfkTime < 0)
            {
                _animator.SetInteger(_idleIndexHash, Random.Range(1, animationsCount + 1));

                yield return new WaitForSeconds(1f);

                _animator.SetInteger(_idleIndexHash, 0);

                ResetCurrentAfkTime();
            }

            yield return null;
        }

    }

    private void ResetCurrentAfkTime()
    {
        _currentAfkTime = _afkStartTimeDelay;
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(_groundCheck.position, -Vector3.up, _groundCheckDistance, _groundMask);

    }

    private void Jump()
    {
        var jumpsAnimationsCount = 2;

        if (_isGrounded)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _animator.SetTrigger(_jumpHash);
            _animator.SetInteger(_jumpIndexHash, Random.Range(0, jumpsAnimationsCount + 1));
            ResetCurrentAfkTime();
        }
    }

    private void SetGravity()
    {
        if (_isGrounded == false)
        {
            if (_rigidbody.velocity.y < 0 || _rigidbody.velocity.y > _maxJumpVelocity)
            {
                _rigidbody.velocity += Vector3.up * Physics.gravity.y * (_gravityMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _joystickDotOffset;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Transform _joystickDot;
    [SerializeField] private Transform _ultimateIndicator;

    private Animator _animator;
    private int _runHash = Animator.StringToHash("Run");

    private float _inputX;
    private float _inputY;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _inputX = _joystick.Horizontal + Input.GetAxis("Horizontal");
        _inputY = _joystick.Vertical + Input.GetAxis("Vertical");

        _joystickDot.position = new Vector3(_inputX * _joystickDotOffset + transform.position.x, _joystickDot.position.y, _inputY * _joystickDotOffset + transform.position.z);
        _ultimateIndicator.position = new Vector3(transform.position.x, _ultimateIndicator.position.y, transform.position.z);

        if (Mathf.Abs(_inputX) > 0.1f || Mathf.Abs(_inputY) > 0.1f)
        {
            transform.LookAt(_joystickDot);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.forward);
            _animator.SetBool(_runHash, true);
        }
        else
        {
            if (_animator.GetBool(_runHash))
            {
                _animator.SetBool(_runHash, false);
            }
        }
    }
}
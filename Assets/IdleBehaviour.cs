using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _timeUntilBored;
    [SerializeField] private float _numberOfBoredAnimations;

    private bool _isBored;
    private float _idleTime;
    private float _idleAnimation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime = Time.deltaTime;

            if (_idleTime > _timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isBored = true;
                _idleAnimation = Random.Range(1, _numberOfBoredAnimations + 1);
                _idleAnimation = _idleAnimation * 2 - 1;

                animator.SetFloat("Idle", _idleAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("Idle", _idleAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (_isBored)
        {
            _idleAnimation--;
        }

        _isBored = false;
        _idleTime = 0;
    }
}

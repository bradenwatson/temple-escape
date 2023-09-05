using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AnimateHandControl : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference gripInputActionReference;
    public InputActionReference triggerInputActionReference;

    [Header("Hand Animation")]
    private Animator _handAnimator;
    private string _gripName = "Grip";
    private float _gripValue;
    private string _triggerName = "Trigger";
    private float _triggerValue;

    private void Start()
    {
        _handAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }

    private void AnimateGrip()
    {
        _gripValue = gripInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat(_gripName, _gripValue);
    }

    private void AnimateTrigger()
    {
        _triggerValue = triggerInputActionReference.action.ReadValue<float>();
        _handAnimator.SetFloat(_triggerName, _triggerValue);
    }
}

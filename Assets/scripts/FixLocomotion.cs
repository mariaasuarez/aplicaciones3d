using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FixLocomotion : MonoBehaviour
{
    private CharacterController _cc;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _cc.enabled = true;
    }

    void Start()
    {
        _cc.enabled = true;
    }

    void OnEnable()
    {
        _cc.enabled = true;
    }

    void Update()
    {
        if (!_cc.enabled)
            _cc.enabled = true;
    }
}
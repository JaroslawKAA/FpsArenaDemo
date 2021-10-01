using System.Collections.Generic;
using __Scripts;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private NavMeshAgent _agent;
    private Character _character;
    
    // Animator variables ids
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int AttackParam = Animator.StringToHash("Attack");

    private Dictionary<string, float> animationsLenghts;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<Character>();

        _character.OnDeath += () => { _animator.SetTrigger(Death); };
        
        Assert.IsNotNull(_animator);

        animationsLenghts = _animator.GetClipsLengths();
    }

    void Update()
    {
        float moveAnimatorParam = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat(Move, moveAnimatorParam);
    }
    
    public void PlayAttackAnimation(Utils.Callback callbackCoroutine)
    {
        if (!_animator.IsAnimatorState("Attack") 
            && !_animator.GetBool(AttackParam))
        {
            _animator.SetTrigger(AttackParam);
            StartCoroutine(callbackCoroutine(animationsLenghts["Z_Attack"] * 0.7f));
        }
    }
}

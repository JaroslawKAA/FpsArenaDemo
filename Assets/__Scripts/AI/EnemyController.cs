using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    
    private NavMeshAgent _agent;
    private Character _character;
    private EnemyAnimationController _animationController;
    private Character _player;
    private EnemyState _state = EnemyState.Idle;

    private float DistanceToPlayer => Vector3.Distance(transform.position, _player.transform.position);

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<Character>();
        _animationController = GetComponent<EnemyAnimationController>();

        _character.OnDeath += () =>
        {
            // Disable collision
            GetComponent<Collider>().enabled = false;
            
            // Stop moving
            _agent.speed = 0f;
            _agent.enabled = false;

            _state = EnemyState.Death;
        };
    }

    void Start()
    {
        _player = GameManager.S.Player.GetComponent<Character>();
        _agent.SetDestination(_player.transform.position);
    }

    void Update()
    {
        if (!_character.Alive)
            return;

        switch (_state)
        {
            case EnemyState.Idle:
                _state = DistanceToPlayer > _agent.stoppingDistance ? EnemyState.Chase : EnemyState.Attack;
                break;
            
            case EnemyState.Chase:
                _agent.SetDestination(_player.transform.position);
                
                if (DistanceToPlayer < _agent.stoppingDistance)
                    _state = EnemyState.Attack;
                break;
            
            case EnemyState.Attack:
                LookAtPlayer();
                
                // As callback run method Attack
                _animationController.PlayAttackAnimation(Attack);

                if (DistanceToPlayer > _agent.stoppingDistance)
                    _state = EnemyState.Chase;
                break;
            
            case EnemyState.Death:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Apply damage to player
    /// </summary>
    /// <param name="delay">Determined by animator, in order to apply damage in half of animation time.</param>
    /// <returns></returns>
    private IEnumerator Attack(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (DistanceToPlayer < _agent.stoppingDistance
        && _character.Alive)
        {
            _player.ApplyDamage(_character.Damage);
            _player.PlayDamageEffect();
            _player.PlayDamageSound();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 playerPosition = _player.transform.position;
        playerPosition.y = 0;
        Vector3 position = transform.position;
        position.y = 0;

        Vector3 lookDirection = playerPosition - position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    private bool IsLookingOnPlayer()
    {
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        float angleToTargetAngle = Vector3.Angle(transform.forward, directionToPlayer);
        
        return Mathf.Abs(angleToTargetAngle) < 5;
    }
}
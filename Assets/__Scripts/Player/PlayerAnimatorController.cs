using UnityEngine;
using UnityEngine.Assertions;

public class PlayerAnimatorController : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    private PlayerController _playerController;
    private static readonly int MovementX = Animator.StringToHash("MovementX");
    private static readonly int MovementZ = Animator.StringToHash("MovementZ");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Reload = Animator.StringToHash("Reload");

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(_animator);
        Assert.IsNotNull(_playerController);

        _playerController.OnShoot += () => { _animator.SetTrigger(Shoot); };
        _playerController.OnReload += () => { _animator.SetTrigger(Reload); };
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat(MovementX, 
            _playerController.CharacterVelocity.x / _playerController.RunSpeed);
        _animator.SetFloat(MovementZ, 
            _playerController.CharacterVelocity.z / _playerController.RunSpeed);
    }
}

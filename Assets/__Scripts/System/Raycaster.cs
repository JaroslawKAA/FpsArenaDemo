using UnityEngine;

public class Raycaster : Singleton<Raycaster>
{
    [SerializeField] private LayerMask _mask;
    
    private RaycastHit _aimerHit;

    public RaycastHit AimerHit => _aimerHit;

    // Update is called once per frame
    void Update()
    {
        Ray ray = GameManager.S.Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);
        
        if (!Physics.Raycast(ray, out _aimerHit, 500, _mask))
        {
            // If raycast didn't hit, return hit with position on center of screen
            _aimerHit = new RaycastHit() { point = ray.origin + ray.direction * 20};
        }
    }
}

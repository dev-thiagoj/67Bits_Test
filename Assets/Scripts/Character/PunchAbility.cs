using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterMovements))]
public class PunchAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterMovements _movements;

    [Header("Punch Setup")]
    [SerializeField] float punchForce;
    [SerializeField] float punchCooldown;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float effectDelay;
    float _punchTimer;
    int _punchHash;

    [Header("Punch Effect")]
    [SerializeField] float animScale;
    [SerializeField] float AnimDuration;

    readonly RaycastHit[] _results = new RaycastHit[5];

    private void OnDestroy()
    {
        _movements.InputActions.Gameplay.Punch.performed -= ctx
            => StartCoroutine(PunchCouroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_movements)
            _movements = GetComponent<CharacterMovements>();

        _movements.InputActions.Gameplay.Punch.performed += ctx
            => StartCoroutine(PunchCouroutine());

        _punchHash = Animator.StringToHash("Punch");
    }

    // Update is called once per frame
    void Update()
    {
        //Punch Cooldown timer
        if (_punchTimer > 0)
            _punchTimer -= Time.deltaTime;

#if UNITY_EDITOR
        //Debug
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1,
            transform.position.z), transform.forward * rayDistance, Color.magenta);
#endif
    }

    IEnumerator PunchCouroutine()
    {
        if (_punchTimer > 0)
            yield break;

        _punchTimer = punchCooldown;

        PuppyBehaviour puppy = GetPuppy();
       
        _movements.Animator.SetTrigger(_punchHash);
        yield return new WaitForSeconds(effectDelay);

        if (!puppy)
            yield break;

        PunchEffect(puppy);
    }

    PuppyBehaviour GetPuppy()
    {
        int hits = Physics.RaycastNonAlloc(transform.position, transform.forward, _results, rayDistance, enemyLayer);

        for (int i = 0; i < hits; i++)
        {
            if (!_results[i].collider || !_results[i].transform.CompareTag("Puppies"))
                continue;

            if (!_results[i].transform.TryGetComponent(out PuppyBehaviour puppies))
                continue;

            return puppies;
        }

        return null;
    }

    void PunchEffect(PuppyBehaviour puppy)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = new Collider[10];
            
        int numColliders = Physics.OverlapSphereNonAlloc(explosionPos, 5, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];

            if (!collider.transform.CompareTag("Puppies"))
                continue;

            if(!collider.TryGetComponent(out Rigidbody rb))
                continue;

            rb.mass = 1;
            rb.AddExplosionForce(punchForce, explosionPos, 3, 3);

            puppy.EnableRagdoll();
            break;
        }
    }
}

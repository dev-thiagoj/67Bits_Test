using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PlayerMovements))]
public class PunchAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMovements _movements;

    [Header("Punch Setup")]
    [SerializeField] float punchForce;
    [SerializeField] float punchCooldown;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float effectDelay;
    [SerializeField] float upwardModifier;
    float _punchTimer;
    int _punchHash;

    [Header("Punch Effect")]
    [SerializeField] GameObject[] arms;
    [SerializeField] float animScale;
    [SerializeField] float animStepTime;
    [SerializeField] float waitingTime;
    [SerializeField] float startDelay;

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
            _movements = GetComponent<PlayerMovements>();

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

        _movements.Animator.SetTrigger(_punchHash);
        PuppyBehaviour puppy = GetPuppy();

        StartCoroutine(PunchEffect());

        yield return new WaitForSeconds(effectDelay);

        if (!puppy)
            yield break;
        
        StartCoroutine(PunchDamage(puppy));
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

    IEnumerator PunchDamage(PuppyBehaviour puppy)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = new Collider[20];

        int numColliders = Physics.OverlapSphereNonAlloc(explosionPos, 5, colliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliders[i];

            if (!collider.transform.CompareTag("Puppies"))
                continue;

            if (!collider.TryGetComponent(out Rigidbody rb))
                continue;

            rb.mass = 1;
            rb.AddExplosionForce(punchForce, explosionPos, 2, upwardModifier);

            yield return new WaitForSeconds(.05f);
            puppy.EnableRagdoll();
            break;
        }
    }

    IEnumerator PunchEffect()
    {
        Vector3 step = new(0.1f, 0.1f, 0.1f);

        yield return new WaitForSeconds(startDelay);

        while (arms[0].transform.localScale.x <= animScale)
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale += step;
            }
            yield return new WaitForSeconds(animStepTime);
        }

        yield return new WaitForSeconds(waitingTime);

        while (arms[0].transform.localScale.x > 1)
        {
            foreach (GameObject arm in arms)
            {
                arm.transform.localScale -= step;
            }
            yield return new WaitForSeconds(animStepTime);
        }
    }
}

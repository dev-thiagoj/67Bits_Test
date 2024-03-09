using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PuppyBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject triggerPFB;
    public Collider mainCollider;
    public Rigidbody mainRB;
    public Rigidbody[] rbs;
    public Collider[] colls;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colls = GetComponentsInChildren<Collider>();

        mainCollider = colls[0];
        mainRB = rbs[0];
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;

        foreach (Rigidbody bone in rbs)
        {
            bone.isKinematic = false;
            bone.detectCollisions = true;
        }

        foreach (Collider coll in colls)
        {
            coll.enabled = true;
        }

        var child = transform.GetChild(1);
        var obj = Instantiate(triggerPFB, child);

        if (obj.TryGetComponent(out MeshRenderer renderer))
            renderer.enabled = false;

        if (obj.TryGetComponent(out BoxCollider collider))
            collider.isTrigger = true;
    }
}

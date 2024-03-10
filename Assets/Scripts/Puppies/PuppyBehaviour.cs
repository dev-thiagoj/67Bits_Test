using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PuppyBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject triggerPFB;
    [SerializeField] CarryBodies bag;

    Rigidbody mainRB;
    Rigidbody[] rbs;
    Collider[] colls;

    [Space]
    public UnityEvent OnBodyTaked;

    private void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!bag)
            bag = GameObject.FindFirstObjectByType<CarryBodies>();
    }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colls = GetComponentsInChildren<Collider>();

        mainRB = rbs[0];
    }

    [ContextMenu("Enable Ragdoll")]
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

        mainRB.detectCollisions = false;

        var child = transform.GetChild(1);
        var obj = Instantiate(triggerPFB, child);

        if (obj.TryGetComponent(out MeshRenderer renderer))
            renderer.enabled = false;

        if (obj.TryGetComponent(out BoxCollider collider))
            collider.isTrigger = true;
    }

    public void Take()
    {
        bag.Add();
        OnBodyTaked?.Invoke();
        Destroy(gameObject);
    }
}

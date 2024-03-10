using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarryBodies : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterMovements cMovement;
    [SerializeField] BodySensor bodiesSensor;
    [SerializeField] BodyBag bodyPFB;
    [Tooltip("Height offset between bodies")]
    [SerializeField] float heightOffset;

    [Header("Inertia Movement Setup")]
    [SerializeField] float maxAngle;
    [SerializeField] float speed;
    [SerializeField] float delay;

    readonly List<BodyBag> _bodyBags = new();

    public UnityEvent<float, int> OnMove;

    Coroutine currCoroutine;

#if UNITY_EDITOR
    private void Reset()
    {
        cMovement = GetComponent<CharacterMovements>();
        bodiesSensor = GetComponentInChildren<BodySensor>();

        //default value
        heightOffset = 0.3f;
    }
#endif

    //private void FixedUpdate()
    //{
    //    if (_bodyBags.Count <= 0)
    //        return;

    //    float direction = cMovement.MovementX;
    //    float angle = direction * maxAngle;

    //    for (int i = 0; i < _bodyBags.Count; i++)
    //    {
    //        BodyBag body = _bodyBags[i];
    //        body.Rotate(angle, speed);
    //        //yield return new WaitForSeconds(delay);
    //    }
    //}

    [ContextMenu("Add Body")]
    public void Add()
    {
        BodyBag body = Instantiate(bodyPFB);
        _bodyBags.Add(body);
        int index = _bodyBags.IndexOf(body);

        Transform parent = _bodyBags.Count > 1 ?
            _bodyBags[^2].transform : bodiesSensor.transform;

        body.transform.SetParent(parent);

        body.transform.SetLocalPositionAndRotation(
            Vector3.zero,
            bodyPFB.transform.rotation);

        Vector3 position = body.transform.localPosition;

        position.y = heightOffset;
        body.transform.localPosition = position;

        SetInertiaMovements();
    }

    [ContextMenu("Debug Bodies")]
    void DebugBodies()
    {
        int amount = 10;

        for (int i = 0; i < amount; i++)
        {
            BodyBag body = Instantiate(bodyPFB);
            _bodyBags.Add(body);
            int index = _bodyBags.IndexOf(body);

            Transform parent = _bodyBags.Count > 1 ?
                _bodyBags[^2].transform : bodiesSensor.transform;

            body.transform.SetParent(parent);

            body.transform.SetLocalPositionAndRotation(
                Vector3.zero,
                bodyPFB.transform.rotation);

            Vector3 position = body.transform.localPosition;

            position.y = heightOffset;
            body.transform.localPosition = position;

            SetInertiaMovements();
        }
    }

    public void Remove()
    {
        for (int i = 0; i < _bodyBags.Count; i++)
        {
            Destroy(_bodyBags[i].gameObject);
            _bodyBags.Remove(_bodyBags[i]);
        }

        _bodyBags.Clear();
    }

    void SetInertiaMovements()
    {
        if (currCoroutine != null)
        {
            StopCoroutine(currCoroutine);
            currCoroutine = null;
        }

        Coroutine coroutine = StartCoroutine(InertiaMovement());
        currCoroutine = coroutine;
    }

    IEnumerator InertiaMovement()
    {
        while (true)
        {
            float direction = cMovement.MovementX;
            float angle = direction * maxAngle;

            for (int i = 0; i < _bodyBags.Count; i++)
            {
                yield return new WaitForSeconds(delay);
                BodyBag body = _bodyBags[i];
                body.Rotate(angle, speed);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarryBodies : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LevelController levelController;
    [SerializeField] BodySensor bodiesSensor;
    [SerializeField] BodyBag bodyPFB;

    [Header("Inertia Movement Setup")]
    [SerializeField] float maxAngle;
    [SerializeField] float speed;
    [SerializeField] float delay;
    bool canMove;
    Coroutine currCoroutine;

    readonly List<BodyBag> _bodyBags = new();

    [Space]
    public UnityEvent<int> OnMove;

    public List<BodyBag> Bodies => _bodyBags;

    private void Awake()
    {
        levelController = GetComponent<LevelController>();
        bodiesSensor = GetComponentInChildren<BodySensor>();
    }

    private void Update()
    {
        if (!canMove && currCoroutine != null)
            StopCoroutine(currCoroutine);
    }

    public void CanMove(bool value) => canMove = value;


    [ContextMenu("Add Body")]
    public bool Add()
    {
        if (!levelController.CanAdd(_bodyBags.Count))
            return false;

        BodyBag body = Instantiate(bodyPFB);
        _bodyBags.Add(body);

        if (body == _bodyBags[0])
        {
            body.transform.SetLocalPositionAndRotation(
            bodiesSensor.transform.position,
            Quaternion.identity);

            StartCoroutine(StartMove());

            return true;
        }

        _bodyBags[^2].Next(body);

        return true;
    }

    public void Remove(int index)
    {
        Destroy(_bodyBags[index].gameObject);
        _bodyBags.RemoveAt(index);
    }

    #region Debug
#if UNITY_EDITOR
    [ContextMenu("Debug Bodies")]
    void DebugBodies()
    {
        int amount = 20;

        for (int i = 0; i < amount; i++)
        {
            Add();
        }
    }
#endif
    #endregion

    IEnumerator StartMove()
    {
        while (true)
        {
            if(_bodyBags.Count <= 0)
            {
                currCoroutine = null;
                yield break;
            }

            _bodyBags[0].Move(bodiesSensor.transform);
            yield return null;
        }
    }
}

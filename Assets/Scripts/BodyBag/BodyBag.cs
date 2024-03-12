using System.Collections;
using UnityEngine;

public class BodyBag : MonoBehaviour
{
    [SerializeField] BodyBag next;
    Transform target;
    [Tooltip("Height offset between bodies")]
    [SerializeField] float heightOffset;
    [SerializeField] float animTime;
    bool isMoving = false;

    public void Next(BodyBag next)
    {
        this.next = next;

        var position = transform.position;
        position.y += heightOffset;

        next.transform.position = position;
    }

    private void Update()
    {
        if (target)
            transform.rotation = target.rotation;
    }

    public void Move(Transform target)
    {
        if (isMoving)
            return;

        this.target = target;

        var position = target.position;
        position.y = transform.position.y;

        StartCoroutine(Moving(position, transform.position, animTime, target));
    }

    IEnumerator Moving(Vector3 position, Vector3 initial, float time, Transform target)
    {
        isMoving = true;
        float current = 0;

        while (current < time)
        {
            transform.position = Vector3.Lerp(initial, position, current / time);

            yield return null;
            current += Time.deltaTime;

            if (!next)
                continue;


            next.Move(transform);
        }

        isMoving = false;
    }
}

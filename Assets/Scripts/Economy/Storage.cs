using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
    [SerializeField] int bodyValue;

    [Space]
    public UnityEvent OnSelling;
    public UnityEvent<int> OnBodySold;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.TryGetComponent(out CarryBodies bag))
                return;

            StartCoroutine(BuyBodies(bag));
        }
    }

    IEnumerator BuyBodies(CarryBodies bag)
    {
        OnSelling?.Invoke();

        List<BodyBag> bodies = bag.Bodies;

        if (bodies.Count <= 0)
            yield break;

        for (int i = bodies.Count - 1; i >= 0; i--)
        {
            bag.Remove(i);
            yield return new WaitForSeconds(.1f);
            Pay();
        }
    }

    public void Pay()
    {
        OnBodySold?.Invoke(bodyValue);
    }
}

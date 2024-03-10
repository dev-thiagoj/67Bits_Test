using UnityEngine;

public class BodySensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Body"))
            return;

        PuppyBehaviour puppy = other.GetComponentInParent<PuppyBehaviour>();

        if (!puppy)
            return;

        puppy.Take();
    }
}

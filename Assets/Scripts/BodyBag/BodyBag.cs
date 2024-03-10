using UnityEngine;

public class BodyBag : MonoBehaviour
{
    Vector3 currRotation;

    public void Rotate(float amount, float speed)
    {
        Vector3 target = Vector3.zero;
        target.z = amount;

        currRotation = target;

        transform.localRotation = Quaternion.Euler(Vector3.Lerp(
            currRotation,
            target,
            speed * Time.deltaTime));
    }
}

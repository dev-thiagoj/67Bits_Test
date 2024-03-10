using UnityEngine;

public class PuppyTrigger : MonoBehaviour
{
    public PuppyBehaviour Get()
    {
        PuppyBehaviour behaviour = GetComponentInParent<PuppyBehaviour>();
        return behaviour;
    }
}

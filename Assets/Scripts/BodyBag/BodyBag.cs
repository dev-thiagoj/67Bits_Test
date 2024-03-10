using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BodyBag : MonoBehaviour
{
    [SerializeField] bool usingCharMovement;
    [HideInInspector] public UnityEvent<float> moveEvent;

    //Animator _animator;
    float _direction;
    public int bagIndex;

    public bool BagFilled { get; private set; }

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (bagIndex == 0)
        {
            usingCharMovement = true;
        }
    }

    private void Update()
    {
        //_animator.SetFloat("Blend", _direction);

        if (usingCharMovement)
        {
            GetMovement();
        }
    }

    void GetMovement()
    {
        //float directionX = CharacterMovements.Instance.movement.x;
        //float dir = Mathf.Lerp(_direction, directionX, Time.deltaTime);
        //Move(dir);
    }

    public void Move(float f)
    {
        _direction = f;
        StartCoroutine(MoveDelay(f));
    }

    IEnumerator MoveDelay(float direction)
    {
        yield return new WaitForSeconds(.4f);
        moveEvent.Invoke(direction * -1);
    }

    public void ChangeStatus()
    {
        BagFilled = !BagFilled;
    }
}

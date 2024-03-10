using System.Collections.Generic;
using UnityEngine;

public class CarryBodies : MonoBehaviour
{
    [SerializeField] BodySensor bodiesSensor;
    [SerializeField] BodyBag bodyPFB;
    [Tooltip("Height offset between bodies")]
    [SerializeField] float heightOffset;
    
    List<BodyBag> _bodyBags = new();

#if UNITY_EDITOR
    private void Reset()
    {
        bodiesSensor = GetComponentInChildren<BodySensor>();

        //default value
        heightOffset = 0.3f;
    }
#endif

    [ContextMenu("Add Body")]
    public void Add()
    {
        BodyBag body = Instantiate(bodyPFB);
        body.transform.SetParent(bodiesSensor.transform);
        body.transform.SetLocalPositionAndRotation(Vector3.zero, bodyPFB.transform.rotation);

        _bodyBags.Add(body);

        int index = _bodyBags.IndexOf(body);
        Vector3 position = body.transform.localPosition;
        
        position.y = index * heightOffset;
        body.transform.localPosition = position;
    }

    public void Delete()
    {
        for (int i = 0; i < _bodyBags.Count; i++)
        {
            Destroy(_bodyBags[i].gameObject);
            _bodyBags.Remove(_bodyBags[i]);
        }

        _bodyBags.Clear();
    }

    //public void StorageBodies(BodyStorage storage)
    //{
    //    StartCoroutine(StorageBodiesCoroutine(storage));
    //}

    //IEnumerator StorageBodiesCoroutine(BodyStorage storage)
    //{
    //    int lastIndex = _bodyBags.Count - 1;

    //    for (int i = lastIndex; i >= 0; i--)
    //    {
    //        if (!_bodyBags[i].BagFilled) 
    //            continue;

    //        // fazer ir até a storage
    //        PuppiesColliders body = _bodyBags[i].GetComponentInChildren<PuppiesColliders>();
    //        body.transform.DOMove(storage.transform.position, .5f);
    //        body.transform.SetParent(null);

    //        storage.Pay();

    //        _bodyBags[i].ChangeStatus();
    //        yield return new WaitForSeconds(.5f);
    //    }

    //    currEmptyBag = 0;
    //}
}

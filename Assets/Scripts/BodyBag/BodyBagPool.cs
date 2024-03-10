using UnityEngine;

public class BodyBagPool : MonoBehaviour
{
    [SerializeField] CarryBodies carryBodies;
    //[SerializeField] CharacterLevelManager levelManager;
    [SerializeField] int bagPoolSize;

    // Start is called before the first frame update
    void Awake()
    {
        carryBodies= GetComponent<CarryBodies>();
        //levelManager= GetComponent<CharacterLevelManager>();
    }

    private void Start()
    {
        //levelManager.levelUppedEvent.AddListener(CreateBagsPool);
        //CreateBagsPool((int)levelManager.level);
    }

    //int GetBagAmountFromCharacterLevelManager(int level)
    //{
    //    //int amount = levelManager.GetBagAmountFromSetup(level);
    //    //return amount;
    //}

    public void CreateBagsPool(int level)
    {
        carryBodies.Delete();

        bagPoolSize = 3;

        for (int i = 0; i < bagPoolSize; i++)
        {
            CreateBag(i);
        }
    }

    void CreateBag(int index)
    {
        //BodyBag sB = Instantiate(carryBodies.body);
        //carryBodies.Add(sB, index);
    }

    
}
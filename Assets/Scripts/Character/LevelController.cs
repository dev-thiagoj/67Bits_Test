using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelController : MonoBehaviour
{
    Storage _storage;
    [SerializeField] PlayerLevel currentLevel;

    [Header("Levels Setup")]
    [SerializeField] LevelSetup _setup;

    [Header("Materials Manager")]
    [SerializeField] List<Material> materials;
    [SerializeField] Renderer[] renderers;

    [Space]
    public UnityEvent LevelUpped;
    
    public PlayerLevel CurrentLevel => currentLevel;
    bool canInvoke;

    private void Awake()
    {
        currentLevel = PlayerLevel.Noobie;
    }

    public int BagAmount()
    {
        var setup = _setup.GetSetupByLevel(CurrentLevel);
        return setup.bagsAmount;
    }

    public bool CanAdd(int amount)
    {
        int bags = _setup.GetSetupByLevel(CurrentLevel).bagsAmount;
        return bags > amount;
    }

#if UNITY_EDITOR
    [ContextMenu("Debug LevelUp")]
#endif
    public void InvokeLevelUp()
    {
        if ((int)CurrentLevel >= (int)PlayerLevel.SerialKiller)
            return;

        int index = (int)CurrentLevel;
        index++;

        currentLevel = (PlayerLevel)index;

        LevelUpped?.Invoke();
    }
}

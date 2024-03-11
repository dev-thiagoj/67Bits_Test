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
    public UnityEvent<int> OnLevelUpped;

    public LevelSetup Setup => _setup;
    public PlayerLevel CurrentLevel => currentLevel;
    public Setup CurrentSetup => _setup.GetSetupByLevel(currentLevel);

    private void Awake()
    {
        currentLevel = PlayerLevel.Noobie;
    }

    public bool CanAdd(int amount)
    {
        int bags = _setup.GetSetupByLevel(CurrentLevel).bagsAmount;
        return bags > amount;
    }

    [ContextMenu("Debug LevelUp")]
    public void LevelUp()
    {
        if ((int)CurrentLevel >= (int)PlayerLevel.SerialKiller)
            return;

        int index = (int)CurrentLevel;
        index++;

        currentLevel = (PlayerLevel)index;

        OnLevelUpped?.Invoke((int)currentLevel);
    }
}

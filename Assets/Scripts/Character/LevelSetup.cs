using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Level Setup")]
public class LevelSetup : ScriptableObject
{
    [SerializeField] List<Setup> _setups;

    public Setup GetSetupByLevel(PlayerLevel level) => _setups.Find(x => x.level == level);
    public Setup GetSetupByTarget(int target) => _setups.Find(x => x.target == target);
}

[Serializable]
public class Setup
{
    public PlayerLevel level;
    public int target;
    public int bagsAmount;
    public GameObject clothes;
}

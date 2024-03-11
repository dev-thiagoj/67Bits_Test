using UnityEngine;

public interface ILevelItem
{
    public LevelController LevelController { get; }
    public PlayerLevel Level { get; }
    public Renderer Renderer { get; }
}
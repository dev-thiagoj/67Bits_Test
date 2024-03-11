using UnityEngine;

public class LevelItem : MonoBehaviour, ILevelItem
{
    [SerializeField] PlayerLevel itemLevel;

    LevelController _levelController;
    Renderer _renderer;

    public PlayerLevel Level => itemLevel;
    public Renderer Renderer => _renderer;

    public LevelController LevelController => _levelController;

    private void OnDestroy()
    {
        _levelController.LevelUpped.RemoveListener(SetStatus);
    }

    private void Awake()
    {
        if(!_levelController)
            _levelController = GetComponentInParent<LevelController>();

        if(!_renderer)
            _renderer = GetComponent<Renderer>();

        _levelController.LevelUpped.AddListener(SetStatus);

        SetStatus();
    }

    public void SetStatus()
    {
        bool status = _levelController.CurrentLevel == itemLevel;
        _renderer.enabled = status;
    }
}

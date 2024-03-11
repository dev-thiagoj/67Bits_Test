using UnityEngine;

public class LevelItem : MonoBehaviour
{
    LevelController _levelController;
    Renderer _renderer;

    private void OnDestroy()
    {
        _levelController.OnLevelUpped.RemoveListener(SetStatus);
    }

    private void Awake()
    {
        if(!_levelController)
            _levelController = GetComponentInParent<LevelController>();

        if(!_renderer)
            _renderer = GetComponent<Renderer>();

        _levelController.OnLevelUpped.AddListener(SetStatus);

        SetStatus();
    }

    public void SetStatus(int value = 0)
    {
        var setup = _levelController.Setup.GetSetupByLevel((PlayerLevel)value);
        _renderer.material.mainTexture = setup.pallete;
    }
}

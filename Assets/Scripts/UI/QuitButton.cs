using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    [SerializeField] Button quitButton;

    private void Awake()
    {
        if (!quitButton)
            quitButton = GetComponent<Button>();

        quitButton.onClick.AddListener(Quit);
    }

    void Quit()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif

        Application.Quit();
    }
}

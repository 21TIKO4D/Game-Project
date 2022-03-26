using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    /*[SerializeField]
    private static bool GameIsPaused = false;*/
    [SerializeField]
    private GameObject PauseMenuUI;

    public void Start()
    {
        PauseMenuUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        LevelLoader.Current.LoadLevel("Menu");
    }
    
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        //GameIsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //GameIsPaused = false;
    }
}

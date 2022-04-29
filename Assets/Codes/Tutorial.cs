using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject pages;

    private int currentPage = 1;

    public void CloseTutorial()
    {
        LevelLoader.Current.CloseTutorial();
    }

    public void ChangePage(int value)
    {
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(false);
        currentPage += value;
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(true);
    }
}

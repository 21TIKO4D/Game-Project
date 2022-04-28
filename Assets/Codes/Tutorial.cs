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

    public void NextPage()
    {
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(false);
        currentPage++;
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(true);
    }

    public void PreviousPage()
    {
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(false);
        currentPage--;
        pages.transform.GetChild(currentPage - 1).gameObject.SetActive(true);
    }
}

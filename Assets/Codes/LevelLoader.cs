using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public enum LoadingState
	{
		None,
		Started,
		InProgress,
		Tutorial
	}

	public const string LoaderName = "Loader";
	public const string TutorialName = "Tutorial";

	public static LevelLoader Current
	{
		get;
		private set;
	}

	public int currentLevel = -1;
	private string nextSceneName;
	private Fader fader;
	private GameObject AnimalsObj;
	private LoadingState state = LoadingState.None;
	private Scene originalScene;
	private Scene loadingScene;
	private Scene tutorialScene;

	// Nk. Singleton, eli tästä oliosta voi olla vain yksi kopio olemassa kerralla
	private void Awake()
	{
		if (Current == null)
		{
			Current = this;
			PlayerPrefs.SetString("currentUI", "MainMenu");
			PlayerPrefs.SetInt("Level0", 0);
			PlayerPrefs.Save();
		}
		else
		{
			// LevelLoader on jo olemassa! Tuhotaan uusi instanssi
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		// Aletaan kuunnella eventtiä
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	private void OnDisable()
	{
		// Lopetetaan eventin kuuntelu
		SceneManager.sceneLoaded -= OnLevelLoaded;
	}

	public void LoadTutorial()
	{
		// Pysäytä peli
		Time.timeScale = 0;
		state = LoadingState.Tutorial;
		SceneManager.LoadSceneAsync(TutorialName, LoadSceneMode.Additive);
	}

	public void CloseTutorial()
	{
		state = LoadingState.None;
		SceneManager.UnloadSceneAsync(tutorialScene);
		Time.timeScale = 1;
	}

	public void LoadScene(string sceneName)
	{
		LoadScene(sceneName, -1);
	}

	public void LoadScene(string sceneName, int level)
	{
		currentLevel = level;
		nextSceneName = sceneName;
		originalScene = SceneManager.GetActiveScene();
		state = LoadingState.Started;
		// Ladataan loading screen additiivisesti (nykyisen scenen rinnalle)
		SceneManager.LoadSceneAsync(LoaderName, LoadSceneMode.Additive);
	}

	private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
	{
		switch (state)
		{
			case LoadingState.Started:
				loadingScene = scene;
				// Aloitetaan Fade animaatio
				GameObject[] rootObjects = loadingScene.GetRootGameObjects(); // Palauttaa scenen kaikki root-GameObjectit
				foreach (GameObject item in rootObjects)
				{
					fader = item.GetComponentInChildren<Fader>();
				}
				if (fader != null)
				{
					StartCoroutine(ContinueLoad(fader.FadeIn()));
				}
				break;
			case LoadingState.InProgress:
				StartCoroutine(FinalizeLoad(fader.FadeOut()));

				state = LoadingState.None;
				break;
			case LoadingState.Tutorial:
				tutorialScene = scene;
				break;
		}
	}

	private IEnumerator FinalizeLoad(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		SceneManager.UnloadSceneAsync(loadingScene);
	}

	private IEnumerator ContinueLoad(float waitTime)
	{
		yield return new WaitForSeconds(waitTime); // Odottaa waitTime:n verran

		SceneManager.UnloadSceneAsync(originalScene);
		if (nextSceneName.Equals("Menu"))
		{
			PlayerPrefs.SetString("currentUI", "LevelMenu");
		}
		// Ladataan seuraava scene
		SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
		state = LoadingState.InProgress;
	}
}

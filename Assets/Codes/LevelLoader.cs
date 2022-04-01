using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public enum LoadingState
	{
		None,
		Started,
		InProgress,
		Options
	}

	public const string LoaderName = "Loader";
	public const string OptionsName = "Options";

	public static LevelLoader Current
	{
		get;
		private set;
	}

	private string nextSceneName;
	private Fader fader;
	private GameObject AnimalsObj;
	private LoadingState state = LoadingState.None;
	private Scene originalScene;
	private Scene loadingScene;
	private Scene optionsScene;

	// Nk. Singleton, eli tästä oliosta voi olla vain yksi kopio olemassa kerralla
	private void Awake()
	{
		if (Current == null)
		{
			Current = this;
			PlayerPrefs.DeleteAll();
        	PlayerPrefs.SetString("currentUI", "MainMenu");
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

	public void LoadOptions()
	{
		// Pysäytä peli
		Time.timeScale = 0;
		state = LoadingState.Options;
		SceneManager.LoadSceneAsync(OptionsName, LoadSceneMode.Additive);
	}

	public void CloseOptions()
	{
		state = LoadingState.None;
		SceneManager.UnloadSceneAsync(optionsScene);
		Time.timeScale = 1; // Palauta pelin normaalinopeus
	}

	public void LoadScene(string sceneName)
	{
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
			case LoadingState.Options:
				optionsScene = scene;
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

		// Suoritus jatkuu waitTime:n kuluttua
		// Näyttö on musta, joten pelaaja ei enää näe alkuperäistä sceneä.
		// Unloadataan se
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

using UnityEngine.SceneManagement;

public static class Extension_SceneManager
{
	public static void ReloadCurrentScene(this SceneManager sceneManager)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
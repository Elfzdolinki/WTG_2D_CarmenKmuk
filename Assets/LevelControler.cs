using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private static int _nextLevelIndex = 1; // Starting at Level 1
    private Enemy[] _enemies;

    private void OnEnable()
    {
        _enemies = FindObjectsOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
                return; // If any enemy is still alive, exit the method
        }

        Debug.Log("All enemies killed, loading next level.");

        _nextLevelIndex++;
        string nextLevelName = "Level" + _nextLevelIndex.ToString(); // Use ToString to ensure correct formatting
        SceneManager.LoadScene(nextLevelName);
    }
}
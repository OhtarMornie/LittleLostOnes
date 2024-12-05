using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class Restart : MonoBehaviour
    {
        [MenuItem("Helpers/Restart Scene")]
        private static void RestartScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}



using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Scene2");
    }
}

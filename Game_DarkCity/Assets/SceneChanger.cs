using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Hàm chuyển scene
    public void ChangeScene(int scence)
    {
        SceneManager.LoadScene(scence);
    }
}


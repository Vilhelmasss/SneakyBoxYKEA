using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    private const string DATA_FILE_NAME = "data.json";

    public void NewGame()
    {
        string appDataPath = Path.Combine(Application.persistentDataPath, DATA_FILE_NAME);

        if (File.Exists(appDataPath))
        {
            File.Delete(appDataPath);
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public string LostSceneName;
    public string OperatorSceneName;

    public void OpenScene(CharacterType character)
    {
        if (character == CharacterType.Lost)
        {
            SceneManager.LoadScene(LostSceneName);
        }
        else
        {
            SceneManager.LoadScene(OperatorSceneName);
        }
    }
}

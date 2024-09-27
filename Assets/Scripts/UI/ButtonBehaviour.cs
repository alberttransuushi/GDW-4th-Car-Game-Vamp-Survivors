using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public void onPlayClick()
    {

        SceneManager.LoadScene("MockupLevel");
    }

    public void onOptionsClick()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void onQuitClick()
    {
        Application.Quit(0);
    }
}

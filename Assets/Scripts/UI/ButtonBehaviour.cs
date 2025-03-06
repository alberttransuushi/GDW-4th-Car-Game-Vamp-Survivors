using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public bool _selected = false;
    private Button _button;

    private bool _disableControls = false;

    [SerializeField]
    private GameObject associatedCar;

    [SerializeField]
    private Camera cameraRef;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if(associatedCar != null)
        {
            _button.gameObject.transform.position = cameraRef.WorldToScreenPoint(associatedCar.gameObject.transform.position);
        }
        if(_selected == true)
        {

            Quaternion look = Quaternion.LookRotation(associatedCar.transform.position - cameraRef.transform.position);

            cameraRef.transform.rotation = Quaternion.Slerp(cameraRef.transform.rotation, look, 5 * Time.deltaTime);
        }
    }

    public void onPlayClick()
    {

        SceneManager.LoadScene("CarSelect");
    }

    public void onOptionsClick()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void onQuitClick()
    {
        Application.Quit(0);
    }

    public void OnCarClick()
    {
        SceneManager.LoadScene("MockupLevel");
    }

    public void SwappedTo()
    {
        _selected = true;


    }

    public void SwappedOff()
    {
        _selected = false;
    }

    public void ClickButton()
    {
        if (!_disableControls)
        {
            _disableControls = true;

            _button.onClick.Invoke();


            _disableControls = false;
        }
    }

    public bool GetDisableControls()
    {
        return _disableControls;
    }
}

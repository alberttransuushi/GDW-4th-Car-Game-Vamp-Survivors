using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public bool _selected = false;
    private Button _button;
    private Image _image;

    [SerializeField]
    //private bool _disableControls = false;

    //[SerializeField]
    private GameObject associatedCar;

    [SerializeField]
    private Camera cameraRef;

    private MenuController canvas;
    private MenuDefinition weaponSelectMenu;

    private void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        if (_selected)
        {
            _image.color = Color.white;
        }
        else
        {
            _image.color = Color.grey;
        }
    }

    private void Update()
    {
        //_disableControls = false;
        if (associatedCar != null)
        {
            _button.gameObject.transform.position = cameraRef.WorldToScreenPoint(associatedCar.gameObject.transform.position);

            if (_selected == true)
            {

                Quaternion look = Quaternion.LookRotation(associatedCar.transform.position - cameraRef.transform.position);

                cameraRef.transform.rotation = Quaternion.Slerp(cameraRef.transform.rotation, look, 5 * Time.deltaTime);
            }
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

    public void OnFireTruckClick()
    {
        PlayerPrefs.SetInt("CarSelected", 0);
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuController>();
        canvas.changeActiveMenu();
        canvas._activeButton = 0;
        //_disableControls = false;
    }
    public void OnSamuraiCarClick()
    {
        PlayerPrefs.SetInt("CarSelected", 1);
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuController>();
        canvas.changeActiveMenu();
        canvas._activeButton = 0;
        //_disableControls = false;
    }
    public void OnPoliceCarClick()
    {
        PlayerPrefs.SetInt("CarSelected", 2);
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MenuController>();
        canvas.changeActiveMenu();
        canvas._activeButton = 0;
        //_disableControls = false;
    }
    public void OnRPGClick()
    {
        PlayerPrefs.SetInt("WeaponSelected", 0);
        SceneManager.LoadScene("AlbertsTestingGround");
    }
    public void OnPlowClick()
    {
        PlayerPrefs.SetInt("WeaponSelected", 1);
        SceneManager.LoadScene("AlbertsTestingGround");
    }
    public void OnHOGClick()
    {
        PlayerPrefs.SetInt("WeaponSelected", 2);
        SceneManager.LoadScene("AlbertsTestingGround");
    }
    public void onMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SwappedTo()
    {
        _selected = true;
        _image.color = Color.white;


    }

    public void SwappedOff()
    {
        _selected = false;
        _image.color = Color.grey;
    }

    public void ClickButton()
    {
        //if (!_disableControls)
        //{
           // _disableControls = true;

            _button.onClick.Invoke();


            //_disableControls = false;
        //}
    }

    //public bool GetDisableControls()
    //{
        //return _disableControls;
    //}
}

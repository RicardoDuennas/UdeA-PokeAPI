using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WelcomePanelController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _jsonMessage;
    private Button _loadJSONButton;
    private Button _fetchPokemonsButton;
    private VisualElement _welcomePanel;
    [SerializeField] InfoSideBarManager _infoSideBar;
    [SerializeField] private GameManager gameManager;
    private InfoPanelManager _infoPanelManager;


    private void OnEnable()
    {
        // Get the UIDocument component
        _uiDocument = GetComponent<UIDocument>();

        // Get references to the buttons
        _loadJSONButton = _uiDocument.rootVisualElement.Q<Button>("LoadJSONButton");
        _fetchPokemonsButton = _uiDocument.rootVisualElement.Q<Button>("FetchPokemonsButton");
        _welcomePanel = _uiDocument.rootVisualElement.Q<VisualElement>("WelcomePanel");
        _jsonMessage  = _uiDocument.rootVisualElement.Q<Label>("JSONMessage");

        // Add click event handlers
        _loadJSONButton.clicked += OnLoadJSONClicked;
        _fetchPokemonsButton.clicked += OnFetchPokemonsClicked;
    }

    private void Start() {
        _infoPanelManager = FindObjectOfType<InfoPanelManager>();

        if (!gameManager.FileExist()) 
        {
            _loadJSONButton.style.display = DisplayStyle.None;
            _jsonMessage.style.display = DisplayStyle.None;
            _fetchPokemonsButton.text = "Iniciar";
        }
    }

    private void OnDisable()
    {
        // Remove event handlers to avoid memory leaks
        _loadJSONButton.clicked -= OnLoadJSONClicked;
        _fetchPokemonsButton.clicked -= OnFetchPokemonsClicked;
    }

    private void OnLoadJSONClicked()
    {
        // Trigger the transition
        _welcomePanel.style.opacity = 0;
        _welcomePanel.style.scale = new Scale(new Vector2(0.5f, 0.5f));
        _infoSideBar.showUI();
        // Disable the panel after the transition
        _welcomePanel.schedule.Execute(() =>
        {
            _welcomePanel.style.display = DisplayStyle.None;
            _welcomePanel.RemoveFromHierarchy();
        }).StartingIn(500); 
        GameManager.Instance.LoadData();
    }

    private void OnFetchPokemonsClicked()
    {
        // Trigger the transition
        _welcomePanel.style.opacity = 0;
        _welcomePanel.style.scale = new Scale(new Vector2(0.5f, 0.5f));
        _infoSideBar.showUI();
        _infoPanelManager.AddMessage("Usando API para cargar Pokemones");
        // Disable the panel after the transition
        _welcomePanel.schedule.Execute(() =>
        {
            _welcomePanel.style.display = DisplayStyle.None;
            _welcomePanel.RemoveFromHierarchy();
        }).StartingIn(500); 
        GameManager.Instance.StartGame();
    }
}
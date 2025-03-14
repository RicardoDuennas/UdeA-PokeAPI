using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEditor.Search;

public class InfoSideBarManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _sidebarPanel;
    private VisualElement _pokemonListPanel;
    private VisualElement _cardPanel;
    private VisualElement _openCloseBtn;
    private bool _sidebarOpen = false; 
    private const float ANIMATION_DURATION = 0.3f;
    private PlayerController _playerCont;
    private VisualElement _listContainer;
    private Button _backButton;

    [SerializeField] private PokemonAPIManager _pokeApiManager;    
    [SerializeField] private GameObject _player;

    private Coroutine animationCoroutine;
    
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _playerCont = _player.GetComponent<PlayerController>();
    }
    
    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        
        _sidebarPanel = root.Q<VisualElement>("Sidebar");
        _openCloseBtn = root.Q<VisualElement>("OpenCloseBtn");
        _pokemonListPanel = root.Q<VisualElement>("PokemonList");
        _cardPanel = root.Q<VisualElement>("Card");
        _listContainer = root.Q<VisualElement>("ListView");
        _backButton = root.Q<Button>("BackButton");
        
        _backButton.clicked += OnBackButtonClicked;

        _openCloseBtn.RegisterCallback<ClickEvent>(OnOpenCloseBtnClick);
        
        InitializeSidebar();
    }
    
    private void InitializeSidebar()
    {
        // Set initial width to closed state and transparent
        _sidebarPanel.style.width = new StyleLength(new Length(0f, LengthUnit.Percent));
        _sidebarPanel.style.opacity = 0;
        _cardPanel.style.display = DisplayStyle.None;
        _pokemonListPanel.style.display = DisplayStyle.Flex;
        // PopulatePokemonList();
    }
    
    private void OnOpenCloseBtnClick(ClickEvent evt)
    {

        if (_sidebarOpen) {
            _playerCont.ResumeGame();
         } 

        // Toggle sidebar state
        _sidebarOpen = !_sidebarOpen;
        
        AnimateSidebar();
    }
    
    private void AnimateSidebar()
    {
        // Stop any ongoing animations
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSidebarWidthAndOpacity());
    }
    
    private IEnumerator AnimateSidebarWidthAndOpacity()
    {
        float startWidth = _sidebarPanel.style.width.value.value;
        float targetWidth = _sidebarOpen ? 30f : 0f;
        
        float startOpacity = _sidebarPanel.style.opacity.value;
        float targetOpacity = _sidebarOpen ? 1f : 0f;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < ANIMATION_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ANIMATION_DURATION);
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            float currentWidth = Mathf.Lerp(startWidth, targetWidth, smoothT);
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, smoothT);
            
            _sidebarPanel.style.width = new StyleLength(new Length(currentWidth, LengthUnit.Percent));
            _sidebarPanel.style.opacity = currentOpacity;            
            yield return null;
        }
        
        _sidebarPanel.style.width = new StyleLength(new Length(targetWidth, LengthUnit.Percent));
        _sidebarPanel.style.opacity = targetOpacity;

        if (_sidebarOpen) {
            _playerCont.PauseGame();
         } 
    }
    
    private void OnDisable()
    {
        // Unregister the callback when the component is disabled
        if (_openCloseBtn != null)
        {
            _openCloseBtn.UnregisterCallback<ClickEvent>(OnOpenCloseBtnClick);
        }
        
        // Stop any animations
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    public void PopulatePokemonList()
    {

        for (int i = 0; i < 20; i++)
        {   
            // Create a new message panel
            var messagePanel = new VisualElement
            {
                style =
                {
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 10,
                    paddingRight = 10,
                    marginBottom = 5,
                    borderBottomWidth = 2,
                    borderRightWidth = 2,
                    borderLeftWidth = 2,
                    borderTopWidth = 2,
                    borderTopLeftRadius =10,
                    borderTopRightRadius =10,
                    borderBottomLeftRadius =10,
                    borderBottomRightRadius =10,
                    borderTopColor = Color.gray,
                    borderBottomColor = Color.gray,
                    borderLeftColor = Color.gray,
                    borderRightColor = Color.gray,
                    whiteSpace = WhiteSpace.Normal
                }
            };
            // Add a Label to the message panel
            var messageLabel = new Label($"Pokemon {i}")
            {
                style =
                {
                    fontSize = 12,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };
            messagePanel.Add(messageLabel);
            _listContainer.contentContainer.Add(messagePanel);

            // Add the message panel to the container
            _listContainer.Add(messagePanel);
        }
    }    
    
    public void AddPokemonToList(string pokemonName)
    {

        // Create a new message panel
        var messagePanel = new VisualElement
        {
            style =
            {
                backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                paddingTop = 5,
                paddingBottom = 5,
                paddingLeft = 10,
                paddingRight = 10,
                marginBottom = 5,
                borderBottomWidth = 2,
                borderRightWidth = 2,
                borderLeftWidth = 2,
                borderTopWidth = 2,
                borderTopLeftRadius =10,
                borderTopRightRadius =10,
                borderBottomLeftRadius =10,
                borderBottomRightRadius =10,
                borderTopColor = Color.gray,
                borderBottomColor = Color.gray,
                borderLeftColor = Color.gray,
                borderRightColor = Color.gray,
                whiteSpace = WhiteSpace.Normal
            }
        };
        var messageLabel = new Label(pokemonName)
        {
            style =
            {
                fontSize = 12,
                color = Color.white,
                unityTextAlign = TextAnchor.MiddleLeft
            }
        };

        messagePanel.RegisterCallback<MouseEnterEvent>(evt =>
        {
            messagePanel.style.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.9f);
        });

        messagePanel.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            messagePanel.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        });

        messagePanel.Add(messageLabel);
        _listContainer.contentContainer.Add(messagePanel);

        // Register a click event handler for the message panel
        messagePanel.RegisterCallback<ClickEvent>(OnMessagePanelClicked);

        _listContainer.Add(messagePanel);
    }

    private void OnMessagePanelClicked(ClickEvent evt)
    {
        // Get the clicked message panel
        var clickedPanel = evt.target as VisualElement;

        // Get the Pokemon name from the Label inside the panel
        var pokemonNameLabel = clickedPanel.Q<Label>();
        if (pokemonNameLabel != null)
        {
            string pokemonName = pokemonNameLabel.text;
            ShowCard(pokemonName);
        }
    }

    private void OnBackButtonClicked()
    {
        // Switch back to the PokemonList view
        _cardPanel.style.display = DisplayStyle.None;
        _pokemonListPanel.style.display = DisplayStyle.Flex;
    }

    private void ShowCard(string name){
        _cardPanel.style.display = DisplayStyle.Flex;
        _pokemonListPanel.style.display = DisplayStyle.None;

        var headerElement = _cardPanel.Q<VisualElement>("Header");
        var nameElement = headerElement.Q<VisualElement>("Name");
        var nameLabel = nameElement.Q<Label>();
        nameLabel.text = name;

        string[] moves = _pokeApiManager.GetMovesByName(name); 
        var movesContainer = _cardPanel.Q<ScrollView>("MovesContainer");
        movesContainer.Clear();

        foreach (string move in moves)
        {
            // Create a new message panel
            var movePanel = new VisualElement
            {
                style =
                {
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    paddingTop = 2,
                    paddingBottom = 2,
                    paddingLeft = 10,
                    paddingRight = 10,
                    marginBottom = 5,
                    borderTopLeftRadius =10,
                    borderTopRightRadius =10,
                    borderBottomLeftRadius =10,
                    borderBottomRightRadius =10,
                    whiteSpace = WhiteSpace.Normal
                }
            };

            // Add a Label to the message panel
            var moveLabel = new Label(move)
            {
                style =
                {
                    fontSize = 12,
                    color = Color.white
                }
            };
            movePanel.Add(moveLabel);

            // Add hover effects
            movePanel.RegisterCallback<MouseEnterEvent>(evt =>
            {
                movePanel.style.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.9f); // Darker background on hover
            });

            movePanel.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                movePanel.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Revert background color
            });

            movesContainer.Add(movePanel);
        }

        string[] abilities = _pokeApiManager.GetAbilitiesByName(name); 
        var abilitiesContainer = _cardPanel.Q<ScrollView>("AbilitiesContainer");
        abilitiesContainer.Clear();

        foreach (string ability in abilities)
        {

            // Create a new message panel
            var abilityPanel = new VisualElement
            {
                style =
                {
                    backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f),
                    unityTextAlign = TextAnchor.MiddleCenter,
                    paddingTop = 2,
                    paddingBottom = 2,
                    paddingLeft = 10,
                    paddingRight = 10,
                    marginBottom = 5,
                    borderTopLeftRadius =10,
                    borderTopRightRadius =10,
                    borderBottomLeftRadius =10,
                    borderBottomRightRadius =10,
                    whiteSpace = WhiteSpace.Normal
                }
            };

            // Add a Label to the message panel
            var abilityLabel = new Label(ability)
            {
                style =
                {
                    fontSize = 12,
                    color = Color.white
                }
            };
            abilityPanel.Add(abilityLabel);

            // Add hover effects
            abilityPanel.RegisterCallback<MouseEnterEvent>(evt =>
            {
                abilityPanel.style.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.9f); // Darker background on hover
            });

            abilityPanel.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                abilityPanel.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Revert background color
            });
            
            abilitiesContainer.Add(abilityPanel);
        }
    }
}
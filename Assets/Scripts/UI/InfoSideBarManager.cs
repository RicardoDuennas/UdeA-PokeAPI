using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class InfoSideBarManager : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement sidebarPanel;
    private VisualElement pokemonListPanel;
    private VisualElement cardsPanel;
    private VisualElement openCloseBtn;
    private bool sidebarOpen = false; 
    private const float ANIMATION_DURATION = 0.3f;
    private PlayerController playerCont;
    private VisualElement _listContainer;

    
    [SerializeField]
    private GameObject player;

    private Coroutine animationCoroutine;
    
    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        playerCont = player.GetComponent<PlayerController>();
    }
    
    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        
        sidebarPanel = root.Q<VisualElement>("Sidebar");
        openCloseBtn = root.Q<VisualElement>("OpenCloseBtn");
        pokemonListPanel = root.Q<VisualElement>("PokemonList");
        cardsPanel = root.Q<VisualElement>("Cards");
        _listContainer = root.Q<VisualElement>("ListView");
        
        openCloseBtn.RegisterCallback<ClickEvent>(OnOpenCloseBtnClick);
        
        InitializeSidebar();
    }
    
    private void InitializeSidebar()
    {
        // Set initial width to closed state and transparent
        sidebarPanel.style.width = new StyleLength(new Length(0f, LengthUnit.Percent));
        sidebarPanel.style.opacity = 0;
        cardsPanel.style.display = DisplayStyle.None;
        pokemonListPanel.style.display = DisplayStyle.Flex;
        // PopulatePokemonList();
    }
    
    private void OnOpenCloseBtnClick(ClickEvent evt)
    {

        if (sidebarOpen) {
            playerCont.ResumeGame();
         } 

        // Toggle sidebar state
        sidebarOpen = !sidebarOpen;
        
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
        float startWidth = sidebarPanel.style.width.value.value;
        float targetWidth = sidebarOpen ? 50f : 0f;
        
        float startOpacity = sidebarPanel.style.opacity.value;
        float targetOpacity = sidebarOpen ? 1f : 0f;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < ANIMATION_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ANIMATION_DURATION);
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            float currentWidth = Mathf.Lerp(startWidth, targetWidth, smoothT);
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, smoothT);
            
            sidebarPanel.style.width = new StyleLength(new Length(currentWidth, LengthUnit.Percent));
            sidebarPanel.style.opacity = currentOpacity;            
            yield return null;
        }
        
        sidebarPanel.style.width = new StyleLength(new Length(targetWidth, LengthUnit.Percent));
        sidebarPanel.style.opacity = targetOpacity;

        if (sidebarOpen) {
            playerCont.PauseGame();
         } 
    }
    
    private void OnDisable()
    {
        // Unregister the callback when the component is disabled
        if (openCloseBtn != null)
        {
            openCloseBtn.UnregisterCallback<ClickEvent>(OnOpenCloseBtnClick);
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
            Debug.Log($"Clicked on Pokemon: {pokemonName}");

            // Add your custom logic here, e.g., display details, highlight the panel, etc.
            //clickedPanel.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f); // Example: Change background color on click
        }
    }

    
}
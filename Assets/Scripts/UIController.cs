using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class SidebarController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement sidebar;
    private VisualElement openCloseBtn;
    private bool sidebarOpen = false; 
    private const float ANIMATION_DURATION = 0.3f;
    private PlayerController playerCont;
    
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
        
        sidebar = root.Q<VisualElement>("Sidebar");
        openCloseBtn = root.Q<VisualElement>("OpenCloseBtn");
        
        openCloseBtn.RegisterCallback<ClickEvent>(OnOpenCloseBtnClick);
        
        InitializeSidebar();
    }
    
    private void InitializeSidebar()
    {
        // Set initial width to closed state and transparent
        sidebar.style.width = new StyleLength(new Length(0f, LengthUnit.Percent));
        sidebar.style.opacity = 0;
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
        float startWidth = sidebar.style.width.value.value;
        float targetWidth = sidebarOpen ? 50f : 0f;
        
        float startOpacity = sidebar.style.opacity.value;
        float targetOpacity = sidebarOpen ? 1f : 0f;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < ANIMATION_DURATION)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / ANIMATION_DURATION);
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            float currentWidth = Mathf.Lerp(startWidth, targetWidth, smoothT);
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, smoothT);
            
            sidebar.style.width = new StyleLength(new Length(currentWidth, LengthUnit.Percent));
            sidebar.style.opacity = currentOpacity;            
            yield return null;
        }
        
        sidebar.style.width = new StyleLength(new Length(targetWidth, LengthUnit.Percent));
        sidebar.style.opacity = targetOpacity;

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
}
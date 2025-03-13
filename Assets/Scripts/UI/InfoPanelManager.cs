using UnityEngine;
using UnityEngine.UIElements;

public class InfoPanelManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Label _infoPanelMessageLabel;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _infoPanelMessageLabel = root.Q<Label>("InfoPanelMessage");
    }

    public void UpdateMessage(string newMessage)
    {
        if (_infoPanelMessageLabel != null)
        {
            _infoPanelMessageLabel.text = newMessage;
        }
        else
        {
            Debug.LogError("InfoPanelMessage Label not found!");
        }
    }
}
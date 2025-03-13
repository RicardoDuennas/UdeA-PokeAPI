using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class InfoPanelManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _messageContainer;
    private const float MessageLifetime = 2f; 
    private const int MaxMessages = 3; 

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _messageContainer = root.Q<VisualElement>("InfoPanel");
    }

    public void AddMessage(string message)
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
        var messageLabel = new Label(message)
        {
            style =
            {
                fontSize = 12,
                color = Color.white,
                unityTextAlign = TextAnchor.MiddleLeft
            }
        };
        messagePanel.Add(messageLabel);

        // Add the message panel to the container
        _messageContainer.Add(messagePanel);

        // Remove the message panel after 2 seconds
        StartCoroutine(RemoveMessageAfterDelay(messagePanel, MessageLifetime));

        // Ensure only the last MaxMessages are visible
        if (_messageContainer.childCount > MaxMessages)
        {
            // Remove the oldest message (first child)
            _messageContainer.RemoveAt(0);
        }
    }

    private System.Collections.IEnumerator RemoveMessageAfterDelay(VisualElement messagePanel, float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel.RemoveFromHierarchy();
    }
}
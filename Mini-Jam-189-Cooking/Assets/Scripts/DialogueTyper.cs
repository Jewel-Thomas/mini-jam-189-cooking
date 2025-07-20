using UnityEngine;
using UnityEngine.UI; // For Legacy Text
using System.Collections;

public class DialogueTyper : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Assign the Legacy Text component that will display the dialogue.")]
    public Text dialogueText;

    [Header("Dialogue Content")]
    [Tooltip("The full dialogue text this box will display.")]
    [TextArea(3, 10)] // Makes the string field a multi-line text area in the Inspector
    public string dialogueContent;

    [Header("Typing Duration")]
    [Tooltip("The total time (in seconds) it should take for the entire dialogueContent to be typed out.")]
    [Range(0.1f, 10.0f)] // Provides a slider for easy adjustment
    public float totalTypingDuration = 2.5f; // Default to 2.5 seconds as requested

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // Call this method from another script (e.g., your CutsceneManager)
    // to start the dialogue typing for this specific DialogueBox.
    public void StartDialogueTyping()
    {
        if (dialogueText == null)
        {
            Debug.LogError("DialogueText (Legacy Text) is not assigned! Please assign it in the Inspector.", this);
            return;
        }

        if (string.IsNullOrEmpty(dialogueContent))
        {
            Debug.LogWarning("Dialogue content is empty for this DialogueTyper instance.", this);
            return;
        }

        // Ensure the dialogue box (its parent panel) is active
        if (dialogueText.gameObject.transform.parent != null)
        {
            dialogueText.gameObject.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            dialogueText.gameObject.SetActive(true); // Fallback if no parent panel
        }

        // Stop any existing typing coroutine before starting a new one
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start the typing effect
        typingCoroutine = StartCoroutine(TypeLine(dialogueContent, totalTypingDuration));
    }

    // Call this method to immediately complete the typing of the current line.
    public void CompleteTyping()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogueText.text = dialogueContent; // Show full text immediately
            isTyping = false;
        }
    }

    // Coroutine to type out the full line over the specified total duration
    private IEnumerator TypeLine(string line, float duration)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the text initially

        if (line.Length == 0)
        {
            isTyping = false;
            yield break; // Nothing to type
        }

        // Calculate the delay per character based on total duration and line length
        float delayPerChar = duration / line.Length;

        for (int i = 0; i < line.Length; i++)
        {
            dialogueText.text += line[i]; // Append character
            yield return new WaitForSeconds(delayPerChar);
        }

        isTyping = false;
    }

    // Optional: Keep this for manual testing/skipping during development
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CompleteTyping();
        }
    }

    // Optional: Hide dialogue box when the GameObject is disabled (e.g., by another script or Timeline)
    void OnDisable()
    {
        if (dialogueText != null && dialogueText.gameObject.transform.parent != null)
        {
            dialogueText.gameObject.transform.parent.gameObject.SetActive(false);
        }
        else if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }
}

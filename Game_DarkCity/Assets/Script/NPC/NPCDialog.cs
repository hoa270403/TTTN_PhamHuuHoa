using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public string text;
        public Sprite image;
    }

    public Dialogue[] dialogues;
    public GameObject dialoguePanel;
    public Image npcImage;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    private int currentDialogueIndex = 0;
    private bool isPlayerNear = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialogues.Length > 0)
        {
            currentDialogueIndex = 0;
            dialoguePanel.SetActive(true);
            StartTypingDialogue();
        }
    }

    private void OnContinuePressed()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[currentDialogueIndex].text;
            isTyping = false;
        }
        else
        {
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            StartTypingDialogue();
        }
        else
        {
            dialoguePanel.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void StartTypingDialogue()
    {
        dialogueText.text = "";
        npcImage.sprite = dialogues[currentDialogueIndex].image;
        typingCoroutine = StartCoroutine(TypeText(dialogues[currentDialogueIndex].text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Tốc độ đánh máy
        }
        isTyping = false;
    }
}

using System;
using TMPro;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class DialogueHandler : MonoBehaviour
{
    public static event Action<Story> OnCreateStory;
    public static event Action OnStoryEnd;

    [Header("Story Configuration")]
    public TextAsset inkJSONAsset = null;

    [Header("UI References")]
    [SerializeField, Required]
    private RectTransform dialogueArea = null;
    
    [SerializeField, Required]
    private RectTransform choiceArea = null;

    [Header("UI Prefabs")]
    [SerializeField, Required]
    private TextMeshProUGUI textPrefab = null;
    
    [SerializeField, Required]
    private Button buttonPrefab = null;
    private Story story;

    [Button]
    public void StartStory()
    {
        if (inkJSONAsset == null)
        {
            Debug.LogError("No Ink JSON asset assigned to DialogueHandler!");
            return;
        }

        ClearUI();
        InitializeStory();
        RefreshView();
    }
    private void InitializeStory()
    {
        try
        {
            story = new Story(inkJSONAsset.text);
            OnCreateStory?.Invoke(story);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize story: {e.Message}");
        }
    }

    private void RefreshView()
    {
        if (story == null) return;

        DisplayStoryText();
        DisplayChoices();
    }

    private void DisplayStoryText()
    {
        while (story.canContinue)
        {
            string text = story.Continue().Trim();
            CreateDialogueText(text);
        }
    }

    private void DisplayChoices()
    {
        if (story.currentChoices.Count > 0)
        {
            foreach (var choice in story.currentChoices)
            {
                CreateChoiceButton(choice);
            }
        }
        else
        {
            OnStoryEnd?.Invoke();
        }
    }

    private void CreateDialogueText(string text)
    {
        if (textPrefab == null || dialogueArea == null) return;

        var storyText = Instantiate(textPrefab, dialogueArea);
        storyText.text = text;
    }

    private void CreateChoiceButton(Choice choice)
    {
        if (buttonPrefab == null || choiceArea == null) return;

        var button = Instantiate(buttonPrefab, choiceArea);
        var choiceText = button.GetComponentInChildren<TextMeshProUGUI>();
        
        if (choiceText != null)
        {
            choiceText.text = choice.text.Trim();
        }

        button.onClick.AddListener(() => OnChoiceSelected(choice));
    }

    private void OnChoiceSelected(Choice choice)
    {
        if (story == null) return;

        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    private void ClearUI()
    {
        if (choiceArea != null)
        {
            choiceArea.DeleteChildrens();
        }
        
        if (dialogueArea != null)
        {
            dialogueArea.DeleteChildrens();
        }
    }
}

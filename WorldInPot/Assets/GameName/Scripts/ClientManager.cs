using System.Linq;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

[System.Serializable]
public class Client
{
    public string clientName;
    public GameObject clientObject;
    public TextAsset talkFile;
}

public class ClientManager : SingletonMono<ClientManager>
{
    [SerializeField] private DialogueHandler dialogueHandler;
    [SerializeField] private TextMeshProUGUI clientNameSlot;
    [SerializeField] private Client[] clients;
    [SerializeField] private GameObject[] charas;
    public TextAsset[] skipFile;

    [SerializeField] private int currentClientIndex = 0;
    [SerializeField] private bool isContinuyingDialogue = false;

    protected override void Awake()
    {
        base.Awake();
        DialogueHandler.OnStoryEnd += OnDialogueEnd;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        DialogueHandler.OnStoryEnd -= OnDialogueEnd;
    }

    private void Start()
    {
        ValidateReferences();
        StartClientTalk();
    }

    private void ValidateReferences()
    {
        if (dialogueHandler == null)
        {
            Debug.LogError("DialogueHandler is not assigned in ClientManager!");
        }
        if (clientNameSlot == null)
        {
            Debug.LogError("ClientNameSlot is not assigned in ClientManager!");
        }
        if (clients == null || clients.Length == 0)
        {
            Debug.LogError("No clients assigned in ClientManager!");
        }
    }

    private void StartClientTalk()
    {
        if (currentClientIndex >= clients.Length)
        {
            currentClientIndex = clients.Length;
            StartClientTalk();
            return;
        }

        foreach (var item in charas)
        {
            item.SetActive(false);
        }

        // Start client intro dialogue
        clients[currentClientIndex].clientObject.SetActive(true);
        clientNameSlot.text = clients[currentClientIndex].clientName;
        dialogueHandler.inkJSONAsset = clients[currentClientIndex].talkFile;

        isContinuyingDialogue = skipFile.Contain(clients[currentClientIndex].talkFile);
        currentClientIndex++;

        dialogueHandler.StartStory();
    }

    private void OnDialogueEnd()
    {
        if (isContinuyingDialogue)
        {
            StartClientTalk();
            return;
        }
        else
        {
            GameHandler.Instance.ChangeGamePhase(true);
        }
    }

    public void CompleteCurrentClient()
    {
        StartClientTalk();
    }
}

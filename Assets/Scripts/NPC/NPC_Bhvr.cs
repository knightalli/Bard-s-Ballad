using DialogueEditor;
using UnityEngine;

public class NPC_Bhvr : MonoBehaviour
{
    [SerializeField] private GameObject _button;
    [SerializeField] private NPCConversation _conversation;

    private bool _isPlayerInTrigger = false;

    public bool isFirstTalk = true;

    private void Update()
    {
        if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {            
            ConversationManager.Instance.StartConversation(_conversation);
            ConversationManager.Instance.SetBool("FirstTalk", isFirstTalk);
            isFirstTalk = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _button.SetActive(true);
            _isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _button.SetActive(false);
            _isPlayerInTrigger = false;
        }
    }
}

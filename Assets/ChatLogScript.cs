using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatLogScript : MonoBehaviour
{
    public InputField theChatBox;
    public GameObject theChatLog;
    public GameObject textObject;

    private List<GameObject> chatLogTextObjects = new List<GameObject>();
    private int maxChatMessages = 25;

    private string theTranslation;

    public GameObject TranslateObject;
    private LanguageTranslatorScript _theLanguageTranslator;

    // Start is called before the first frame update
    void Start()
    {
        _theLanguageTranslator = TranslateObject.GetComponent<LanguageTranslatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theChatBox.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                DoTranslation(theChatBox.text);
                theChatBox.text = "";
            }
        }

        if(_theLanguageTranslator.TranslationReady())
        {
            GetTranslation();
        }
    }

    public void DoTranslation(string typedMessage)
    {
        _theLanguageTranslator.AddString(typedMessage);
    }

    public void GetTranslation()
    {
        theTranslation = _theLanguageTranslator.ReturnTranslation();
        SendMessageToChat(theTranslation);
    }

    public void SendMessageToChat(string chatMessage)
    {
        if(chatLogTextObjects.Count >= maxChatMessages)
        {
            Destroy(chatLogTextObjects[0]);
            chatLogTextObjects.Remove(chatLogTextObjects[0]);
        }

        GameObject newText = Instantiate(textObject, (theChatLog.transform));

        Text theMessageSent = newText.GetComponent<Text>();

        theMessageSent.text = chatMessage;
        chatLogTextObjects.Add(newText);
    }
}

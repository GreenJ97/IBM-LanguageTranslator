using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DropdownScript : MonoBehaviour
{
    public GameObject TranslateObject;
    private LanguageTranslatorScript _theLanguageTranslator;

    private Dictionary<string, string> theLanguageForDropdown;

    public Dropdown theDropdown;

    private List<Dropdown.OptionData> messagesDrop = new List<Dropdown.OptionData>();

    // Start is called before the first frame update
    void Start()
    {
        _theLanguageTranslator = TranslateObject.GetComponent<LanguageTranslatorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_theLanguageTranslator.LanguageListDone() == true)
        {
            theLanguageForDropdown = _theLanguageTranslator.GetTheDictionary();

            SetDropdownValues();
        }

        if(theDropdown.captionText.text != "")
        {
            _theLanguageTranslator.SetLanguage(theLanguageForDropdown[theDropdown.captionText.text]);
        }
    }

    public void SetDropdownValues()
    {
        foreach(KeyValuePair<string, string> kvp in theLanguageForDropdown)
        {
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = kvp.Key;

            messagesDrop.Add(newData);
        }

        foreach(Dropdown.OptionData message in messagesDrop)
        {
            theDropdown.options.Add(message);
        }
    }
}

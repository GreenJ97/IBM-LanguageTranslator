using IBM.Watson.LanguageTranslator.V3;
using IBM.Watson.LanguageTranslator.V3.Model;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Cloud.SDK;

public class LanguageTranslatorScript : MonoBehaviour
{
    #region
    [Space(10)]
    [SerializeField]
    private string iamApikey;
    [SerializeField]
    private string serviceUrl;
    [SerializeField]
    private string versionDate;
    #endregion

    private LanguageTranslatorService service;

    private List<string> textToTranslateList = new List<string>();
    private string theUserLanguage = "en";
    private string theTargetLanguage = "es";
    private string translationResponseHere;
    private bool translationFinished = false;

    private bool languageListReady = false;
    private Dictionary<string, string> languageNameAndCode = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    public IEnumerator CreateService()
    {
        if(string.IsNullOrEmpty(iamApikey))
        {
            throw new IBMException("Please provide IAM Apikey");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

        while (!authenticator.CanAuthenticate())
            yield return null;

        service = new LanguageTranslatorService(versionDate, authenticator);
        if(!string.IsNullOrEmpty(serviceUrl))
        {
            service.SetServiceUrl(serviceUrl);
        }

        GetLanguageList();
    }
    
    public IEnumerator TranslateThisNow(List<string> thisIsTheString, string targetLangWanted, string currentLang)
    {
        TranslationResult translateResponse;
        Translation theTranslateString;
        string theReponse = null;
        service.Translate(
            callback: (DetailedResponse<TranslationResult> response, IBMError error) =>
            {
                translateResponse = response.Result;
                theTranslateString = translateResponse.Translations[0];

                theReponse = theTranslateString._Translation;
            },
            text: thisIsTheString,
            source: currentLang,
            target: targetLangWanted
            );

        while (theReponse == null)
            yield return null;

        translationResponseHere = theReponse;
        translationFinished = true;
    }

    public void AddString(string theString)
    {
        textToTranslateList.Add(theString);

        if(textToTranslateList.Count > 0)
        {
            Runnable.Run(TranslateThisNow(textToTranslateList, theTargetLanguage, theUserLanguage));
            textToTranslateList.Remove(textToTranslateList[0]);
        }
    }

    public bool TranslationReady()
    {
        if (translationFinished)
        {
            translationFinished = false;
            return true;
        }
        else
            return false;
    }

    public string ReturnTranslation()
    {
        return translationResponseHere;
    }

    public void GetLanguageList()
    {
        Languages theLanguageList = null;
        List<Language> LanguagesAvailable = new List<Language>();

        service.ListLanguages(
            callback: (DetailedResponse<Languages> response, IBMError error) =>
            {
                theLanguageList = response.Result;

                LanguagesAvailable = theLanguageList._Languages;

                foreach (var res in LanguagesAvailable)
                {
                    languageNameAndCode.Add(res.LanguageName, res._Language);
                }
                languageListReady = true;
            }
            );
    }

    public bool LanguageListDone()
    {
        if (languageListReady)
        {
            languageListReady = false;
            return true;
        }
        else
            return false;
    }

    public Dictionary<string, string> GetTheDictionary()
    {
        return languageNameAndCode;
    }

    public void SetLanguage(string theLang)
    {
        theTargetLanguage = theLang;
    }
}

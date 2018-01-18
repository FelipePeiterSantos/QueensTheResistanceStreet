using UnityEngine;
using System.Collections;

public class loadRanking : MonoBehaviour {

    public UnityEngine.UI.Text printRank;

    const string publicCode = "58c5804dd60245055cd53fe3";
    const string webUrl = "http://dreamlo.com/lb/";
    
    IEnumerator Start() {
        printRank.text = "Downloading scores...";
        WWW www = new WWW(webUrl+publicCode+"/pipe/");
        yield return www;

        if(string.IsNullOrEmpty(www.error)) {
            FormatScore(www.text);
        }
        else {
            printRank.text = "Try Again: "+ www.error;
        }
    } 

    void FormatScore(string textStream) {
        printRank.text = "";
        string[] entries = textStream.Split(new char[] { '\n' },System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < entries.Length; i++){
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string user = entryInfo[0];
            string score = entryInfo[1];
            printRank.text += entryInfo[1]+" - " + entryInfo[0] + "\n";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TMP_Text objectiveText;
    public TMP_Text promptText;
    public TMP_Text journalTitle;
    public TMP_Text journalBody;
    public GameObject journalPanel;
    public GameObject completionPanel;

    void Start()
    {
        if (journalPanel) journalPanel.SetActive(false);
        if (completionPanel) completionPanel.SetActive(false);
        UpdateObjectives(0, 3, false);
    }
    public void UpdateObjectives(int found, int total, bool unlocked)
    {
        if (objectiveText)
            objectiveText.text = "PATMOS  |  FRAGMENTS " + found + "/" + total +
                "\n" + (unlocked ? "Find the heavenly gateway" : "Discover the three Scripture fragments");
    }
    public void SetPrompt(string value) { if (promptText) promptText.text = value; }
    public void ShowJournal(string title, string verse, string body)
    {
        if (journalPanel) journalPanel.SetActive(true);
        if (journalTitle) journalTitle.text = title + "\n" + verse;
        if (journalBody) journalBody.text = body;
    }
    public void CloseJournal() { if (journalPanel) journalPanel.SetActive(false); }
    public void ShowNotice(string value) { if (promptText) promptText.text = value; }
    public void ShowChapterComplete() { if (completionPanel) completionPanel.SetActive(true); }
}

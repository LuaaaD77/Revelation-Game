using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int FragmentsFound { get; private set; }
    public const int TotalFragments = 3;
    public bool ChapterComplete { get; private set; }
    public HUDController HUD { get; private set; }

    const string SaveKey = "RLK_CH1_FRAGMENTS";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        HUD = FindObjectOfType<HUDController>();
        FragmentsFound = PlayerPrefs.GetInt(SaveKey, 0);
        FragmentsFound = Mathf.Clamp(FragmentsFound, 0, TotalFragments);
    }

    public void RegisterFragment(ScriptureFragment fragment)
    {
        if (fragment == null || fragment.Collected) return;
        fragment.MarkCollected();
        FragmentsFound = Mathf.Min(TotalFragments, FragmentsFound + 1);
        PlayerPrefs.SetInt(SaveKey, FragmentsFound);
        PlayerPrefs.Save();
        if (HUD == null) HUD = FindObjectOfType<HUDController>();
        if (HUD != null) HUD.ShowJournal(fragment.Title, fragment.Verse, fragment.JournalText);
        AudioManager.Instance?.PlayCollect();
        RefreshHUD();
    }

    public bool GatewayUnlocked => FragmentsFound >= TotalFragments;

    public void CompleteChapter()
    {
        if (!GatewayUnlocked || ChapterComplete) return;
        ChapterComplete = true;
        PlayerPrefs.SetInt("RLK_CH1_COMPLETE", 1);
        PlayerPrefs.Save();
        HUD?.ShowChapterComplete();
        AudioManager.Instance?.PlayChapterComplete();
    }

    public void RefreshHUD()
    {
        if (HUD == null) HUD = FindObjectOfType<HUDController>();
        HUD?.UpdateObjectives(FragmentsFound, TotalFragments, GatewayUnlocked);
    }

    public void ResetChapterProgress()
    {
        FragmentsFound = 0;
        ChapterComplete = false;
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.DeleteKey("RLK_CH1_COMPLETE");
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

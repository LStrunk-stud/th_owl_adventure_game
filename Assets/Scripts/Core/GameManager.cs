using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string PREFIX_ITEM   = "item_collected_";
    private const string PREFIX_SPOKEN = "npc_spoken_";
    private const string KEY_BACKPACK  = "backpack_unlocked";
    private const string KEY_HAS_SAVE  = "has_save";

    private bool _pendingReset = false;
    public  bool PendingReset  => _pendingReset;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Items ─────────────────────────────────────────────────────────────────

    public bool IsItemCollected(string itemID)
        => PlayerPrefs.GetInt(PREFIX_ITEM + itemID, 0) == 1;

    public void MarkItemCollected(string itemID)
    {
        PlayerPrefs.SetInt(PREFIX_ITEM + itemID, 1);
        PlayerPrefs.SetInt(KEY_HAS_SAVE, 1);
        PlayerPrefs.Save();
    }

    // ── Backpack ──────────────────────────────────────────────────────────────

    public bool IsBackpackUnlocked()
        => PlayerPrefs.GetInt(KEY_BACKPACK, 0) == 1;

    public void MarkBackpackUnlocked()
    {
        PlayerPrefs.SetInt(KEY_BACKPACK, 1);
        PlayerPrefs.SetInt(KEY_HAS_SAVE, 1); // backpack counts as progress
        PlayerPrefs.Save();
    }

    // ── NPC ───────────────────────────────────────────────────────────────────

    public bool HasSpokenTo(string npcID)
        => PlayerPrefs.GetInt(PREFIX_SPOKEN + npcID, 0) == 1;

    public void MarkSpokenTo(string npcID)
    {
        PlayerPrefs.SetInt(PREFIX_SPOKEN + npcID, 1);
        PlayerPrefs.SetInt(KEY_HAS_SAVE, 1); // talking counts as progress
        PlayerPrefs.Save();
    }

    // ── Save state ────────────────────────────────────────────────────────────

    public bool HasSaveData()
        => PlayerPrefs.GetInt(KEY_HAS_SAVE, 0) == 1;

    // ── New Game ──────────────────────────────────────────────────────────────

    public void StartNewGame(string firstSceneName = "Room_ApartmentBedroom")
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        _pendingReset = true;

        // Reset all persistent runtime state before loading
        InventoryManager.Instance.ResetInventory();

        Time.timeScale = 1f;
        SceneManager.LoadScene(firstSceneName);
    }

    public void ClearPendingReset() => _pendingReset = false;
}
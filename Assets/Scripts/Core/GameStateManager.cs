using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Central game state manager. Lives on PERSISTOBJECTS.
/// Tracks collected items and dialogue flags across scenes.
/// Uses PlayerPrefs for persistence across sessions.
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    // PlayerPrefs key prefix
    private const string PREFIX_ITEM    = "item_collected_";
    private const string PREFIX_SPOKEN  = "npc_spoken_";
    private const string KEY_BACKPACK   = "backpack_unlocked";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Items ─────────────────────────────────────────────────────────────────

    /// Returns true if this item has already been picked up in a previous session.
    public bool IsItemCollected(string itemID)
    {
        return PlayerPrefs.GetInt(PREFIX_ITEM + itemID, 0) == 1;
    }

    /// Mark item as collected and save immediately.
    public void MarkItemCollected(string itemID)
    {
        PlayerPrefs.SetInt(PREFIX_ITEM + itemID, 1);
        PlayerPrefs.Save();
    }

    // ── Backpack ──────────────────────────────────────────────────────────────

    public bool IsBackpackUnlocked()
    {
        return PlayerPrefs.GetInt(KEY_BACKPACK, 0) == 1;
    }

    public void MarkBackpackUnlocked()
    {
        PlayerPrefs.SetInt(KEY_BACKPACK, 1);
        PlayerPrefs.Save();
    }

    // ── NPC spoken state ──────────────────────────────────────────────────────

    /// Returns true if the player has spoken to this NPC before.
    /// npcID should be unique per NPC, e.g. "Randy_Bedroom".
    public bool HasSpokenTo(string npcID)
    {
        return PlayerPrefs.GetInt(PREFIX_SPOKEN + npcID, 0) == 1;
    }

    public void MarkSpokenTo(string npcID)
    {
        PlayerPrefs.SetInt(PREFIX_SPOKEN + npcID, 1);
        PlayerPrefs.Save();
    }

    // ── New Game / Reset ──────────────────────────────────────────────────────

    /// Wipes all game state. Call from main menu "New Game" button.
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[GameManager] Game state reset.");
    }

    /// Reset and reload the first gameplay scene.
    public void StartNewGame(string firstSceneName = "Room_ApartmentBedroom")
    {
        ResetGame();

        // Also reset runtime managers
        InventoryManager.Instance.ResetInventory();

        SceneManager.LoadScene(firstSceneName);
    }
}
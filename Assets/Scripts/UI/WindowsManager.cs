using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager instance;
    private Player player;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject spellsPanel;
    [SerializeField] private TextMeshProUGUI spellPointsToAssign;
    [SerializeField] private TextMeshProUGUI statPointsToAssign;
    [SerializeField] private List<TextMeshProUGUI> stats;

    private Dictionary<TextMeshProUGUI, string> spellsLevelBeforeApply = new Dictionary<TextMeshProUGUI, string>();
    private Dictionary<TextMeshProUGUI, string> maxSpellsLevel = new Dictionary<TextMeshProUGUI, string>();
    private Dictionary<TextMeshProUGUI, string> statsBeforeApply = new Dictionary<TextMeshProUGUI, string>();

    private int spellPointsIncreased = 0;  
    private int statPointsIncreased = 0; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        if (player != null)
        {
            player.OnStatsPressed += ToggleStatsPanel;
            player.OnSpellUIPressed += ToggleSpellsPanel;
        }
        Experience.Instance.OnLevelUp += AssignPoints;
    }

    private void AssignPoints()
    {
        spellPointsIncreased += 2;
        statPointsIncreased += 10;
    }

    public void ToggleStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        if (statsPanel.activeSelf)
            Cancel();
    }

    public void ToggleSpellsPanel()
    {
        spellsPanel.SetActive(!spellsPanel.activeSelf);
        if (spellsPanel.activeSelf)
            Cancel();
    }

    private void ApplyChanges(Dictionary<TextMeshProUGUI, string> beforeApply)
    {
        foreach (var item in beforeApply)
        {
            item.Key.text = item.Key.text;
            
            if(maxSpellsLevel.ContainsKey(item.Key) && int.Parse(maxSpellsLevel[item.Key]) < int.Parse(item.Key.text))
                maxSpellsLevel[item.Key] = item.Key.text;
            else if (!maxSpellsLevel.ContainsKey(item.Key))
                maxSpellsLevel.Add(item.Key, item.Key.text);
        }
    }


    private void CancelStatChanges()
    {
        foreach (var item in statsBeforeApply)
            item.Key.text = item.Value;  
        statsBeforeApply.Clear();  
    }

    private void CancelSpellChanges()
    {
        foreach (var item in spellsLevelBeforeApply)
        {
            if (maxSpellsLevel.ContainsKey(item.Key))
                item.Key.text = maxSpellsLevel[item.Key];
            else
                item.Key.text = item.Value;
        }
        spellsLevelBeforeApply.Clear();
    }

    public void ApplyStatsFromUI()
    {
        ApplyPlayerStats();
        statsBeforeApply.Clear();  
        statPointsIncreased = 0; 
    }

    private void ApplyPlayerStats()
    {
        player.stats.strength = int.Parse(stats[0].text);
        player.stats.dexterity = int.Parse(stats[1].text);
        player.stats.agility = int.Parse(stats[2].text);
        player.stats.intelligence = int.Parse(stats[3].text);
        player.stats.vitality = int.Parse(stats[4].text);
        player.stats.luck = int.Parse(stats[5].text);
    }

    public void Apply()
    {
        ApplyChanges(spellsLevelBeforeApply);
        ApplyStatsFromUI();

        spellsLevelBeforeApply.Clear();
        statsBeforeApply.Clear();

        spellPointsIncreased = 0;
        statPointsIncreased = 0;
    }

    public void Cancel()
    {
        
        CancelSpellChanges();
        CancelStatChanges();

        spellPointsToAssign.text = (int.Parse(spellPointsToAssign.text) + spellPointsIncreased).ToString();
        statPointsToAssign.text = (int.Parse(statPointsToAssign.text) + statPointsIncreased).ToString();

        spellPointsIncreased = 0;
        statPointsIncreased = 0;
    }

    public void SpellIncrease(TextMeshProUGUI spellChanged)
    {
        if (int.Parse(spellChanged.text) == 10 || int.Parse(spellPointsToAssign.text) <= 0) return;

        TrackAndModifyStat(spellsLevelBeforeApply, spellChanged, 1);
        spellPointsIncreased++; 
    }

    public void SpellDecrease(TextMeshProUGUI spellChanged)
    {
        if (spellChanged.text == "0" || (maxSpellsLevel.ContainsKey(spellChanged) && int.Parse(spellChanged.text) > int.Parse(maxSpellsLevel[spellChanged]))) return;
        
        TrackAndModifyStat(spellsLevelBeforeApply, spellChanged, -1);

        spellPointsIncreased--;
    }

    public void StatIncrease(TextMeshProUGUI statChanged)
    {
        if (statPointsToAssign.text == "0") return; 

        TrackAndModifyStat(statsBeforeApply, statChanged, 1);
        statPointsIncreased++;
    }

    public void StatDecrease(TextMeshProUGUI statChanged)
    {
        if (!statsBeforeApply.ContainsKey(statChanged) || int.Parse(statChanged.text) <= int.Parse(statsBeforeApply[statChanged])) return;

        TrackAndModifyStat(statsBeforeApply, statChanged, -1);
        statPointsIncreased--; 
    }

    private void TrackAndModifyStat(Dictionary<TextMeshProUGUI, string> tracker, TextMeshProUGUI stat, int amount)
    {
        int currentValue = int.Parse(stat.text);
        int newValue = currentValue + amount;

        if (amount < 0 && (tracker == statsBeforeApply && (!tracker.ContainsKey(stat) || newValue < int.Parse(tracker[stat])))) return;

        stat.text = newValue.ToString();

        if (amount > 0 && !tracker.ContainsKey(stat))
            tracker[stat] = currentValue.ToString();

        if (tracker == spellsLevelBeforeApply && amount > 0 && ((maxSpellsLevel.ContainsKey(stat) && int.Parse(maxSpellsLevel[stat]) < newValue) || !maxSpellsLevel.ContainsKey(stat)))
            spellPointsToAssign.text = (int.Parse(spellPointsToAssign.text) - amount).ToString();
        else if (tracker == statsBeforeApply)
            statPointsToAssign.text = (int.Parse(statPointsToAssign.text) - amount).ToString();
    }

    public bool PlayerHasAppliedChanges()
    {
        bool hasApplied = true;

        foreach(var item in spellsLevelBeforeApply)
        {
            if(maxSpellsLevel.ContainsKey(item.Key) && int.Parse(maxSpellsLevel[item.Key]) < int.Parse(item.Key.text))
                hasApplied = false;
        }

        return hasApplied;
    }
}

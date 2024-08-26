using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private List<TextMeshProUGUI> stats;

    private void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.onStatsPressed.AddListener(ShowStatsPanel);
    }

    private void ShowStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);

        if (statsPanel.activeSelf)
        {
            stats[0].text = player.stats.strength.ToString();
            stats[1].text = player.stats.dexterity.ToString();
            stats[2].text = player.stats.agility.ToString();
            stats[3].text = player.stats.intelligence.ToString();
            stats[4].text = player.stats.vitality.ToString();
            stats[5].text = player.stats.luck.ToString();
        }
        else
        {
            player.stats.strength = Int32.Parse(stats[0].text);
            player.stats.dexterity = Int32.Parse(stats[1].text);
            player.stats.agility = Int32.Parse(stats[2].text);
            player.stats.intelligence = Int32.Parse(stats[3].text);
            player.stats.vitality = Int32.Parse(stats[4].text);
            player.stats.luck = Int32.Parse(stats[5].text);
        }
    }
}

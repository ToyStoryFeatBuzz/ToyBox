using Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    public GameObject panelMatchUI;

    [Header("UI Match")]
    public List<GameObject> fillBars;
    public List<TextMeshProUGUI> textSlots;

    public Dictionary<string, int> _dictPlayer;

    private void Start()
    {
        HideLeaderBoard();
        _dictPlayer = new();
        CheckPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowLeaderBoard();
        }
    }

    public void ShowLeaderBoard()
    {
        UpdateLeaderBoard();
        panelMatchUI.SetActive(true);
    }

    public void HideLeaderBoard()
    {
        panelMatchUI.SetActive(false);
    }

    public void UpdateLeaderBoard()
    {
        CheckPlayer();

        var sortedList = GetSortedPlayers();

        for (int i = 0; i < fillBars.Count && i < textSlots.Count; i++)
        {
            Transform parent = fillBars[i].transform.parent;

            if (i < sortedList.Count)
            {
                var (name, score) = sortedList[i];
                Debug.Log($"AFTER SORTING Nom : {name} Points : {score}");

                // Activer le parent (slot complet)
                parent.gameObject.SetActive(true);
                fillBars[i].GetComponent<Image>().fillAmount = score / 100f;
                textSlots[i].text = name;
            }
            else
            {
                parent.gameObject.SetActive(false);
            }
        }
    }

    public void CheckPlayer()
    {
        if (playerManager.Players.Count < 1)
        {
            Debug.Log("Pas de joueur");
            return;
        }

        foreach (StPlayer player in playerManager.Players)
        {
            if (!_dictPlayer.ContainsKey(player.Name))
            {
                _dictPlayer.Add(player.Name, Random.Range(1, 100));
                SimulateMatchScores(player);  // Simule des scores de match
            }
        }
    }

    public void SimulateMatchScores(StPlayer player)
    {
        // Générer des scores de match aléatoires pour ce joueur
        List<int> matchScores = new List<int>();
        int numMatches = Random.Range(1, 10); // Nombre de matchs aléatoires (par exemple entre 5 et 15 matchs)

        for (int i = 0; i < numMatches; i++)
        {
            matchScores.Add(Random.Range(1, 10)); // Scores de matchs entre 10 et 50
        }

        player.PlayerStats.MatchScores = matchScores;
    }

    public List<(string name, int score)> GetSortedPlayers()
    {
        var list = new List<(string name, int score)>();

        foreach (var player in playerManager.Players)
        {
            if (_dictPlayer.ContainsKey(player.Name))
            {
                list.Add((player.Name, _dictPlayer[player.Name]));
            }
        }

        return list.OrderByDescending(x => x.score).ToList();
    }
}

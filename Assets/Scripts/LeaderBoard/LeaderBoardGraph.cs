using Managers;
using System.Collections.Generic;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using System.Linq;

public class LeaderBoardGraph : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LeaderBoard leaderBoard;
    [SerializeField] private PlayerManager playerManager;
    
    [Header("UI Elements")]
    public GameObject panelEndGameUI;
    public List<TextMeshProUGUI> textName;
    public List<TextMeshProUGUI> textPoints;
    public List<LineRenderer> lineRenderers;
    
    [Header("Graph Settings")]
    public List<Color> playerColors = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        new Color(1, 0, 1),    // rose
        new Color(0.5f, 0, 0.5f), // violet
        new Color(1, 0.5f, 0)    // orange
    };

    [Header("Positioning")]
    [SerializeField] private Vector3 graphOrigin = new Vector3(-5f, -2f, 0f);
    [SerializeField] private float lineWidth = 0.15f;

    [Header("Graph Limits")]
    [SerializeField] private float maxGraphWidth = 10f;
    [SerializeField] private float maxGraphHeight = 5f;
    [SerializeField] private float xMargin = 1f;
    [SerializeField] private float yMargin = 0.5f;

    private void Start()
    {
        panelEndGameUI.SetActive(false);
        InitializeLineRenderers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateLeaderBoard();
        }
    }

    private void InitializeLineRenderers()
    {
        foreach (var lr in lineRenderers)
        {
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 0;
        }
    }

    public void UpdateLeaderBoard()
    {
        panelEndGameUI.SetActive(true);
        var sortedPlayers = leaderBoard.GetSortedPlayers();

        // Update text displays
        for (int i = 0; i < textPoints.Count && i < textName.Count; i++)
        {
            Transform parent = textPoints[i].transform.parent;
            parent.gameObject.SetActive(i < sortedPlayers.Count);
            
            if (i < sortedPlayers.Count)
            {
                var (name, score) = sortedPlayers[i];
                textPoints[i].text = score.ToString();
                textName[i].text = name;
            }
        }

        // Calculate maximum values
        int maxScore = Mathf.Max(1, sortedPlayers.Max(p => p.Item2)); // Au moins 1 pour éviter la division par 0
        int maxMatches = playerManager.Players.Max(p => p.PlayerStats?.MatchScores.Count ?? 0);

        // Draw score lines
        for (int i = 0; i < sortedPlayers.Count && i < lineRenderers.Count; i++)
        {
            DrawPlayerLine(i, sortedPlayers[i].Item1, maxScore, maxMatches);
        }
    }
    
private void DrawPlayerLine(int playerIndex, string playerName, int maxScore, int maxMatches)
{
    var player = playerManager.Players.FirstOrDefault(p => p.Name == playerName);
    if (player.Name == null || string.IsNullOrEmpty(player.Name)) return;

    var stats = player.PlayerStats;
    if (stats == null || stats.MatchScores.Count == 0) return;

    List<int> matchScores = stats.MatchScores;

    // Pad with last score if needed
    while (matchScores.Count < maxMatches)
    {
        matchScores.Add(matchScores.Count > 0 ? matchScores.Last() : 0);
    }

    LineRenderer lr = lineRenderers[playerIndex];
    lr.positionCount = matchScores.Count + 1; // +1 pour le point de départ

    // Set line color
    Color playerColor = playerColors[playerIndex % playerColors.Count];
    lr.startColor = lr.endColor = playerColor;

    Vector3[] positions = new Vector3[matchScores.Count + 1];
    
    // Calculate spacing and scaling with limits
    float availableWidth = maxGraphWidth - 2 * xMargin;
    float availableHeight = maxGraphHeight - 2 * yMargin;
    
    // Calcul du pas en fonction du nombre de points (moins 1 car on compte le point de départ)
    float xStep = matchScores.Count > 0 ? availableWidth / matchScores.Count : 0;
    float yScaleFactor = availableHeight / maxScore;

    // Point de départ (index 0) - toujours à Y=0 + marge
    positions[0] = graphOrigin + new Vector3(xMargin, yMargin, 0);
    
    // Points des scores (commencent à index 1)
    int cumulativeScore = 0;
    for (int i = 0; i < matchScores.Count; i++)
    {
        cumulativeScore += matchScores[i];
        float xPos = xMargin + ((i + 1) * xStep); // +1 pour décaler après le point de départ
        float yPos = yMargin + (cumulativeScore * yScaleFactor);
        
        // Clamp positions within graph bounds
        xPos = Mathf.Clamp(xPos, xMargin, maxGraphWidth - xMargin);
        yPos = Mathf.Clamp(yPos, yMargin, maxGraphHeight - yMargin);
        
        positions[i + 1] = graphOrigin + new Vector3(xPos, yPos, 0);
    }
    
    lr.SetPositions(positions);
    
    // Debug log
    Debug.Log($"Line for {playerName} (maxScore: {maxScore}, matches: {matchScores.Count})");
    Debug.Log($"Dimensions: {availableWidth}w x {availableHeight}h, XStep: {xStep}");
    for (int i = 0; i < positions.Length; i++)
    {
        Debug.Log($"Index {i}: X={positions[i].x.ToString("F1")}, Y={positions[i].y.ToString("F1")}");
    }
}
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoteManager : MonoBehaviour
{
    public VoteZone[] VoteZones;

    //test temporaire
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TallyVotes();
        }
    }
    
    public void TallyVotes()
    {
        VoteZone winner = null;
        int highestVote = 0;

        foreach (var zone in VoteZones)
        {
            int count = zone.GetVoteCount();
            Debug.Log($"{zone.MapName} : {count} votes");

            if (count > highestVote)
            {
                highestVote = count;
                winner = zone;
            }
        }

        if (winner != null)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(winner.MapName);
            Debug.Log($"Carte sélectionnée : {winner.MapName}");
            // Launch the winning scene or card here
        }
    }
}
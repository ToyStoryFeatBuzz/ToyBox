using UnityEngine;
using UnityEngine.Events;

public class Map : MonoBehaviour
{
    public string sceneName = "";

    int playerOnMap = 0;

    public UnityEvent playerOnMapEvent = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerOnMap++;
            playerOnMapEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerOnMap--;
            playerOnMapEvent.Invoke();
        }
    }

    public int GetPlayersOn() => playerOnMap;
}

using TMPro;
using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;

        GameModeManager _gameModeManager;

        float _time;
        bool _running;

        void Start()
        {
            _gameModeManager = GameModeManager.Instance;
            if (!PlayerPrefs.HasKey("BestTime"))
            {
                PlayerPrefs.SetFloat("BestTime", float.MaxValue);
            }

            _gameModeManager.OnRaceStartExtern += StartTimer;
            _gameModeManager.OnRaceEndExtern += StopTimer;
        }

        void StartTimer()
        {
            _time = 0.0f;
            _running = true;
        }

        void StopTimer()
        {
            _running = false;
            
            if (PlayerPrefs.GetFloat("BestTime") > _time)
            {
                PlayerPrefs.SetFloat("BestTime", _time);
            }
            _time = 0.0f;
        }

        void Update()
        {
            if (_running)
            {
                _time += Time.deltaTime;
                _timerText.text = _time.ToString("00:00<style=\"Smaller\">.00</style>");
            }
        }


    }
}
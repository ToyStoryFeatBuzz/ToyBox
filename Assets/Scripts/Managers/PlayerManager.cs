using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using ToyBox.Player;
using ToyBox.Build;
using ToyBox.InputSystem;
using UnityEngine.SceneManagement;
using static ToyBox.Enums;

namespace ToyBox.Managers {
    public class PlayerManager : MonoBehaviour {
        public List<Player> Players = new();

        public static PlayerManager Instance;
        
        public PlayerInputManager PlayerInputManager;
        public List<StLayerColor> AnimationClips;
        private List<RuntimeAnimatorController> Animators = new();
        
        [SerializeField] Transform _spawnPoint;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }
        
        private void Start() {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;
            PlayerInputManager = GetComponent<PlayerInputManager>();
            SetNewPlayersEntries(true);
        }

        public void SetNewPlayersEntries(bool possibility)
        {
            PlayerInputManager.enabled = possibility;
        }

        public void SetPlayersMovements(bool activation) {
            foreach (Player player in Players) {
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = !activation;
            }
        }

        public void SetAnimInIddle(bool activation)
        {
         
            foreach (Player player in Players )
            {
                player.PlayerObject.GetComponentInChildren<Animator>().enabled = !activation;
            }
        }
        
        public void ClampScoreToMax(int score)
        {
            foreach (Player player in Players)
            {
                if (player.PlayerStats.Score > score)
                {
                    player.PlayerStats.SetScore(score);
                }
            }
        }

        public bool DoesAllPlayersFinishedBuilding()
        {
            return Players.All(player => !player.PlayerObject.GetComponent<PlayerEdition>().IsPlacing());
        }


        public void AddPlayer(PlayerInput player) {
            if(Players.FirstOrDefault(_player => _player.PlayerInput == player) != null){
                return;
            }
            Debug.Log("Player added");
            AudioManager.Instance.PlaySFX("PlayerJoin");
            string name = "Player " + (Players.Count + 1);
            player.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
            InputDevice device = player.user.pairedDevices.First();
            player.gameObject.name = name;
            
            PlayerStats playerStats = player.gameObject.GetComponent<PlayerStats>();
            playerStats.color = AnimationClips[Players.Count].color;
            playerStats.sprite = AnimationClips[Players.Count].sprite;
            
            ReadyUpHandler readyUpHandler = player.gameObject.GetComponent<ReadyUpHandler>();
            
            Animator animator = player.gameObject.GetComponentInChildren<Animator>();
            if (animator != null && AnimationClips.Count >= Players.Count + 1) {
                animator.runtimeAnimatorController = AnimationClips[Players.Count].animator;
            }
            
            Players.Add(new Player
                { Name = name, PlayerInput = player, PlayerObject = player.gameObject, Device = device, PlayerStats = playerStats, PlayerState = EPlayerState.Alive , Color = AnimationClips[Players.Count].color, sprite = AnimationClips[Players.Count].sprite ,ReadyUpHandler = readyUpHandler});

            player.transform.parent = transform;
            player.transform.position = GetSpawnPoint();
        }
        

        public Vector3 GetSpawnPoint()=> _spawnPoint.position;
        
        public void ResetAllPlayerPositions() {
            foreach (Player player in Players) {
                player.PlayerObject.transform.position = GetSpawnPoint();
            }
        }
        
        void OnDeviceChange(InputDevice device, InputDeviceChange change) {
            if (change != InputDeviceChange.Disconnected) return;
            foreach (Player player in Players.Where(_player => _player.Device == device)) {
                RemovePlayer();
                break;
            }
        }
        void RemovePlayer() {
            SceneManager.LoadScene(0);
            DestroyImmediate(ManagerHolder.Instance.gameObject);
        }

        public Player GetPlayer(GameObject playerGameObject) {
            return Players.FirstOrDefault(_player => _player.PlayerObject == playerGameObject);
        }


        
        public void SetPlayerState(GameObject player, EPlayerState state) {
            SetPlayerState(GetPlayer(player), state);
        }

        public void SetPlayerState(Player player, EPlayerState state) {
            player.PlayerState = state;
        }


        public List<Player> GetAlivePlayers() {
            return Players.Where(_player => _player.PlayerState == EPlayerState.Alive).ToList();
        }

        public int GetBestScore() {
            return Players.Max(player => player.PlayerStats.Score);
        }
        
    }

    [Serializable]
    public class Player {
        public string Name;
        public PlayerInput PlayerInput;
        public GameObject PlayerObject;
        public InputDevice Device;
        public PlayerStats PlayerStats;
        public EPlayerState PlayerState;
        public ReadyUpHandler ReadyUpHandler;
        public Color Color;
        public Sprite sprite;
    }

    [Serializable]
    public struct StLayerColor
    {
        public RuntimeAnimatorController animator;
        public Sprite sprite;
        public Color color;
        public Sprite spriteIdlle;
        public Sprite spriteClic;
    }
}
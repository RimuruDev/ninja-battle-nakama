using UnityEngine;
using UnityEngine.SceneManagement;

using Nakama.Helpers;

using NinjaBattle.General;

namespace NinjaBattle.Game
{
    public class GameManager : MonoBehaviour
    {
        #region FIELDS

        public const int VictoriesRequiredToWin = 3;

        #endregion

        #region PROPERTIES

        public static GameManager Instance { get; private set; } = null;
        public int[] PlayersWins { get; private set; } = new int[4];
        public int? Winner { get; private set; } = 0;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MultiplayerManager.Instance.Subscribe(MultiplayerManager.Code.PlayerWon, ReceivedPlayerWonRound);
            MultiplayerManager.Instance.Subscribe(MultiplayerManager.Code.Draw, ReceivedDrawRound);
            MultiplayerManager.Instance.Subscribe(MultiplayerManager.Code.ChangeScene, ReceivedChangeScene);
            MultiplayerManager.Instance.onMatchJoin += ResetPlayerWins;
            MultiplayerManager.Instance.onMatchLeave += GoToHome;
        }

        private void OnDestroy()
        {
            MultiplayerManager.Instance.Unsubscribe(MultiplayerManager.Code.PlayerWon, ReceivedPlayerWonRound);
            MultiplayerManager.Instance.Unsubscribe(MultiplayerManager.Code.Draw, ReceivedDrawRound);
            MultiplayerManager.Instance.Unsubscribe(MultiplayerManager.Code.PlayerInput, ReceivedChangeScene);
            MultiplayerManager.Instance.onMatchJoin -= ResetPlayerWins;
            MultiplayerManager.Instance.onMatchLeave -= GoToHome;
        }

        private void ReceivedPlayerWonRound(MultiplayerMessage message)
        {
            PlayerWonData playerWonData = message.GetData<PlayerWonData>();
            PlayersWins[playerWonData.PlayerNumber]++;
            Winner = playerWonData.PlayerNumber;
        }

        private void ReceivedDrawRound(MultiplayerMessage message)
        {
            Winner = null;
        }

        private void ReceivedChangeScene(MultiplayerMessage message)
        {
            SceneManager.LoadScene(message.GetData<int>());
        }

        private void ResetPlayerWins()
        {
            PlayersWins = new int[4];
        }

        private void GoToHome()
        {
            SceneManager.LoadScene((int)Scenes.Home);
        }

        #endregion
    }
}

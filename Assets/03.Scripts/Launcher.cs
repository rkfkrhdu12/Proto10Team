using UnityEngine;
using System.Collections;

using Photon.Pun;
using Photon.Realtime;

namespace NetWork
{
    // 네트워크 상태를 알려줄 enum
    public enum eNetWorkState
    {
        /// <summary> 연결되지않음. </summary>
        Disconnect,
        /// <summary> 연결중. </summary>
        Connecting,
        /// <summary> 연결됨. </summary>
        Connect,
        /// <summary> 불안정. </summary>
        Unstable,
    }

    public class Launcher : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// 서버를 연결시킵니다.
        /// </summary>
        IEnumerator Connect()
        {
            // 현재 연결이 되는중이거나 되어잇는 상태면 return
            if (_curState == eNetWorkState.Disconnect)
            {
                // 현재 상태를 연결중 으로 변경
                UpdateState(eNetWorkState.Connecting);

                LogManager.Log("Start");

                string playerName = PlayerPrefs.GetString(_playerNamePrefkey);

                if (string.IsNullOrWhiteSpace(playerName))
                { // 유저의 이름을 불러오는데 실패

                    // 유저 이름 입력 Sequence
                    _createPlayerNameUI.gameObject.SetActive(true);
                    _createPlayerNameUI.OnStart(_playerNamePrefkey);

                    Debug.Log("MakeName");

                    while (_createPlayerNameUI.CurState != CreatePlayerName.eCreateNameState.Check) yield return _checkNameWaitTime;

                    playerName = PlayerPrefs.GetString(_playerNamePrefkey);
                }

                // 유저의 이름을 제대로 가져왔을때 || 이미 저장된 이름이 있을때
                GameManager.Instance.PlayerName = playerName;

                // Photon Cloud 에 연결 시작
                PhotonNetwork.ConnectUsingSettings();
            }

            yield return null;
        }

        #region Variable

        // 게임버젼을 나타내는 변수
        private readonly string _gameVersion = "0.1";

        // 플레이어의 이름을 저장할 PlayerPref에 이름을 저장할 변수
        private readonly string _playerNamePrefkey = "PlayerName";

        // 현재 네트워크 상태를 알려줄 변수
        [SerializeField]
        private eNetWorkState _curState = eNetWorkState.Unstable;
        // 현재 네트워크 상태를 다른 스크립트에서도 알수 잇게 해줄 변수
        public eNetWorkState CurState { get { return _curState; } }

        // 이름을 만들어야할때 사용될 Object,Script의 변수
        public CreatePlayerName _createPlayerNameUI;
        
        // 이름을 만들때 waiting을 할 시간 변수
        WaitForSeconds _checkNameWaitTime = new WaitForSeconds(1f);

        // 현재 연결상태를 직접 보여줄 UIText
        public TMPro.TMP_Text _stateText;

        // 네트워크를 관리해줄 오브젝트
        public GameObject _netMgrPrefab;
        #endregion

        // Awake Update
        #region Monobehaviour Function

        private void Start()
        {
            Time.maximumDeltaTime = 1f / 3f;
            Time.fixedDeltaTime = 1f / 60f;
            Application.targetFrameRate = 60;
            useGUILayout = false;
            Screen.SetResolution(960, 540, false);

            // 게임 버젼을 PhotonNetwork 에 등록
            PhotonNetwork.GameVersion = _gameVersion;
            // 마스터 클라이언트와 일반 클라이언트의 씬을 동기화한다. 
            PhotonNetwork.AutomaticallySyncScene = true;

            UpdateState(eNetWorkState.Disconnect);
        }

        public void Update()
        {
            // 접속시작
            if (!PhotonNetwork.IsConnected && _curState == eNetWorkState.Disconnect)
            {
                if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(Connect());
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerPrefs.DeleteKey(_playerNamePrefkey);
            }
        }

        #endregion

        // UpdateState
        #region Private Function

        void UpdateState(eNetWorkState state)
        {
            _curState = state;

            switch (_curState)
            {
                case eNetWorkState.Disconnect:
                    _stateText.text = "Disconnect";
                    LogManager.Log("Server Disconnected");
                    break;
                case eNetWorkState.Connecting:
                    _stateText.text = "Connecting";
                    break;
                case eNetWorkState.Connect:
                    _stateText.text = "Connect";
                    LogManager.Log("Server Connected");
                    break;
                case eNetWorkState.Unstable:
                    break;
            }

        }

        #endregion

        #region Photon Callback Function
        public override void OnConnectedToMaster()
        {
            UpdateState(eNetWorkState.Connect);

            Instantiate(_netMgrPrefab);

            PhotonNetwork.LocalPlayer.NickName = GameManager.Instance.PlayerName;
            PhotonNetwork.LoadLevel(1);
        }
        #endregion
    }
}
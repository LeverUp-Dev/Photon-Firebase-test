using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1.0";

    public Text connectionInfoText;
    public Button joinButton;
    
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings(); //마스터 서버에 접속 시도

        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속중...";
    }
    
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : 마스터 서버에 접속 완료됨";
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : 마스터 서버에 접속 종료됨. 사유 : {cause.ToString()}" +
            "-재접속 중...";

        PhotonNetwork.ConnectUsingSettings(); //재접속 시도
    }
    
    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "아무 방이나 연결중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = $"Offline : 마스터 서버에 접속 종료됨 +" +
                "재접속 중...";

            PhotonNetwork.ConnectUsingSettings(); //재접속 시도
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "방을 찾을 수 없습니다. 방을 생성합니다.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "찾은 방에 접속되었습니다,";

        //동기화를 위해 LoadLevel 메서드를 사용, SceneManager를 사용 시 동기화 X
        //(즉, 같은 방에 접속해도 서로를 볼 수 없는 상태가 됨)
        PhotonNetwork.LoadLevel("Main");
    }
}
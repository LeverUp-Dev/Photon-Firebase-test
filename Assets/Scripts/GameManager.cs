using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public Text scoreText;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    private int[] playerScores;

    private void Start()
    {
        playerScores = new int[] { 0, 0 };
        SpawnPlayer();

        // 게임 시작 시 각자 실행할 매서드와 공통적으로 한번만 실행할 메서드를 PhotonNetwork.IsMasterClient를 통해 구분 지어야 한다.
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBall();
        }
    }

    private void SpawnPlayer()
    {
        //PhotonNetwork.LocalPlayer.ActorNumber는 포톤네트워크에서 플레이어에게 자동으로 부여하는 int 변수이다.(방장 기준 0부터 카운트 된다.)
        int localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
    }

    private void SpawnBall()
    {
        PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void AddScore(int playerNumber, int score)
    {
        // playerScore처럼 게임 진행에 영향을 끼치는 중요한 요소 방장만 관리할 수 있도록 조건문을 걸어 주어야 함
        if (!PhotonNetwork.IsMasterClient) return;

        playerScores[playerNumber - 1] += score;

        // RPC 함수를 인수로 전달한 내용 모두에게 전달해 준다(스코어는 방장만 변경할 수 있기에 RPC함수를 통해 모두에게 점수를 갱신해 주어야 한다.)
        photonView.RPC("RPCUpdateScoreText", RpcTarget.All,
                                    playerScores[0].ToString(), playerScores[1].ToString());
    }

    
    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    }
}
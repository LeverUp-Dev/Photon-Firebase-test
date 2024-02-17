using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun
{
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private Vector2 direction = Vector2.right;
    private readonly float speed = 10f;
    private readonly float randomRefectionIntensity = 0.1f;
    
    private void FixedUpdate()
    {
        if (!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        float distance = speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

        if (hit.collider != null)
        {
            Goalpost goalPost = hit.collider.GetComponent<Goalpost>();

            if (goalPost != null)
            {
                GameManager.Instance.AddScore(goalPost.playerNumber == 2 ? 1 : 2, 1);
            }

            // 공이 부딪힌 반대 각도로 튕겨나가는 방향을 구하기
            direction = Vector2.Reflect(direction, hit.normal);
            // 콘텐츠의 재미를 위해 변칙 값 추가
            direction += Random.insideUnitCircle * randomRefectionIntensity;
        }

        transform.position = (Vector2) transform.position + direction * distance;
    }
}
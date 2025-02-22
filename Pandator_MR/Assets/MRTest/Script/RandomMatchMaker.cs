using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RandomMatchMaker : MonoBehaviourPunCallbacks
{
    public GameObject PhotonObject;
    public GameObject PhotonFailureObject;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 固定ルーム "Pandator" に参加
        PhotonNetwork.JoinRoom("Pandator");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ルーム参加に失敗した場合はルームを新規作成（最大8人まで）
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.CreateRoom("Pandator", roomOptions);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonObject == null)
        {
            Debug.LogError("PhotonObject is not set in the inspector.");
            return;
        }

        // ルームに入室できたら、PhotonObjectを生成する
        PhotonNetwork.Instantiate(PhotonObject.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            // カメラのスクリプトを有効にするなどの処理をここに記述可能
            // mainCamera.GetComponent<UnityChan.ThirdPersonCamera>().enabled = true;
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError("Disconnected from Photon: " + cause.ToString());

        // 接続に失敗した場合、PhotonFailureObject を表示する
        if (PhotonFailureObject != null)
        {
            // ローカルにオブジェクトを Instantiate する例（PhotonNetwork.Instantiate は使用できないため）
            Instantiate(PhotonFailureObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
        else
        {
            Debug.LogError("PhotonFailureObject is not set in the inspector.");
        }
    }
}
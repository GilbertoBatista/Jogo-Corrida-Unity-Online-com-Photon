using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    int nomeSala = 1;

    private void Awake()
    {
        // Atribui um NickName aleatorio e conecta com o Photon.
        PhotonNetwork.LocalPlayer.NickName = "UserName" + Random.Range(1, 1000);
        PhotonNetwork.ConnectUsingSettings();
    }

    //Executa ao conectar no Photon.
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(); // Conecta no Lobby.
    }

    // Executa ao entrar no lobby.
    public override void OnJoinedLobby()
    {
        RoomOptions options = new RoomOptions();
        options.CleanupCacheOnLeave = false;
        options.MaxPlayers = 2; // numero maximo de player na sala
        options.PlayerTtl = -1; // milisegundos (controla o tempo em que o jogador fica online depois de desconectar, -1 para ficar online até a sala fechar)
        //options.EmptyRoomTtl = 300000; // milisegundos (controla o tempo em que a sala fica online depois que todo mundo sai, o maximo é de 5 minutos)
        PhotonNetwork.JoinOrCreateRoom(nomeSala.ToString(), options, null); // Entra ou cria uma sala de jogo.
    }

    // Executada quando falha ao entrar na sala.
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Erro ao entrar na sala - Cod.: " + returnCode + " Mensagem: " + message);
        nomeSala++; // incrementa o nome da sala para criar salas em sequencia numerica 1, 2, 3, ...
        OnJoinedLobby(); // Retorna ao Lobby e tenta criar a sala com o novo nome.
    }

    // Executada ao entar na sala.
    public override void OnJoinedRoom()
    {
        // Organiza os jogadores em posição na pista.
        Vector3 pos = new Vector3(604.73f, 0.8f, 805.1519f);

        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1) pos.x = 539.2741f;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) pos.x = 606.73f;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 3) pos.x = 602.73f;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4) pos.x = 608.73f;

        // Instancia o prefab do carro.
        PhotonNetwork.Instantiate("free_car", pos, Quaternion.Euler(0f, 0f, 0f));
    }

    // Detecta quando um novo jogador vora Rost ou quando o Rost sai.
    public override void OnMasterClientSwitched(Player newMasterClient) // Trata situações quando o Host sai da Sala.
    {
        /*
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber) // Se o ActorNumber (código no player) for igual ao códido do novo Master Cliente.
        {
        
        }
        */
    }

    // Executada quando o player sai da sala.
    public override void OnLeftRoom()
    {
        print("Você saiu da Sala!!!!");
    }

    // Retorna a causa da perda de coneção.
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("Você foi desconectado!!!! : " + cause);
    }
}

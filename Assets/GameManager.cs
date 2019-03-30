using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform player2;
    [SerializeField]
    Transform player1;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Text opponentLives;
    [SerializeField]
    Text myLives;

    bool gameInProgress;
    bool gameOver=false;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.OfflineMode = true;
            StartGame();
        }
        if (PhotonNetwork.PlayerList.Length > 2)
            return;

        if (!PhotonNetwork.IsMasterClient)
        {
            mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
            PhotonNetwork.Instantiate("Ship",player2.position,player2.rotation);
        }
        else
            PhotonNetwork.Instantiate("Ship",player1.position,player1.rotation);
      
        Hashtable props = new Hashtable() { { "Lives", 3f }, { "PLAYER_LOADED_LEVEL", true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += StartGame;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= StartGame;
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (gameOver)
            return;

        if (otherPlayer.IsLocal)
        {
            myLives.text = "Loser";
            opponentLives.text = "Winner";
            StartCoroutine(EndOfGame());
        }
        else
        {
            myLives.text = "Winner";
            opponentLives.text = "Loser";
            StartCoroutine(EndOfGame());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        object health;
        if (target.CustomProperties.TryGetValue("Lives", out health))
        {
            if (target.IsLocal)
            { 
                myLives.text = "Lives: " +health;
                if ((float)health <= 0)
                {
                    myLives.text = "Loser";
                    opponentLives.text = "Winner";
                    StartCoroutine(EndOfGame());
                }
            }
        else
            {
                opponentLives.text = "Lives: " + health;
                if ((float)health <= 0)
                {
                    myLives.text = "Winner";
                    opponentLives.text = "Loser";
                    StartCoroutine(EndOfGame());
                }
            }
        }


        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (changedProps.ContainsKey("PLAYER_LOADED_LEVEL"))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
        }

    }

    private IEnumerator EndOfGame()
    {
        gameInProgress = false;
        PlayerInput.inputEnabled = false;
        gameOver = true;
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LeaveRoom();
    }


    void StartGame()
    {
        gameInProgress = true;
        PlayerInput.inputEnabled = true;
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue("PLAYER_LOADED_LEVEL", out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.grey;
            case 6: return Color.magenta;
            case 7: return Color.white;
        }

        return Color.black;
    }
}

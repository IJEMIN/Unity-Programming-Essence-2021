// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectAndJoinRandomLb.cs" company="Exit Games GmbH"/>
// <summary>Prototyping / sample code for Photon Realtime.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Realtime.Demo
{
    public class ConnectAndJoinRandomLb : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
    {
        [SerializeField]
        private AppSettings appSettings = new AppSettings();
        private LoadBalancingClient lbc;

        private ConnectionHandler ch;
        public Text StateUiText;

        public void Start()
        {
            this.lbc = new LoadBalancingClient();
            this.lbc.AddCallbackTarget(this);

            if (!this.lbc.ConnectUsingSettings(appSettings))
            {
                Debug.LogError("Error while connecting");
            }

            this.ch = this.gameObject.GetComponent<ConnectionHandler>();
            if (this.ch != null)
            {
                this.ch.Client = this.lbc;
                this.ch.StartFallbackSendAckThread();
            }
        }

        public void Update()
        {
            LoadBalancingClient client = this.lbc;
            if (client != null)
            {
                client.Service();


                Text uiText = this.StateUiText;
                string state = client.State.ToString();
                if (uiText != null && !uiText.text.Equals(state))
                {
                    uiText.text = "State: " + state;
                }
            }
        }


        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            this.lbc.OpJoinRandomRoom();    // joins any open room (no filter)
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnJoinedLobby()
        {
        }

        public void OnLeftLobby()
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            this.lbc.OpCreateRoom(new EnterRoomParams());
        }

        public void OnLeftRoom()
        {
        }


        /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
        /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
            this.lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
        }
    }
}
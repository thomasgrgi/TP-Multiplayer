using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class VRConnectionManager : MonoBehaviour
{
    [Header("Paramètres de connexion VR")]
    public string profileName = "JoueurVR";
    public string sessionName = "MaSessionPartagee";
    public int maxPlayers = 10;

    private ConnectionState _state = ConnectionState.Disconnected;
    private ISession _session;
    private NetworkManager m_NetworkManager;

    private enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    private async void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        m_NetworkManager.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        
        try 
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnSessionOwnerPromoted(ulong sessionOwnerPromoted)
    {
        if (m_NetworkManager.LocalClient.IsSessionOwner)
        {
            Debug.Log($"Client-{m_NetworkManager.LocalClientId} is the session owner!");
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (m_NetworkManager.LocalClientId == clientId)
        {
            Debug.Log($"Client-{clientId} est connecté en VR !");
        }
    }

    private void OnDestroy()
    {
        if (_session != null)
        {
            _session.LeaveAsync();
        }
        
        if (m_NetworkManager != null)
        {
            m_NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
            m_NetworkManager.OnSessionOwnerPromoted -= OnSessionOwnerPromoted;
        }
    }

    // Méthode PUBLIQUE qui sera déclenchée par notre objet interactif en VR
    public void ConnectToSharedWorld()
    {
        if (_state != ConnectionState.Disconnected)
        {
            Debug.LogWarning("Déjà connecté ou en cours de connexion...");
            return;
        }

        Debug.Log("Tentative de connexion au monde partagé...");
        _ = CreateOrJoinSessionAsync();
    }

    private async Task CreateOrJoinSessionAsync()
    {
        _state = ConnectionState.Connecting;

        try
        {
            AuthenticationService.Instance.SwitchProfile(profileName);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var options = new SessionOptions() {
                Name = sessionName,
                MaxPlayers = maxPlayers
            }.WithDistributedAuthorityNetwork();

            _session = await MultiplayerService.Instance.CreateOrJoinSessionAsync(sessionName, options);

            _state = ConnectionState.Connected;
            Debug.Log("Connexion réussie !");
        }
        catch (Exception e)
        {
            _state = ConnectionState.Disconnected;
            Debug.LogException(e);
        }
    }
}
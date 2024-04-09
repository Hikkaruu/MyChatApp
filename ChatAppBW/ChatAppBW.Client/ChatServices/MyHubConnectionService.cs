using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatAppBW.Client.ChatServices
{
    public class MyHubConnectionService
    {
        public bool IsConnected { get; set; }
        private readonly HubConnection _hubConnection;

        public MyHubConnectionService(NavigationManager navManager)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navManager.ToAbsoluteUri("/chathub"))
                .Build();

            _hubConnection.StartAsync();
            GetConnectionState();
        }

        public HubConnection GetHubConnection() => _hubConnection;

        public bool GetConnectionState()
        {
            var hubConnection = GetHubConnection();
            IsConnected = hubConnection != null;
            return IsConnected;
        }
        

    }
}

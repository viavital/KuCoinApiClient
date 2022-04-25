using KuCoinApiClient.Config;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
using KuCoinApiClient.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
=======
using KuCoinApiClient.Model;
using Newtonsoft.Json;
using System.Timers;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
using WebSocket4Net;

namespace KuCoinApiClient.Services
{
    public class KucoinMessagingService
    {
        private readonly HttpService _httpService;
        private readonly MessagesStorage _storage;
        private readonly string _subscriptionId;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs

        private string _currentPair;
        private WebSocket _socket;
=======
        private readonly ILogger _logger;

        private string _currentPair;
        private WebSocket _socket;
        private int pingInterval;
        private int pingTimeout;
        private System.Timers.Timer TimerPingPong;   
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs

        //todo those bools better to make enum flags "state"
        private bool _isConnected;
        private bool _isWelcomeReceived;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
        private bool _isSubscribedAndReady;

        private TaskCompletionSource<bool> _connectTcs;

        public KucoinMessagingService(HttpService httpService, MessagesStorage storage)
        {
            _httpService = httpService;
            _connectTcs = new TaskCompletionSource<bool>();
            _subscriptionId = Guid.NewGuid().ToString();
            _storage = storage;
=======
        private bool _isSubscribedAndReady;       

        private TaskCompletionSource<bool> _connectTcs;
        private TaskCompletionSource _pingPongTcs;

        public KucoinMessagingService(HttpService httpService, MessagesStorage storage, ILogger<KucoinMessagingService> logger)
        {
            _httpService = httpService;
            _connectTcs = new TaskCompletionSource<bool>();
            _pingPongTcs = new TaskCompletionSource();
            _subscriptionId = Guid.NewGuid().ToString();
            _storage = storage;
            _logger = logger;
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        }

        internal async Task<bool> ConnectToSocket(string pairId)
        {
            if (!string.IsNullOrEmpty(_currentPair) && _currentPair != pairId)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
                return false; //todo log wrong pair
=======
                _logger.LogError("wrong pair (ConnectToSocket)");
                return false; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
            }
            _currentPair = pairId;

            if (_socket == null)
            {
                var initialData = await GetInitData();
                if (initialData == null)
                {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
                    return false; //todo add log
=======
                    _logger.LogError("wrong bad request (GetInitData)");
                    return false; 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
                }

                CreateSocket(initialData);
            }

            _socket.Open();

            _connectTcs.TrySetResult(false);
            _connectTcs = new TaskCompletionSource<bool>();
            return await _connectTcs.Task;
        }

        private void CreateSocket(SocketInitInfoModel initialData)
        {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
            _socket = new WebSocket(initialData.instanceServers[0].endpoint + "?token=" + initialData.token);
            _socket.Opened += OnSocketOpened;
            _socket.Closed += OnSocketClosed;
            _socket.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
=======
            _socket = new WebSocket(initialData.instanceServers[0].endpoint + "?token=" + initialData.token);            
             pingInterval = initialData.instanceServers[0].pingInterval;
             pingTimeout = initialData.instanceServers[0].pingTimeout;
            _socket.Opened += OnSocketOpened;
            _socket.Closed += OnSocketClosed;
            _socket.MessageReceived += OnMessageReceived;   
        }

        private async void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        {
            if (_isSubscribedAndReady)
            {
                HandleRegularMessage(e);
                return;
            }

            if (!_isWelcomeReceived)
            {
                var simpleMessage = JsonConvert.DeserializeObject<SimpleMessageModel>(e.Message);
                if (simpleMessage?.type == "welcome")
                {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
                    SubscribeToPair();
                    //todo start ping pong
                    _isWelcomeReceived = true;
                    return;
                }
                _connectTcs.TrySetResult(false); //todo log welcome not received
                return;
            }

=======
                    var id = simpleMessage.id;
                    SubscribeToPair();                    
                    _isWelcomeReceived = true;
                    return;
                }                
                _logger.LogError("didn't receive Welcome Message");
                _connectTcs.TrySetResult(false); 
                return;
            }
            
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
            if (!_isSubscribedAndReady)
            {
                var simpleMessage = JsonConvert.DeserializeObject<SimpleMessageModel>(e.Message);
                if (simpleMessage?.id == _subscriptionId && simpleMessage?.type == "ack")
                {
                    _isSubscribedAndReady = true;
                    _connectTcs.TrySetResult(true);
                    return;
                }
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
                _connectTcs.TrySetResult(false); //todo log subscription ack not received
=======
                _logger.LogError("subscription ack not received");
                _connectTcs.TrySetResult(false); 
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
                return;
            }
        }

        private void HandleRegularMessage(MessageReceivedEventArgs e)
        {
            var model = JsonConvert.DeserializeObject<FullMessageModel>(e.Message);
            if (model == null || model.data == null || model.data.changes == null)
            {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
                return; //todo log
            }

            _storage.AddToChache(model.data.changes);
=======
              var pongModel = JsonConvert.DeserializeObject<SimpleMessageModel>(e.Message);
                if (pongModel.type == "pong")
                {
                   _pingPongTcs.TrySetResult();
                   _logger.LogInformation("pong received - {0}", e.Message);
                    return;
                }
                _logger.LogError("couldn't deserialize message (HandleRegularMessage)");
                return; 
            }
            _storage.AddToChache(model.data.changes);           
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        }

        private void SubscribeToPair()
        {
            var subscriptionRequest = new SocketSubscribeRequestModel();
            subscriptionRequest.type = "subscribe";
            subscriptionRequest.id = _subscriptionId;
            subscriptionRequest.response = true;
            subscriptionRequest.topic = $"/market/level2:{_currentPair}";

            var reqJson = JsonConvert.SerializeObject(subscriptionRequest);
            _socket.Send(reqJson);
        }

<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
=======

>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        private void OnSocketClosed(object? sender, EventArgs e)
        {
            _isConnected = false;
            _isWelcomeReceived = false;
            _isSubscribedAndReady = false;
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
            //todo log socket connection closed + time

=======
            _logger.LogInformation("Socket is closed" + DateTime.Now);
            TimerPingPong.Stop();
            
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
            ConnectToSocket(_currentPair); //reconnect, stupid simple solution
        }

        private void OnSocketOpened(object? sender, EventArgs e)
        {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
            _isConnected = true;
            //todo log socket connection opened + time
=======
            _logger.LogInformation("Socket is opened" + DateTime.Now);
            _isConnected = true;
            SetTimer();
            TimerPingPong.Start();
            
        }

        private async void RunAutoPing(object? sender, EventArgs e)
        {
            if (_isConnected) 
            {
                Random randomId = new Random();
                SimpleMessageModel PingMess = new SimpleMessageModel() { id = randomId.Next(0, 100000000).ToString(), type = "ping" };
                var serializedPingMess = JsonConvert.SerializeObject(PingMess);
                _socket.Send(serializedPingMess);
                _logger.LogInformation("Ping sent - {0}", serializedPingMess);
                var task = await Task.WhenAny(_pingPongTcs.Task, Task.Delay(pingTimeout));
                if (_pingPongTcs.Task.IsCompleted)
                {
                    _pingPongTcs = new TaskCompletionSource();
                    return;
                }
                _isConnected = false;
                _logger.LogError("pong didn't receive");
                _socket.CloseAsync();
            }
        }
        private void SetTimer()
        {            
            TimerPingPong = new System.Timers.Timer(pingInterval);
            TimerPingPong.Elapsed += RunAutoPing;
            TimerPingPong.AutoReset = true;
            TimerPingPong.Enabled = true;            
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        }

        internal bool isConnectedAndReady(string pairId)
        {
            if (_currentPair == null || _currentPair != pairId)
            {
                return false;
            }

            return _isSubscribedAndReady;
        }

        private Task<SocketInitInfoModel> GetInitData()
        {
<<<<<<< HEAD:KuCoinApiClient/Services/KucoinMessagingService.cs
            return _httpService.DoPost<SocketInitInfoModel>(KucoinSettings.BaseUrl + "/api/v1/bullet-public", new { });
=======
            return _httpService.DoPost<SocketInitInfoModel>(KucoinSettings.BaseUrl + "/api/v1/bullet-public", new {});
>>>>>>> MaxSizeBuffer:KukoinServer/Services/KucoinMessagingService.cs
        }
    }
}

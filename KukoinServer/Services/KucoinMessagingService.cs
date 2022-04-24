using KuCoinApiClient.Config;
using KuCoinApiClient.Model;
using Newtonsoft.Json;
using WebSocket4Net;

namespace KuCoinApiClient.Services
{
    public class KucoinMessagingService
    {
        private readonly HttpService _httpService;
        private readonly MessagesStorage _storage;
        private readonly string _subscriptionId;

        private string _currentPair;
        private WebSocket _socket;

        //todo those bools better to make enum flags "state"
        private bool _isConnected;
        private bool _isWelcomeReceived;
        private bool _isSubscribedAndReady;

        private TaskCompletionSource<bool> _connectTcs;

        public KucoinMessagingService(HttpService httpService, MessagesStorage storage)
        {
            _httpService = httpService;
            _connectTcs = new TaskCompletionSource<bool>();
            _subscriptionId = Guid.NewGuid().ToString();
            _storage = storage;
        }

        internal async Task<bool> ConnectToSocket(string pairId)
        {
            if (!string.IsNullOrEmpty(_currentPair) && _currentPair != pairId)
            {
                return false; //todo log wrong pair
            }
            _currentPair = pairId;

            if (_socket == null)
            {
                var initialData = await GetInitData();
                if (initialData == null)
                {
                    return false; //todo add log
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
            _socket = new WebSocket(initialData.instanceServers[0].endpoint + "?token=" + initialData.token);
            _socket.Opened += OnSocketOpened;
            _socket.Closed += OnSocketClosed;
            _socket.MessageReceived += OnMessageReceived;   
        }

        private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
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
                    SubscribeToPair();
                    //todo start ping pong
                    _isWelcomeReceived = true;
                    return;
                }
                _connectTcs.TrySetResult(false); //todo log welcome not received
                return;
            }
            
            if (!_isSubscribedAndReady)
            {
                var simpleMessage = JsonConvert.DeserializeObject<SimpleMessageModel>(e.Message);
                if (simpleMessage?.id == _subscriptionId && simpleMessage?.type == "ack")
                {
                    _isSubscribedAndReady = true;
                    _connectTcs.TrySetResult(true);
                    return;
                }
                _connectTcs.TrySetResult(false); //todo log subscription ack not received
                return;
            }
        }

        private void HandleRegularMessage(MessageReceivedEventArgs e)
        {
            var model = JsonConvert.DeserializeObject<FullMessageModel>(e.Message);
            if (model == null || model.data == null || model.data.changes == null)
            {
                return; //todo log
            }

            _storage.AddToChache(model.data.changes);           
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

        private void OnSocketClosed(object? sender, EventArgs e)
        {
            _isConnected = false;
            _isWelcomeReceived = false;
            _isSubscribedAndReady = false;
            //todo log socket connection closed + time

            ConnectToSocket(_currentPair); //reconnect, stupid simple solution
        }

        private void OnSocketOpened(object? sender, EventArgs e)
        {
            _isConnected = true;
            //todo log socket connection opened + time
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
            return _httpService.DoPost<SocketInitInfoModel>(KucoinSettings.BaseUrl + "/api/v1/bullet-public", new {});
        }
    }
}

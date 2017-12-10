using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.Windows.Forms;
namespace SignalRClient
{
    /// <summary>
    /// Если связь потеряна, будет пытаться восстановить связь перед каждым Call
    /// </summary>
    class ServerCall
    {

        public bool IsConnected { get; set; }
        public Exception LastException { get; set; }
        IHubProxy hubProxy;
        Form _form;
        Action<string> _logMessageAction;
        string _serverAddress, _login, _password;
        public ServerCall(Form form, string serverAddress, string login, string password, Action<string> LogMessageAction)
        {

            _form = form;
            _logMessageAction = LogMessageAction;
            _serverAddress = serverAddress;
            _login = login;
            _password = password;

            //hubProxy.On("hello", () => Invoke(new Action(() => this.Text = "Success")));
            //hubProxy.On("authorizeResult", new Action<int, bool>((myId, isAdmin) => { this.myIdOnServer = (int)myId; ImAdmin = isAdmin; this.Invoke(new Action(() => SetAdminButtonsVisibility(isAdmin))); }));
            //hubProxy.On("fileId", (myFileId) => UploadFileToServer((int)myFileId));
            //hubProxy.On("message", (text) => AddServerMessage(text));
            //hubProxy.On("setRoomState", (x) =>
            //{
            //    gameList = JsonConvert.DeserializeObject<List<ServerGame>>(x.gameList.ToString());
            //    playerList = JsonConvert.DeserializeObject<List<ServerPlayer>>(x.playerList.ToString());
            //    this.Invoke(new Action(() => this.RefreshGameFrid()));
            //});
            //hubProxy.On("roundFinished", new Action<int, int, dynamic>((gameId, roundNumber, x) => RoundFinished(gameId, roundNumber, JsonConvert.DeserializeObject<object>(x.ToString()))));
            //hubProxy.On("runGameFromServer", (game) => RunGameFromServer(JsonConvert.DeserializeObject<ServerGame>(game.ToString())));

            //если тут вылез эксепшн, вероятно, Core.Config.serverAddress некорректен


            Connect();
        }


        public bool CallVoid(string serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
        {
            //задублировано с кодом ниже
            if (!IsConnected)
            {
                Connect();
            }
            if (IsConnected)
            {
                try
                {
                    Message(messageBeforeCall);
                    var task = hubProxy.Invoke<string>(serverMethod, args);
                    task.Wait();
                    if (string.IsNullOrEmpty(task.Result) == false)
                    {
                        Message(task.Result);
                    }
                    else
                    {
                        Message(messageAfterSuccess);
                        return true;
                    }
                }
                catch
                {
                    Message("Ошибка при отправке сообщения серверу");
                }
            }
            return false;
        }

        public T Call<T>(string serverMethod, string messageBeforeCall, string messageAfterSuccess, T returnIfError, params object[] args)
        {
            //задублировано с кодом выше
            if (!IsConnected)
            {
                Connect();
            }
            if (IsConnected)
            {
                try
                {
                    Message(messageBeforeCall);
                    var task = hubProxy.Invoke<Tuple<T, string>>(serverMethod, args);
                    task.Wait();
                    if (string.IsNullOrEmpty(task.Result.Item2) == false)
                    {
                        Message(task.Result.Item2);
                    }
                    else
                    {
                        Message(messageAfterSuccess);
                        return task.Result.Item1;
                    }
                }
                catch
                {
                    Message("Ошибка при отправке сообщения серверу");

                }
            }

            return returnIfError ;
        }

        //public void Call(ServerMethods serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
        //{
        //    Call(serverMethod.ToString(), messageBeforeCall, messageAfterSuccess, args);
        //}

        //public T Call<T>(ServerMethods serverMethod, string messageBeforeCall, string messageAfterSuccess, params object[] args)
        //{
        //    return Call<T>(serverMethod.ToString(), messageBeforeCall, messageAfterSuccess, args);
        //}


        private void Connect()
        {
            try
            {
                Message("Установка соединения...");
                var hubConnection = new HubConnection(_serverAddress);
                hubProxy = hubConnection.CreateHubProxy("MainHub");
                hubConnection.Closed += () => { Message("Соединение с сервером потеряно"); IsConnected = false; };

                hubConnection.Start().Wait();
                //id или сообщение об ошибке
                var task = hubProxy.Invoke< string>("ConnectAsParticipant", _login, _password);
                task.Wait();
                if (string.IsNullOrEmpty(task.Result) == false)
                {
                    Message(task.Result);
                }
                else
                {
                    IsConnected = true;
                    Message("Соединение установлено");
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
                Message("Не удалось установить соединение. Возможно, введенный адрес сервера является некорректным");
            }
        }

        void Message(string message)
        {
            var text = DateTime.Now.ToShortTimeString() + ": " + message;
            _form.Invoke(new Action(() => _logMessageAction(message)));
        }


    }
}

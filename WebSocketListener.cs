using System;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace Headphone_tracker
{
    public class WebSocketListener : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            //Send("Connected");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            //Send("Closed");
            base.OnClose(e);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            Send(e.Message);
            base.OnError(e);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Message received: " + e.Data);
        }
        public void SendMessage(string message)
        {
            Sessions.Broadcast(message);
        }
    }
}
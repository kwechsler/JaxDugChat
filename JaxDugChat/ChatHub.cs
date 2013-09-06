using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace JaxDugChat
{
    public class ChatHub : Hub
    {
        private List<string> users = new List<string>();
        public List<string> Users {
            get
            {
                List<string> tempUsers = HttpContext.Current.Application["Users"] as List<string>;
                if (tempUsers == null)
                {
                    HttpContext.Current.Application["Users"] = new List<string>();
                    tempUsers = HttpContext.Current.Application["Users"] as List<string>;
                }
                users = tempUsers;
                return users;
            }
            set
            {
                users = value;
                HttpContext.Current.Application["Users"] = value;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ChatHub()
        {
            
        }

        


        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void AddChatUser(string user)
        {

            if (!Users.Contains(user))
            {
                this.Users.Add(user);
            }
            Clients.All.newUserAddedMessage(user);
        }

        public void RemoveChatUser(string user)
        {
            this.Users.Remove(user);
            Clients.All.userRemovedMessage(user);   
        }

        public void GetAllChatUsers()
        {
            Clients.All.usersInChat(this.Users.ToArray());
        }

    }
}
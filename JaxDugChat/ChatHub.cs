using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace JaxDugChat
{
    public class ChatHub : Hub
    {
        // Creating Application In-Memory storage
        public List<string> Users 
        {
            get
            {
                List<string> tempUsers = HttpContext.Current.Application["Users"] as List<string>;
                if (tempUsers == null)
                {
                    HttpContext.Current.Application["Users"] = new List<string>();
                    tempUsers = HttpContext.Current.Application["Users"] as List<string>;
                }
                return tempUsers;
            }
            set
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["Users"] = value;
                HttpContext.Current.Application.UnLock();
            }
        }
        
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        //This method adds a user to the User list property and notifies all users
        public void AddChatUser(string user)
        {
            if (!Users.Contains(user))
            {
                this.Users.Add(user);
            }
            Clients.All.newUserAddedMessage(user);
        }

        //This method removes the user from the Users property and notifies all users
        public void RemoveChatUser(string user)
        {
            this.Users.Remove(user);
            Clients.All.userRemovedMessage(user);   
        }

        //Returns a list of all Users
        public void GetAllChatUsers()
        {
            Clients.All.usersInChat(this.Users.ToArray());
        }

    }
}
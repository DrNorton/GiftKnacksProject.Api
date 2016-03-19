using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Services.Storages
{
    public class UserConnectionSignalR
    {
        private readonly long _userId;
        private List<string> _connections; 

        public UserConnectionSignalR(long userId)
        {
            _userId = userId;
            _connections=new List<string>();
        }

        public long UserId
        {
            get { return _userId; }
        }

        public List<string> Connections
        {
            get { return _connections; }
        }

        public void AddConnection(string connectionId)
        {
            _connections.Add(connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            if (_connections.IndexOf(connectionId) != -1)
            {
                _connections.Remove(connectionId);
            }
        }

        public bool IsEmpty()
        {
            return !_connections.Any();
        }
    }
}

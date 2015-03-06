using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Signalr
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> m_connections;

        public int Count
        {
            get
            {
                return m_connections.Count;
            }
        }

        public IEnumerable<T> Keys
        {
            get
            {
                lock (m_connections)
                    return m_connections.Keys.ToList();
            }
        }

        public ConnectionMapping()
        {
            m_connections = new Dictionary<T, HashSet<string>>();
        }

        public ConnectionMapping(IEqualityComparer<T> comparer)
        {
            m_connections = new Dictionary<T, HashSet<string>>(comparer);
        }

        public void Add(T key, string connectionId)
        {
            lock (m_connections)
            {
                HashSet<string> connections;
                if (!m_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    m_connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (m_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (m_connections)
            {
                HashSet<string> connections;
                if (!m_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        m_connections.Remove(key);
                    }
                }
            }
        }
    }
}
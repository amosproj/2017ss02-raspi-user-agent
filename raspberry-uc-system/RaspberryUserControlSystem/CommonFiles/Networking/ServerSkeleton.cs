﻿using CommonFiles.TransferObjects;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace CommonFiles.Networking
{

    /// <summary>
    /// This class represents a Server which listens for incoming Requests
    /// and invokes the requested method
    /// </summary>
    public class ServerSkeleton
    {
        private Object service;

        /// <summary>
        /// Runs the server in a new Thread
        /// </summary>
        /// <param name="service">Service, which methods will be called on incoming Request</param>
        /// <param name="port">Port, where the server listens for incoming Requests</param>
        public ServerSkeleton(Object service, int port)
        {
            this.service = service;
            Task.Factory.StartNew(() => runAsync(port));
        }

        private async Task runAsync(int port)
        {
            TCPServer<Request, Result> requestServer = new TCPServer<Request, Result>(port);
            while (true)
            {
                try
                {
                    Debug.WriteLine(this.GetType().Name + "::: Awaiting request...");
                    using (ObjConn<Request, Result> connection = await requestServer.acceptConnectionAsync())
                    {
                        handleRequestConnection(connection);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Network error: " + e.Message);
                }
            }
        }

        private void handleRequestConnection(ObjConn<Request, Result> conn)
        {
            while (true)
            {
                //Receive a Request from the client
                Debug.WriteLine("Awaiting Request...");
                Request request = conn.receiveObject();
                Debug.WriteLine(string.Format("Received Request with content : (command= {0}) and (paramater= {1})", request.command, request.parameters));

                //Process Request
                Result result = Request.handleRequest(service, request);

                //Send back Result to the client
                conn.sendObject(result);
            }
        }
    }
}
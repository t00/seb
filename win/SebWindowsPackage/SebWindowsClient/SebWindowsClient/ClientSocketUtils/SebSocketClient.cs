﻿// -------------------------------------------------------------
//     Viktor tomas
//     BFH-TI, http://www.ti.bfh.ch
//     Biel, 2012
// -------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using SebWindowsClient.DiagnosticsUtils;
using System.Windows.Forms;
using SebWindowsClient.ConfigurationUtils;

namespace SebWindowsClient.ClientSocketUtils
{
    /// ---------------------------------------------------------------------------------------
    /// <summary>
    /// Provides socket communication.
    /// </summary>
    /// ---------------------------------------------------------------------------------------
    public class SebSocketClient
    {
        /// <summary>
        /// Opens socket.
        /// </summary>
        /// <returns></returns>
        public bool OpenSocket(string hostName, string port)
        {
            bool result = false;
            // Sind wir bereits verbunden?
            if (_clientSocket != null && _clientSocket.Connected)
            {
                _clientSocket.Close();
            }

            try
            {

                // Verwende den angegebenen Host, ansonsten nimm den lokalen Host
                string server = (hostName.Length > 0) ? hostName : "::1";
                // Verwende den angegebenen Port, ansonsten nimm den Standardwert 7
                int servPort = (port.Length > 0) ? Convert.ToInt32(port) : 3663;

                IPAddress servIPAddress = IPAddress.Parse(server);
                IPEndPoint servEndPoint = new IPEndPoint(servIPAddress, servPort);

                // Erzeuge neuen IPv4 Tcp Socket
                _clientSocket = new Socket(servIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Verbinde...
                _clientSocket.Connect(servEndPoint);

                // Zeige dem User das wir verbunden sind
                if (_clientSocket.Connected)
                {
                    Logger.AddInformation("Connected to " + server, null, null);
                }
                result = true;
            }
            catch (SocketException se)
            {
                Logger.AddError("OpenSocket failed with socket error. " + se.Message, null, se);
            }
            catch (Exception ex)
            {
                Logger.AddError("OpenSocket failed with error. " + ex.Message, null, ex);
            }
            return result;
        }

        /// <summary>
        /// Closes socket.
        /// </summary>
        /// <returns></returns>
        public void CloseSocket(object sender, EventArgs e)
        {
            if (_clientSocket == null)
            {
                MessageBox.Show("You must first connect to a Server!", "MessageBox",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Schließe den Socket
                if (_clientSocket.Connected)
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                }

                _clientSocket.Close();

                MessageBox.Show("Disconnected!", "MessageBox",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.AddInformation("Disconnected!", null, null);
            }
            catch (SocketException se)
            {
                Logger.AddError("CloseSocket failed with socket error. " + se.Message, null, se);
            }
            catch (Exception ex)
            {
                Logger.AddError("CloseSocket failed with error. " + ex.Message, null, ex);
            }

        }

        /// <summary>
        /// Sets Recv Timeout.
        /// </summary>
        /// <returns></returns>
        public bool SetRecvTimeout(int timeout)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
            {
                MessageBox.Show("You must first connect to a Server!", "MessageBox",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            bool result = false;
            try
            {
                 // Set timeout
                _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, timeout);
                result = true;
             }
            catch (SocketException se)
            {
                Logger.AddError("SetRecvTimeout failed with socket error. " + se.Message, null, se);
            }
            catch (Exception ex)
            {
                Logger.AddError("SetRecvTimeout failed with error. " + ex.Message, null, ex);
            }
            return result;
        }

        /// <summary>
        /// Sends data to server.
        /// </summary>
        /// <returns></returns>
        public bool SendEquationToSocketServer(string leftSide, string rightSide, int sendInterval)
        {
            if (_clientSocket == null || !_clientSocket.Connected)
            {
                MessageBox.Show("You must first connect to a Server!", "MessageBox",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            bool result = false;
            try
            {
                // Sende Daten
                StringBuilder buffer = new StringBuilder(leftSide).Append("0").Append(rightSide).Append(SEBClientInfo.END_OF_STRING_KEYWORD);
                byte[] byteBuffer = System.Text.Encoding.ASCII.GetBytes(buffer.ToString());
                _clientSocket.Send(byteBuffer, 0, byteBuffer.Length, SocketFlags.None);

                System.Threading.Thread.Sleep(sendInterval);
                result = true;
            }
            catch (SocketException se)
            {
                Logger.AddError("SendEquationToSocketServer failed with socket error. " + se.Message, null, se);
            }
            catch (Exception ex)
            {
                Logger.AddError("SendEquationToSocketServer failed with error. " + ex.Message, null, ex);
            }
            return result;
        }

        /// <summary>
        /// Recives data from server.
        /// </summary>
        /// <returns></returns>
        public string[] RecvEquationOfSocketServer()
        {
            string[] leftRightSide = null;
 
            if (_clientSocket == null || !_clientSocket.Connected)
            {
                MessageBox.Show("You must first connect to a Server!", "MessageBox",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return leftRightSide;
            }

            try
            {
                // Empfange Daten
                byte[] byteBuffer = new byte[2048];
                int bytesRcvd = _clientSocket.Receive(byteBuffer);
                char[] chars = new char[bytesRcvd];

                Decoder dec = Encoding.UTF8.GetDecoder();
                int charLen = dec.GetChars(byteBuffer, 0, bytesRcvd, chars, 0);
                string newData = new string(chars);

                int endData = newData.IndexOf(SEBClientInfo.END_OF_STRING_KEYWORD);
                if (endData > 0)
                {
                    string leftRight = newData.Substring(0, newData.Length - endData);
                    leftRightSide = leftRight.Split(new Char[] { '=' });
                }
            }
            catch (SocketException se)
            {
                Logger.AddError("RecvEquationOfSocketServer failed with socket error. " + se.Message, null, se);
            }
            catch (Exception ex)
            {
                Logger.AddError("RecvEquationOfSocketServer failed with error. " + ex.Message, null, ex);
            }

            return leftRightSide;
        }

        private Socket _clientSocket;

    }
}

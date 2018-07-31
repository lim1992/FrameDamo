using ETModel;
using System;

namespace ETHotfix
{
    public class FSPClient
    {
        //===========================================================
        public delegate void FSPTimeoutListener(FSPClient target, int val);
        
        //===========================================================
        //基本数据
        
//         //线程模块
        private bool m_IsRunning = false;
//         
//         //基础通讯模块
//         private KCPSocket m_Socket;
//         private string m_Host;
//         private int m_Port;
//         private IPEndPoint m_HostEndPoint = null;
//         private ushort m_SessionId = 0;
        
        //===========================================================

        //===========================================================
        //接收逻辑
        private Action<FSPFrame> m_RecvListener;
        private byte[] m_TempRecvBuf = new byte[10240];
        
        //发送逻辑
        private bool m_EnableFSPSend = true;
        private int m_AuthId;
        private FSPDataC2S m_TempSendData = new FSPDataC2S();
        private byte[] m_TempSendBuf = new byte[128];
        
        private bool m_WaitForReconnect = false;
        private bool m_WaitForSendAuth = false;

        //===========================================================
        //===========================================================
        //------------------------------------------------------------
        #region 构造与析构
        public FSPClient()
        {

        }

        public void Close()
        {
            Disconnect();
            m_RecvListener = null;
            m_WaitForReconnect = false;
            m_WaitForSendAuth = false;
        }


        #endregion


        //------------------------------------------------------------
        #region 设置通用参数

        public void SetSessionId(uint sid)
        {
            m_TempSendData = new FSPDataC2S();
            m_TempSendData.Vkeys.Add(new FSPVKey());
            m_TempSendData.Sid = sid;
        }



        #endregion

        //------------------------------------------------------------
        #region 设置FSP参数

        public void SetFSPAuthInfo(int authId)
        {
            m_AuthId = authId;
        }

        public void SetFSPListener(Action<FSPFrame> listener)
        {
            m_RecvListener = listener;
        }

        #endregion

        //------------------------------------------------------------

    #region 基础连接函数

        public bool IsRunning { get { return m_IsRunning; } }

        public void VerifyAuth()
        {
            m_WaitForSendAuth = false;
            SendFSP(FSPVKeyBase.AUTH, m_AuthId, 0);
        }

        public void Reconnect()
        {
            m_WaitForReconnect = false;

            Disconnect();
            Connect();
            VerifyAuth();
        }

        public bool Connect()
        {
            return true;
        }

        private void Disconnect()
        {
            m_IsRunning = false;
        }

        #endregion


        //------------------------------------------------------------

        #region Receive 先用公用消息

//         private void OnReceive(byte[] buffer, int size, IPEndPoint remotePoint)
//         {
//             FSPDataS2C data = PBSerializer.NDeserialize<FSPDataS2C>(buffer);
//             
//             if (m_RecvListener != null)
//             {
//                 for (int i = 0; i < data.frames.Count; i++)
//                 {
//                     m_RecvListener(data.frames[i]);
//                 }
//                 
//             }
//         }

        #endregion


        //------------------------------------------------------------

        #region Send

        public bool SendFSP(int vkey, int arg, int clientFrameId)
        {
            if (m_IsRunning)
            {
                FSPVKey cmd = m_TempSendData.Vkeys[0];
                cmd.Vkey = vkey;
                cmd.Args.Add(arg);
                cmd.PlayerIdOrClientFrameId = (uint)clientFrameId;
				ETModel.SessionComponent.Instance.Session.Send(m_TempSendData);
				return true;
            }
            return false;
        }

        #endregion  


        //------------------------------------------------------------
        public void EnterFrame()
        {
            if (!m_IsRunning)
            {
                return;
            }

            if (m_WaitForReconnect)
            {
                if (NetCheck.IsAvailable())
                {
                    Reconnect();
                }
            }

            if (m_WaitForSendAuth)
            {
                VerifyAuth();
            }
        }
    }
}

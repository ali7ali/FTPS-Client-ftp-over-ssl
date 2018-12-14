using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.Security;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Threading;

namespace FTPClient
{
    public partial class FTPSClient : Form
    {
        #region Constructor
        public FTPSClient()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();

            txtpassword.TextBox.PasswordChar = '*';
        }
        #endregion 

        #region Destructor
        ~FTPSClient()
        {
            LogOut();
        }
        #endregion

        #region Var
        private Socket FTPSocket = null, DataSock = null;
        private FileInfo fi, fi2;
        private ImageList lstServerImages = new ImageList();
        private string StatusMessage = "", Result = "", Server = "", UserName = "", 
            Password = "", Path = "/",OldName="" , clientdir = "";

        private int StatusCode, bytes, Port;
        private long upfilesize;
        private string[] Msg;
        private bool Logged = false, Changed = false , Deleted = false;
        private bool EncryptedStream =false ;
        long percentage;
        private Stream stream = null;
        SslStream _sslStream;

        private Byte[] buffer = new Byte[16384 * 16384];
        Encoding ASCII = Encoding.ASCII;
       
        // DirectoryInfo directoryInfo = new DirectoryInfo(clientdir);
        DirectoryInfo directoryInfo = new DirectoryInfo(@"D:/client");
        #endregion


        #region TLS Secure Connection Functions
        /// <summary>
        /// TLS Secure Connection
        /// </summary>
        /// <param name="certificate"></param>
        /// 
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            AppendText(richTextLog, "Server Certificate Issued To: " + certificate.Subject + "\n", Color.Green);
            AppendText(richTextLog, "Server Certificate Issued By: " + certificate.Issuer + "\n", Color.Green);

            //my self-signed certificate
            if (certificate.Subject == "CN=mylocalsite.local")
            { return true; }

            // Return true if there are no policy errors
            else if (errors != SslPolicyErrors.None)
            {
                AppendText(richTextLog, "Server Certificate Validation Error" + "\n", Color.Red);
                AppendText(richTextLog, errors.ToString() + "\n", Color.Red);
                return false;
            }
            else
            {
                AppendText(richTextLog, "No Certificate Validation Errors" + "\n", Color.Green);
                return true;
            }
        }
        private void showCertificateInfo(X509Certificate remoteCertificate, bool verbose)
        {

            AppendText(richTextLog, "Certficate Information for: " + remoteCertificate.Subject + "\n", Color.Gray);
            AppendText(richTextLog, "Valid From: " + remoteCertificate.GetEffectiveDateString() + "\n", Color.Gray);
            AppendText(richTextLog, "Valid To: " + remoteCertificate.GetExpirationDateString() + "\n", Color.Gray);
            AppendText(richTextLog, "Certificate Format: " + remoteCertificate.GetFormat() + "\n", Color.Gray);
            AppendText(richTextLog, "Issuer Name: " + remoteCertificate.Issuer + "\n", Color.Gray);

            if (verbose)
            {

                AppendText(richTextLog, "Serial Number: " + remoteCertificate.GetSerialNumberString() + "\n", Color.Gray);
                AppendText(richTextLog, "Hash: " + remoteCertificate.GetCertHashString() + "\n", Color.Gray);
                AppendText(richTextLog, "Key Algorithm: " + remoteCertificate.GetKeyAlgorithm() + "\n", Color.Gray);
                AppendText(richTextLog, "Key Algorithm Parameters: " + remoteCertificate.GetKeyAlgorithmParametersString() + "\n", Color.Gray);
                AppendText(richTextLog, "Public Key: " + remoteCertificate.GetPublicKeyString() + "\n", Color.Gray);
            }
        }
        private void showSslInfo(string serverName, SslStream sslStream, bool verbose)
        {

            showCertificateInfo(sslStream.RemoteCertificate, true);
            AppendText(richTextLog, "\nSSL Connect Report for : " + serverName + "\n", Color.Green);
            AppendText(richTextLog, "Is Authenticated: " + sslStream.IsAuthenticated + "\n", Color.Green);
            AppendText(richTextLog, "Is Encrypted: " + sslStream.IsEncrypted + "\n", Color.Green);
            AppendText(richTextLog, "Is Signed: " + sslStream.IsSigned + "\n", Color.Green);
            AppendText(richTextLog, "Is Mutually Authenticated: " + sslStream.IsMutuallyAuthenticated + "\n", Color.Green);

            AppendText(richTextLog, "Hash Algorithm: " + sslStream.HashAlgorithm + "\n", Color.Green);
            AppendText(richTextLog, "Hash Strength: " + sslStream.HashStrength + "\n", Color.Green);
            AppendText(richTextLog, "Cipher Algorithm: " + sslStream.CipherAlgorithm + "\n", Color.Green);
            AppendText(richTextLog, "Cipher Strength: " + sslStream.CipherStrength + "\n", Color.Green);

            AppendText(richTextLog, "Key Exchange Algorithm: " + sslStream.KeyExchangeAlgorithm + "\n", Color.Green);
            AppendText(richTextLog, "Key Exchange Strength: " + sslStream.KeyExchangeStrength + "\n", Color.Green);
            AppendText(richTextLog, "SSL Protocol: " + sslStream.SslProtocol + "\n\n", Color.Green);
        }
        public void OpenSslStream(Socket socket)
        {
            Stream str = new NetworkStream(FTPSocket);
            RemoteCertificateValidationCallback callback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            //_sslStream = new SslStream(str , false,callback,null);
            _sslStream = new SslStream(str, false, callback);
            try
            {


                _sslStream.AuthenticateAsClient(
                    Server,
                    null,
                    System.Security.Authentication.SslProtocols.Ssl3 | System.Security.Authentication.SslProtocols.Tls,
                    false);

                if (_sslStream.IsAuthenticated)
                    stream = _sslStream;

                //_sslStream.ReadTimeout = 5000;
                //_sslStream.WriteTimeout = 5000;

            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message);
                AppendText(richTextLog, "exception: " + ex.Message + "\n", Color.Red);
                if (ex.InnerException != null)
                {
                    AppendText(richTextLog, "Inner exception: " + ex.InnerException.Message + "\n", Color.Red);
                }
            }
            showSslInfo(Server, _sslStream, true);
        }
        #endregion

        #region FTP Functions
        private void RenameFile(string oldName, string newName)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            SendCommand("RNFR " + oldName);
            /*if (StatusCode==550)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                lstServerFiles.SelectedItems[0].Text = RnmOldName.Substring(RnmOldName.LastIndexOf('/') + 1);
                return;
            }*/
            if (StatusCode != 350)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            SendCommand("RNTO " + newName);
            if (StatusCode != 250)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Status Renamed file " + oldName + " to " + newName + "\n", Color.Red);
        }
        private void DeleteFile(string file)
        {
            Deleted = false;
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            SendCommand("DELE " + file);
            if (StatusCode != 250)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Status Deleted file " + file + "\n", Color.Red);
            Deleted = true;

        }
        private void UploadFile(string LocalPath )
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            Socket dataSocket = null;
            dataSocket = OpenSocketForTransfer();
            SendCommand("STOR " + System.IO.Path.GetFileName(LocalPath));
            if (StatusCode != 125 && StatusCode != 150)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Status : Uploading File from " + LocalPath + "\n", Color.Red);
            AppendText(richTextLog, "Status : Uploading File size " + upfilesize + "\n", Color.Red);
            int by =0;
            FileStream input = new FileStream(LocalPath, FileMode.Open);
            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
              
               by+= dataSocket.Send(buffer, bytes, 0);
               percentage = 100 * (long)by / upfilesize;

                progressBar1.Value = (int)percentage % 100;
                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();


            }
            percentage = 0;
            progressBar1.Value = 0;
            input.Close();
            if (dataSocket.Connected)
            {
                dataSocket.Close();
            }
            ReadResponse();
            if (StatusCode != 226 && StatusCode != 250)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
        }
        private void DownloadFile(string FtpPath, string LocalPath)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Status : Downloading File From " + FtpPath + " To " + LocalPath + "\n", Color.Red);
            long fsize = FileSize(FtpPath);
            if (LocalPath.Equals(""))
                return;
            FileStream output = null;
            
            if (!File.Exists(LocalPath))
                output = File.Create(LocalPath);
            else
            {
                output = new FileStream(LocalPath, FileMode.Open) ;
                
                AppendText(richTextLog, "Status : OverWriting...To " + LocalPath + "\n", Color.Red);
            }
            Socket dataSocket = OpenSocketForTransfer();
            SendCommand("RETR " + FtpPath);
            if (StatusCode != 150 && StatusCode != 125)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            
            DateTime timeout = DateTime.Now.AddSeconds(3600);
            while (timeout > DateTime.Now)
            {
                bytes = dataSocket.Receive(buffer, buffer.Length, 0);
               
                output.Write(buffer, 0, bytes);
                var file = output;
                percentage = 100 * (long)file.Length / fsize;

                 progressBar1.Value = (int)percentage%100;
                
                if (!backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();
                if (bytes <= 0)
                    break;
            }
            percentage = 0;
            progressBar1.Value = 0;
            output.Close();
            if (dataSocket.Connected)
                dataSocket.Close();
            ReadResponse();
            if (StatusCode != 226 && StatusCode != 250)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Status : Download Completed Sucessfully\n", Color.Red);
        }
        private long FileSize(string FileName)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return 0;
            }
            SendCommand("SIZE " + FileName);
            long Filesize;
            if (StatusCode == 213)
                Filesize = long.Parse(Result.Substring(4));
            else
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return 0;
            }
            return Filesize;
        }
        private void WriteCommand(string message)
        {
            System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();

            byte[] WriteBuffer = new byte[1024];
            WriteBuffer = en.GetBytes(message);

            stream.Write(WriteBuffer, 0, WriteBuffer.Length);

        }
        private void SendCommand(string msg)
        {
            Byte[] CommandBytes = Encoding.ASCII.GetBytes((msg +"\r\n").ToCharArray());
            if (EncryptedStream == true)
            {
                    AppendText(richTextLog, "Command : " + msg + "\n", Color.Blue);
                    WriteCommand(msg + "\r\n");
            }
            else
            {
                    AppendText(richTextLog, "Command : " + msg + "\n", Color.Blue);
                    FTPSocket.Send(CommandBytes, CommandBytes.Length, 0); 
            }
            ReadResponse();
        }
        private string ResponseMsg()
        {
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] serverbuff = new Byte[4096];
                int count = 0;
                while (true)
                {
                    byte[] buff = new Byte[4096];

                    int bytes = stream.Read(buff, 0, 1);

                    if (bytes == 1)
                    {
                        serverbuff[count] = buff[0];
                        count++;

                        if (buff[0] == '\n')
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    };

                };
                StatusMessage += enc.GetString(serverbuff, 0, count);
                return StatusMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppendText(richTextLog, "Status : ERROR. " + ex.Message + "\n", Color.Red);
                stream.Close();
                return "";
            }
        }
        private void ReadResponse()
        {
            if (EncryptedStream == true)
            {
                StatusMessage = "";
                Result = ResponseMsg();
                AppendText(richTextLog, "*Response :" + Result, Color.Green);
                StatusCode = int.Parse(Result.Substring(0, 3));

            }
            else
            {
                StatusMessage = "";
                Result = readLine();
                //AppendText(richTextLog, "Response :" + Result, Color.Green);
                StatusCode = int.Parse(Result.Substring(0, 3));
            }
        }
        private string readLine()
        {
            try
            {
                while (EncryptedStream == false)
                {
                    bytes = FTPSocket.Receive(buffer, buffer.Length, 0); //Number Of Bytes (Count)
                    StatusMessage = Encoding.ASCII.GetString(buffer, 0, bytes); //Convert to String
                    if (bytes < buffer.Length)  //End Of Response
                        break;

                }
                string[] msg = StatusMessage.Split('\n');
                if (StatusMessage.Length > 2)
                    StatusMessage = msg[msg.Length - 2];  //Remove Last \n
                else
                    StatusMessage = msg[0];
                if (!StatusMessage.Substring(3, 1).Equals(" "))
                    return readLine();
                for (int i = 0; i < msg.Length - 1; i++)
                {
                    AppendText(richTextLog, "Response : " + msg[i] + "\n", Color.Green);
                    StatusMessage = msg[i];
                }
                return StatusMessage;
            }
            catch (Exception ex)
            {
                AppendText(richTextLog, "Status : ERROR. " + ex.Message + "\n", Color.Red);
                FTPSocket.Close();
                return "";
            }
        }
        private void FTPSecureLogin()
        {
            
            if (Logged)
                CloseConnection();
            IPAddress remoteAddress = null;
            IPEndPoint addrEndPoint = null;
            AppendText(richTextLog, "Status : Opening Connection to : " + Server + "\n", Color.Red);
            try
            {
                FTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AppendText(richTextLog, "Status : Resolving IP Address\n", Color.Red);

                remoteAddress = Dns.GetHostEntry(Server).AddressList[0];
                AppendText(richTextLog, "Status : IP Address Found ->" + remoteAddress.ToString() + "\n", Color.Red);
                addrEndPoint = new IPEndPoint(remoteAddress, Port);
                AppendText(richTextLog, "Status : EndPoint Found ->" + addrEndPoint.ToString() + "\n", Color.Red);
                FTPSocket.Connect(addrEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppendText(richTextLog, "Status : Couldn't connect to remote server. " + ex.Message + "\n", Color.Red);
                return;
            }
            ReadResponse();
            if (StatusCode != 220) //220->Server Ready for New User
            {
                LogOut();
                CloseConnection();
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red); //Error
                return;
            }
            EncryptedStream = false;
            SendCommand("AUTH TLS");
            OpenSslStream(FTPSocket);
            EncryptedStream = true;
            try
            {
                if (FTPSocket == null || FTPSocket.Connected == false)
                    AppendText(richTextLog, "Status : Couldn't connect to remote server. " + "\n", Color.Red);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new IOException("Couldn't connect to remote server");
            }

            SendCommand("USER " + UserName);
            if (!(StatusCode == 331 || StatusCode == 230) || StatusCode == 530) //230->Logged in , 331->UserName Okey,Need Password , 530->Login Fail
            {
                //Something Wrong!
                LogOut();
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }

            if (StatusCode != 230) //If Not Logged in!
            {
                SendCommand("PASS " + Password);
                if (!(StatusCode == 230 || StatusCode == 202)) //202 ->Command Not implemented(Password Not Required)
                {
                    //Something Wrong
                    LogOut();
                    AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                    return;
                }
            }
           // ReadResponse();

            Logged = true;
            AppendText(richTextLog, "Status : Connected to " + Server + "\n", Color.Black);
            ChangeWorkingDirectory(Path);



        }
        private void FTP_Login()
        {
            EncryptedStream = false;
            if (Logged)
                CloseConnection();
            IPAddress remoteAddress = null;
            IPEndPoint addrEndPoint = null;
            AppendText(richTextLog, "Status : Opening Connection to : " + Server + "\n", Color.Red);
            try
            {
                FTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AppendText(richTextLog, "Status : Resolving IP Address\n", Color.Red);
                remoteAddress = Dns.GetHostEntry(Server).AddressList[0];
                AppendText(richTextLog, "Status : IP Address Found ->" + remoteAddress.ToString() + "\n", Color.Red);
                addrEndPoint = new IPEndPoint(remoteAddress, Port);
                AppendText(richTextLog, "Status : EndPoint Found ->" + addrEndPoint.ToString() + "\n", Color.Red);
                FTPSocket.Connect(addrEndPoint);
            }
            catch (Exception ex)
            {
                if (FTPSocket != null && FTPSocket.Connected)
                {
                    FTPSocket.Close();
                }
                AppendText(richTextLog, "Status : Couldn't connect to remote server. " + ex.Message + "\n", Color.Red);
                return;
            }
            ReadResponse();
            if (StatusCode != 220) //220->Server Ready for New User
            {
                CloseConnection();
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red); //Error
                return;
            }
            EncryptedStream = false;
            SendCommand("USER " + UserName);
            if (!(StatusCode == 331 || StatusCode == 230) || StatusCode == 530) //230->Logged in , 331->UserName Okey,Need Password , 530->Login Fail
            {
                //Something Wrong!
                LogOut();
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            if (StatusCode != 230) //If Not Logged in!
            {
                SendCommand("PASS " + Password);
                if (!(StatusCode == 230 || StatusCode == 202)) //202 ->Command Not implemented(Password Not Required)
                {
                    //Something Wrong
                    LogOut();
                    AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                    return;
                }
            }
            //ReadResponse();
            Logged = true;
            AppendText(richTextLog, "Status : Connected to " + Server + "\n", Color.Red);
            ChangeWorkingDirectory(Path);
        }
        private void ChangeWorkingDirectory(string Path)
        {
            if (Path == null || Path.Length == 0)
            {
                AppendText(richTextLog, "Status : Directory Was Not Found\n", Color.Red);
                Changed = false;
                return;
            }
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                Changed = false;
                return;
            }
            SendCommand("CWD " + Path);
            if (StatusCode != 250)  //250->Requested file action okay
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                Changed = false;
                return;
            }
            SendCommand("PWD");// PWD -> Print Working Directory
            if (StatusCode != 257) //257->"PATHNAME" created.
            {
                AppendText(richTextLog, "Status :" + Result.Substring(4) + "\n", Color.Red);
                Changed = false;
                return;
            }
            Path = StatusMessage.Split('"')[1]; //Get Response Path
            AppendText(richTextLog, "Status : Current Working Directory is " + Path + "\n", Color.Red);
            Changed = true;
        }
        public string[] GetListFiles()
        {
            if (!Logged)
                AppendText(richTextLog, "Status : You Need To Log In First\n", Color.Red);
            DataSock = OpenSocketForTransfer();
            if (DataSock == null)
            {
                AppendText(richTextLog, "Status : Socket Error\n", Color.Red);
                return Msg;
            }
            SendCommand("NLST");

            if (!(StatusCode == 150 || StatusCode == 125 || StatusCode==226)) //150->File status okay , 125->Data connection already open; transfer starting
            {
                AppendText(richTextLog, "status : " + Result + "\n", Color.Red);
                return Msg;
            }
            StatusMessage = "";
            DateTime timeout = DateTime.Now.AddSeconds(60);
            while (timeout > DateTime.Now)
            {
                int Bytes = DataSock.Receive(buffer, buffer.Length, 0);
                StatusMessage += Encoding.ASCII.GetString(buffer, 0, Bytes);
                if (Bytes < buffer.Length) break;
            }
            AppendText(richTextLog, "status : " + StatusMessage + "\n", Color.Gold);
            Msg = StatusMessage.Replace("\r", "").Split('\n');
            DataSock.Close();
            if (StatusMessage.Contains("No files found"))
                Msg = new string[] { };
            if (EncryptedStream == true)
            {
                ReadResponse();
            }
            if (StatusCode != 226) //226->Closing data connection. Requested file action successful
                Msg = new string[] { };
            return Msg;
        }
        private Socket OpenSocketForTransfer()
        {
            SendCommand("PASV");
            if (StatusCode != 227) //227->Succed
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
            //response from server is the IP and port number for the client in "(" & ")"
            Socket tranferSocket = null;
            IPEndPoint ipEndPoint = null;
            int indx1 = Result.IndexOf('(');
            int indx2 = Result.IndexOf(')');
            string IpPort = Result.Substring((indx1 + 1), (indx2 - indx1) - 1);
            int[] Parts = new int[6];
            int PartCount = 0;
            string Buffer = "";
            for (int i = 0; i < IpPort.Length && PartCount <= 6; i++)
            {
                char chr = char.Parse(IpPort.Substring(i, 1)); //Convert To Char
                if (char.IsDigit(chr)) //Are Chars Numeric? 
                    Buffer += chr;
                else if (chr != ',') //Pasv Result should come only numeric and ',' Chars
                {
                    AppendText(richTextLog, "Status : Wrong PASV result->" + Result, Color.Red);
                    return null;
                }
                else
                {
                    if (chr == ',' || i + 1 == IpPort.Length)
                    {
                        try
                        {
                            Parts[PartCount++] = int.Parse(Buffer);
                            Buffer = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            AppendText(richTextLog, "Status : Wrong PASV result (not supported?): " + Result + "\n", Color.Red);
                            return null;
                        }
                    }
                }
            }
            Parts[PartCount] = int.Parse(Buffer);
            string ipAddress = Parts[0] + "." + Parts[1] + "." + Parts[2] + "." + Parts[3];
            int port = (Parts[4] << 8) + Parts[5];  //Parts[4] <<8 = Parts[4]*256
            try
            {
                tranferSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                tranferSocket.Connect(ipEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (tranferSocket != null && tranferSocket.Connected) tranferSocket.Close();
                AppendText(richTextLog, "Status : Can't connect to remote server ->" + ex.Message + " \n", Color.Red);
                return null;
            }
            return tranferSocket;
        }
        private void UploadDirectory(string LocalPath, string FTPPath, string RootDirName, TreeNode ParentNode)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            NewFoldertoTreeView(RootDirName, ParentNode);
            ChangeWorkingDirectory(FTPPath + "/" + RootDirName);
            if (!Changed)
                return;
            foreach (string item in Directory.GetFiles(LocalPath))
            {
                upfilesize = (long)item.Length;
                UploadFile(item);
            }
            if (Directory.GetDirectories(LocalPath).Length == 0)
                return;
            string NewFtpPath = FTPPath + "/" + RootDirName;
            TreeNode NewParent = new TreeNode();
            NewParent = ParentNode.Nodes[RootDirName + "_"];
            foreach (string directory in Directory.GetDirectories(LocalPath))
            {
                string temp = directory.Substring(directory.LastIndexOf('\\') + 1);
                UploadDirectory(directory, NewFtpPath, temp, NewParent);
            }
            AppendText(richTextLog, "Status : All Directory And Included Files Uploaded Sucessfully\n", Color.Red);
        }
        private void NewFoldertoTreeView(string Text, TreeNode ParentNode)
        {
            if (ServerView.SelectedNode != null && Text == "")
            {
                TreeNode temp = new TreeNode();
                temp.Name = "New Folder" + "_";
                temp.Text = "New Folder";
                if (ServerView.SelectedNode.Text != @"/")
                    temp.Tag = ServerView.SelectedNode.Tag.ToString().Trim(' ') + @"/" + "New Folder";
                else
                    temp.Tag = @"/" + "New Folder";
                temp.ImageIndex = 1;
                ServerView.SelectedNode.Nodes.Add(temp);
                ServerView.SelectedNode.Expand();
                ServerView.LabelEdit = true;
                temp.BeginEdit();
            }
            else if (Text != "" && ParentNode != null)
            {
                TreeNode temp = new TreeNode();
                temp.Name = Text + "_";
                temp.Text = Text;
                if (Text != @"/")
                    temp.Tag = ParentNode.Tag.ToString().Trim(' ') + @"/" + Text;
                else
                    temp.Tag = @"/" + Text;
                temp.ImageIndex = 1;
                ParentNode.Nodes.Add(temp);
                ParentNode.Expand();
                CreateDirectory(temp.Tag.ToString(), temp.Index);
            }
        }
        private void RemoveDirectory(string DirPath)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            if (DirPath == null || DirPath.Equals(".") || DirPath.Length == 0)
            {
                AppendText(richTextLog, "Status : A directory name wasn't provided. Please provide one and try your request again.\n", Color.Red);
                return;
            }
            SendCommand("RMD " + DirPath);
            if (StatusCode != 250)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(richTextLog, "Statu : Removed directory " + DirPath + "\n", Color.Red);
            ServerView.SelectedNode.Remove();
        }
        private void CreateDirectory(string DirPath, int DirIndex)
        {
            if (!Logged)
            {
                AppendText(richTextLog, "Status : Login First Please\n", Color.Red);
                ServerView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            if (DirPath == null || DirPath.Equals(".") || DirPath.Length == 0)
            {
                AppendText(richTextLog, "Status : A directory name wasn't provided. Please provide one and try your request again.\n", Color.Red);
                ServerView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            SendCommand("MKD " + DirPath);
            if (StatusCode != 250 && StatusCode != 257)
            {
                AppendText(richTextLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                ServerView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            AppendText(richTextLog, "Status : Created directory to" + DirPath + "\n", Color.Red);
        }
        private void CloseConnection()
        {
            AppendText(richTextLog, "Status : Closing Connection to " + Server + "\n", Color.Red);
            if (FTPSocket != null)
            {
                SendCommand("QUIT");
            }
            LogOut();
        }
        private void LogOut()
        {
            if (FTPSocket != null)
            {
                FTPSocket.Close();
                FTPSocket = null;
            }
            if (DataSock != null)
            {
                DataSock.Close();
                DataSock = null;
            }
            if (_sslStream != null)
            {
                _sslStream.Close();
                _sslStream = null;
            }
            Logged = false;
        }
        #endregion


        #region Form Functions3ؤ
        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            if (text.Contains("PASS"))
            {
                int len = text.Length - 16;
                text = "Command : PASS ";
                for (int i = 0; i < len; i++)
                    text += "*";
                text += "\n";
            }
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        private void ParseDirNames(TreeNode ParentNode)
        {
            GetListFiles();
            ServerFiles.Items.Clear();
            int Temp;
            string Temp_Name = "";
            if (Msg.Length != 0)
            {
                for (int i = 0; i < Msg.Length - 1; i++)
                {
                    
                    Temp = Msg[i].ToString().IndexOf(".");
                    if (Temp == -1)
                    {
                        Temp_Name = Msg[i].Substring(0, Msg[i].Length);
                        AddTreeNode(ParentNode, "dir", Temp_Name);
                    }
                    else if (Temp != -1)
                    {
                        Temp_Name = Msg[i].Substring(0, Msg[i].Length);
                        AddListServerFilesItem(ServerView.SelectedNode.Tag + "/" + Temp_Name.Trim(' '), Temp_Name.Trim(' '));
                    }
                }
                ServerView.Nodes[0].Expand();
            }
            else return;
        }
        private void AddTreeNode(TreeNode ParentNode, string NodeType, string Text)
        {
            TreeNode temp = new TreeNode();
            temp.Name = Text + "_";
            temp.Text = Text.Trim(' ');
            if (ParentNode.Text != @"/")
                temp.Tag = ParentNode.Tag.ToString().Trim(' ') + @"/" + Text.Trim(' ');
            else
                temp.Tag = @"/" + Text.Trim(' ');
            if (NodeType == "dir")
                temp.ImageIndex = 1;
            ParentNode.Nodes.Add(temp);
        }
        private void AddListServerFilesItem(string Path, string Name)
        {
            
            ServerFiles.SmallImageList = lstServerImages;
            ServerFiles.Items.Add(Name);
            ServerFiles.Items[ServerFiles.Items.Count - 1].SubItems.Add(FileSize(Path).ToString() + " Bytes");
            ServerFiles.Items[ServerFiles.Items.Count - 1].ImageIndex = lstServerImages.Images.Count - 1;
            try
            {
                fi = new FileInfo(Name);
            }
            catch
            {
                
                ServerFiles.Items[ServerFiles.Items.Count - 1].SubItems.Add("");
                return;
            }
            ServerFiles.Items[ServerFiles.Items.Count - 1].SubItems.Add(GetFileType(fi.Extension));
        }
        private void richTextLog_TextChanged(object sender, EventArgs e)
        {
            richTextLog.ScrollToCaret();
        }
        private void removeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveDirectory(ServerView.SelectedNode.Tag.ToString());
        }
        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ServerFiles.SelectedItems.Count > 0)
                ServerFiles.SelectedItems[0].BeginEdit();
        }
        private void ServerFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            OldName = ServerView.SelectedNode.Tag + "/" + ServerFiles.SelectedItems[0].Text;
        }
        private void ServerFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != "" && e.Label != null)
                RenameFile(OldName, e.Label);
        }
        private void newDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFoldertoTreeView("", null);
        }
        private void Dpwork(object sender, DoWorkEventArgs e)
        {     
                backgroundWorker1.ReportProgress((int)percentage);
        }
        private void Progresschanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar   
            progressBar1.Value = e.ProgressPercentage;
            
        }
        private void setClientDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            folderBrowserDialog1.ShowDialog();
            clientdir = folderBrowserDialog1.SelectedPath.ToString();
            AppendText(richTextLog, clientdir + "\n", Color.Red);
            directoryInfo = new DirectoryInfo(clientdir);
            
            FTPSClient_Load(sender,e);
            
            
            
        }
        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ServerFiles.Items.Count; i++)
            {
                if (ServerFiles.Items[i].Selected == true)
                    DeleteFile(ServerFiles.Items[i].Text);
            }
            if (Deleted)
                ServerFiles.SelectedItems[0].Remove();
        }
        private string GetFileType(string ext)
        {
            RegistryKey rKey = null;
            RegistryKey sKey = null;
            string FileType = "";
            try
            {
                rKey = Registry.ClassesRoot;
                sKey = rKey.OpenSubKey(ext);
                if (sKey != null && (string)sKey.GetValue("", ext) != ext)
                {
                    sKey = rKey.OpenSubKey((string)sKey.GetValue("", ext));
                    FileType = (string)sKey.GetValue("");
                }
                else
                    FileType = ext.Substring(ext.LastIndexOf('.') + 1).ToUpper() + " File";
                return FileType;
            }
            finally
            {
                if (sKey != null) sKey.Close();
                if (rKey != null) rKey.Close();
            }
        }
        private void ServerFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo directory in directories)
            {
                if (directory.Name == ClientView.SelectedNode.Text)
                {
                     Path = directory.FullName;
                }
            }
                    if (ServerFiles.SelectedItems.Count != 0)
            {
                for (int i = 0; i < ServerFiles.Items.Count; i++)
                {
                    if (ServerFiles.Items[i].Selected)
                        DownloadFile(ServerView.SelectedNode.Tag + "/" + ServerFiles.Items[i].Text, Path + "\\" + ServerFiles.Items[i].Text);
                }
            }
            ClientView.Refresh();
            ClientView.BeginUpdate();
            ClientView.EndUpdate();
        }
        private void ServerView_AfterSelect(object sender, TreeViewEventArgs e)
        {
             
            if (ServerView.SelectedNode.Index != -1)
            {
                ServerView.SelectedNode.Nodes.Clear();
                ChangeWorkingDirectory(ServerView.SelectedNode.Tag.ToString());
                if (!Changed)
                    return;
                ParseDirNames(ServerView.SelectedNode);
                for (int i = 0; i < ServerFiles.Items.Count; i++)
                {
                    if (ServerFiles.Items[i].SubItems[2].Text == "")
                    {
                        try
                        {
                            fi2 = new FileInfo(ServerFiles.Items[i].Text);
                        }
                        catch
                        {
                            ServerFiles.Items[i].SubItems.Add("");
                            return;
                        }
                        ServerFiles.Items[i].SubItems[2].Text = fi2.Extension.ToUpper().Trim('.') + " File";
                    }
                }
            }
            
        }
        private void ServerView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != "" && e.Label != null)
            {
                e.Node.Tag = e.Node.Tag.ToString().Replace("New Folder", e.Label);
                CreateDirectory(e.Node.Tag.ToString(), e.Node.Index);
            }
            if (e.Label == null)
            {
                AppendText(richTextLog, "Status : You Should Give a Name to Your Directory\n", Color.Red);
                e.Node.Remove();
            }
        }
        private void sendCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string send = Interaction.InputBox("String to Send : ", "Send String", "", this.Location.X, this.Location.Y);
            if (send != "" || send != null)
                SendCommand(send);
            else
                MessageBox.Show("ERROR", "Bad Characters in Send String");
        }
        private void FTPSClient_Load(object sender, EventArgs e)
        {
            
            if (Directory.Exists(@"/"))
            {
                try
                {
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();

                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        if (file.Exists)
                        {
                            
                            TreeNode nodes = ClientView.Nodes[0].Nodes.Add(file.Name);
                            nodes.ImageIndex = nodes.SelectedImageIndex = 1;
                        }
                    }
                    
                    if (directories.Length > 0)
                    {
                        foreach (DirectoryInfo directory in directories)
                        {
                            TreeNode node = ClientView.Nodes[0].Nodes.Add(directory.Name);
                            node.ImageIndex = node.SelectedImageIndex = 0;
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                if (file.Exists)
                                {
                                    TreeNode nodes = ClientView.Nodes[0].Nodes[node.Index].Nodes.Add(file.Name);
                                    nodes.ImageIndex = nodes.SelectedImageIndex = 2;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                AppendText(richTextLog, "not found.", Color.Red);
            }
        }
        private void toolStripMenuItem1UploadFile_Click(object sender, EventArgs e)
        {
            if (ClientView.SelectedNode != null)
            {
                try
                {
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                        foreach (DirectoryInfo directory in directories)
                        {
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                if (file.Exists && file.Name == ClientView.SelectedNode.Text)
                                {
                                    string pa = file.FullName;
                                    AppendText(richTextLog, "\n" + "Upload file from:" + pa + "\n", Color.Green);
                                    upfilesize = (long)file.Length;
                                    UploadFile(pa);
                                    return;
                                }
                            
                        }
                       
                        }
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void toolStripMenuItem2UploadDirectory_Click(object sender, EventArgs e)
        {
            if (ClientView.SelectedNode != null)
            {
                try
                {
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    if (directories.Length > 0)
                    {
                        foreach (DirectoryInfo directory in directories)
                        {
                            if (directory.Name == ClientView.SelectedNode.Text)
                            {
                                string pa = directory.FullName;
                                AppendText(richTextLog, "\n" + "Upload directory from:" + pa + "\n", Color.Green);
                                UploadDirectory(pa, ServerView.SelectedNode.Tag.ToString(), directory.Name, ServerView.SelectedNode);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void btn_desconnect_Click(object sender, EventArgs e)
        {
            if (Logged)
            {
                LogOut();
                
                btn_desconnect.Enabled = false;
                ServerView.Nodes.Clear();
                ServerView.Nodes.Add("Waiting..");
                
            }
        }
        private void btn_connect_Click(object sender, EventArgs e)
        {
            ServerView.Nodes[0].Nodes.Clear();
            if (ServerView.Nodes[0].Name != "main")
            {
                ServerView.Nodes[0].Remove();
                TreeNode main = new TreeNode();
                main.Name = "main";
                main.Text = @"/";
                ServerView.Nodes.Add(main);
            }
            Server = txtserver.Text;
            UserName = txtusername.Text;
            Password = txtpassword.Text;
            Port = int.Parse(txtport.Text);
            FTP_Login();
            EncryptedStream = false;
            if (Logged)
            {
                ParseDirNames(ServerView.Nodes[0]);
                btn_desconnect.Enabled = true;
            }
            else
                ServerView.Nodes[0].Nodes.Clear();
        }
        private void btn_Sconnect_Click(object sender, EventArgs e)
        {
            ServerView.Nodes[0].Nodes.Clear();
            if (ServerView.Nodes[0].Name != "main")
            {
                ServerView.Nodes[0].Remove();
                TreeNode main = new TreeNode();
                main.Name = "main";
                main.Text = @"/";
                ServerView.Nodes.Add(main);
            }
            Server = txtserver.Text;
            UserName = txtusername.Text;
            Password = txtpassword.Text;
            Port = int.Parse(txtport.Text);
            FTPSecureLogin();
            if (Logged)
            {
                ParseDirNames(ServerView.Nodes[0]);
                btn_desconnect.Enabled = true;
            }
            else
                ServerView.Nodes[0].Nodes.Clear();
        }
        #endregion
    }
}

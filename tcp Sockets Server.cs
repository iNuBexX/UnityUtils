using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading; 
using UnityEngine;  
using UnityEngine.UI;

public class TCPTestServer : MonoBehaviour {  
	public TMPro.TextMeshProUGUI thatext;

	
    private static TCPTestServer instance;
	string message = " ";
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	

	public reticlemover reticle;
	public portalMover teleport;

	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient;

	public CharacterControls controller;

	bool isRunning = true; 	
	
	public bool searching=false;
	#endregion 	
	// bool iscasting=false;
	// Use this for initialization
	void Start () { 	
		// int exindex= "someinput!102#69".IndexOf("!");
		// int shindex= "someinput!102#69".IndexOf("#");
			
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
	}  	
	
	// Update is called once per frame
	void Update () { 		
		//geting the inputs 
		// extractposition();
		if(controller!=null)
		Inputs();
		else
		if(!searching)
		StartCoroutine(search());

		
	}
	void Awake() 
    { 
    DontDestroyOnLoad (this);
         
     if (instance == null) {
        instance = this;
     } else {
         Destroy(gameObject);
     }
 
    DontDestroyOnLoad(transform.gameObject);

    
    
    }

	 void OnApplicationQuit()
	 {
	 	try
		{
			isRunning=false;
			connectedTcpClient.Close();

		}
		// catch(Exception e)
		catch(Exception e)
		{
			Debug.Log(e.Message);
		}

		// You must close the tcp listener
		//must also restart it when reloading the level
	}
	
	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  			
			while (isRunning) { 				
				using (connectedTcpClient = tcpListener.AcceptTcpClient()) { 					
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream()) { 						
						int length; 						
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 							
							var incommingData = new byte[length]; 							
							Array.Copy(bytes, 0, incommingData, 0, length);  							
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData); 							
							Debug.Log("client message received as: " + clientMessage); 	

							message=clientMessage;	
							// do logic based on this message 
							//from different scripts inside unity
                            //in case you wanted to break the connection
							// if(message =="stop")
							//break;	
									
						} 					
					} 				
				} 			
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 	

		}    
		finally
    	{
       		// Stop listening for new clients.
			print("stopped listening, server is shut down ");
       		tcpListener.Stop();
    	} 
	}  	
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	private void SendMessage() { 		
		if (connectedTcpClient == null) {           
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				string serverMessage = "This is a message from your server."; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	}
	void Inputs()
	{
			if(message != " ")
			{ 
				this.thatext.text=message;


			}	
	} 
    /// <summary>
    /// in case you need to reset the message
    // <summary>
	public void reset()
	{
		message ="";
	}






}
import socket
SERVER = "127.0.0.1"
PORT = 8080
#establish a connexion and send a message
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((SERVER, PORT))
client.sendall(bytes("hello"))
#in case you would like to receive data use this 
# bytes=client.recv(1024).decode()
# print(bytes)
#use loops and functions to your liking to repeat the communications above

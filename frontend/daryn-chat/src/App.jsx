import { useState } from "react";
import "./App.css";
import { Login } from "./components/Login";
import { WaitingRoom } from "./components/WaitingRoom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { Chat } from "./components/Chat";

export function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [connection, setConnection] = useState(null);
  const [chatRoom, setChatRoom] = useState("");
  const [messages, setMessages] = useState([]);

  const joinToChat = async (userName, chatRoom) => {
    var connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5186/chat")
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveMessage", (userName, message) => {
      console.log(`Received message: ${message} from user: ${userName}`);
      setMessages((messages) => [...messages, { userName, message }]);
    });

    try {
      await connection.start();
      await connection.invoke("JoinToChat", { userName, chatRoom });

      setConnection(connection);
      setChatRoom(chatRoom);
    } catch (error) {
      console.log(error);
    }
  };

  const sendMessage = (message) => {
    connection.invoke("SendMessage", message);
  };

  const closeChat = async () => {
    await connection.stop();
    setConnection(null);
  };


  const handleLoginSuccess = () => {
    setIsAuthenticated(true);
  };

  return (
    <div>
    {isAuthenticated ? (
        connection ? (
          <Chat
            messages={messages}
            chatRoom={chatRoom}
            sendMessage={sendMessage}
            closeChat={closeChat}
          />
        ) : (
          <WaitingRoom joinToChat={joinToChat} />
        )
      ) : (
        <Login onLoginSuccess={handleLoginSuccess} />
      )}
    </div>
  );
}

export default App;

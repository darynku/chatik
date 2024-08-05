import { useState, useEffect, useRef } from "react";
import { Message } from "./Message";
import '../index.css'
export const Chat = ({ chatRoom, closeChat, messages, sendMessage }) => {
  const [message, setMessage] = useState('');
  const messagesEndRef = useRef(null);

  const handleSendMessage = () => {
    if (message.trim() !== '') { // Проверка на пустое сообщение
      sendMessage(message);
      setMessage(''); // Очистка поля ввода после отправки
    }
  };

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const handleKeyDown = (event) => {
    if (event.key === 'Enter') {
      handleSendMessage();
    }
  };

  const handleChange = (event) => {
    setMessage(event.target.value); // Обновление состояния сообщения
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  return (
    <div className="flex flex-col max-w-lg mx-auto bg-gray-100 rounded-lg shadow-lg p-4">
      <div className="flex justify-between items-center border-b pb-2 mb-4">
        <h2 className="text-xl font-bold text-gray-800">{chatRoom}</h2>
        <button onClick={closeChat} className="text-red-500 hover:text-red-700">
          &times;
        </button>
      </div>

      <div className="flex-1 overflow-y-auto max-h-80 mb-4 hide-scrollbar">
        {messages.map((messageInfo, index) => (
          <Message messageInfo={messageInfo} key={index} />
        ))}
        {/* Добавляем реф для прокрутки */}
        <div ref={messagesEndRef} />
      </div>

      <div className="flex items-center border-t pt-2">
        <input
          type="text"
          value={message}
          onKeyDown={handleKeyDown}
          onChange={handleChange}
          placeholder="Enter message"
          className="flex-1 p-2 border border-gray-300 rounded-l focus:outline-none focus:border-blue-500"
        />
        <button
          onClick={handleSendMessage}
          className="bg-blue-500 text-white p-2 rounded-r hover:bg-blue-600"
        >
          Send
        </button>
      </div>
    </div>
  );
};
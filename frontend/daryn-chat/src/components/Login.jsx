import { useState } from 'react';
import axios from 'axios';

export const Login = ({ onLoginSuccess }) => {

  

  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault(); // Предотвращает автоматическую отправку формы

    try {
      const response = await axios.post('authentication/login', {
        userName,
        password,
      });

      if (response.status === 200) {
        const { token } = response.data;
        console.log('Token received:', token);
        localStorage.setItem('token', token);
        onLoginSuccess();
      } else {
        alert('Failed to login');
      }
    } catch (error) {
      console.error('Login error:', error);
      alert('Failed to login');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col max-w-md mx-auto p-4">
      <h2 className="text-2xl font-bold mb-4">Login</h2>
      <input
        type="text"
        value={userName}
        onChange={(e) => setUserName(e.target.value)} // Обработчик изменения текста
        placeholder="Username"
        className="p-2 mb-4 border rounded"
        required
      />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)} // Обработчик изменения текста
        placeholder="Password"
        className="p-2 mb-4 border rounded"
        required
      />
      <button 
      type="submit" 
      className="p-2 bg-blue-500 text-white rounded">
        Login
      </button>
    </form>
  );
};

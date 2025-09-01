// src/App.js
import React, { useState, useEffect } from 'react';
import './App.css'; // Keep your existing App.css if you have custom styles
import LoginPage from './components/loginPage';
import Dashboard from './components/dashboard';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [clientRib, setClientRib] = useState(null);
  const [clientFullName, setClientFullName] = useState(null);

  // Check for stored login info on component mount (e.g., from localStorage)
  useEffect(() => {
    const storedRib = localStorage.getItem('clientRib');
    const storedFullName = localStorage.getItem('clientFullName');
    if (storedRib && storedFullName) {
      setClientRib(storedRib);
      setClientFullName(storedFullName);
      setIsLoggedIn(true);
    }
  }, []);

  const handleLoginSuccess = (rib, fullName) => {
    setIsLoggedIn(true);
    setClientRib(rib);
    setClientFullName(fullName);
    // Store login info (e.g., in localStorage) for persistence
    localStorage.setItem('clientRib', rib);
    localStorage.setItem('clientFullName', fullName);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setClientRib(null);
    setClientFullName(null);
    // Clear stored login info
    localStorage.removeItem('clientRib');
    localStorage.removeItem('clientFullName');
  };

  return (
    <div className="App">
      {isLoggedIn ? (
        <Dashboard
          rib={clientRib}
          fullName={clientFullName}
          onLogout={handleLogout}
        />
      ) : (
        <LoginPage onLoginSuccess={handleLoginSuccess} />
      )}
    </div>
  );
}

export default App;

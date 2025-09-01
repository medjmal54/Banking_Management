// src/components/LoginPage.js
import React, { useState } from 'react';

function LoginPage({ onLoginSuccess }) {
  const [rib, setRib] = useState('');
  const [fullName, setFullName] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      // Replace with your actual backend API URL
      const response = await fetch('http://localhost:5124/api/Auth/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ rib, fullName }),
      });

      if (response.ok) {
        // Assuming successful login means we can proceed to dashboard
        // In a real app, you'd get a token or session info here
        onLoginSuccess(rib, fullName); // Pass RIB and FullName to parent for dashboard
      } else {
        const errorText = await response.text();
        setError(errorText || 'Login failed. Please check your credentials.');
      }
    } catch (err) {
      console.error('Network error during login:', err);
      setError('Network error. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold text-center text-gray-800 mb-6">Login to Banking System</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label htmlFor="rib" className="block text-gray-700 text-sm font-bold mb-2">
              RIB
            </label>
            <input
              type="text"
              id="rib"
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Enter your RIB"
              value={rib}
              onChange={(e) => setRib(e.target.value)}
              required
            />
          </div>
          <div className="mb-6">
            <label htmlFor="fullName" className="block text-gray-700 text-sm font-bold mb-2">
              Full Name
            </label>
            <input
              type="text"
              id="fullName"
              className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Enter your Full Name"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              required
            />
          </div>
          {error && (
            <p className="text-red-500 text-xs italic mb-4">{error}</p>
          )}
          <div className="flex items-center justify-between">
            <button
              type="submit"
              className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline w-full"
              disabled={loading}
            >
              {loading ? 'Logging in...' : 'Login'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default LoginPage;

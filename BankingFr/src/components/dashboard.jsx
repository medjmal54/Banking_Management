import { useEffect, useState } from 'react';

function Dashboard({ rib, fullName, onLogout }) {
  const [analytics, setAnalytics] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showTransferForm, setShowTransferForm] = useState(false);

  useEffect(() => {
    const fetchAnalytics = async () => {
      setLoading(true);
      setError(null);
      try {
        const url = `http://localhost:5124/api/Auth/ClientsAnalytics/${encodeURIComponent(rib)}`;
        const response = await fetch(url);
        if (!response.ok) {
          throw new Error('Failed to fetch analytics');
        }
        const data = await response.json();
        setAnalytics(data);
      } catch (err) {
        setError(err.message || 'Error fetching analytics');
      } finally {
        setLoading(false);
      }
    };

    fetchAnalytics();
  }, [rib]);

  // Transfer form state
  const [transferData, setTransferData] = useState({
    cvv: '',
    expirationDate: '',
    amount: '',
    ribTo: '',
  });
  const [transferError, setTransferError] = useState(null);
  const [transferSuccess, setTransferSuccess] = useState(null);
  const [transferLoading, setTransferLoading] = useState(false);

  const handleTransferChange = (e) => {
    setTransferData({ ...transferData, [e.target.name]: e.target.value });
  };

  const handleTransferSubmit = async (e) => {
    e.preventDefault();
    setTransferError(null);
    setTransferSuccess(null);
    setTransferLoading(true);

    if (!transferData.cvv || !transferData.expirationDate || !transferData.amount || !transferData.ribTo) {
      setTransferError('Please fill in all fields');
      setTransferLoading(false);
      return;
    }

    try {
      const response = await fetch('http://localhost:5124/api/Auth/MakeTransfer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify({
          rib,
          cvv: transferData.cvv,
          expirationDate: transferData.expirationDate,
          amount: parseFloat(transferData.amount),
          ribTo: transferData.ribTo,
        }),
      });

      if (!response.ok) {
        const errText = await response.text();
        throw new Error(errText || 'Transfer failed');
      }

      setTransferSuccess('Transfer successful!');
      setTransferData({ cvv: '', expirationDate: '', amount: '', ribTo: '' });

      setTimeout(() => setTransferSuccess(null), 7000);

      const refreshed = await fetch(`http://localhost:5124/api/Auth/ClientsAnalytics/${encodeURIComponent(rib)}`);
      const refreshedData = await refreshed.json();
      setAnalytics(refreshedData);
      setShowTransferForm(false);
    } catch (err) {
      setTransferError(err.message);
    } finally {
      setTransferLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-blue-700 text-white p-4 shadow-md flex justify-between items-center">
        <h1 className="text-2xl font-semibold tracking-wide">Banking Dashboard</h1>
        <div className="flex items-center space-x-4">
          <span className="text-lg font-semibold text-yellow-300">
            Welcome, <span className="underline text-yellow-400">{fullName}</span> (<span className="text-yellow-400">{rib}</span>)
          </span>
          <button
            onClick={onLogout}
            className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline transition"
          >
            Logout
          </button>
        </div>
      </header>

      <main className="p-6 max-w-5xl mx-auto">
        {loading && <p className="text-gray-600 text-center">Loading your analytics...</p>}
        {error && <p className="text-red-700 font-semibold text-center mb-4">{error}</p>}

        {transferSuccess && (
          <div className="mb-4 p-4 bg-green-100 border border-green-400 text-green-800 rounded shadow-md text-center font-semibold">
            {transferSuccess}
          </div>
        )}

        {analytics && (
          <>
            <section className="mb-8">
              <h2 className="text-3xl font-bold mb-6 text-blue-800 border-b-4 border-blue-600 pb-2 select-none">
                Your Analytics
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="bg-white p-6 rounded shadow hover:shadow-lg transition cursor-default">
                  <h3 className="font-semibold mb-2 text-indigo-700">Country</h3>
                  <p className="text-indigo-900 text-lg">{analytics.country || 'N/A'}</p>
                </div>
                <div className="bg-white p-6 rounded shadow hover:shadow-lg transition cursor-default">
                  <h3 className="font-semibold mb-2 text-indigo-700">Credit Card RIB</h3>
                  <p className="text-indigo-900 text-lg">{analytics.creditCard?.rib || 'N/A'}</p>
                </div>
                <div className="bg-white p-6 rounded shadow hover:shadow-lg transition cursor-default">
                  <h3 className="font-semibold mb-2 text-indigo-700">Number of Loans</h3>
                  <p className="text-indigo-900 text-lg">{analytics.loans?.length || 0}</p>
                </div>
              </div>
            </section>

            <section className="mb-8">
              <h2 className="text-3xl font-bold mb-6 text-teal-800 border-b-4 border-teal-600 pb-2 select-none">
                Recent Transactions
              </h2>
              {analytics.transactions && analytics.transactions.length > 0 ? (
                <div className="overflow-x-auto rounded shadow-lg">
                  <table className="min-w-full bg-white rounded">
                    <thead className="bg-teal-700 text-white select-none">
                      <tr>
                        <th className="py-3 px-6 text-left font-semibold">Date</th>
                        <th className="py-3 px-6 text-left font-semibold">Amount</th>
                        <th className="py-3 px-6 text-left font-semibold">Type</th>
                        <th className="py-3 px-6 text-left font-semibold">Recipient RIB</th>
                      </tr>
                    </thead>
                    <tbody>
                      {analytics.transactions.slice(0, 100).map((tx, index) => (
                        <tr
                          key={index}
                          className="hover:bg-teal-50 cursor-pointer transition-colors duration-200"
                          title={`Transaction on ${tx.tranDate ? new Date(tx.tranDate).toLocaleDateString() : '-'}`}
                        >
                          <td className="py-3 px-6 border-b text-teal-900">{tx.tranDate ? new Date(tx.tranDate).toLocaleDateString() : '-'}</td>
                          <td className="py-3 px-6 border-b text-teal-900">${tx.amount?.toFixed(2) ?? '-'}</td>
                          <td className="py-3 px-6 border-b text-teal-900">{tx.tranType}</td>
                          <td className="py-3 px-6 border-b text-teal-900">{tx.ribTo || '-'}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              ) : (
                <p className="italic text-gray-500 select-none">No transactions found.</p>
              )}
            </section>

            <section className="mb-8">
              <h2 className="text-3xl font-bold mb-6 text-purple-800 border-b-4 border-purple-600 pb-2 select-none">
                Loans
              </h2>
              {analytics.loans && analytics.loans.length > 0 ? (
                <div className="overflow-x-auto rounded shadow-lg">
                  <table className="min-w-full bg-white rounded">
                    <thead className="bg-purple-700 text-white select-none">
                      <tr>
                        <th className="py-3 px-6 text-left font-semibold">Loan ID</th>
                        <th className="py-3 px-6 text-left font-semibold">Loan Type</th>
                        <th className="py-3 px-6 text-left font-semibold">Status</th>
                        <th className="py-3 px-6 text-left font-semibold">Payments</th>
                      </tr>
                    </thead>
                    <tbody>
                      {analytics.loans.map((loan, index) => (
                        <tr
                          key={index}
                          className="hover:bg-purple-50 cursor-pointer transition-colors duration-200"
                        >
                          <td className="py-3 px-6 border-b text-purple-900">{loan.loanId}</td>
                          <td className="py-3 px-6 border-b text-purple-900">{loan.loanType}</td>
                          <td className="py-3 px-6 border-b text-purple-900">{loan.loanStatus}</td>
                          <td className="py-3 px-6 border-b text-purple-900">
                            {loan.loanPayment && loan.loanPayment.length > 0 ? (
                              <ul className="list-disc list-inside">
                                {loan.loanPayment.map((payment, pIndex) => (
                                  <li key={pIndex}>
                                    Paid: ${payment.amountPaid?.toFixed(2) ?? '-'}, Remaining: ${payment.rmainingBalance?.toFixed(2) ?? '-'}
                                  </li>
                                ))}
                              </ul>
                            ) : (
                              <span className="italic text-gray-500">No payments</span>
                            )}
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              ) : (
                <p className="italic text-gray-500 select-none">No loans found.</p>
              )}
            </section>

            <div className="text-center">
              <button
                onClick={() => setShowTransferForm(true)}
                className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-6 rounded focus:outline-none focus:shadow-outline transition"
              >
                Transfer Money
              </button>
            </div>
          </>
        )}

        {showTransferForm && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-lg relative">
              <h3 className="text-xl font-bold mb-4 text-gray-900">Transfer Money</h3>
              {transferError && <p className="text-red-700 mb-2 font-semibold">{transferError}</p>}
              {transferSuccess && <p className="text-green-700 mb-2 font-semibold">{transferSuccess}</p>}
              <form onSubmit={handleTransferSubmit} className="space-y-4">
                <div>
                  <label htmlFor="cvv" className="block font-semibold mb-1 text-gray-800">
                    CVV
                  </label>
                  <input
                    type="text"
                    id="cvv"
                    name="cvv"
                    value={transferData.cvv}
                    onChange={handleTransferChange}
                    className="w-full border border-gray-300 rounded px-3 py-2 text-black focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                    maxLength={3}
                    required
                  />
                </div>
                <div>
                  <label htmlFor="expirationDate" className="block font-semibold mb-1 text-gray-800">
                    Expiration Date (MM/YY)
                  </label>
                  <input
                    type="text"
                    id="expirationDate"
                    name="expirationDate"
                    value={transferData.expirationDate}
                    onChange={handleTransferChange}
                    className="w-full border border-gray-300 rounded px-3 py-2 text-black focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                    placeholder="MM/YY"
                    maxLength={5}
                    required
                  />
                </div>
                <div>
                  <label htmlFor="amount" className="block font-semibold mb-1 text-gray-800">
                    Amount
                  </label>
                  <input
                    type="number"
                    id="amount"
                    name="amount"
                    value={transferData.amount}
                    onChange={handleTransferChange}
                    className="w-full border border-gray-300 rounded px-3 py-2 text-black focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                    min="0.01"
                    step="0.01"
                    required
                  />
                </div>
                <div>
                  <label htmlFor="ribTo" className="block font-semibold mb-1 text-gray-800">
                    Recipient RIB
                  </label>
                  <input
                    type="text"
                    id="ribTo"
                    name="ribTo"
                    value={transferData.ribTo}
                    onChange={handleTransferChange}
                    className="w-full border border-gray-300 rounded px-3 py-2 text-black focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                    required
                  />
                </div>
                <div className="flex justify-end space-x-4 mt-4">
                  <button
                    type="button"
                    onClick={() => {
                      setShowTransferForm(false);
                      setTransferError(null);
                      setTransferSuccess(null);
                    }}
                    className="bg-gray-400 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded transition"
                    disabled={transferLoading}
                  >
                    Cancel
                  </button>
                  <button
                    type="submit"
                    className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded transition"
                    disabled={transferLoading}
                  >
                    {transferLoading ? 'Transferring...' : 'Transfer'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}

export default Dashboard;
 
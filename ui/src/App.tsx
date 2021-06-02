import React from 'react';
import FileBrowser from './components/FileBrowser';

import './App.css';

function App() {
  return (
    <div className="App">
      <div style={{ width: 800, margin: "0 auto", marginTop: 50 }}>
        <FileBrowser></FileBrowser>
      </div>
    </div>
  );
}

export default App;

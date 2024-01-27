
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import SignIn from './pages/SignIn';
import SignUp from './pages/SignUp';
import Pocetna from './pages/Pocetna';
import Admin from './pages/Admin';
import ZaboravljenaSifra from './pages/ZaboravljenaSifra';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SignIn />} />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/pocetna" element={<Pocetna/>}/>
        <Route path="/admin" element={<Admin/>}/>
        <Route path="/izmena-naloga" element={<ZaboravljenaSifra/>}/>
      </Routes>
    </Router>
  );
}

export default App;

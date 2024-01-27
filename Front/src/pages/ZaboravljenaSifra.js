
import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './ZaboravljenaSifra.css';

const IzmenaNaloga = () => {
  const [email, setEmail] = useState('');
  const [username, setUsername] = useState('');
  const [newPassword, setNewPassword] = useState('');

  const navigate = useNavigate();

  const handleUpdatePassword = async () => {
    console.log(email, newPassword);
  try {
    const url = `https://localhost:7193/Korisnik/PromeniSifru?email=${encodeURIComponent(email)}&newPassword=${encodeURIComponent(newPassword)}`;

    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (response.ok) {
      navigate('/');
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.indexOf("application/json") !== -1) {
        const data = await response.json();
        console.log('Šifra je uspešno promenjena:', data);
        window.alert(data.message);  // Prikazi poruku iz odgovora
      } else {
        // Ako nije JSON odgovor, obradi tekstualni odgovor
        const text = await response.text();
        console.log('Odgovor od servera:', text);
        window.alert(text);
      }
    } else {
      // Obrada greške ako server nije uspešno odgovorio
      const errorData = await response.json();
      console.error('Greška prilikom promene šifre:', errorData);
      window.alert('Greška prilikom promene šifre: ' + errorData.message);
    }
  } catch (error) {
    console.error('Greška prilikom slanja zahteva:', error);
    window.alert('Greška prilikom slanja zahteva: ' + error.message);
  }
};

  return (
    <div className='izmena-naloga-container'>
      <h2>Promena lozinke</h2>
      <div>
        <label>Email:</label>
        <input type="text" value={email} onChange={(e) => setEmail(e.target.value)} />
      </div>
      <div>
        <label>Novi password:</label>
        <input type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} />
      </div>
      <div>
        <button onClick={handleUpdatePassword}>Izmeni password</button>
      </div>
      <div>
        <p>
          Ne zelite da menjate sifru? <Link to="/">Prijavite se</Link>
        </p>
      </div>
    </div>
  );
};

export default IzmenaNaloga;

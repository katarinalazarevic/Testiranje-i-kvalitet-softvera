import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './SignUp.css'; 
import axios from 'axios';

const SignUp = () => {
  const [ime, setIme] = useState('');
  const [prezime, setPrezime] = useState('');
  const [sifra, setSifra] = useState('');
  const [grad, setGrad] = useState('');
  const [broj, setBroj] = useState('');
  const [ulica, setUlica] = useState('');
  const [email, setEmail] = useState('');
  const [brojTelefona, setBrojTelefona] = useState('');
  const navigate = useNavigate();
  
  const handleSignUp = async () => {
    console.log(ime, prezime, email, sifra, grad, broj, ulica, brojTelefona);
    try {
      const response = await fetch("https://localhost:7193/Korisnik/Register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ime: ime,
          prezime: prezime,
          email: email,
          password: sifra,
          grad: grad,
          ulica: ulica,
          broj: broj,
          kontaktTelefon: brojTelefona,
        }),
      });
  
      console.log(response); 
  
      const data = await response.text();

    if (response.status === 200 || response.status === 201) {
      window.alert("Uspešna registracija:", data);
      navigate("/pocetna");
    } else {
      console.log("Neuspešna registracija! Proverite status i poruku odgovora.");
      window.alert(`Neuspešna registracija: ${data}`);
    }
  } catch (error) {
    console.error("Došlo je do greške prilikom registracije:", error);
    window.alert("Neuspešna registracija!");
  }
};
  
  
  const handleImeChange = (event) => {
    setIme(event.target.value);
  };

  const handlePrezimeChange = (event) => {
    setPrezime(event.target.value);
  };

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setSifra(event.target.value);
  };

  

  const handleGradChange = (event) => {
    setGrad(event.target.value);
  };

  const handleBrojChange = (event) => {
    setBroj(event.target.value);
  };

  const handleUlicaChange = (event) => {
    setUlica(event.target.value);
  };

  const handleBrojTelefonaChange = (event) => {
    setBrojTelefona(event.target.value);
  };

  return (
    <div className='signup-container'>
      <h2>Napravite nalog:</h2>
      <div>
        <label>Ime:</label>
        <input type="text" value={ime} onChange={(e) => setIme(e.target.value)} />
      </div>
      <div>
        <label>Prezime:</label>
        <input type="text" value={prezime} onChange={(e) => setPrezime(e.target.value)} />
      </div>
      <div>
        <label>Šifra:</label>
        <input type="password" value={sifra} onChange={(e) => setSifra(e.target.value)} />
      </div>
      
      <div>
        <label>Grad:</label>
        <input type="text" value={grad} onChange={(e) => setGrad(e.target.value)} />
      </div>

      <div>
        <label>Ulica:</label>
        <input type="text" value={ulica} onChange={(e) => setUlica(e.target.value)} />
      </div>
      <div>
        <label>Broj:</label>
        <input type="text" value={broj} onChange={(e) => setBroj(e.target.value)} />
      </div>
      <div>
        <label>Email:</label>
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
      </div>
      <div>
        <label>Broj telefona:</label>
        <input type="tel" value={brojTelefona} onChange={(e) => setBrojTelefona(e.target.value)} />
      </div>
      <div>
        <button onClick={handleSignUp}>Registruj se</button>
      </div>
    </div>
  );
};

export default SignUp;

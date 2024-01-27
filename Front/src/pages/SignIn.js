import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './SignIn.css';
import axios from 'axios'

const SignIn = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const navigate = useNavigate();
  const handleSignIn = async () => {
    console.log(email, password);
    try {
      const response = await fetch("https://localhost:7193/Korisnik/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
        }),
      });
  
      if (response.status === 200) {
        if (email.includes("@admin")) {
          navigate('/admin');
        } else {
          localStorage.setItem("email",email);
          navigate('/pocetna');
        }
      } else {
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.indexOf("application/json") !== -1) {
          const data = await response.json();
          alert(`Neuspela prijava: ${data.errors}`);
        } else {
          const text = await response.text();
          alert(`Neuspela prijava - tekst od servera: ${text}`);
        }
      }
    } catch (error) {
      console.error('Greška prilikom slanja zahteva:', error);
    }
  };
  

  return (
    <div className='signin-container'>
      <h2>Prijavite se</h2>
      <div>
        <label>Email:</label>
        <input type="text" value={email} onChange={(e) => setEmail(e.target.value)} />
      </div>
      <div>
        <label>Password:</label>
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
      </div>
      <div>
        <button onClick={handleSignIn}>Prijava</button>
      </div>
      <div>
        <p>Nemate nalog? <Link to="/signup">Kreirajte nalog</Link></p>
        <p> <Link to="/izmena-naloga">Zaboravili ste lozinku?</Link></p>
      </div>
    </div>
  );
};

export default SignIn;

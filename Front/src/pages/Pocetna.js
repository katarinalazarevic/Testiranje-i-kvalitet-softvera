import React, { useState, useEffect } from 'react';
import './Pocetna.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBasketShopping, faPalette } from '@fortawesome/free-solid-svg-icons';

function Pocetna() {
  const [filters, setFilters] = useState({
    imeUmetnika: '',
    nazivUmetnickogDela: '',
    dimenzije: 'Sve dimenzije',
    ram: 'Svi ramovi',
    tehnika: 'Sve tehnike',
  });
  const storedEmail=localStorage.getItem('email');
  
  const [korpa, setKorpa] = useState([]);
  const [nazivSlike,setNazivSlike]=useState([]);
  const [dimenzijeOpcije, setDimenzijeOpcije] = useState([]);
  const [ramOpcije, setRamOpcije] = useState([]);
  const [tehnikaOpcije, setTehnikaOpcije] = useState([]);
  const [umetnici, setUmetnici] = useState([]); 
  const [slike, setSlike] = useState([]);
  const putanja = '../../images/';

  useEffect(() => {
    const fetchUmetnickaDela = async () => {
      try {
        const response = await fetch('https://localhost:7193/Kolekcija/VratiUmetnickaDela');
        const data = await response.json();
        const sveDimenzije = [...new Set(data.map((slika) => slika.dimenzije))];
        const sviRamovi = [...new Set(data.map((slika) => slika.ram))];
        const sveTehnike = [...new Set(data.map((slika) => slika.tehnika))];
        const sviUmetnici = [...new Set(data.map((slika) => slika.kolekcija.umetnik))];

        setDimenzijeOpcije(['Sve dimenzije', ...sveDimenzije]);
        setRamOpcije(['Svi ramovi', ...sviRamovi]);
        setTehnikaOpcije(['Sve tehnike', ...sveTehnike]);
        setUmetnici(['', ...sviUmetnici]); 

        setSlike(data);
      } catch (error) {
        console.error('Greška prilikom dohvatanja umetničkih dela:', error);
      }
    };

    fetchUmetnickaDela();
  }, []);

  const ukupnaCena = korpa.reduce((acc, slika) => acc + slika.cena, 0);

  const handleMenuChange = (value, filterName) => {
    setFilters((prevFilters) => ({ ...prevFilters, [filterName]: value }));
  };

  const dodajUKorpu = (slika) => {
    setKorpa([...korpa, slika]);
    setNazivSlike([...nazivSlike,slika.naziv]);
  };

  const platiHandler=async ()=>{
    var kolicina=korpa.length
    console.log(korpa,nazivSlike,kolicina,ukupnaCena);
    console.log(storedEmail);
    try {
      const response = await fetch("https://localhost:7193/Narudzbina/NapraviNarudzbinu", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          nazivaDela:nazivSlike ,
          emailKorisnik:storedEmail,
          kolicina: kolicina,
          ukupnaCena: ukupnaCena
        }),
      });
  
      if (response.status === 200) {
        const data= await response.text();
        window.alert(data);
      } else {
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.indexOf("application/json") !== -1) {
          const data = await response.json();
          alert(`Neuspela kupovina: ${data.errors}`);
        } else {
          const text = await response.text();
          alert(`Neuspela kupovina - tekst od servera: ${text}`);
        }
      }
    } catch (error) {
      console.error('Greška prilikom slanja zahteva:', error);
    }
    setKorpa([]);
    setNazivSlike([]);
  };
  const ukloniIzKorpe = (index,naziv) => {
    console.log(naziv);
  const novaKorpa = [...korpa];
  novaKorpa.splice(index, 1);
  setKorpa(novaKorpa);

  const noviNazivi = [...nazivSlike];
  const indeksZaUklanjanje = noviNazivi.indexOf(naziv);

  if (indeksZaUklanjanje !== -1) {
    noviNazivi.splice(indeksZaUklanjanje, 1);
    setNazivSlike(noviNazivi);
  }
  };

  const filtriraneSlike = slike
    .filter((slika) => (filters.imeUmetnika !== '' ? slika.kolekcija.umetnik === filters.imeUmetnika : true))
    .filter((slika) => (filters.dimenzije === 'Sve dimenzije' ? true : slika.dimenzije === filters.dimenzije))
    .filter((slika) => (filters.ram === 'Svi ramovi' ? true : slika.ram === filters.ram))
    .filter((slika) => (filters.tehnika === 'Sve tehnike' ? true : slika.tehnika === filters.tehnika))
    .filter((slika) => (filters.nazivUmetnickogDela === '' ? true : slika.naziv === filters.nazivUmetnickogDela));

  return (
    <div className="veliki-container">
      <div className="naslov-container">
        <h1 className="naslov">
          <FontAwesomeIcon className="logo" icon={faPalette} />
          <strong> KA-TI</strong> art shop{' '}
        </h1>
      </div>
      <div className="pocetna-container">
        <div className="levi-div">
          <div>
            <label>Ime umetnika:</label>
            <select onChange={(e) => handleMenuChange(e.target.value, 'imeUmetnika')}>
              {umetnici.map((umetnik, index) => (
                <option key={index} value={umetnik}>
                  {umetnik === '' ? 'Svi umetnici' : umetnik}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label>Naziv umetničkog dela:</label>
            <select onChange={(e) => handleMenuChange(e.target.value, 'nazivUmetnickogDela')}>
              <option value="">Sva umetnička dela</option>
              {slike.map((slika, index) => (
                <option key={index} value={slika.naziv}>
                  {slika.naziv}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>Dimenzije:</label>
            <select onChange={(e) => handleMenuChange(e.target.value, 'dimenzije')}>
              {dimenzijeOpcije.map((opcija, index) => (
                <option key={index} value={opcija}>
                  {opcija}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>Ram:</label>
            <select onChange={(e) => handleMenuChange(e.target.value, 'ram')}>
              {ramOpcije.map((opcija, index) => (
                <option key={index} value={opcija}>
                  {opcija}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>Tehnika:</label>
            <select onChange={(e) => handleMenuChange(e.target.value, 'tehnika')}>
              {tehnikaOpcije.map((opcija, index) => (
                <option key={index} value={opcija}>
                  {opcija}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="slike-container">
          {/* Prikazi slike prema filterima */}
          {filtriraneSlike.map((slika, index) => (
            <div key={index} className="slika-kvadratic">
              <img src={putanja + slika.slika} alt={slika.naziv} />
              <div className="naziv-slike">{slika.naziv}</div>
              <div className="autor">{slika.kolekcija.umetnik}</div>
              <div className="cena">Cena: {slika.cena} dinara</div>
              <button onClick={() => dodajUKorpu(slika)}>Ubaci u korpu</button>
            </div>
          ))}
        </div>

        <div className="korpa-container">
          <h2>
            <FontAwesomeIcon className="logo" icon={faBasketShopping} /> Korpa
          </h2>
          <ul>
            {korpa.map((slika, index) => (
              <li key={index} className="korpa-stavka">
                <span className="naziv-span">{slika.naziv}</span>
                <span>Cena: {slika.cena} dinara</span>
                <button className="ukloni-stavku" onClick={() => ukloniIzKorpe(index,slika.naziv)}>
                  Ukloni
                </button>
              </li>
            ))}
          </ul>
          <hr />
          <div className="ukupna-cena">
            <p>Ukupna cena: {ukupnaCena} dinara</p>
            <button disabled={korpa.length === 0}onClick={() => platiHandler()}>Plati</button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Pocetna;

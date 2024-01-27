import React, { useState } from 'react';
import './Admin.css'; 
import axios from 'axios';

const Admin = () => {
  const [selectedButton, setSelectedButton] = useState(null);
  const [novaKolekcija, setNovaKolekcija] = useState({
    nazivKolekcije: '',
    umetnik: '',
    godina: '',
    stil: ''
  });
  const [novoUmetnickoDelo, setNovoUmetnickoDelo]= useState({
    nazivKolekcije: '',
    naziv: '',
    tehnika: '',
    dimenzije: '',
    cena: '',
    ram: '',
    slika:''
  });

  const [showData, setShowData] = useState(false);
  const [korisnici, setKorisnici] = useState([]);
  const [showVratiSvaUmetnickaDelaInput, setShowVratiSvaUmetnickaDelaInput] = useState(false);
  const [umetnickaDela, setUmetnickaDela] = useState([]);
  const [showVratiSveKorisnikeInput, setShowVratiSveKorisnikeInput] = useState(false);
  const [kolekcije, setKolekcije] = useState([]);
  const [showVratiSveKolekcijeInput, setShowVratiSveKolekcijeInput] = useState(false);
  const [narudzbine, setNarudzbine]=useState([]);
  const [showVratiSveNarudzbineInput,setShowVratiSveNarudzbineInput]=useState(false);
  const [idKolekcijeZaBrisanje, setIdKolekcijeZaBrisanje]=useState('');
  const [idKorisnikaZaBrisanje, setIdKorisnikaZaBrisanje]=useState('');
  const [idKolekcijeZaIzmenu, setIdKolekcijeZaIzmenu] = useState('');
  const [idUmetnickogDelaZaIzmenu, setIdUmetnickogDelaZaIzmenu]= useState('');
  const [noviNazivUmetnickogDela, setNoviNazivUmetnickogDela]=useState('');
  const [idUmetnickogDelaZaBrisanje, setIdUmetnickogDelaZaBrisanje] = useState('');
  const [noviNazivKolekcije, setNoviNazivKolekcije] = useState('');
  const [noviUmetnik, setNoviUmetnik] = useState('');
  const [novaGodina, setNovaGodina] = useState('');
  const [noviStil, setNoviStil] = useState('');
  const [nazivKolekcije, setNazivKolekcije] = useState('');
  const [nazivUmetnickogDela, setNazivUmetnickogDela] = useState('');
  const [tehnika, setTehnika] = useState('');
  const [dimenzije, setDimenzije] = useState('');
  const [cena, setCena] = useState('');
  const [ram, setRam] = useState('');
  const [narudzbinaIdZaIzmenu, setNarudzbinaIdZaIzmenu] = useState('');
  const [novaCenaNarudzbine, setNovaCenaNarudzbine] = useState('');
  const [idNarudzbineZaBrisanje, setIdNarudzbineZaBrisanje] = useState('');




  const handleButtonClick = async (button) => {
    if (selectedButton === button) {
      // Ako se ponovo klikne na isto dugme, sakrij podatke
      setSelectedButton(null);
      setShowData(false);
      setShowVratiSveKolekcijeInput(false); 
    } else {
      setSelectedButton(button);
      setShowData(true);
      setShowVratiSveKolekcijeInput(false); 
  
      // Fetch podataka na osnovu odabranog dugmeta
      switch (button) {
        case 'Vrati korisnike':
          await fetchKorisnici();
          break;
        case 'Vrati kolekcije':
          await fetchKolekcije();
          break;
        case 'Vrati umetnicka dela':
          await fetchUmetnickaDela();
          break;
        case 'Vrati narudzbine':
          await fetchNarudzbine();
          break;
        default:
          break;
}
}};
      const fetchKorisnici = async () => {
        try {
          const response = await axios.get('https://localhost:7193/Korisnik/PrikaziKorisnike', {
            headers: {
              'Content-Type': 'application/json',
            }
          });
          
          if (response.status === 200) {
            const data = response.data;
            console.log('Korisnici:', data);
            setKorisnici(data);
            setShowVratiSveKorisnikeInput(true);
            console.log('Korisnici:', data);
          } else {
            console.error('Greška prilikom vraćanja korisnika');
          }
        } catch (error) {
          console.error(error);
        }
      };
      
      const fetchKolekcije = async () => {
        try {
          const response = await axios.get('https://localhost:7193/Kolekcija/VratiKolekcije', {
            headers: {
              'Content-Type': 'application/json',
            }
          });
      
          if (response.status === 200) {
            const data = response.data;
            setKolekcije(data);
            setShowVratiSveKolekcijeInput(true);
            console.log('Dobijene kolekcije:', data);
          } else {
            console.error('Greška prilikom vraćanja kolekcija');
          }
        } catch (error) {
          console.error(error);
        }
      };
      const fetchUmetnickaDela = async () => {
        try {
          const response = await axios.get('https://localhost:7193/Kolekcija/VratiUmetnickaDela', {
            headers: {
              'Content-Type': 'application/json',
            }
          });
      
          if (response.status === 200) {
            const data = response.data;
            setUmetnickaDela(data);
            setShowVratiSvaUmetnickaDelaInput(true);
            console.log('Dobijena umetnička dela:', data);
          } else {
            console.error('Greška prilikom vraćanja umetničkih dela');
          }
        } catch (error) {
          console.error(error);
        }
      };
      const fetchNarudzbine = async () => {
        try {
          const response = await axios.get('https://localhost:7193/Narudzbina/VratiNarudzbine', {
            headers: {
              'Content-Type': 'application/json',
            }
          });
      
          if (response.status === 200) {
            const data = response.data;
            setNarudzbine(data);
            setShowVratiSveNarudzbineInput(data);
            console.log('Dobijene narudžbine:', data);
          } else {
            console.error('Greška prilikom vraćanja narudžbina');
          }
        } catch (error) {
          console.error(error);
        }
      };
      const handleDodajKolekciju = async () => {
        try {
          const response = await axios.post('https://localhost:7193/Kolekcija/DodajKolekciju', novaKolekcija, {
            headers: {
              'Content-Type': 'application/json',
            }
          });
      
          if (response.status === 200) {
            window.alert( response.data);
            
          } else {
            window.alert(response.data);
          }
        } catch (error) {
          if (error.response) {

            console.error('Odgovor sa servera:', error.response.data);
            window.alert('Greška sa servera: ' + error.response.data);
          } else if (error.request) {
            console.error('Greška prilikom slanja zahteva:', error.request);
          } else {
            console.error('Neidentifikovana greška:', error.message);
          }
        }
      };
  
      const handleObrisiKolekciju = () => {
          console.log(idKolekcijeZaBrisanje)
          fetch(`https://localhost:7193/Kolekcija/ObrisiKolekciju/${idKolekcijeZaBrisanje}`, {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
            }
          })
          .then(odgovor => odgovor.text())
          .then(odgovorText => {
            window.confirm(odgovorText);
          })
          .catch((error) => {
            console.log(error);
          });
          setIdKolekcijeZaBrisanje('');
      };
      const handleObrisiKorisnika = () => {
        try{
          console.log(idKorisnikaZaBrisanje)
        fetch(`https://localhost:7193/Korisnik/ObrisiKorisnika/${idKorisnikaZaBrisanje}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
          }
        })
        .then(odgovor => odgovor.text())
        .then(odgovorText => {
          window.confirm(odgovorText);
        })
        .catch((error) => {
          console.log(error);
        });
        }
        catch(error){

        }
        setIdKorisnikaZaBrisanje('');
      };
      const handleObrisiUmetnickoDelo = () => {
        try {
          console.log(idUmetnickogDelaZaBrisanje);
          fetch(`https://localhost:7193/Kolekcija/ObrisiUmetnickoDelo/${idUmetnickogDelaZaBrisanje}`, {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
            },
          })
            .then((odgovor) => odgovor.text())
            .then((odgovorText) => {
              window.confirm(odgovorText);
            })
            .catch((error) => {
              console.log(error);
            });
        } catch (error) {
            //obrada
        }
        setIdUmetnickogDelaZaBrisanje('');
      };
      const handleObrisiNarudzbinu = async () => {
        try {
          const response = await fetch(`https://localhost:7193/Narudzbina/ObrisiNarudzbinu/${idNarudzbineZaBrisanje}`, {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
            },
          });
      
          if (response.ok) {
            const odgovorText = await response.text();
            window.confirm(odgovorText);
          } else {
            console.error('Greška prilikom brisanja narudžbine');
          }
        } catch (error) {
          console.log(error);
        }

        setIdNarudzbineZaBrisanje('');
      };
  
  
      const handleIzmeniKolekciju = async (id, newData) => {
        try {
          const response = await fetch(`https://localhost:7193/Kolekcija/IzmeniKolekciju/${id}`, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify(newData),
          });
      
          if (response.status === 200) {
            const data= await response.text();
        window.alert(data);
      } else {
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.indexOf("application/json") !== -1) {
          const data = await response.json();
          alert(` ${data.errors}`);
        } else {
          const text = await response.text();
          alert(` tekst od servera: ${text}`);
        }
      }
        } catch (error) {
          console.error('Greška prilikom slanja zahteva:', error);
          window.alert(`Greška prilikom slanja zahteva: ${error.message}`);
        }
      };
      
      const handleIzmeniUmetnickoDelo = async() => {
        try {
          console.log(idUmetnickogDelaZaIzmenu);
          const response = await fetch(`https://localhost:7193/Kolekcija/IzmeniUmetnickoDelo/${idUmetnickogDelaZaIzmenu}/${noviNazivUmetnickogDela}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (response.ok) {
      const successData = await response.text();
      window.confirm(successData);
    } else {
      const errorText = await response.text();
      window.confirm(`Neuspela izmena: ${errorText}`);
    }
  } catch (error) {
    console.error('Greška prilikom slanja zahteva:', error);
  }
        setIdUmetnickogDelaZaIzmenu('');
        setNoviNazivUmetnickogDela('');
      };
      
  
  
  const handleDodajUmetnickoDelo = async () => {
    try {
      const response = await axios.post('https://localhost:7193/Kolekcija/DodajUmetnickoDelo', novoUmetnickoDelo, {
        headers: {
          'Content-Type': 'application/json',
        },
      });
  
      if (response.status === 200) {
        window.alert(response.data);
      } else {
        window.alert(response.data);
      }
    } catch (error) {
      if (error.response) {
        console.error('Odgovor sa servera:', error.response.data);
        window.alert('Greška sa servera: ' + error.response.data);
      } else if (error.request) {
        console.error('Greška prilikom slanja zahteva:', error.request);
      } else {
        console.error('Neidentifikovana greška:', error.message);
      }
    }
  };
    const handleIzmeniNarudzbinu = async () => {
      try {
        const response = await fetch(`https://localhost:7193/Narudzbina/IzmeniNarudzbina?narudzbinaId=${narudzbinaIdZaIzmenu}&novaCena=${novaCenaNarudzbine}`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
        });
    
        if (response.status === 200) {
          const data= await response.text();
          window.alert(data);
        } else {
          const contentType = response.headers.get("content-type");
          if (contentType && contentType.indexOf("application/json") !== -1) {
            const data = await response.json();
            alert(` ${data.errors}`);
          } else {
            const text = await response.text();
            alert(` - tekst od servera: ${text}`);
          }
        }
      } catch (error) {
        console.error('Greška prilikom slanja zahteva:', error);
      }
    
      setNarudzbinaIdZaIzmenu('');
      setNovaCenaNarudzbine('');
    };
  
  
  const renderInputs = () => {
    switch (selectedButton) {
      case 'Kolekcija':
        return (
          <div>
          <div>
            <input
              type="text"
              placeholder="Naziv kolekcije"
              value={novaKolekcija.nazivKolekcije}
              onChange={(e) => setNovaKolekcija({ ...novaKolekcija, nazivKolekcije: e.target.value })}
            />
            <input
              type="text"
              placeholder="Umetnik"
              value={novaKolekcija.umetnik}
              onChange={(e) => setNovaKolekcija({ ...novaKolekcija, umetnik: e.target.value })}
            />
            <input
              type="number"
              placeholder="Godina"
              value={novaKolekcija.godina}
              onChange={(e) => setNovaKolekcija({ ...novaKolekcija, godina: e.target.value })}
            />
            <input
              type="text"
              placeholder="Stil"
              value={novaKolekcija.stil}
              onChange={(e) => setNovaKolekcija({ ...novaKolekcija, stil: e.target.value })}
            />
            <button className='u-inputu' onClick={handleDodajKolekciju}>
              Dodaj
            </button>
          </div>

          <div>
               <input
                  type="number"
                  placeholder="ID za brisanje"
                  value={idKolekcijeZaBrisanje}
                  onChange={(e) => setIdKolekcijeZaBrisanje(e.target.value)}
                />

                <button className='u-inputu' onClick={() => handleObrisiKolekciju(idKolekcijeZaBrisanje)}>
                  Obrisi</button>
          </div>
          <div>
            <button className='button-administrator' onClick={fetchKolekcije}>
              Vrati sve kolekcije
            </button>

            {showVratiSveKolekcijeInput && (
              <div>
                {kolekcije.length > 0 ? (
                  <table className='vrati-kolekcije'>
                    <thead>
                      <tr>
                      <th>ID </th>
                        <th>Naziv Kolekcije</th>
                        <th>Umetnik</th>
                        <th>Godina</th>
                        <th>Stil</th>
                      </tr>
                    </thead>
                    <tbody>
                      {kolekcije.map((kolekcija) => (
                        <tr key={kolekcija.id}>
                          <td>{kolekcija.id}</td>
                          <td>{kolekcija.nazivKolekcije}</td>
                          <td>{kolekcija.umetnik}</td>
                          <td>{kolekcija.godina}</td>
                          <td>{kolekcija.stil}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                ) : (
                  <p>Nema kolekcija.</p>
                )}
              </div>
            )}
          </div>

            
            <div>
                <input
                  type="text"
                  placeholder='ID'
                  value={idKolekcijeZaIzmenu}
                  onChange={(e) => setIdKolekcijeZaIzmenu(e.target.value)}
                />
                <input
                  type="text"
                  placeholder="Novi naziv kolekcije"
                  value={noviNazivKolekcije}
                  onChange={(e) => setNoviNazivKolekcije(e.target.value)}
                />
                <input
                  type="text"
                  placeholder="Novi umetnik"
                  value={noviUmetnik}
                  onChange={(e) => setNoviUmetnik(e.target.value)}
                />
                <input
                  type="number"
                  placeholder="Nova godina"
                  value={novaGodina}
                  onChange={(e) => setNovaGodina(e.target.value)}
                />
                <input
                  type="text"
                  placeholder="Novi stil"
                  value={noviStil}
                  onChange={(e) => setNoviStil(e.target.value)}
                />
                <button className='u-inputu' onClick={() => handleIzmeniKolekciju(idKolekcijeZaIzmenu, {
                  NazivKolekcije: noviNazivKolekcije,
                  Umetnik: noviUmetnik,
                  Godina: novaGodina,
                  Stil: noviStil,
                })}>
                  Izmeni
                </button>
              </div>
          </div>
        );
      case 'Korisnik':
        return (
          <div>
              <div>
                <input type="number" placeholder="ID za brisanje" value={idKorisnikaZaBrisanje} onChange={(e)=>setIdKorisnikaZaBrisanje(e.target.value)} />
                <button className='u-inputu' onClick={() => handleObrisiKorisnika()}>
                  Obrisi
                </button>
              </div>
              <button className='button-administrator' onClick={fetchKorisnici}>Vrati sve korisnike</button>
                  {showVratiSveKorisnikeInput && (
                    <div>
                      {korisnici.length > 0 ? (
                        <table className='vrati-korisnike'>
                          <thead>
                            <tr>
                              <th>ID</th>
                              <th>Ime</th>
                              <th>Prezime</th>
                              <th>Email</th>
                              <th>Kontakt Telefon</th>
                              <th>Ulica</th>
                              <th>Grad</th>
                              <th>Broj</th>
                            </tr>
                          </thead>
                          <tbody>
                            {korisnici.map((korisnik) => (
                              <tr key={korisnik.id}>
                                <td>{korisnik.id}</td>
                                <td>{korisnik.ime}</td>
                                <td>{korisnik.prezime}</td>
                                <td>{korisnik.email}</td>
                                <td>{korisnik.kontaktTelefon}</td>
                                <td>{korisnik.ulica}</td>
                                <td>{korisnik.grad}</td>
                                <td>{korisnik.broj}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      ) : (
                        <p>Nema korisnika.</p>
                      )}
                    </div>
                  )}


            </div>
          );
      case 'Umetnicko Delo':

        return (
          <div>
            <div>
              <input
                type="text"
                placeholder="Naziv kolekcije"
                value={novoUmetnickoDelo.nazivKolekcije}
                onChange={(e) => {
                  setNovoUmetnickoDelo({ ...novoUmetnickoDelo, nazivKolekcije: e.target.value });
                }}
              />
              <input
                type="text"
                placeholder="Naziv umetnickog dela"
                value={novoUmetnickoDelo.naziv}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, naziv: e.target.value })}
              />
              <input
                type="text"
                placeholder="Tehnika"
                value={novoUmetnickoDelo.tehnika}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, tehnika: e.target.value })}
              />
              <input
                type="text"
                placeholder="Dimenzije"
                value={novoUmetnickoDelo.dimenzije}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, dimenzije: e.target.value })}
              />
              <input
                type="number"
                placeholder="Cena"
                value={novoUmetnickoDelo.cena}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, cena: e.target.value })}
              />
              <input
                type="text"
                placeholder="Ram"
                value={novoUmetnickoDelo.ram}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, ram: e.target.value })}
              />
              <input
                type="text"
                placeholder="Slika"
                value={novoUmetnickoDelo.slika}
                onChange={(e) => setNovoUmetnickoDelo({ ...novoUmetnickoDelo, slika: e.target.value })}
              />
              <button className='u-inputu' onClick={handleDodajUmetnickoDelo}>
                Dodaj
              </button>
  </div>

            <div>
              <input
                type="number"
                placeholder="ID za brisanje"
                value={idUmetnickogDelaZaBrisanje}
                onChange={(e) => setIdUmetnickogDelaZaBrisanje(e.target.value)}
              
              />
              <button className='u-inputu' onClick={handleObrisiUmetnickoDelo}>
                Obrisi
              </button>
            </div>

            <div>
          <button className='u-inputu' onClick={fetchUmetnickaDela}>
            Vrati Umetnicka Dela
          </button>
          {showVratiSvaUmetnickaDelaInput && (    
            <div>      
            {umetnickaDela.length > 0 ? (
            <table className='vrati-umetnicka-dela'>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Naziv</th>
                  <th>Tehnika</th>
                  <th>Dimenzije</th>
                  <th>Cena</th>
                  <th>Ram</th>
                  <th>Slika</th>
                </tr>
              </thead>
              <tbody>
                {umetnickaDela.map((umetnickoDelo) => (
                  <tr key={umetnickoDelo.id}>
                    <td>{umetnickoDelo.id}</td>
                    <td>{umetnickoDelo.naziv}</td>
                    <td>{umetnickoDelo.tehnika}</td>
                    <td>{umetnickoDelo.dimenzije}</td>
                    <td>{umetnickoDelo.cena}</td>
                    <td>{umetnickoDelo.ram}</td>
                    <td>{umetnickoDelo.slika}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <p>Nema umetničkih dela.</p>
          )}
          </div>
          )}

        </div>

            <div>
              <input type="number" placeholder="ID umetnickog dela" value={idUmetnickogDelaZaIzmenu}
                  onChange={(e) => setIdUmetnickogDelaZaIzmenu(e.target.value)}
                  />
              <input type="text" placeholder="Novi naziv umetnickog dela" value={noviNazivUmetnickogDela}
                  onChange={(e) => setNoviNazivUmetnickogDela(e.target.value)}/>
              <button className='u-inputu' onClick={handleIzmeniUmetnickoDelo}>Izmeni</button>
            </div>
          </div>
);
      case 'Narudzbina':
        return (
            <div>
  
              <div>
              <input
                type="number"
                placeholder="ID za brisanje"
                value={idNarudzbineZaBrisanje}
                onChange={(e) => setIdNarudzbineZaBrisanje(e.target.value)}
              />
              <button className='u-inputu' onClick={handleObrisiNarudzbinu}>
                Obrisi
              </button>
            </div>

  
            <div>
                <button className='u-inputu' onClick={fetchNarudzbine}>Vrati Narudzbine</button>
                {showVratiSveNarudzbineInput && (
                    <div>
                      {narudzbine.length > 0 ? (
                        <table className='vrati-narudzbine'>
                          <thead>
                            <tr>
                            <th>ID</th>
                            <th>Datum</th>
                            <th>Ukupna Cena</th>
                            <th>Email narucioca</th>
                            <th>Umetnicka dela</th>
                            </tr>
                          </thead>
                          <tbody>
                              {narudzbine.map((narudzbina) => (
                              <tr key={narudzbina.narudzbinaId}>
                                <td>{narudzbina.narudzbinaId}</td>
                                <td>{new Date(narudzbina.datum).toLocaleDateString()}</td>
                                <td>{narudzbina.ukupnaCena}</td>
                                <td>{narudzbina.narucilacEmail}</td>
                                <td>{narudzbina.umetnickaDela}</td>

                              </tr>
                            ))}
                          </tbody>
                        </table>
                      ) : (
                        <p>Nema dostupnih narudzbina.</p>
                      )}
                    </div>
                  )}
    </div>
  
              <div>
                <input
                  type="text"
                  placeholder="ID narudžbine"
                  value={narudzbinaIdZaIzmenu}
                  onChange={(e) => setNarudzbinaIdZaIzmenu(e.target.value)}
                />
                <input
                  type="number"
                  placeholder="Nova ukupna cena"
                  value={novaCenaNarudzbine}
                  onChange={(e) => setNovaCenaNarudzbine(e.target.value)}
                />
                <button className='u-inputu' onClick={handleIzmeniNarudzbinu}>
                  Izmeni
                </button>
              </div>
            </div>
          );
      default:
        return null;
    }
  };

  return (
    <div className="main-container">
      <div className="buttons-container">
        <button onClick={() => handleButtonClick('Korisnik')}>Korisnik</button>
        <button onClick={() => handleButtonClick('Kolekcija')}>Kolekcija</button>
        <button onClick={() => handleButtonClick('Umetnicko Delo')}>Umetnicko Delo</button>
        <button onClick={() => handleButtonClick('Narudzbina')}>Narudzbina</button>
      </div>

      <div className="operations-container">
        {renderInputs()}
      </div>
    </div>
  );
};

export default Admin;
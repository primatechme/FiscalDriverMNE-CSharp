# Biblioteka Primatech.FondZdravstvaWS 

.NET client za pristup web servisu FZOCG.

U folderu Primatech.FondZdravstvaWS.Sample se nalazi primjer korišćenja biblioteke. Pogledati fajl *Primatech.FondZdravstvaWS.Sample/Program.cs*


## Konfiguracija
 
Instancirati prvo *config* objekat sa parametrima za apoteku.

    var config = new FondZdravstvaWSConfig {
        // Obavezno - sifra koju su sve ustanove dobile od fonda
        SifraUstanove = USTANOVA_ID,
        
        // Sifra organizacione jedinice - dobija se pozivom 
        // sifarnika GetOrganizacioneJedinice
        OrgJedinicaId = ORG_JEDINICA,
        
        // Korisnicko ime i lozinka koje su ustanove dobile od fonda, potpisom ugovora
        Username = USERNAME,
        Password = PASSWORD
    };
    
## Klijent

U konstruktoru servisa predati *config* objekat.<br />
**Napomena**: Sve metode iz oba servisa su implemetarana u ovom klijentu.

    var client = new FondZdravstvaWSClient(config);
    
## Poziv metoda servisa

Primjer pozivanja sifarnika proizvodjaca.

    var result = client.GetProizvodjaci();
    
## Slanje fajla (sa putanjom)

Za slanje fajla direktno, predati punu putanju do fajla.

    string fileName=<enter full path here>;
    var result = client.PostLagerFromFile(fileName);

# Problemi
## Verzija Visual Studio
Koristiti VS2015 i novije. Za starije VS zamjeniti liniju 
      
      if (Int32.TryParse(res,var out broj))
sa
      
      var broj = 0;
      if (Int32.TryParse(res, out broj))
            
## Greska "The request was aborted: Could not create SSL/TLS secure channel"
Prije instanciranja Configa dodati sledece dvije linije koda.

    ServicePointManager.Expect100Continue = true;
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

# Open source licenca



    The MIT License

    Copyright (c) 2018 Primatech, doo. http://www.primatech.me

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.

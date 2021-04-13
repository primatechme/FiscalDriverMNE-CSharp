# FiscalDriverMNE-CSharp
Fiskalni drajver je namjenjen za komunikaciju sa servisom Poreske uprave Crne Gore. 

# Kako koristiti FiscalDriverMNE
Za rad fiskalnog drajvera potrebno je obezbjediti:
1. Certifikat za fiskalizaciju
2. Unijeti podatke na portal Poreske uprave o poslovnom prostoru i operaterima
3. Dobiti aktivacioni kod za upotrebu biblioteka
4. U kodu softvera slati kod poslovnog prostora i operatera (uz ostale podatke o racunu ili depozitu)

Za detalje nas mozete kontaktirati na support@primatech.me.

__Nacin rada:__ 
1. Ukoliko se kreiraju samo bezgotovinski racuni, depozit se ne salje. 
2. Za gotovinske racune, svaki radni dan je neophodno zapoceti unosom depozita.

# Prvi korak
U projektu se nalaze dva __*.dll__ fajla koja treba dodati u projekat. 
To su:
1. _Primatech.FiscalDriver_
2. _Primatech.FiscalModels_

Pogledati na kraju dokumenta moguce greske pri kompajliranju.
 
# Kreiranje depozita

Inicijalni depozit (nula ili iznos veci od nule) se moze poslati samo jednom na pocetku radnog dana. Ukoliko je bilo kucanja racuna tog dana, depozit se vise ne moze poslati.
U slucaju da zelimo povuci novac iz kase saljemo iznos manji od nule. 

__Bitno:__ Za gotovinske racune, svaki radni dan je neophodno zapoceti unosom depozita.
```
var deposit = DepositBuilder.Build("**********", 25m)
                 //.SetTime(DateTime.Now)
                 //.SetAmount(25m)
                 .SetUser("Marko Markovic", "**********");
```
# Kreiranje racuna

## Greska u nekompatibilnosti *Newtonsoft.Json* biblioteke
Potrebno je povesti racuna o verziji __NewtSoft.Json__ biblioteke, ukoliko se u projektu koristi druga verzija, od one koja je u navedenim bibliotekama. 
U tom slucaju u AppConfig fajlu dodati sledece:

```
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
  <dependentAssembly>
    <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
    <bindingRedirect oldVersion="0.0.0.0-11.0.0.0" newVersion="11.0.0.0" />
  </dependentAssembly>
  ..
</runtime>
```

## Greska "The request was aborted: Could not create SSL/TLS secure channel"
Prije pozivanja servisa u projektu dodati sledece dvije linije koda.

    ServicePointManager.Expect100Continue = true;
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

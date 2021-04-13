# FiscalDriverMNE-CSharp
Fiskalni drajver je namjenjen za komunikaciju sa servisom Poreske uprave Crne Gore. 

# Kako koristiti FiscalDriverMNE
Za rad fiskalnog drajvera potrebno je obezbjediti:
1. Certifikat za fiskalizaciju.
2. Unijeti podatke na portal Poreske uprave o poslovnom prostoru i operaterima.
3. Dobiti aktivacioni kod za upotrebu biblioteka.
4. U kodu softvera slati kod poslovnog prostora i operatera (uz ostale podatke o racunu ili depozitu).

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

Prvo instanciramo model depozita. U konstruktoru se predaje kod elektronskog naplatnog uredjaja i iznos.
Nakon toga mozemo podesiti i vrijeme slanje, iznos koji ce da override-uje iznos iz konstruktora, korisnika sa korisnickim imenom i kodom sa SEP portala.

```
var deposit = DepositBuilder.Build("**********", 25m)
                 //.SetTime(DateTime.Now)
                 //.SetAmount(25m)
                 .SetUser("Marko Markovic", "**********");
```
Nakon toga pozivamo servis sa slanje depozita. __FiscalApiService__ je HttpClient koji koristi autorizaciju tokenom, prema adresi _BASE_URL_. 

```
var service = new FiscalApiService(BASE_URL, TOKEN);
return await service.CreateDeposit(deposit.ToXMLModel());
```

## Odgovor servisa

Odgovor se dobija u formatu __EFCommandResponse__ (JSON ispod):
```
{
  "UIDRequest": "320ab2d2-f560-439a-9fbf-cf5e9a4f3f90",
  "UIDResponse": "e805fa98-50a4-4638-8bdd-2000db0d5dcb",
  "ResponseCode": "f3474734-6c46-4f4d-886b-bbb3d4631658",
  "RawMessage": "..",
  "IsSucccess": true
}
```

U slucaju uspjesnog poziva, __IsSucccess__ ima vrijednost _true_, u slucaju greske _false_, sadrzaj greske je u polju __Error__.
```
{
  "UIDRequest": "ef8721d8-0eea-4a5c-a6b4-6d709a6e3f41",
  "UIDResponse": "ef8721d8-0eea-4a5c-a6b4-6d709a6e3f41",
  "IsSucccess": false,
  "Error": {
    "ErrorCode": "56",
    "ErrorMessage": "INITIAL cash deposit cannot be changed after invoice fiscalization."
  }
}
```
Poruke gresaka su na engleskom jeziku, i iz sadrzaja polja __ErrorMessage__ lako je zakljuciti o cemu se radi u navedenom primjeru, depozit je vec poslat i nije ga moguce poslati ponovo jer je racun vec kreiran za taj radni dan.

# Kreiranje racuna

Prvo instanciramo model racuna. U konstruktoru se predaje jedinstveni identifikator racuna (Guid) i redni broj racuna. Zatim pozivamo sledece metode: 

- SetTCRCode - postavljamo kod elektrnonskog naplatnog uredjaja.
- SetDates - postavljamo datum racuna i valute, u primjeru valute nema
- SetIsCash - da li je racun gotovinski ili bezgotovinski
- SetUser - postavljamo naziv i kod operatera
- SetSeller - unosimo podatke o firmi, prodavcu (naziv, pib i adresa firme koja izdaje racun)
- SetBuyer - unosimo podatke o kupcu, ukoliko ih imamo (neophodno za virmanske racune)
- AddSaleItem - dodajemo stavke prodaje
- CalculateTotalAmount postavljamo nacin placanja, i pustamo biblioteku da preracuna ukupan iznos iz stavki, ili
- AddPayment - dodamo jedan ili vise nacina placanja.

## Primjer gotovinskog racuna
```
var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(true)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller("Primatech d.o.o.", "02863782", "Podgorica")
                .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
                .AddSaleItem("12", "Coca Cola 0.25l", 2, 2.20m, 21m)
                .AddSaleItem("22", "Fanta 0.5l", 2, 2.20m, 21m, 10)
                .AddSaleItem("19", "Sir Gauda", 2, 2.20m, 7m, 0)
                //.AddPayment("ACCOUNT", 100m)
                //.AddPayment("BUSINESSCARD", 20m);
                .CalculateTotalAmount("BANKNOTE");
```                  
U primjeru su tri stavke, i citav kod za kreiranje racuna je povezan u jednu cjelinu kako bi se lakse objasnile moguce operacije, realniji primjer koda i mapiranja stavki i nacina placanja je dat ispod, pod pretpostavkom da imamo racun sa sledecim stavkama:

```    
var buyer = new
{
    CompanyName = "Buyer Company Name",
    VATNumber = "07654321",
    Address = "Some Buyer Address"
};

var sales =
new[]{
    new{ ItemCode="12", ItemName="Coca Cola 0.25l", Quantity=2, Price=2.20m, TaxRate=21m,Discount=0},
    new{ ItemCode="22", ItemName="Fanta 0.5l", Quantity=2, Price=2.20m, TaxRate=21m,Discount=10},
    new{ ItemCode="19", ItemName="Sir Gauda", Quantity=1, Price=2.20m, TaxRate=7m,Discount=0}
};
```    
a poziv servisa sa ovim podacima je dat ispod

```   
var service = new FiscalApiService(BASE_URL, TOKEN);

var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                .SetTCRCode("**********")
                .SetDates(DateTime.Now, null)
                .SetUser("Marko Markovic", "**********")
                .SetSeller("Seller Company Name", "01234567", "Some Seller Address");

if (buyer != null)
{
    receipt.SetBuyer(buyer.CompanyName, buyer.VATNumber, buyer.Address);
}

foreach(var saleRow in sales)
{
    receipt.AddSaleItem(saleRow.ItemCode, saleRow.ItemName, saleRow.Quantity, saleRow.Price, saleRow.TaxRate);
}

receipt.CalculateTotalAmount("CASH")

return await service.CreateReceipt(receipt.ToXMLModel());
```   

## Primjer bezgotovinskog racuna

Za bezgotovinske racune, postaviti __SetIsCash__ na _false_.
``` 
var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 3)
    ...
    .SetIsCash(false)
    ...
    .CalculateTotalAmount("ACCOUNT");
``` 
 
 ## Odgovor servisa
 Odgovor se dobija u formatu __EFCommandResponse__ (JSON ispod):
``` 
{
  "Url": {
    "Value": "https://efitest.tax.gov.me/ic/#/verify?iic=A8B1EEA971BF47E06979E099142CEBB9&tin=02863782&crtd=2021-04-13T12:54:27+02:00&ord=2&bu=lv970pg430&cr=ck788xq400&sw=pf099nu664&prc=120.00"
  },
  "UIDRequest": "A8B1EEA971BF47E06979E099142CEBB9",
  "UIDResponse": "b97ac1dd-3572-409c-b706-cddd46f1fcff",
  "ResponseCode": "b97ac1dd-3572-409c-b706-cddd46f1fcff",
  "RawMessage": "...",
  "IsSucccess": true
}
``` 
__IKOF__ je _UIDRequest_.
_ResponseCode_ je __JIKR__ racuna.

Za svaki racun je potrebno prikazati sledece podatke:
1. Qr kod - kod koji sadrzi vrijednost __url.Value__ , link ka racunu na portalu poreske uprave
2. Datum i vrijeme fiskalizacije - vrijeme iz URL-a
3. Broj racuna
4. IKOF - jednistveni identifikator racuna, koga kreira fiskalni servis
5. JIKR - odgovor iz poreske uprave

Svi podaci neophodni za racun nalaze se u polju __Url.Value__. Oni se mogu parsirati metodom iz __EFCommandResponse__ odgovora, __ToModel()__.
Odgovor je __EFUrlContent__, JSON je dat ispod.

``` 
{
      "BaseUrl": "https://efitest.tax.gov.me/ic/#/verify",
      "IIC": "A8B1EEA971BF47E06979E099142CEBB9",
      "Tin": "02863782",
      "CreationDate": "2021-04-13T12:54:27+02:00",
      "DocumentNumber": 2,
      "BusinessUnitCode": "lv970pg430",
      "TCRCode": "ck788xq400",
      "SoftwareCode": "pf099nu664",
      "TotalPrice": 120.00,
      "Url": "https://efitest.tax.gov.me/ic/#/verify?iic=A8B1EEA971BF47E06979E099142CEBB9&tin=02863782&crtd=2021-04-13T12:54:27+02:00&ord=2&bu=lv970pg430&cr=ck788xq400&sw=pf099nu664&prc=120.00"
    }
``` 
- IIC - IKOF
- Tin - PIB
- CreationDate - datum i vrijeme fiskalizacije
- DocumentNumber - broj racuna
- BusinessUnitCode - kod poslovne jedinice
- TCRCode - kod elektronskog naplatnog uredjaja
- SoftwareCode - kod softvera
- TotalPrice - ukupna cijena
- Url - url koji treba da bude sadrzaj prikazanog QR koda

Predlog je da se u bazi podataka cuvaju JIKR racuna i URL, ili JIKR, URL i ostali parsirani elementi racuna koji su potrebni za prikaz za racunu.
Broj racuna prikazati u formatu:

__{BusinessUnitCode}/{DocumentNumber}/{CreationDate.Year}/{TCRCode}__

# Crtanje QR koda
QR kod ne smije biti manji od 2.1 cm na racunu. Moguce je koristiti biblioteku za kreiranje QR koda, preko metoda

__TODO__

### Stavke
Stavke dodajemo na sledeci nacin

``` 
 foreach(var saleRow in sales)
{
    receipt.AddSaleItem(saleRow.ItemCode, saleRow.ItemName, saleRow.Quantity, saleRow.Price, saleRow.TaxRate);
}
``` 
## Vrste racuna

### Korektivni racuni

### Avansi

## Nacini placanja
    
Gotovinski nacini placanja su
-BANKNOTE
-CARD
-ADVANCE
-ORDER
-OTHER-CASH

Bezgotovinski nacini placanja su
-BUSINESSCARD
-COMPANY
-ACCOUNT
-OTHER
-ADVANCE

Ostali nacini placanja su i
-SVOUCHER
-FACTORING

Nacine placanja mozemo dodati redom, sa iznosom (pojedinacno ili kombinovano placanje)
``` 
.AddPayment("ACCOUNT", 100m)
``` 
ili bez iznosa, samo za pojedinacno placanje
``` 
.CalculateTotalAmount("ACCOUNT");
``` 
# Greske u integraciji fiskalnog drajvera

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

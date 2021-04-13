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
Nakon toga podesavamo korisnika sa korisnickim imenom i kodom sa SEP portala.

```
var deposit = DepositBuilder.Build("**********", 25m)
                 .SetUser("Marko Markovic", "**********");
```
Nakon toga pozivamo servis sa slanje depozita. __FiscalApiService__ je HttpClient koji koristi autorizaciju tokenom, prema adresi _BASE_URL_. 

```
var service = new FiscalApiService(BASE_URL, TOKEN);
return await service.CreateDeposit(deposit);
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

- __SetTCRCode__ - postavljamo kod elektrnonskog naplatnog uredjaja.
- __SetDates__ - postavljamo datum racuna i valute, u primjeru valute nema
- __SetIsCash__ - da li je racun gotovinski ili bezgotovinski
- __SetUser__ - postavljamo naziv i kod operatera
- __SetSeller__ - unosimo podatke o firmi, prodavcu (naziv, pib i adresa firme koja izdaje racun)
- __SetBuyer__ - unosimo podatke o kupcu, ukoliko ih imamo (neophodno za virmanske racune)
- __AddSaleItem__ - dodajemo stavke prodaje
- __CalculateTotalAmount__ - postavljamo nacin placanja, i pustamo biblioteku da preracuna ukupan iznos iz stavki, ili
- __AddPayment__ - dodamo jedan ili vise nacina placanja.

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
                .CalculateTotalAmount(EFIPaymentTypeEnum.BANKNOTE);
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

receipt.CalculateTotalAmount(EFIPaymentTypeEnum.BANKNOTE);

return await service.CreateReceipt(receipt);
```   

Kreirani racun saljemo istim servisom, kao kod depozita, metodom _CreateReceipt_
``` 
var service = new FiscalApiService(BASE_URL, TOKEN);
return await service.CreateReceipt(receipt);
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
1. __Qr kod__ - kod koji sadrzi vrijednost __url.Value__ , link ka racunu na portalu poreske uprave
2. __Datum i vrijeme fiskalizacije__ - vrijeme iz URL-a
3. __Broj racuna__ - redni broj racuna
4. __IKOF__ - jednistveni identifikator racuna, koga kreira fiskalni servis
5. __JIKR__ - odgovor iz poreske uprave

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
- __IIC__ - IKOF
- __Tin__ - PIB
- __CreationDate__ - datum i vrijeme fiskalizacije
- __DocumentNumber__ - broj racuna
- __BusinessUnitCode__ - kod poslovne jedinice
- __TCRCode__ - kod elektronskog naplatnog uredjaja
- __SoftwareCode__ - kod softvera
- __TotalPrice__ - ukupna cijena
- __Url__ - url koji treba da bude sadrzaj prikazanog QR koda

Predlog je da se u bazi podataka cuvaju JIKR racuna i URL, ili JIKR, URL i ostali parsirani elementi racuna koji su potrebni za prikaz za racunu.
Broj racuna prikazati u formatu:

__{BusinessUnitCode}/{DocumentNumber}/{CreationDate.Year}/{TCRCode}__

## Crtanje QR koda (funkcionalnost u izradi)
QR kod ne smije biti manji od 2.1 cm na racunu. Moguce je koristiti slededece API-je za kreiranje QR koda

``` 
//Jpeg
service.GetImage(url);
//Base64
service.GetBase64(url);
``` 

ili biblioteku _Primatech.FiscalDriver.Drawing_ i metode

``` 
//Jpeg
QRCodeHelper.GetImage(url);
//Base64
QRCodeHelper.GetBase64(url);
``` 

## Stavke
Stavke dodajemo na sledeci nacin

``` 
 foreach(var saleRow in sales)
{
    receipt.AddSaleItem(saleRow.ItemCode, saleRow.ItemName, saleRow.Quantity, saleRow.Price, saleRow.TaxRate);
}
``` 
# Vrste racuna
Osim standardnih racuna, moguce je i brisanje racuna, kreiranje avansnih racuna i porudzbina.

Tipovi u modelu su:
- __INVOICE__ - gotovinski ili bezgotovinski racun, sve stavke moraju imati pozitivne kolicine
- __CORRECTIVE_INVOICE__ - korektivni racuni, potrebno je navesti i povezane racune koji se koriguju ovim racunom, kolicine su negativne
- __SUMMARY_INVOICE__ - grupni racuni, potrebno je navesti i povezane racune kojima je nacin placanja ORDER, a koji se zavtvaraju ovim racunom

Za postavljanje korektivnog, ili sumarnog racuna, koristimo sledece metode:

```
... 
.SetCorrectiveInvoice()
...
.SetSummaryInvoice()
...
``` 

## Korektivni racuni
Ispod je dat primjer kompletnog ponistavanja racuna (moguce je i parcijalno ponistiti stavke racuna, sto se ne preporucuje). Kod ponistavanja racuna, kolicine su negativne.

``` 
//1. Create receipt
  var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
        ...
        .AddSaleItem("1", "Coca Cola 0.5", 2, 2.5m, 21m)
        .CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

  var result = await SendReceipt(receipt);
  var receiptReference = new
  {
      IKOFReference = result.UIDRequest,
      IssuedAt = receipt.ReceiptTime
  };

  //2. Delate receipt
  var correctiveReceipt = ReceiptBuilder.Build(Guid.NewGuid(), 2)
      ...
      .SetCorrectiveInvoice()
      .AddIKOFReference(receiptReference.IKOFReference, receiptReference.IssuedAt)
      .AddSaleItem("1", "Coca Cola 0.5", -2, 2.5m, 21m)
      .CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

  result = await SendReceipt(correctiveReceipt);
``` 

Novi racun kojim korigujemo stavke polaznog racuna moze da sadrzi referencu na polazni racun.
``` 
  ...
  .AddIKOFReference(receipt.IKOFReference, receipt.IssuedAt)
  ...
``` 

## Avansi

Avans mozemo iskoristiti u potpunosti, ili djelimicno. Prije svake upotrebe avansa, dio ili citav avansni racun koji se koristi se ponistava korektivnim racunom (postavljamo tip racuna __SetCorrectiveInvoice__ i dodajemo referencu na avansni racun __AddIKOFReference__). Na kraju se kreira racun koji takodje sadrzi referencu na grupni racun (__AddIKOFReference__).

``` 
//1. Create advance
var advance = ReceiptBuilder.Build(Guid.NewGuid(), 1)
      ...
      .AddSaleItem("1", "Avans za gradjevinski materijal", 1, 400m, 21m)
      .CalculateTotalAmount(EFIPaymentTypeEnum.ADVANCE);

var result = await SendReceipt(advance);
MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);
var advanceReference = new
{
    IKOFReference = result.UIDRequest,
    IssuedAt = advance.ReceiptTime
};
//2. Delate partialy or full amount of the advance payment
var correctiveInvoice = ReceiptBuilder.Build(Guid.NewGuid(), 2)
    ...
    .SetCorrectiveInvoice()
    .AddIKOFReference(advanceReference.IKOFReference, advanceReference.IssuedAt)
    .AddSaleItem("1", "Avans za gradjevinski materijal", -1, 150m, 21m)
    .CalculateTotalAmount(EFIPaymentTypeEnum.ADVANCE);

result = await SendReceipt(correctiveInvoice);
MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);

//3. Use partialy or full amount of the Advance payment
var connectedInvoice = ReceiptBuilder.Build(Guid.NewGuid(), 3)
    ...
    .AddIKOFReference(advanceReference.IKOFReference, advanceReference.IssuedAt)
    .AddSaleItem("2", "Gradjevinski materijal tip 1", 1, 100m, 21m)
    .AddSaleItem("3", "Gradjevinski materijal tip 2", 1, 50m, 21m)
    .CalculateTotalAmount(EFIPaymentTypeEnum.ACCOUNT);

result = await SendReceipt(connectedInvoice);
``` 

## Porudzbine

U ugostiteljstvu se mogu kreirati prvo porudzbine, nacin placanja ORDER, potom se na jedan grupni racun dodati sve stavke iz svih porudzbina. U grupnom racunu navesti tip upotrebom metoda __SetSummaryInvoice__. Dodati na grupni racun i sve reference na sve porudzbine za koje se kreira grupni racun metodom __AddIKOFReference__.
``` 
var order1 = ReceiptBuilder.Build(Guid.NewGuid(), 1)
      ...
      .AddSaleItem("1", "Coca Cola 0.25l", 2, 2.50m, 21m)
      .CalculateTotalAmount(EFIPaymentTypeEnum.ORDER);

  var result=await SendReceipt(order1);
  var firstOrder = new
  {
      IKOFReference = result.UIDRequest,
      IssuedAt = order1.ReceiptTime
  };
  var order2 = ReceiptBuilder.Build(Guid.NewGuid(), 2)
      ...
      .AddSaleItem("2", "Fanta 0.25l", 1, 1.50m, 21m)
      .AddSaleItem("2", "Bavaria", 1, 3.50m, 21m)
      .CalculateTotalAmount(EFIPaymentTypeEnum.ORDER);

  result = await SendReceipt(order2);
  var secondOrder = new
  {
      IKOFReference = result.UIDRequest,
      IssuedAt = order2.ReceiptTime
  };

  //summary invoice
  //1. Set Header
  var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 3)
      ...
      .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address);

  //2. Add items from all orders
  foreach (var saleItem in order1.Sales)
  {
      receipt.AddSaleItem(saleItem);
  }
  foreach (var saleItem in order2.Sales)
  {
      receipt.AddSaleItem(saleItem);
  }
  //3. Set type (summary), and add references to the orders
  receipt.SetSummaryInvoice();
  receipt.AddIKOFReference(firstOrder.IKOFReference, firstOrder.IssuedAt);
  receipt.AddIKOFReference(secondOrder.IKOFReference, secondOrder.IssuedAt);

  //4. Set payment type of summary invoice
  receipt.CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

  result = await SendReceipt(receipt);
```

# Nacini placanja
    
Gotovinski nacini placanja su
- BANKNOTE
- CARD
- ADVANCE
- ORDER
- OTHER-CASH

Bezgotovinski nacini placanja su
- BUSINESSCARD
- COMPANY
- ACCOUNT
- OTHER
- ADVANCE

Ostali nacini placanja su i
- SVOUCHER
- FACTORING

Nacine placanja mozemo dodati redom, sa iznosom (pojedinacno ili kombinovano placanje)
``` 
.AddPayment(EFIPaymentTypeEnum.ACCOUNT, 100m)
``` 
ili bez iznosa, samo za pojedinacno placanje
``` 
.CalculateTotalAmount(EFIPaymentTypeEnum.ACCOUNT);
``` 

Za nacine placanja, koristiti enumeraciju __EFIPaymentTypeEnum__.

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

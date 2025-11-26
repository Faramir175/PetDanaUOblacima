# Studentska Menza - Sistem za Rezervaciju Obroka

Ovaj projekat implementira backend API za sistem rezervacije obroka u studentskim menzama. Korisnici mogu da pregledaju menije, rezervišu obroke i otkažu rezervacije putem RESTful API-ja.

## Tehnički Zahtevi

### 1. Lista Korišćenih Tehnologija i Verzija

| Tehnologija | Verzija | Uloga |
| **Jezik / Runtime** | **C# / .NET 10.0 SDK** | Primarni jezik i platforma |
| **Framework** | ASP.NET Core Web API | HTTP API implementacija |
| **Baza Podataka** | In-memory DB (Statička klasa) | In-memory perzistencija podataka |
| **Unit Testing** | XUnit | Testni framework |

### 2. Korišćeni NuGet Paketi (Zavisnosti)

Ovi paketi su ključni za izvršavanje i testiranje rešenja:

| Projekat | NuGet Paket | Verzija | Uloga |
| **Glavni Projekat** | `Microsoft.AspNetCore.OpenApi` | 10.0.0 | Generisanje API dokumentacije |
| **Test Projekat (Testovi)** | `xunit` | 2.9.3 | Testni framework |
| **Test Projekat (Testovi)** | `Moq` | 4.20.72 | Kreiranje mock zavisnosti |
| **Test Projekat (Testovi)** | `FluentAssertions` | 8.8.0 | Asertacije za čitljivost |
| **Test Projekat (Testovi)** | `AutoFixture` | 4.18.1 | Generisanje testnih podataka |
| **Test Projekat (Testovi)** | `Microsoft.NET.Test.Sdk` | 17.14.1 | Pokretanje testova |

## Instrukcije za Podešavanje i Build

### 1. Struktura Rešenja

Rešenje se sastoji od **dva odvojena projekta** (`PetDanaUOblacima` i `Testovi`) unutar root direktorijuma.

### 2. Podešavanje Okruženja

Za build projekta je neophodno imati instaliran:

* **1. .NET 10.0 SDK**
  * Preuzeti sa [zvaničnog Microsoft sajta](https://dotnet.microsoft.com/download).

### 3. Kloniranje Repozitorijuma i Restore Paketa

Otvorite terminal i izvršite sledeće komande iz **root direktorijuma repozitorijuma** (URL: `https://github.com/Faramir175/PetDanaUOblacima.git`):

1. Kloniranje repozitorijuma:
   git clone https://github.com/Faramir175/PetDanaUOblacima.git

2. Navigacija do root foldera (gde je .slnx fajl):
   cd PetDanaUOblacima

3. Restore NuGet paketa:
   dotnet restore

### 4. Pokretanje Build-a

Izvršite build celog rešenja:
   dotnet build

## Instrukcije za Pokretanje Aplikacije

Aplikaciju pokrenite u Development okruženju, ciljajući na **glavni projekat**:

   dotnet run --project PetDanaUOblacima/PetDanaUOblacima.csproj

Aplikacija će se pokrenuti i biti dostupna na adresi poput `http://localhost:5104` i/ili `https://localhost:7255`.

**NAPOMENA:** Ako port nije 5104, obavezno ažurirajte `baseURL` varijablu u Postman Environment-u (`Levi9Cloud`).

## Instrukcije za Pokretanje Testova

### 1. Pokretanje Unit Testova (XUnit)

Unit testovi se pokreću iz root direktorijuma, automatski pronalazeći test projekat (`Testovi.csproj`):

   dotnet test

### 2. Pokretanje Funkcionalnih Testova (Postman)

**Preduslovi:**
1. Aplikacija mora biti pokrenuta (videti sekciju Pokretanje Aplikacije).
2. `baseURL` u Postman Environment-u (`Levi9Cloud`) mora biti ispravno postavljen na adresu vašeg servera (npr. `http://localhost:5104`).

**Koraci:**
1. U Postmanu, importujte priložene `Postman Collection` i `Environment` fajlove.
2. Izaberite kolekciju `5DanaUOblacima2025ChallengePublic` i kliknite `Run`.
3. U Runner prozoru, uverite se da je izabran `Levi9Cloud` environment i pokrenite testove.
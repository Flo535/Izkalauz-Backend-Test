# 🍲 Ízkalauz Backend - Tesztelési Projekt

Ez a projekt az **Ízkalauz** receptkezelő alkalmazás backend logikájának és adatmodelljeinek automatizált tesztelését tartalmazza. A cél a megbízható működés és az adatvalidáció garantálása a vizsgaremek részeként.

## 🚀 Alkalmazott technológiák
* **Keretrendszer:** .NET 8.0
* **Tesztelő keretrendszer:** xUnit 
* **Validáció:** Data Annotations (System.ComponentModel.DataAnnotations)
* **Adatkezelés:** Entity Framework Core (Integrációs tesztekhez)
* **Verziókezelés:** Git & GitHub

## 🧪 Tesztelt funkciók
A projekt az alábbi kritikus backend folyamatokat ellenőrzi:
### 🛡️ Controller & Autentikáció Tesztelés
A `FavoritesControllerTests` osztály az alábbi kritikus biztonsági és logikai pontokat ellenőrzi:

* **Jogosultságkezelés:** Annak biztosítása, hogy bejelentkezés nélkül (Unauthorized) ne lehessen hozzáférni a kedvencekhez.
* **Mockolt Felhasználói Kontextus:** `ClaimsPrincipal` és `HttpContext` használata a tesztekben a bejelentkezett felhasználó szimulálására.
* **Adatbázis Integritás (In-Memory DB):** Az Entity Framework Core memóriabeli adatbázisával teszteljük:
    * Létező/Nem létező receptek kezelése (`NotFound`).
    * Duplikáció elleni védelem (`Conflict` válasz, ha a recept már kedvenc).
    * Sikeres mentési folyamat igazolása.

## 🛠️ Futtatási útmutató
A tesztek futtatásához kövesse az alábbi lépéseket:

1. Nyissa meg a megoldást (`.sln`) Visual Studio 2022-ben.
2. Győződjön meg róla, hogy a NuGet csomagok (xUnit, Microsoft.NET.Test.Sdk) frissítve vannak.
3. Nyissa meg a **Test Explorer** (Tesztkezelő) ablakot (**Test -> Test Explorer**).
4. Kattintson a **Run All** gombra.

> **Megjegyzés:** A tesztek egy része valós adatbázis-kapcsolatot (Integrációs teszt) igényelhet a pontosabb verifikáció érdekében.

## 📈 Verziókezelési stratégia
A fejlesztés során **Git** verziókezelőt használtam, a kód tárolása a GitHub felhő alapú rendszerében történt. A `.gitignore` fájl segítségével biztosítottam, hogy a fejlesztői környezet ideiglenes fájljai

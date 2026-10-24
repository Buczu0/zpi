# WebStore — projekt ZPI

Sprint 01 dostarcza szkielet ASP.NET Core + React, model danych, migrację SQL Server oraz autonomiczne usługi produktów, zamówień, faktur, sklepów i adresów. Strona startowa jest po polsku. Katalog, koszyk, logowanie i biznesowe endpointy HTTP są poza zakresem tej iteracji.

Zatwierdzone wymagania, reguły P01–P09, wyniki sprawdzeń i wersja dostawy znajdują się w [dokumencie sprintu](sprinty/sprint-01/sprint.md). Zasady pracy ról opisuje [chatgpt.md](chatgpt.md).

## Wymagane środowisko

| Narzędzie | Wersja użyta do sprawdzeń |
| --- | --- |
| Windows, PowerShell | SQL Server LocalDB działa lokalnie na Windows |
| .NET SDK | 10.0.113; `global.json` dopuszcza nowszą poprawkę tego samego pasma SDK |
| ASP.NET Core / EF Core / Identity | 10.0.13, wszystkie projekty C# mają `net10.0` |
| Node.js / npm | 22.21.0 / 10.9.4; klient wymaga Node `^20.19.0` lub `>=22.12.0` |
| SQL Server Express LocalDB | 2022, silnik 16.0.1000.6, instancja `MSSQLLocalDB` |
| React / Vite | 19.3.1 / 8.3.5 |
| AutoMapper | 16.1.1 |

Zainstaluj .NET SDK, Node.js oraz [SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver17). LocalDB uruchamia instancję przy pierwszym połączeniu. Można też użyć osobnej instancji SQL Server poprzez konfigurację połączenia. Nie są wymagane płatne usługi chmurowe ani kontenery.

## Odtworzenie i uruchomienie

W katalogu głównym repozytorium wykonaj w PowerShell:

```powershell
dotnet tool restore
dotnet restore WebStore.sln --locked-mode
npm --prefix WebStore.Web/ClientApp ci
dotnet build WebStore.sln --no-restore
dotnet ef database update --project WebStore.DAL --no-build
dotnet run --project WebStore.Web --no-build --no-restore
```

Otwórz **http://localhost:5080**. Zatrzymaj serwer przez `Ctrl+C`. Kompilacja solucji buduje również React do `WebStore.Web/wwwroot`; przy braku `node_modules` odtwarza pakiety przez `npm ci`. Po zmianie zależności klienta uruchom ponownie `npm ci`. Pliki `packages.lock.json` i `package-lock.json` utrwalają wersje zależności.

Migracja `20261024131757_InitialCreate` jest już w repozytorium. `database update` tworzy bazę i schemat, a kolejne wywołanie zachowuje istniejące dane. Aplikacja nie migruje bazy automatycznie podczas startu i nie dodaje przykładowych kont ani produktów. Dane przykładowe istnieją wyłącznie w testach.

Podczas pracy nad samym klientem można uruchomić `npm --prefix WebStore.Web/ClientApp run dev` i otworzyć adres wypisany przez Vite. Ten podgląd nie zastępuje sprawdzenia wersji serwowanej przez ASP.NET Core na porcie 5080.

## Konfiguracja bazy i licencji

Domyślne połączenie aplikacji i fabryki migracji:

```text
Server=(localdb)\MSSQLLocalDB;Database=WebStore;Integrated Security=true;TrustServerCertificate=true
```

Nazwa `WebStore` oznacza bazę przeznaczoną dla tej aplikacji. Dla innej bazy ustaw **przed migracją i uruchomieniem** zmienną `ConnectionStrings__WebStore`. Przykład wydzielonej bazy lokalnej:

```powershell
$env:ConnectionStrings__WebStore = 'Server=(localdb)\MSSQLLocalDB;Database=WebStore_Development;Integrated Security=true;TrustServerCertificate=true'
```

Używaj własnej bazy aplikacji. Połączenie zawierające hasło przechowuj w środowisku, nie w repozytorium. `TrustServerCertificate=true` jest ustawieniem lokalnego środowiska ćwiczeniowego.

AutoMapper odczytuje klucz z **`AUTOMAPPER_LICENSE_KEY`** przy utworzeniu mappera w DI. Ustaw tę zmienną w środowisku procesu serwera i testów, używając posiadanego klucza odpowiadającego wybranej licencji. Nie wpisuj klucza do `appsettings.json`, plików śledzonych przez Git ani rozmowy. Po zmianie zmiennej uruchom proces ponownie. Kod nie wypisuje jej wartości.

Klucz nie jest dostarczany z projektem. Według [dokumentacji AutoMappera](https://docs.automapper.io/en/latest/License-configuration.html) brak poprawnego klucza powoduje komunikat licencyjny w logach, bez blokowania mapowania. Taki wynik testów nie potwierdza ważności licencji. Wersja 16.1.1 zastępuje podatną wersję 12 z przykładu PDF zgodnie z decyzją lidera D06.

## Testy

Pełny pakiet obejmuje **77 testów usług i konfiguracji** z EF InMemory oraz **6 testów integracyjnych na rzeczywistym SQL Server**. Po kompilacji:

```powershell
dotnet test WebStore.sln --no-build --no-restore
```

Przykłady filtrowania:

```powershell
dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~ProductServiceTests'
dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'Category=SqlServer'
dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'Category!=SqlServer'
```

Ostatnie polecenie jest szybkim sprawdzeniem bez SQL Server; nie zastępuje pełnego pakietu. Brak SQL Server powoduje błąd testów relacyjnych, a nie ich pominięcie.

Każdy test SQL tworzy osobną bazę `WebStoreTests_<losowy GUID>`, stosuje migrację i usuwa wyłącznie tę bazę po teście. Infrastruktura kontroluje nazwę usuwanej bazy. Nie korzysta z bazy aplikacji `WebStore`. Przygotowanie danych Identity jest oczekiwane przed rozpoczęciem testu. Testy InMemory również otrzymują własne, oddzielne bazy.

Aby uruchomić testy na osobnej instancji SQL Server, ustaw `WEBSTORE_TEST_SQL_CONNECTION` na połączenie z kontem mającym uprawnienia tworzenia i usuwania baz. Infrastruktura zawsze zastępuje `Initial Catalog` własną losową nazwą testową. Nie ustawiaj tej zmiennej na serwer produkcyjny.

Testy SQL sprawdzają ponowne zastosowanie migracji, TPH, daty UTC, precyzję kwot, FK, CHECK, unikalność, zakaz kasowania powiązanych danych, współpracę pięciu usług oraz wycofanie całej transakcji po kontrolowanej awarii zapisu pozycji zamówienia. Dwa testy regresji obejmują ponowne użycie kontekstu po fakturowaniu zamówienia lub zmianie jego klienta w innym kontekście.

## Układ rozwiązania

| Projekt | Odpowiedzialność |
| --- | --- |
| `WebStore.Model` | 12 encji domenowych; użytkownicy Identity z kluczem `int` |
| `WebStore.DAL` | `ApplicationDbContext`, SQL Server, TPH, ograniczenia i migracje |
| `WebStore.ViewModels` | Modele wejściowe i wynikowe `Vm` |
| `WebStore.Services` | Interfejsy, pięć usług, walidacja, mapowanie i transakcje |
| `WebStore.Web` | Konfiguracja DI i serwowanie klienta React |
| `WebStore.Tests` | Testy jednostkowe i integracyjne |

Usługi działają bez HTTP. Rejestruje je `AddWebStoreServices`; wymagają kontekstu EF i są tworzone w zakresie DI. Modele z `Id = null` lub `0` tworzą rekord, dodatnie ID aktualizuje istniejący. Odczyty są uporządkowane po ID i nie śledzą zmian. Zapis w SQL Server używa transakcji `Serializable`; przed sprawdzeniem reguł odłącza niezmienione encje zapamiętane przez kontekst, dzięki czemu zapytania wczytują bieżący stan bazy. Kontrakty błędów i reguły biznesowe są opisane w P04–P09 dokumentu sprintu.

Przy kolejnej uzgodnionej zmianie modelu utwórz **nową** migrację przez `dotnet ef migrations add NazwaZmiany --project WebStore.DAL`, a następnie zastosuj ją przez `database update`. Nie generuj ponownie migracji początkowej przy zwykłym odtwarzaniu projektu.

Kontrola znanych podatności zależności:

```powershell
dotnet list WebStore.sln package --vulnerable --include-transitive --no-restore
npm --prefix WebStore.Web/ClientApp audit
```

Wyniki kompilacji, `wwwroot`, `node_modules`, lokalne bazy i pliki środowiskowe są wyłączone z Git. Oddanie do `origin/main` następuje według bramek akceptacji i recenzji określonych w `chatgpt.md`.

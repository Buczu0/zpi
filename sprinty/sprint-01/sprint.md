# Sprint 01 — fundament aplikacji WebStore

## Metryka

| Pole | Wartość |
| --- | --- |
| Sprint | `sprint-01` |
| Data rozpoczęcia analizy | 2026-10-24, strefa `Europe/Warsaw` |
| Etap | `RECENZJA` |
| Aktywna rola | Recenzent — publikacja zablokowana uwierzytelnianiem GitHub |
| Następne działanie | Lider kończy logowanie GitHub; następnie ponowienie push i potwierdzenie zdalnej referencji. |
| Wersja user stories | `US1` — zaakceptowana 2026-10-24 |
| Wersja kodu | `K2` — zaakceptowana i zweryfikowana; commit kodu `4995dc234ce0567360445e9c524691f5d34a8c48`, publikacja zablokowana |
| Źródło wymagań | `wymagania funkcjonalne/sprint-01/ZPI_N_Lab1.pdf`, 21 stron |
| SHA-256 źródła | `aa930d99d17cd202d1935c2cbba9b491f04788dba6e75d7114a47575942ef761` |
| Stan wejściowy kodu | Brak aplikacji; repozytorium zawiera instrukcje agentów i PDF. Commit wejściowy: `71488d1`. |
| Repozytorium docelowe | `https://github.com/Buczu0/zpi.git`, zdalne `origin` |
| Gałąź docelowa | `main` |

Przeczytano cały dokument i obejrzano jego elementy graficzne, w tym diagram UML na stronie 12. Numery stron poniżej oznaczają strony pliku PDF liczone od 1.

## Ustalenia i akceptacje lidera

Ustalenia z rozmowy z 2026-10-24:

| ID | Rozstrzygnięcie | Wiadomość lidera |
| --- | --- | --- |
| D01 | Cała aplikacja i testy używają .NET 10; zależności i szkielet React należy dostosować. Zastępuje to niespójne wskazania .NET 5 i 6 w PDF. | „.NET 10, z dostosowaniem zależności i szkieletu React” |
| D02 | Zakres obejmuje uruchamialny szkielet ASP.NET Core + React, model, bazę, ProductService oraz OrderService, InvoiceService, StoreService i AddressService z testami. Dodatkowe ekrany biznesowe i endpointy API są poza sprintem. | „Tak, taki zakres sprintu” — odpowiedź na pytanie zawierające powyższy zakres. |
| D03 | Analityk ma zaproponować brakujące pola i reguły nowych usług do późniejszej akceptacji lidera. | „Zaproponuj model i reguły do mojej akceptacji” |
| D04 | Oddanie sprintu obejmuje zapisanie wyniku w obecnym repozytorium GitHub. | „tak oddawaj sprint do repo na githubie, niech zostanie na gałęzi master, main” — repozytorium wskazane w pytaniu: `https://github.com/Buczu0/zpi.git`. |
| D05 | Pozostaje istniejąca gałąź `main`; nie tworzymy gałęzi studenta ani `master`. To jawne odstępstwo od instrukcji organizacyjnej PDF. | „Pozostajemy na istniejącej gałęzi main” |
| D06 | Używamy poprawionej wersji AutoMappera; klucz licencji będzie odczytywany ze zmiennej środowiskowej `AUTOMAPPER_LICENSE_KEY`. Wybrano 16.1.1. | „Użyj poprawionej wersji; przygotuj konfigurację klucza licencji w środowisku” |

**Akceptacja user stories US1: otrzymana 2026-10-24 (Europe/Warsaw), lider: użytkownik projektu.** Zakres: S01-US-001–S01-US-013, model i reguły P01–P09. Dokładna wiadomość: „Akceptuję US1 wraz z proponowanym modelem i regułami P01–P09. Rozpocznij implementację sprintu-01 jako programista fullstack.”

**Akceptacja kodu K1: otrzymana 2026-10-24 (Europe/Warsaw), lider: użytkownik projektu.** Dokładna wiadomość: „tak akceptuje kod sprintu-01”. Odpowiedź odnosi się do właśnie przedstawionej wersji K1 obejmującej 80 plików manifestu. Przed rozpoczęciem recenzji potwierdzono zgodność wszystkich 80 sum SHA-256 oraz źródłowego PDF.

**Akceptacja kodu K2: otrzymana 2026-10-24 (Europe/Warsaw), lider: użytkownik projektu.** Dokładna wiadomość: „Tak, akceptuję K2. Przejdź do ponownej recenzji sprintu-01.” Zakres: dostawa K2 opisana manifestem K1 i trzema zastępującymi sumami K2, łącznie 80 plików. Przed ponowną recenzją potwierdzono wszystkie sumy SHA-256, komplet ścieżek i niezmieniony źródłowy PDF. Wcześniejsza akceptacja K1 dotyczy wyłącznie historycznej wersji K1.

## Zakres i sposób czytania wymagań

Rezultatem sprintu ma być fundament WebStore: pięć warstw aplikacji, dodatkowy projekt testów, działający szkielet webowy, model danych, migracja SQL Server oraz pięć autonomicznych usług sprawdzonych testami.

Wymagania opisane wprost w PDF mają podane źródło. Doprecyzowania analityka powstały na podstawie D03 i zostały zaakceptowane wraz z US1. Obowiązują kryteria wszystkich stories, reguły P01–P09 i opisany poniżej model. Zmiana ich znaczenia wymaga ponownego uzgodnienia z liderem.

Instrukcje obsługi VS Code, Git Graph i menedżera poświadczeń na stronach 3–7 są sposobem wykonania ćwiczenia, a nie funkcjami aplikacji. Nie wymagają zmiany danych konta ani usuwania istniejących poświadczeń. Oddanie w Git pozostaje wymaganym rezultatem zgodnie z D04–D05.

Poza zakresem US1 pozostają: ekrany katalogu i zakupów, biznesowe endpointy HTTP, logowanie i rejestracja przez interfejs, płatności, wysyłka, eksport faktur i rozliczenia podatkowe. Uzgodniony minimalny zakres usług nie zawiera usuwania rekordów, rezerwacji ani pomniejszania stanów magazynowych. Model użytkowników i integracja Identity pozostają wymagane przez diagram i przykłady, mimo braku funkcji logowania.

## Zatwierdzony model i reguły wspólne

### P01 — technologia i uruchomienie

Zgodnie z D01 wszystkie projekty C# mają target `net10.0`. Uzgodnienie: ASP.NET Core i pakiety Microsoft Identity/EF Core z linii 10, provider SQL Server, SQL Server LocalDB na Windows, React z Vite w `WebStore.Web/ClientApp`, xUnit, AutoMapper i wstrzykiwany logger. Zachowujemy pięć projektów aplikacji z PDF oraz szósty `WebStore.Tests`. Dokładne wersje zależności utrwalono w plikach lock; wymagania Node.js i sprawdzone wersje środowiska podano w sekcji implementacji i README.

EF Core 10 wymaga .NET 10 ([dokumentacja Microsoft](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)). Microsoft opisuje integrację ASP.NET Core z nowymi narzędziami SPA, w tym Vite ([dokumentacja SPA](https://learn.microsoft.com/en-us/aspnet/core/client-side/spa/intro?view=aspnetcore-10.0)). Umieszczenie klienta w `ClientApp` jest uzgodnionym rozwiązaniem zachowującym podział projektu z PDF, a nie wymaganiem nowego szablonu Microsoft.

### P02 — encje z diagramu i ich rozszerzenia

Pola wymienione przy diagramie stanowią wymaganie źródłowe. Klucze obce, dodatkowe identyfikatory i wskazane doprecyzowania są zatwierdzonym doprecyzowaniem analityka. Kolekcje nawigacyjne odpowiadają relacjom P03. Encje mają klucz `Id: int`, z wyjątkiem `OrderProduct`, którego kluczem jest para `OrderId, ProductId`.

| Encja | Pola i przeznaczenie |
| --- | --- |
| `User : IdentityUser<int>` | Z diagramu: `FirstName: string`, `LastName: string`, `RegistrationDate: DateTime`. Uzgodnienie: imię i nazwisko niepuste, data w UTC. |
| `Customer : User` | Z diagramu: `Orders`, `BillingAddress`, `ShippingAddress`. Z zadania: adresy stają się obiektami `Address`. Uzgodnienie: kolekcja `Addresses`, opcjonalne `BillingAddressId` i `ShippingAddressId`; oba wskazania mogą dotyczyć tego samego własnego adresu. |
| `Supplier : User` | Z diagramu: kolekcja `Products`. |
| `Product` | Z diagramu: `Id`, `Name: string`, `Description: string`, `ImageBytes: byte[]`, `Price: decimal`, `Weight: float`, `Supplier`. Z zadania: `Category` jako encja zamiast tekstu. Uzgodnienie: wymagane `SupplierId`, `CategoryId`, kolekcje stanów magazynowych i pozycji zamówień. |
| `ProductStock` | Z diagramu: `Product`, `Quantity: int`. Uzgodnienie: `Id`, `ProductId`, ilość nieujemna; rekordy nie są przypisywane do konkretnych sklepów w tym sprincie. |
| `Order` | Z diagramu: `Id`, `Customer`, `OrderDate: DateTime`, `DeliveryDate: DateTime`, `TotalAmount: decimal`, `TrackingNumber: long`. Uzgodnienie: `CustomerId`, opcjonalny `InvoiceId`, kolekcja `OrderProducts`; szczegółowe znaczenie pól określa P07. |
| `OrderProduct` | Z diagramu: `Order`, `Product`, `Quantity: int`. Uzgodnienie: `OrderId`, `ProductId`, ilość dodatnia i `UnitPrice: decimal` jako cena zapisana przy ostatnim zapisie niefakturowanego zamówienia. |
| `Address` | Nowa encja wymagana przez zadanie. Uzgodnienie: `Id`, `Street`, `BuildingNumber`, opcjonalny `ApartmentNumber`, `PostalCode`, `City`, `Country`, opcjonalne `CustomerId` i `StationaryStoreId`. Obowiązkowe teksty są niepuste; nie narzucamy formatu kodu pocztowego konkretnego kraju. |
| `StationaryStore` | Nowa encja wymagana przez zadanie. Uzgodnienie: `Id`, niepuste `Name`, kolekcje `Addresses` i `Employees`. Sklep może początkowo nie mieć adresów ani pracowników. |
| `StationaryStoreEmployee : User` | Nowa encja i dziedziczenie wymagane przez zadanie. Uzgodnienie: wymagane `StationaryStoreId` i nawigacja `StationaryStore`. |
| `Category` | Nowa encja wymagana przez zadanie. Uzgodnienie: `Id`, niepuste `Name`, opcjonalny `Tag`, kolekcja `Products`. Pola `Name` i `Tag` występują także w danych przykładowych na stronie 18. |
| `Invoice` | Nowa encja wymagana przez zadanie. Uzgodnienie: `Id`, niepuste i unikalne `Number` o długości do 64 znaków, `IssueDate: DateTime`, `TotalAmount: decimal`, kolekcja `Orders`; reguły określa P08. |

### P03 — relacje i integralność

PDF wymaga następujących relacji 1:M, z lewą encją po stronie „jeden”: `Customer → Order`, `Supplier → Product`, `Product → ProductStock`, `Customer → Address`, `Category → Product`, `Invoice → Order`, `StationaryStore → Address`, `StationaryStore → StationaryStoreEmployee`. Wymaga również `Order ↔ Product` N:M przez `OrderProduct` oraz TPH dla `User`, `Customer`, `Supplier` i `StationaryStoreEmployee`.

Uzgodnienie: adres ma dokładnie jednego właściciela — klienta albo sklep. Nie może należeć jednocześnie do obu ani nie mieć właściciela. Relacje klienta do jego adresu rozliczeniowego i wysyłkowego są dodatkowymi, opcjonalnymi wskazaniami; mogą wskazywać wyłącznie adresy tego klienta. Zamówienie może nie mieć faktury, ale nie może należeć do dwóch faktur. Każdy pracownik należy do jednego sklepu. Nie dopuszczamy duplikatu pary zamówienie–produkt.

Uzgodnienie: klucze obce są egzekwowane w SQL Server; własność adresu jest dodatkowo chroniona ograniczeniem CHECK, numer faktury unikalnym indeksem, a ilości i kwoty odpowiednimi ograniczeniami. Usuwanie powiązanych danych jest blokowane (`Restrict`/`NoAction`), aby nie wprowadzać nieuzgodnionych kasowań kaskadowych. Brak operacji usuwania w usługach nie zwalnia z poprawnego schematu relacyjnego.

### P04 — wspólny kontrakt usług i błędów

Wymagane przez PDF: interfejsy w `WebStore.Services/Interfaces`, implementacje w `ConcreteServices`, dziedziczenie po `BaseService`, wstrzykiwanie `ApplicationDbContext`, `IMapper` i `ILogger`, modele wejściowe i wynikowe z sufiksem `Vm` w `WebStore.ViewModels/VM`.

Uzgodnienie: każda z pięciu usług udostępnia dodanie/aktualizację, odczyt pojedynczego obiektu filtrem oraz listy z opcjonalnym filtrem, analogicznie do `IProductService` z PDF. `StoreService` i `AddressService` mają także operacje przypisania opisane w ich stories. Filtry są predykatami na encjach, jak w przykładzie, i nie są publicznym kontraktem HTTP.

- `Id = null` lub `0` w modelu zapisu oznacza utworzenie; dodatnie `Id` oznacza aktualizację istniejącego rekordu. Ujemne ID jest błędem. Aktualizacja nieistniejącego rekordu nie tworzy nowego.
- Odczyt pojedynczego obiektu wymaga filtra. Brak dopasowania zwraca `null`; wiele dopasowań zwraca pierwszy rekord według rosnącego `Id`. Lista bez filtra zwraca wszystkie rekordy, z filtrem tylko dopasowane, zawsze według rosnącego `Id`; brak wyników oznacza pustą kolekcję.
- Model wejściowy `null` lub wymagany filtr `null` powoduje `ArgumentNullException`. Niepoprawne wartości pól powodują `ArgumentException`. Nieistniejący rekord aktualizacji albo referencja do nieistniejącego lub niewłaściwego typu encji powoduje `KeyNotFoundException`. Konflikt reguł, np. ponowne fakturowanie, powoduje `InvalidOperationException`.
- Walidacja działa także przy bezpośrednim wywołaniu usługi, bez kontrolera HTTP. Samo `[Required]` nie jest dowodem wykonania walidacji.
- Odrzucona operacja nie pozostawia częściowego zapisu. Zapis wymagający wielu zmian jest atomowy. Nieoczekiwane błędy dostępu do danych są logowane i przekazywane wyżej, nie zamieniane na pozorny sukces.
- Modele wynikowe zawierają identyfikator i dane potrzebne do weryfikacji danej operacji; nie ujawniają haseł, skrótów haseł ani wewnętrznych danych Identity.

### P05 — kwoty, ilości i daty

Uzgodnienie: kwoty mają typ `decimal`, precyzję SQL `decimal(18,2)` i nie są ujemne. Cena wejściowa z więcej niż dwoma miejscami dziesiętnymi albo wynik niemieszczący się w tej precyzji są odrzucane, bez cichego zaokrąglania. Ilość w zamówieniu jest dodatnią liczbą całkowitą, a stan magazynowy nieujemną. Waga jest skończoną, nieujemną liczbą `float`. Daty biznesowe przechowujemy w UTC. Kwoty są ćwiczeniowymi wartościami bez obsługi walut, podatków lub rabatów.

### P06 — produkty

Uzgodnione uzupełnienie przykładu: `Name` i `Description` są niepuste, `ImageBytes` zawiera co najmniej jeden bajt, a dostawca i kategoria istnieją. Nie weryfikujemy formatu obrazu, ponieważ PDF używa przykładowych bajtów. `ProductVm` zawiera także `Id`, `SupplierId`, `CategoryId`; jego `Quantity` jest sumą stanów `ProductStock`, a przy braku stanów wynosi 0. Zapis produktu nie zmienia stanów magazynowych ani zapisanych cen pozycji wcześniejszych zamówień.

Na stronie 16 warunek aktualizacji dla `Id = 0` jest sprzeczny z opisem ze strony 17. Obowiązuje rozstrzygnięcie P04: `null`/`0` tworzy produkt, dodatnie ID aktualizuje istniejący. Test ma obejmować oba warianty tworzenia.

### P07 — zamówienia

Uzgodnienie: zapis zamówienia przyjmuje klienta, planowaną datę dostawy, numer śledzenia i co najmniej jedną pozycję z ID produktu i ilością. Wszystkie referencje muszą istnieć; ten sam produkt może wystąpić tylko raz w zamówieniu. `OrderDate` jest ustawiana przez usługę przy utworzeniu i nie zmienia się przy aktualizacji. `DeliveryDate` oznacza planowany termin, nie może być wcześniejsza od `OrderDate`. `TrackingNumber = 0` oznacza brak numeru; wartości ujemne są błędne.

Przy utworzeniu i każdej aktualizacji niefakturowanego zamówienia usługa pobiera bieżące ceny produktów, zapisuje je w `OrderProduct.UnitPrice` i wylicza `TotalAmount = suma(UnitPrice × Quantity)`. Model wejściowy nie pozwala narzucić kwoty ani ceny pozycji. Sama późniejsza zmiana ceny produktu nie przelicza istniejącego zamówienia. Aktualizacja zastępuje komplet pozycji. Zamówienia przypisanego do faktury nie można edytować. Zapis zamówienia nie rezerwuje i nie pomniejsza zapasu.

### P08 — faktury

PDF wymaga relacji `Invoice → Order` 1:M, a nie 1:1. Uzgodnienie: faktura powstaje dla niepustej listy różnych, istniejących i jeszcze niezafakturowanych zamówień jednego klienta. Usługa przyjmuje numer faktury, datę wystawienia i ID zamówień. Numer jest obcinany z zewnętrznych spacji i normalizowany do wielkich liter przed sprawdzeniem unikalności. Data wystawienia nie jest wcześniejsza od dat żadnego z przypisywanych zamówień.

Usługa oblicza `TotalAmount` jako sumę zapisanych kwot zamówień, a wszystkie powiązania zapisuje atomowo. Nie można przypisać tego samego zamówienia do kolejnej faktury. Aktualizacja faktury może zmienić jej numer i datę przy zachowaniu walidacji; przekazany zestaw ID zamówień musi pozostać taki sam. Nie ma usuwania, korekt, podatków ani eksportu dokumentu w tym sprincie.

### P09 — adresy i sklepy

Uzgodnienie: zapis adresu wymaga istniejącego właściciela wskazanego zgodnie z P03. Aktualizacja zmienia pola adresowe, ale nie właściciela. Osobna operacja `AddressService` ustawia lub usuwa wskazania `BillingAddress` i `ShippingAddress` klienta; oba wskazania są zmieniane atomowo, a podanie adresu innego klienta lub sklepu jest błędem reguły biznesowej.

`StoreService` zapisuje nazwę sklepu i umożliwia przypisanie istniejącego `StationaryStoreEmployee` do istniejącego sklepu, w tym przeniesienie z innego sklepu. Zwykły `User`, `Customer` lub `Supplier` nie może zostać przypisany jako pracownik. Tworzenie kont, kategorii i stanów magazynowych nie otrzymuje osobnych usług w tym sprincie; dane do sprawdzeń są przygotowywane przez kontekst i Identity w testach.

## User stories

### S01-US-001 — uruchamialna solucja klient–serwer

**Opis techniczny:** przygotować wspólną bazę aplikacji WebStore, aby kolejne funkcje można było rozwijać w oddzielnych warstwach i uruchamiać lokalnie.

**Źródło:** PDF s. 2–3, sekcja 1; s. 7–12, sekcja 2; s. 12, zadanie 3.1; D01–D02; reguła P01. **Zależności:** brak.

- **S01-US-001-KA-01:** istnieje `WebStore.sln` z projektami `WebStore.Model`, `WebStore.DAL`, `WebStore.Services`, `WebStore.ViewModels`, `WebStore.Web`; wszystkie projekty C# korzystają z `net10.0`.
- **S01-US-001-KA-02:** referencje odpowiadają PDF: DAL → Model; ViewModels → Model; Services → Model, DAL, ViewModels; Web → Model, DAL, ViewModels, Services. Nie ma cyklu zależności.
- **S01-US-001-KA-03:** po odtworzeniu zależności zgodnie z instrukcją `dotnet build WebStore.sln` oraz budowanie klienta React kończą się powodzeniem.
- **S01-US-001-KA-04:** po wykonaniu udokumentowanych kroków i `dotnet run --project WebStore.Web` w przeglądarce otwiera się polskojęzyczna strona startowa React z nazwą WebStore. Nie są wymagane ekrany biznesowe ani biznesowe endpointy API.
- **S01-US-001-KA-05:** dokumentacja uruchomienia podaje wymagane wersje SDK, Node.js, bazy, polecenia instalacji zależności i adres lokalny; konfiguracja nie wymaga sekretów wpisanych w repozytorium.

### S01-US-002 — podstawowy model danych z UML

**Opis techniczny:** odwzorować użytkowników, klientów, dostawców, produkty, stany i zamówienia z diagramu, aby logika usług korzystała z jednolitego modelu domeny.

**Źródło:** PDF s. 12, diagram i zadanie 3.2; reguły P02, P05–P07. **Zależności:** S01-US-001.

- **S01-US-002-KA-01:** w warstwie Model istnieją `User`, `Customer`, `Supplier`, `Product`, `ProductStock`, `Order`, `OrderProduct` z polami i typami wynikającymi z diagramu oraz uzupełnieniami P02.
- **S01-US-002-KA-02:** `User` dziedziczy po `IdentityUser<int>`, a `Customer` i `Supplier` po `User`; nawigacje reprezentują klienta zamówienia, dostawcę produktu, stan produktu i pozycje zamówienia.
- **S01-US-002-KA-03:** pozycja zamówienia przechowuje ilość oraz cenę jednostkową według P07; kwoty, ilości i daty mają znaczenie określone w P05–P07. `Category` i adresy są docelowo obiektami, zgodnie z S01-US-003.

### S01-US-003 — adresy, sklepy, pracownicy, kategorie i faktury

**Opis techniczny:** rozszerzyć model o encje wskazane w zadaniach, aby można było wiązać klientów i sklepy z adresami, produkty z kategoriami oraz zamówienia z fakturami.

**Źródło:** PDF s. 12, zadania 3.3–3.6; s. 18, przykład Category; reguły P02–P03, P08–P09. **Zależności:** S01-US-002.

- **S01-US-003-KA-01:** istnieją encje `Address`, `StationaryStore`, `StationaryStoreEmployee`, `Category`, `Invoice` z polami opisanymi w P02.
- **S01-US-003-KA-02:** `Customer.BillingAddress` i `Customer.ShippingAddress` są opcjonalnymi nawigacjami do `Address`, a nie tekstem; klient ma także kolekcję własnych adresów.
- **S01-US-003-KA-03:** `Product.Category` wskazuje encję Category; `Order.Invoice` jest opcjonalną nawigacją do Invoice; faktura udostępnia kolekcję zamówień.
- **S01-US-003-KA-04:** `StationaryStoreEmployee` dziedziczy po User i wskazuje jeden sklep; sklep udostępnia kolekcje pracowników i adresów.

### S01-US-004 — relacyjna baza, TPH i migracja

**Opis techniczny:** utrwalać model w SQL Server i odtwarzać schemat migracją, aby kolejne sprinty korzystały z poprawnej bazy danych.

**Źródło:** PDF s. 2, sekcja 1.2; s. 12–13, sekcja 4; reguły P01, P03, P05. **Zależności:** S01-US-003.

- **S01-US-004-KA-01:** `ApplicationDbContext` w warstwie DAL integruje Identity z kluczem int oraz odwzorowuje wszystkie 12 encji z P02.
- **S01-US-004-KA-02:** baza odwzorowuje wszystkie osiem relacji 1:M wymienionych w P03 i N:M przez `OrderProduct`; klucz złożony blokuje powtórzenie produktu w tym samym zamówieniu.
- **S01-US-004-KA-03:** User, Customer, Supplier i StationaryStoreEmployee są zapisane w jednej tabeli hierarchii TPH z rozróżnieniem typu; odczyt odtwarza właściwy typ i jego pola.
- **S01-US-004-KA-04:** migracja tworzy schemat na pustej, wydzielonej bazie SQL Server LocalDB; ponowne zastosowanie tej samej migracji nie usuwa danych i nie tworzy ponownie tabel.
- **S01-US-004-KA-05:** sprawdzenie na SQL Server potwierdza FK, unikalność numeru faktury, wyłączną własność adresu, precyzję kwot oraz blokowanie niepoprawnych ilości i niedozwolonego usuwania według P03–P05. Testy EF InMemory nie zastępują tego sprawdzenia.
- **S01-US-004-KA-06:** instrukcja podaje konfigurację połączenia i powtarzalne polecenia utworzenia/zastosowania migracji; wykonanie nie wymaga zmiany ani usuwania cudzych baz.

### S01-US-005 — wspólna infrastruktura autonomicznych usług

**Opis techniczny:** zapewnić wspólne wstrzykiwanie zależności, mapowanie i kontrakty, aby każdą usługę można było wywołać i testować bez interfejsu webowego.

**Źródło:** PDF s. 13–16, sekcja 5.1; s. 19, konfiguracja testów; s. 21, sekcja 5.4; reguła P04. **Zależności:** S01-US-004.

- **S01-US-005-KA-01:** istnieją publiczne interfejsy `IProductService`, `IOrderService`, `IInvoiceService`, `IStoreService`, `IAddressService` w `Interfaces` i odpowiadające im implementacje w `ConcreteServices`.
- **S01-US-005-KA-02:** wszystkie pięć implementacji dziedziczy po `BaseService`, otrzymuje przez konstruktor kontekst, mapper i logger oraz daje się utworzyć przez kontener DI.
- **S01-US-005-KA-03:** modele wejściowe i wynikowe mają sufiks `Vm`, znajdują się w `ViewModels/VM`, a `MainProfile` zawiera potrzebne mapowania. Konfiguracja mapowania przechodzi sprawdzenie poprawności.
- **S01-US-005-KA-04:** każda usługa przestrzega P04: walidacja i błędy działają przy bezpośrednim wywołaniu; błędna operacja nie zapisuje części zmian, a modele wynikowe nie ujawniają danych uwierzytelniających.

### S01-US-006 — dodawanie i aktualizacja produktu

**Opis:** jako osoba utrzymująca katalog chcę dodawać i aktualizować dane produktu przez usługę, aby katalog zawierał właściwe informacje o ofercie.

**Źródło:** PDF s. 14–17, AddOrUpdateProduct; s. 20–21, testy produktu; reguły P04–P06. **Zależności:** S01-US-005.

- **S01-US-006-KA-01:** zapis poprawnego modelu z `Id = null` lub `0` tworzy dokładnie jeden produkt i zwraca VM z nadanym ID i zapisanymi danymi.
- **S01-US-006-KA-02:** zapis dodatniego ID istniejącego produktu aktualizuje ten rekord i nie zwiększa liczby produktów; nieistniejące ID kończy się błędem P04 bez zapisu.
- **S01-US-006-KA-03:** niepuste dane tekstowe/obraz oraz poprawne cena, waga, kategoria i dostawca są sprawdzane według P05–P06; błędny model jest odrzucany bez częściowych zmian.
- **S01-US-006-KA-04:** po zapisie dane można odczytać z nowego kontekstu. Aktualizacja produktu nie zmienia stanów magazynowych ani historycznych wartości pozycji zamówień.

### S01-US-007 — odczyt i filtrowanie produktów

**Opis:** jako osoba korzystająca z katalogu chcę pobierać pojedynczy produkt lub listę spełniającą warunek, aby otrzymywać potrzebne dane oferty.

**Źródło:** PDF s. 14–17, ProductVm, GetProduct, GetProducts; s. 20, testy odczytu; reguły P04, P06. **Zależności:** S01-US-006.

- **S01-US-007-KA-01:** odczyt pojedynczego produktu i listy, filtry, kolejność oraz brak dopasowania zachowują się zgodnie z P04.
- **S01-US-007-KA-02:** wynik zawiera ID, nazwę, opis, bajty obrazu, cenę, wagę, identyfikatory kategorii i dostawcy oraz Quantity.
- **S01-US-007-KA-03:** dla stanów 2 i 3 tego samego produktu Quantity wynosi 5; przy braku stanów wynosi 0. Odczyt nie zmienia danych.

### S01-US-008 — tworzenie, aktualizacja i odczyt zamówień

**Opis:** jako klient chcę, aby system zapisywał moje zamówienie z produktami, ilościami i wyliczoną kwotą, aby można było je później odczytać i rozliczyć.

**Źródło:** PDF s. 12–13, Order i OrderProduct; s. 21, OrderService; uzgodnione zachowanie P04–P05, P07. **Zależności:** S01-US-005, S01-US-006.

- **S01-US-008-KA-01:** poprawny zapis tworzy zamówienie klienta z co najmniej jedną pozycją, datą utworzenia, planowaną dostawą i numerem śledzenia według P07; modele wynikowe zawierają te dane i pozycje z cenami oraz ilościami.
- **S01-US-008-KA-02:** dla pozycji 2 sztuki po 10,00 i 1 sztuka po 5,00 kwota wynosi 25,00. Kwota i ceny jednostkowe pochodzą z obliczenia usługi, nie z danych podanych przez klienta.
- **S01-US-008-KA-03:** aktualizacja niefakturowanego zamówienia zastępuje pozycje i ponownie pobiera bieżące ceny, zachowując OrderDate. Sama zmiana ceny w katalogu nie zmienia zapisanych cen i kwoty zamówienia.
- **S01-US-008-KA-04:** pusty zestaw pozycji, powtórzony produkt, niepoprawne ilości/datowanie/numer śledzenia albo nieistniejący klient lub produkt są odrzucane zgodnie z P04–P07 bez częściowego zapisu.
- **S01-US-008-KA-05:** odczyt pojedynczy i lista spełniają P04; edycja zamówienia mającego InvoiceId jest blokowana. Zapis nie zmienia stanów magazynowych.

### S01-US-009 — wystawianie i odczyt faktur obejmujących zamówienia

**Opis:** jako osoba rozliczająca zamówienia chcę powiązać fakturę z zamówieniami jednego klienta i otrzymać ich łączną kwotę, aby zachować informację o ich rozliczeniu.

**Źródło:** PDF s. 12, zadanie 3.6; s. 13, Invoice–Order; s. 21, InvoiceService; uzgodnione zachowanie P04–P05, P08. **Zależności:** S01-US-008.

- **S01-US-009-KA-01:** zapis faktury dla zamówień tego samego klienta o kwotach 25,00 i 15,00 tworzy jedną fakturę o kwocie 40,00, wiąże oba zamówienia i zwraca ich identyfikatory, numer, datę i kwotę.
- **S01-US-009-KA-02:** pusty/powtórzony numer po normalizacji, niewłaściwa data, pusta lista, powtórzone lub nieistniejące zamówienie, różni klienci i ponowne fakturowanie są odrzucane według P04 i P08; błędny zapis nie przypisuje żadnego z zamówień.
- **S01-US-009-KA-03:** aktualizacja zmienia wyłącznie numer i datę z zachowaniem walidacji. Zmiana zestawu zamówień jest odrzucana, a kwota pozostaje sumą przypisanych zamówień.
- **S01-US-009-KA-04:** odczyt pojedynczy i lista spełniają P04; powiązania i kwota są zachowane po ponownym odczycie z bazy. Zmiana cen katalogowych nie zmienia kwoty faktury.

### S01-US-010 — obsługa sklepów i przypisanie pracowników

**Opis:** jako osoba zarządzająca sklepami chcę zapisywać sklepy i przypisywać do nich istniejących pracowników, aby odczytywać skład personelu każdej placówki.

**Źródło:** PDF s. 12, zadanie 3.4; s. 13, relacje sklepów i TPH; s. 21, StoreService; uzgodnione zachowanie P04, P09. **Zależności:** S01-US-005.

- **S01-US-010-KA-01:** dodanie/aktualizacja sklepu zapisuje niepustą nazwę zgodnie z P04; pusta nazwa i aktualizacja nieistniejącego sklepu są odrzucane.
- **S01-US-010-KA-02:** odczyt pojedynczy i lista spełniają P04; wynik zawiera ID i nazwę sklepu oraz jego adresy i identyfikatory pracowników, również gdy kolekcje są puste.
- **S01-US-010-KA-03:** przypisanie istniejącego StationaryStoreEmployee do istniejącego sklepu zapisuje jedną relację. Przeniesienie pracownika usuwa poprzednie przypisanie i wskazuje nowy sklep w ramach jednej operacji.
- **S01-US-010-KA-04:** nieistniejący sklep lub pracownik oraz osoba o innym typie niż StationaryStoreEmployee powodują błąd P04 bez zmiany przypisania.

### S01-US-011 — obsługa adresów klienta i sklepu

**Opis:** jako osoba utrzymująca dane klientów i sklepów chcę zapisywać adresy oraz wskazywać adres rozliczeniowy i wysyłkowy klienta, aby dane adresowe były spójne i powiązane z właścicielem.

**Źródło:** PDF s. 12, zadanie 3.3 i Customer–Address; s. 13, StationaryStore–Address; s. 21, AddressService; proponowane zachowanie P03–P04, P09. **Zależności:** S01-US-005, S01-US-010.

- **S01-US-011-KA-01:** zapis poprawnego adresu dla klienta albo sklepu utrwala pola P02 i jednego istniejącego właściciela; odczyt i lista spełniają P04 i pozwalają filtrować po właścicielu.
- **S01-US-011-KA-02:** brak właściciela, dwóch właścicieli, nieistniejący właściciel lub puste wymagane pola powodują błąd bez zapisu; ApartmentNumber może nie mieć wartości.
- **S01-US-011-KA-03:** aktualizacja zachowuje właściciela. Próba jego zmiany jest odrzucana jako konflikt reguły biznesowej.
- **S01-US-011-KA-04:** operacja ustawienia adresów klienta przyjmuje dwa opcjonalne ID; pozwala na ten sam własny adres w obu rolach i na wyczyszczenie wskazania przez null. Nieistniejący adres powoduje błąd P04, a cudzy adres konflikt reguły; nie następuje częściowa zmiana wskazań.

### S01-US-012 — powtarzalne testy usług i bazy

**Opis techniczny:** dostarczyć automatyczne sprawdzenia usług i relacyjnego modelu, aby programista i recenzent mogli wykrywać błędy oraz regresje.

**Źródło:** PDF s. 17–21, sekcje 5.2–5.4; s. 12–13, weryfikacja migracji; D01; reguły P01, P04. **Zależności:** S01-US-004–S01-US-011.

- **S01-US-012-KA-01:** `WebStore.Tests` jest dodany do solucji, używa net10.0 i xUnit, ma dostęp do Services, Model, DAL i potrzebnych ViewModels; infrastruktura zapewnia kontekst, DI, mapper i logger bez uruchamiania UI.
- **S01-US-012-KA-02:** istnieją testy każdego z pięciu serwisów, obejmujące poprawny zapis i aktualizację, odczyt pojedynczy, odczyt listy z filtrem i bez, brak wyniku, nieistniejące ID aktualizacji, null wymaganych argumentów i istotne odrzucenia wymienione w stories.
- **S01-US-012-KA-03:** scenariusze szczególne obejmują: produkt z Id null i 0, sumę stanów magazynowych, liczenie i aktualizację kwoty zamówienia, blokadę edycji po fakturowaniu, fakturę dla wielu zamówień, podwójne fakturowanie, przeniesienie pracownika oraz właściciela i wskazania adresu.
- **S01-US-012-KA-04:** dane testowe są odizolowane; przygotowanie danych kończy się przed testem. Dwukrotne uruchomienie pakietu daje te same wyniki i nie zależy od kolejności testów. Nie kopiujemy `async void` do przygotowania danych ani asercji porównującej wynik z nim samym.
- **S01-US-012-KA-05:** `dotnet test WebStore.sln` kończy się sukcesem w przygotowanym środowisku; uruchomienie pojedynczej klasy/testu przez `--filter` działa. Wymagane testy relacyjne na SQL Server są uruchomione i wskazane w raporcie, a nie pominięte jako zaliczone.
- **S01-US-012-KA-06:** wyniki testów i ręcznego sprawdzenia szkieletu web są przypisane do wersji kodu. Brak bazy, zależności lub możliwości uruchomienia testu jest zgłoszony jako blokada, bez deklarowania sukcesu.

### S01-US-013 — oddanie zweryfikowanego sprintu w GitHub

**Opis techniczny:** zapisać kompletny, zweryfikowany wynik sprintu w repozytorium lidera, aby mógł zostać odtworzony i oceniony.

**Źródło:** PDF s. 4–7, sekcja 1.4; s. 21, polecenie Commit & Push; D04–D05; główny `chatgpt.md`. **Zależności:** S01-US-001–S01-US-012, akceptacja kodu przez lidera i pomyślna weryfikacja kryteriów aplikacji przez recenzenta.

- **S01-US-013-KA-01:** kod, testy, migracje, pliki zależności i instrukcja uruchomienia są kompletne na gałęzi `main`; repozytorium nie zawiera sekretów, katalogów zależności, wyników kompilacji ani plików tymczasowych analizy.
- **S01-US-013-KA-02:** po akceptacji kodu i pomyślnej weryfikacji kryteriów aplikacji powstaje commit z oddawanym wynikiem; publikacja odbywa się do `origin/main` w `https://github.com/Buczu0/zpi.git`, bez force push i bez nadpisywania cudzych zmian.
- **S01-US-013-KA-03:** sprawdzenie zdalnej referencji potwierdza obecność commita. Dokument sprintu wskazuje identyfikator dostawy kodu i wynik publikacji; hash końcowego commita może zostać podany w rozmowie, aby nie tworzyć samoodwołującego wpisu we własnym commicie.
- **S01-US-013-KA-04:** jeżeli zdalna gałąź ma niezintegrowane zmiany albo publikacja nie może się powieść, zgłaszana jest blokada; ewentualna zmiana kodu przy integracji wymaga sprawdzeń, ponownej akceptacji lidera i recenzji zgodnie z zasadami projektu.

## Pokrycie dokumentu źródłowego

| Wymaganie PDF | Pokrycie lub rozstrzygnięcie |
| --- | --- |
| Środowisko .NET, SQL Server, VS Code, Git; s. 2–7 | S01-US-001, S01-US-004, S01-US-013; .NET według D01, Git według D04–D05; instrukcje obsługi edytora są materiałem pomocniczym. |
| Pięć warstw, referencje, kompilacja i uruchomienie React; s. 8–12 | S01-US-001; aktualizacja narzędzi według D01. |
| Diagram User/Customer/Supplier/Product/ProductStock/Order/OrderProduct; s. 12 | S01-US-002; pola odwzorowane w P02. |
| Address, StationaryStore, StationaryStoreEmployee, Category, Invoice; s. 12 | S01-US-003; pola i reguły zaproponowane na podstawie D03. |
| Osiem relacji 1:M, OrderProduct N:M, TPH, utworzenie i zastosowanie migracji; s. 12–13 | S01-US-004 i pełna lista P03. |
| Interfaces, ConcreteServices, VM, BaseService, mapper i logger; s. 13–16 | S01-US-005. |
| ProductService: zapis, odczyt, filtrowanie i przykłady testów; s. 14–21 | S01-US-006, S01-US-007, S01-US-012; sprzeczność Id = 0 rozstrzyga reguła P06. |
| OrderService i testy; s. 21 | S01-US-008, S01-US-012. |
| InvoiceService i testy; s. 21 | S01-US-009, S01-US-012. |
| StoreService i testy; s. 21 | S01-US-010, S01-US-012. |
| AddressService i testy; s. 21 | S01-US-011, S01-US-012. |
| Projekt testów, dane startowe, DI, uruchamianie i filtrowanie testów; s. 17–21 | S01-US-012; testy używają .NET 10 zgodnie z D01. |
| Commit & Push w gałęzi studenta; s. 4–7 i 21 | S01-US-013; lider zastąpił gałąź studenta istniejącą `main`. |

## Otwarte pytania i blokady

Publikacja K2 do `origin/main` jest zablokowana uwierzytelnianiem GitHub. `git push origin main` zwrócił „Invalid username or token” i „Authentication failed”. Lider został poproszony o zalogowanie przez `git credential-manager github login --browser` kontem z prawem zapisu do `Buczu0/zpi`, bez przekazywania sekretów w rozmowie. Po potwierdzeniu logowania recenzent ponowi push i sprawdzi zdalną referencję. Kod oraz testy są gotowe; nie ma otwartych usterek aplikacji.

## Implementacja — K1

Programista fullstack zrealizował S01-US-001–S01-US-012 oraz przygotował lokalną dostawę S01-US-013-KA-01 na `main`. Poniższe sprawdzenia są testami programisty, nie recenzją. Lider zaakceptował K1. S01-US-013-KA-02–KA-04 pozostają do wykonania po pomyślnej weryfikacji kryteriów aplikacji przez recenzenta. Nie wykonano commita ani push tej dostawy.

### Zakres i powiązania z kodem

| Stories | Dostarczone pliki i zachowanie | Sprawdzenia programisty |
| --- | --- | --- |
| S01-US-001 | `WebStore.sln`, sześć projektów, konfiguracja SDK i pakietów; `WebStore.Web/Program.cs`, `ClientApp`, instrukcja `README.md` | Pełna kompilacja z Vite, uruchomienie ASP.NET Core, strona React w przeglądarce. |
| S01-US-002–003 | 12 encji w `WebStore.Model/DataModels`, użytkownicy Identity, wymagane pola i nawigacje | Testy usług oraz odtworzenie typów TPH i danych w SQL Server. |
| S01-US-004 | `WebStore.DAL/EF/ApplicationDbContext.cs`, fabryka kontekstu, migracja `20261024131757_InitialCreate` i snapshot | Migracja na pustych bazach testowych, ponowne zastosowanie bez utraty danych, FK, CHECK, klucze i precyzja kwot. |
| S01-US-005 | `WebStore.Services/Interfaces`, `ConcreteServices/BaseService.cs`, `Configuration`, `Validation`, `WebStore.ViewModels/VM` | `ConfigurationTests`; walidacja bez HTTP, DI i AutoMapper; kontrolowana awaria zapisu i rollback na SQL Server. |
| S01-US-006–007 | `ProductService.cs`, VM produktu, mapowanie sumy stanów | `ProductServiceTests` (19 przypadków), testy SQL; tworzenie null/0, aktualizacja, filtry, stany, odrzucenia i trwałość. |
| S01-US-008 | `OrderService.cs`, VM zamówienia i pozycji | `OrderServiceTests`, testy SQL; suma 25,00, zastąpienie pozycji, bieżące ceny przy aktualizacji, zachowanie historii, blokada po fakturowaniu. |
| S01-US-009 | `InvoiceService.cs`, VM faktury | `InvoiceServiceTests`, testy SQL; grupa zamówień, normalizacja numeru, suma 40,00, niezmienny zestaw i blokowanie ponownego fakturowania. |
| S01-US-010 | `StoreService.cs`, VM sklepu | `StoreServiceTests`, test SQL współpracy usług; zapis i odczyt sklepu, adresy i personel, przeniesienie pracownika. |
| S01-US-011 | `AddressService.cs`, VM adresu i wskazań klienta | `AddressServiceTests`, testy SQL; jeden właściciel, brak zmiany właściciela, atomowe ustawienie/wyczyszczenie adresów i odrzucenie cudzych. |
| S01-US-012 | `WebStore.Tests/Infrastructure`, `UnitTests`, `IntegrationTests` | Dwa pełne przebiegi po 81/81 oraz uruchomienie klasy przez filtr: 19/19. Brak pominiętych testów. |
| S01-US-013-KA-01 | `.gitignore`, pliki lock, migracja, kod, testy, `README.md`; manifest K1 | Dostawa na `main`, bez zależności, wyników kompilacji, kluczy i plików tymczasowych w zestawie do Git. |

### Uruchomienie i ustawienia

Pełne polecenia znajdują się w [README.md](../../README.md). Zweryfikowane środowisko: Windows, SDK .NET 10.0.113, runtime/EF Core/Identity 10.0.13, Node.js 22.21.0, npm 10.9.4, React 19.3.1, Vite 8.3.5, AutoMapper 16.1.1, SQL Server LocalDB 2022 (16.0.1000.6).

Domyślne połączenie używa instancji `(localdb)\MSSQLLocalDB` i bazy `WebStore` z uwierzytelnianiem Windows. Zastosowano migrację do lokalnej bazy aplikacji. Zmienna `ConnectionStrings__WebStore` pozwala wskazać inną, wydzieloną bazę. Serwer uruchamia się na `http://localhost:5080`; build solucji buduje również klienta. Aplikacja nie tworzy danych przykładowych. Testy mają własne dane oraz osobne bazy `WebStoreTests_<GUID>`, usuwane po zakończeniu danego testu.

Zgodnie z D06 mapper odczytuje `AUTOMAPPER_LICENSE_KEY` ze środowiska procesu. Klucza nie otrzymano ani nie zapisano w repozytorium; sprawdzenia wykonano bez dostarczonego klucza. Brak klucza nie blokuje mapowania według dokumentacji AutoMappera, lecz może generować komunikat licencyjny. Nie zweryfikowano ważności licencji. Instrukcja opisuje samodzielne ustawienie klucza bez ujawniania go w kodzie lub rozmowie.

### Wykonane sprawdzenia

Data: **2026-10-24, Europe/Warsaw**. Wyniki dotyczą plików z manifestu K1.

| Polecenie lub scenariusz | Wynik |
| --- | --- |
| `dotnet tool restore` | Odtworzono lokalne narzędzie `dotnet-ef` 10.0.13. |
| `dotnet restore WebStore.sln --locked-mode` | Wszystkie sześć projektów odtworzono zgodnie z lockfile. |
| `npm ci` w `WebStore.Web/ClientApp` | Pakiety klienta odtworzone zgodnie z `package-lock.json`. |
| `dotnet build WebStore.sln --no-restore` | Sukces; 0 błędów, 0 ostrzeżeń; Vite zbudował klienta do `wwwroot`. |
| `dotnet ef database update --project WebStore.DAL --no-build` | Zastosowano migrację początkową w lokalnej bazie aplikacji `WebStore`. |
| `dotnet test WebStore.sln --no-build --no-restore` — przebieg 1 | 15:25:18–15:25:27: **81/81**, 0 błędów, 0 pominięć. |
| To samo polecenie — przebieg 2 | 15:26:05–15:26:12: **81/81**, 0 błędów, 0 pominięć. |
| `dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~ProductServiceTests'` | 15:27:13–15:27:15: **19/19**, 0 błędów, 0 pominięć. |
| `dotnet test WebStore.Tests/WebStore.Tests.csproj --no-restore --filter 'Category=SqlServer'` | 15:24:19–15:24:28: **4/4** na rzeczywistym SQL Server; te same cztery testy wchodzą do obu pełnych przebiegów. |
| `dotnet list WebStore.sln package --vulnerable --include-transitive --no-restore` | NuGet nie zgłosił znanych podatności w żadnym z sześciu projektów, także w zależnościach przechodnich. |
| `npm audit --audit-level=low` | 0 zgłoszonych podatności. |
| `dotnet run --project WebStore.Web --no-build --no-restore` i przeglądarka | Strona React pod `localhost:5080`, tytuł „WebStore — witamy”, `lang=pl`, nazwa WebStore i polskie treści, poprawny układ bez poziomego przepełnienia przy szerokości 1103 px; kliknięcie odnośnika logo poprawnie wraca do strony głównej. Konsola nie zgłosiła ostrzeżeń ani błędów. |

Test `Migration_is_repeatable_and_roundtrips_tph_and_utc` sprawdza migrację dwukrotnie, zachowanie danych, cztery typy hierarchii użytkowników, UTC i rzeczywistą precyzję `decimal(18,2)`. `Database_enforces_foreign_keys_checks_unique_keys_and_no_cascade` wykonuje odrzucone zapisy w SQL Server. `Five_services_work_together_on_sql_server` przechodzi przez wszystkie pięć usług. `Failure_after_order_insert_rolls_back_entire_write_and_allows_retry` wymusza awarię po zapisie nagłówka zamówienia i przed zapisem pozycji; potwierdza brak częściowych danych oraz udane ponowienie operacji.

Dowód sprawdzenia interfejsu: [strona-startowa.png](strona-startowa.png), SHA-256 `918857883d90bd664b3fb84c87a84f86c59f5a8a678b7e19b4d294f1ff980a33`. Zrzut jest dowodem w raporcie, nie składnikiem kodu K1.

### Ograniczenia i stan przekazania

Zakres kończy się na szkielecie webowym i autonomicznych usługach; nie dodano katalogu, koszyka, logowania ani biznesowych endpointów HTTP zgodnie z D02. Interfejs sprawdzono w przeglądarce desktopowej. Testy InMemory nie zastępują testów SQL, które zostały faktycznie wykonane. Kontrole zależności opisują stan zgłoszeń na dzień sprawdzenia.

Recenzja K1 zakończyła się wynikiem WYMAGA_POPRAWEK (R01–R02). Publikacja do `origin/main` pozostaje do wykonania po akceptacji poprawionej wersji kodu i pomyślnej recenzji.

### Identyfikacja dostawy K1

Bazowy commit: `71488d19b9570031dfc08c1b54eb79ba4b4bfb88`, gałąź `main`. Dostawa obejmuje 80 dodanych plików poniżej. Zmienione pliki wcześniej śledzone: brak. Usunięte pliki: brak. Dokument sprintu i zrzut ekranu są metadanymi procesu, poza identyfikacją kodu. Sumy SHA-256 odnoszą się do bajtów lokalnych plików przekazanych do akceptacji; ewentualna normalizacja końców linii przez Git wymaga uwzględnienia przy porównywaniu z późniejszym commitem.

| Dodany plik | SHA-256 |
| --- | --- |
| `.config/dotnet-tools.json` | `4b894c9f239697dc1566ef65d72733d6217206c3c9d30399ee899805d0fdff65` |
| `.gitignore` | `7a4929da4c6ad57569f4c3571269237249387db09d12f7007381deb50621cfe7` |
| `Directory.Build.props` | `d89fd08ec70ebb27deb05f7aa253a77bd48c8639f46d9bba09ffac9bcfcbcc62` |
| `NuGet.Config` | `f545a7e2ea14ac53cdfb91217cae67b8ff14275313f6298f9a3331d205b6c948` |
| `README.md` | `d7bed48e4b437b87d919c08c271432f15702a42720c6b41fccb86f7a4f22b589` |
| `WebStore.DAL/EF/ApplicationDbContext.cs` | `c64ce2b79298a81f16429989e745008a7feab849fadd818b9ee831a202f70d8c` |
| `WebStore.DAL/EF/ApplicationDbContextFactory.cs` | `dac8247bad9f23b3458d3199900a82203599d4fc67b02598b947d097b0864c7e` |
| `WebStore.DAL/Migrations/20261024131757_InitialCreate.Designer.cs` | `b8b8da23d5e7343069f5a20519e96a9f37a80fa8bdc32cd42984922388bd0c69` |
| `WebStore.DAL/Migrations/20261024131757_InitialCreate.cs` | `10c5ada7a352d19d38a7f9667f336643caaa8555b2017858f3c33e0c97ac4a66` |
| `WebStore.DAL/Migrations/ApplicationDbContextModelSnapshot.cs` | `00a2796d36931681a4f4a62ec791c1b00d5e6ddacecb849b4351b92b7693c9df` |
| `WebStore.DAL/WebStore.DAL.csproj` | `1dfae0acc1f5b52e3846bb1591404446074178fb216ec45bf8780cf62079ba87` |
| `WebStore.DAL/packages.lock.json` | `eefe5c2fb495720453895b850a1d741f9be277009dd657797141d5f9ac2b02ef` |
| `WebStore.Model/DataModels/Address.cs` | `a0d7035d44526cfcbecd34c70a072a03f718119c3f6f7eb99d4e66a76115d15d` |
| `WebStore.Model/DataModels/Category.cs` | `720cd30ed79947259f6a0ad78df11bfbdf74265dc46df57ec166047a074c4bc8` |
| `WebStore.Model/DataModels/Customer.cs` | `29971641070caf7c99abaacf0c6e0e994f7743ac146b8cfa9cc3da3914c3aed9` |
| `WebStore.Model/DataModels/Invoice.cs` | `fdab68fd5e561a8a339a52065cc0270fd488c1ef457a7b3057fadcf52f716205` |
| `WebStore.Model/DataModels/Order.cs` | `de1223a0e4aad5c51a80b32ffb9098db8ef59cdf2178115849b09b0d69dc649f` |
| `WebStore.Model/DataModels/OrderProduct.cs` | `2d9f82597add5fed303d9a910ce5b9e473bb0c9fe60fbccd243e6baf4691ce8a` |
| `WebStore.Model/DataModels/Product.cs` | `7259c0f6e4081b4b93edf905671cadf496e19edf0b24594fb04f4da923ae3b43` |
| `WebStore.Model/DataModels/ProductStock.cs` | `09138bce595a9de08e12204e1b18d870aedb5b784c22b0529f9e21aabb563ddf` |
| `WebStore.Model/DataModels/StationaryStore.cs` | `bda73d0185822dc7768b0a8454833cd2982884367f4b9c65e03510dfbe147489` |
| `WebStore.Model/DataModels/StationaryStoreEmployee.cs` | `542837aa0afff54c54f18bcbfb2b989079a416a1ef8a0e1ed619aef217278fe7` |
| `WebStore.Model/DataModels/Supplier.cs` | `d2e203a240358270c4bcc7613dd76733327cd951c32c78f88d29bc71fc65ad50` |
| `WebStore.Model/DataModels/User.cs` | `a618bf214ac18a1b5d9486bf4c4646797e97ffa570873a35b975c1477e10161e` |
| `WebStore.Model/WebStore.Model.csproj` | `5534738dcd53d9db047dc4519b8de6f21e8deb5085fc8265e6e77cd6f979f7d7` |
| `WebStore.Model/packages.lock.json` | `af735d61b11fd4e4c8c9a2a12fcc0c348f886b6b13ec21bb58f951af9795ab54` |
| `WebStore.Services/ConcreteServices/AddressService.cs` | `65ed290b246e2b5602794e1342e3982d8bbfa0265f016b111af010d24e92d9e8` |
| `WebStore.Services/ConcreteServices/BaseService.cs` | `ea550a3b32fc8cee2fb0ea5efba042966824a7e2d922f251bb29e18bb60ec75a` |
| `WebStore.Services/ConcreteServices/InvoiceService.cs` | `95939bb539c1c211492fe9b6df9ea21776affb17cc69989ef55a92e786aafdf4` |
| `WebStore.Services/ConcreteServices/OrderService.cs` | `183f5137b5da73becead1a7410745f6687d84f6484136c8f72441a201cad2403` |
| `WebStore.Services/ConcreteServices/ProductService.cs` | `a7900bbfd495be2208a518ca95d0d576b64334e1bbcfb777c1379b3e179959e6` |
| `WebStore.Services/ConcreteServices/StoreService.cs` | `12e3ac241e18f008f00fd59bea31abc6d77579ff8441b986a00919c9474f4594` |
| `WebStore.Services/Configuration/Profiles/MainProfile.cs` | `bdb9f74a0aa00a818619f04ea927d9dcaaf26f59d2f63ffad8a315a12c25f07a` |
| `WebStore.Services/Configuration/ServiceCollectionExtensions.cs` | `e472d77d8781d0dde0c0f130b98761cbc5e1452950db219a983f606e3afece42` |
| `WebStore.Services/Interfaces/IAddressService.cs` | `8a20f6d98475839e88b71c608ce93b89b4f78407d9065386563b127d2b594f71` |
| `WebStore.Services/Interfaces/IInvoiceService.cs` | `ebef1e8a61283b68ecd078a96286029ed94c2c20f799dc5843ad866045b8b507` |
| `WebStore.Services/Interfaces/IOrderService.cs` | `9a559eb0549f576a2bee630848cfca9b104b18cd4f3cc5517fd95e53ed6c9190` |
| `WebStore.Services/Interfaces/IProductService.cs` | `29d51ba44607eb7f1c98e63d893eb780629b08f82a644226addad62d0b8b9a52` |
| `WebStore.Services/Interfaces/IStoreService.cs` | `f967709665884cde60b9ac5a77aa77918fd3df5a63df4959a85e6bc034fe20a1` |
| `WebStore.Services/Validation/Guard.cs` | `12edbce9fbae8463e2e5e3b5226b7d3c704b13b0440b285f7a3d91b79863736f` |
| `WebStore.Services/WebStore.Services.csproj` | `60b9862f6385330b39e092edf0dd22a47ea62ad93642dc3103d3a4c49ec7ce59` |
| `WebStore.Services/packages.lock.json` | `b7d414b9d7c3ac4d2ac74e86df3f12a598d78e1bb1d4e69e7702182cad5ed505` |
| `WebStore.Tests/Infrastructure/TestEnvironment.cs` | `6594e8fbc760909987ff3cec05e2c14e83f8027da937e3e1cd79b1cdbbde67ee` |
| `WebStore.Tests/IntegrationTests/SqlServerTests.cs` | `97e05175913c6d11f5a73a798768ee44e0932d01178306b685e0ecb5bf973396` |
| `WebStore.Tests/UnitTests/AddressServiceTests.cs` | `c995a1d57a92e89fa0dd90c7cb663cdf38892b39d740c4203e2f9b6bbe163075` |
| `WebStore.Tests/UnitTests/ConfigurationTests.cs` | `8e3a91e26810faa6bf9963f68840c1f2dae25e17c56c6a119a1956a4a0cf7c97` |
| `WebStore.Tests/UnitTests/InvoiceServiceTests.cs` | `39b109ab849a97769cb236d5afd1ff69fe0659642a5ac6f5c03053768c1a9598` |
| `WebStore.Tests/UnitTests/OrderServiceTests.cs` | `dc12d83ae6466cd96c4bea767ff0656f23035f50a3082540f7e032b007a4c9a8` |
| `WebStore.Tests/UnitTests/ProductServiceTests.cs` | `342207e3bc590919e1c4f0a2a1dcba6904574da06c7fb78e3169d355faa2e015` |
| `WebStore.Tests/UnitTests/StoreServiceTests.cs` | `d4dcd3b40651b8d20abd9f4710bb4ee311921be10e498bd57eb6f814611c1eb6` |
| `WebStore.Tests/WebStore.Tests.csproj` | `95f280cf0ace87dd54e38492bc72b87aad4c59e289053a1ec7f7eacee4a02f25` |
| `WebStore.Tests/packages.lock.json` | `44a4c444231a8832e15e74fe9f159105979fa64153f4bc22be41c1529f902a25` |
| `WebStore.ViewModels/VM/AddOrUpdateAddressVm.cs` | `9883254953304751b4222c3862de78e67cd9fbb0ff4594d178cf12da9b4e284b` |
| `WebStore.ViewModels/VM/AddOrUpdateInvoiceVm.cs` | `29cbf2f82ec16b0c53cf379be57fc3a9fa11a48bf6e88cce897b086dce823c61` |
| `WebStore.ViewModels/VM/AddOrUpdateOrderVm.cs` | `0b390c5cebbf652749f3ee2fa34fd8eb4b0ee5cc2cfeaa147ef6209b04f14f96` |
| `WebStore.ViewModels/VM/AddOrUpdateProductVm.cs` | `32f0d1e96144fe0856ec0c0f40260005b1cac78e3c75aec57d97053386eb90d0` |
| `WebStore.ViewModels/VM/AddOrUpdateStoreVm.cs` | `72cff881c3cfbe984ba6975af2d819eb81486bce1f24a29ca967c760b761c438` |
| `WebStore.ViewModels/VM/AddOrderProductVm.cs` | `978943f43bd5296256b50def04642460a15271e410b0e95a19705edaaf45a8b8` |
| `WebStore.ViewModels/VM/AddressVm.cs` | `7b3e48f652a640933aab0a6d424402f574dd5d387ad1e552b309994e93b0357a` |
| `WebStore.ViewModels/VM/CustomerAddressesVm.cs` | `66b6c08d31c17b98397df95b10f6d1634392c91b61f4ca207dfd2778dbd5bf11` |
| `WebStore.ViewModels/VM/InvoiceVm.cs` | `89348d4b528a0063fda405fc91c95db511fdd43ef39d8e528f0ac07db2be7622` |
| `WebStore.ViewModels/VM/OrderProductVm.cs` | `2654738e3d773e9f4331c69579d92cb313dcd8914393eea0ee88c6f8405b4557` |
| `WebStore.ViewModels/VM/OrderVm.cs` | `20a4f5621b6e2d89759641123cec08786a3064a814ec01c2bf3c78fc966f54f9` |
| `WebStore.ViewModels/VM/ProductVm.cs` | `50ff62bcbdb5057a76401ba9a68acfe398611db03526247a553c3a2ae12d9ce6` |
| `WebStore.ViewModels/VM/StoreVm.cs` | `3c609be2d71786f5d1e6070aac545240bbe310fd48ad2a5dcd9c20942bb5ecdc` |
| `WebStore.ViewModels/WebStore.ViewModels.csproj` | `6d8f7635da2d28aef1453c4247b48d701f12b681768ff015654a88fec0300bc8` |
| `WebStore.ViewModels/packages.lock.json` | `e4e985952e9eb413fa2d6456ea05b1c55f4007f068d6b1e06a316367ace5d94f` |
| `WebStore.Web/ClientApp/index.html` | `6f1abb05ff1efbb04578487948145c71b2bbf827d155013d9f4a586fd45f45a7` |
| `WebStore.Web/ClientApp/package-lock.json` | `4240c32d30b36530638ff54aac0599699a044a73135c5019c075a6613b8065a5` |
| `WebStore.Web/ClientApp/package.json` | `9196db715bdee5670d90a8ce7793053dceed757697cee09200edfef21af989c3` |
| `WebStore.Web/ClientApp/src/main.jsx` | `1bcf2bb66c6fcd54acbe5d8902b36111597ca44884d731d17661634b4ab4f89b` |
| `WebStore.Web/ClientApp/src/styles.css` | `9e7be5261542f7c9add783bef7d499544e4a485d48c9373abf4d0f0354813583` |
| `WebStore.Web/ClientApp/vite.config.js` | `4bc96adb167e6663807bc60386ca5c341984e4f834f33384e117122e6f4aa18a` |
| `WebStore.Web/Program.cs` | `654a5fd1e14b9c600df27643ebc244e998c082034260a25b55349849cc0c170c` |
| `WebStore.Web/Properties/launchSettings.json` | `77369c5968ecfa90e230fed0fff8c5dbcdde33f3585d3d02478c3ae2d88e8a95` |
| `WebStore.Web/WebStore.Web.csproj` | `6ec64faeca1cc1ab689de73cc3873f8e009d2c3cfdef046b6320a15231dca1d6` |
| `WebStore.Web/appsettings.json` | `181c3aa04761d198c3836536c2662c697429501240c1e5f93beb86d1c52a94e9` |
| `WebStore.Web/packages.lock.json` | `d312369a30f559331f3b3e16d48f1adaad728de04f638f5747b4cd0d52f6ef57` |
| `WebStore.sln` | `ba00beccec2e72b6698f55a3548d06fb3090a843f07a030877ac89378acc7cda` |
| `global.json` | `a8af83ff732582e3ef0beb81a807d92a8ccd33ff51aadd6bfa785fd8e3061e17` |

## Warunek przekazania i zakończenia

Po akceptacji US1 programista realizuje S01-US-001–S01-US-012 i przygotowuje lokalną dostawę spełniającą S01-US-013-KA-01. Przedstawia kod do akceptacji lidera; dopiero potem rozpoczyna się etap RECENZJA. Recenzent najpierw sprawdza kryteria aplikacji i kompletność lokalnej dostawy. Po ich spełnieniu, nadal w etapie RECENZJA, realizowane jest oddanie według S01-US-013-KA-02–KA-04 i sprawdzana obecność wyniku w GitHub. Nie ogłaszaj wyniku ZGODNE przed potwierdzeniem wszystkich kryteriów, w tym publikacji.

Publikacja ma dotyczyć dokładnie zweryfikowanych treści kodu. Końcowe uzupełnienie dokumentacji procesu nie zmienia wersji kodu. Po potwierdzeniu publikacji i wszystkich pozostałych kryteriów można nadać etap `ZAKONCZONY`. Akceptacja kodu i recenzja pozostają odrębnymi etapami po implementacji.


## Recenzja K1 — wynik WYMAGA_POPRAWEK

Data: **2026-10-24, Europe/Warsaw**. Sprawdzono US1 i zaakceptowaną K1, bazowy commit oraz manifest 80 plików z sekcji identyfikacji. Kod, konfiguracja i trwałe testy nie były zmieniane podczas recenzji. Rola recenzenta zakończyła weryfikację wynikiem **WYMAGA_POPRAWEK** i przekazuje R01–R02 programiście. Dotychczasowa akceptacja lidera pozostaje dowodem akceptacji K1; nie obejmuje przyszłej K2.

### Własne sprawdzenia recenzenta

Środowisko jak w sekcji implementacji: .NET SDK 10.0.113, EF Core 10.0.13, LocalDB 2022, Node 22.21.0. Własny `dotnet build WebStore.sln --no-restore` zbudował .NET i React bez błędów i ostrzeżeń. Wykonano:

- `dotnet test WebStore.sln --no-build --no-restore`: 15:48:10–15:48:21, **81/81**, oraz 15:49:34–15:49:46, **81/81**; 0 błędów i pominięć, w obu przebiegach rzeczywiste testy SQL.
- `dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~ProductServiceTests'`: 15:50:02–15:50:04, **19/19**, bez błędów i pominięć.
- Własne uruchomienie ASP.NET Core na porcie 5080 i kontrola przeglądarki: widoczna strona React WebStore, polskie treści, `lang=pl`, poprawny wygląd, konsola bez ostrzeżeń i błędów.
- Dwa dodatkowe scenariusze na losowych bazach SQL utworzonych przez `TestEnvironment(sqlServer: true)`. Tymczasowy program diagnostyczny odwoływał się do zaakceptowanych projektów, nie modyfikował ich i nie używał bazy aplikacji. Potwierdził R01 i R02 opisane poniżej.

### Wynik każdego kryterium

**50 SPEŁNIONE, 3 NIESPEŁNIONE, 3 NIEZWERYFIKOWANE.** Niewykonane kryteria dotyczą publikacji, której warunek wstępny nie został spełniony.

| Kryterium | Wynik dla K1 | Dowód i rzeczywisty wynik |
| --- | --- | --- |
| S01-US-001-KA-01 | SPEŁNIONE | Solucja zawiera sześć projektów; Directory.Build.props ustawia net10.0. |
| S01-US-001-KA-02 | SPEŁNIONE | Odczyt wszystkich csproj potwierdza wymagane referencje i brak cykli. |
| S01-US-001-KA-03 | SPEŁNIONE | Własny dotnet build z Vite: 0 błędów, 0 ostrzeżeń. |
| S01-US-001-KA-04 | SPEŁNIONE | Własne uruchomienie localhost:5080 i przeglądarka: React, WebStore, polski tekst, brak błędów konsoli. |
| S01-US-001-KA-05 | SPEŁNIONE | README zawiera wersje, polecenia, adres i konfigurację środowiska bez klucza. |
| S01-US-002-KA-01 | SPEŁNIONE | Odczyt siedmiu wymaganych encji i porównanie z P02. |
| S01-US-002-KA-02 | SPEŁNIONE | Dziedziczenie IdentityUser<int>, Customer/Supplier oraz wymagane nawigacje w modelu. |
| S01-US-002-KA-03 | SPEŁNIONE | OrderProduct przechowuje Quantity i UnitPrice; testy kwot, dat i migrowany SQL. |
| S01-US-003-KA-01 | SPEŁNIONE | Pięć dodatkowych encji ma pola zatwierdzone w P02. |
| S01-US-003-KA-02 | SPEŁNIONE | Customer ma własne Addresses i opcjonalne wskazania Address. |
| S01-US-003-KA-03 | SPEŁNIONE | Nawigacje Product.Category, Order.Invoice i Invoice.Orders obecne; testy usług. |
| S01-US-003-KA-04 | SPEŁNIONE | StationaryStoreEmployee dziedziczy po User; wymagany sklep i kolekcje sprawdzone w modelu oraz SQL. |
| S01-US-004-KA-01 | SPEŁNIONE | ApplicationDbContext integruje Identity<int> i wszystkie 12 encji. |
| S01-US-004-KA-02 | SPEŁNIONE | Konfiguracja i migracja zawierają osiem relacji oraz klucz OrderId/ProductId; test SQL odrzuca duplikat. |
| S01-US-004-KA-03 | SPEŁNIONE | Test Migration_is_repeatable_and_roundtrips_tph_and_utc odtwarza cztery typy z Users. |
| S01-US-004-KA-04 | SPEŁNIONE | Ten sam test wykonuje migrację dwukrotnie na własnej bazie i zachowuje dane. |
| S01-US-004-KA-05 | SPEŁNIONE | Database_enforces_foreign_keys_checks_unique_keys_and_no_cascade oraz kontrola decimal(18,2): sukces. |
| S01-US-004-KA-06 | SPEŁNIONE | README i fabryka migracji zgodne; testy tworzą i usuwają wyłącznie własne bazy GUID. |
| S01-US-005-KA-01 | SPEŁNIONE | Pięć publicznych interfejsów i implementacji w wymaganych katalogach. |
| S01-US-005-KA-02 | SPEŁNIONE | Odczyt konstruktorów i ConfigurationTests: kontekst, mapper, logger, BaseService, DI. |
| S01-US-005-KA-03 | SPEŁNIONE | MainProfile przechodzi AssertConfigurationIsValid; VM mają wymagany sufiks i warstwę. |
| S01-US-005-KA-04 | NIESPEŁNIONE | R02: błędna faktura różnych klientów zostaje zapisana zamiast odrzucona przez regułę P04. Pozostałe scenariusze walidacji, rollback i brak danych Identity przechodzą. |
| S01-US-006-KA-01 | SPEŁNIONE | Create_assigns_id_and_persists_all_fields: warianty null i 0 przechodzą. |
| S01-US-006-KA-02 | SPEŁNIONE | Update_preserves_id_count_and_stocks i test brakującego ID przechodzą. |
| S01-US-006-KA-03 | SPEŁNIONE | Testy pustych pól, obrazu, kwot, wagi, referencji i braku częściowych zmian przechodzą. |
| S01-US-006-KA-04 | SPEŁNIONE | Nowy kontekst odczytuje produkt; testy stanów i historii cen zamówień przechodzą. |
| S01-US-007-KA-01 | SPEŁNIONE | Queries_filter_sort_and_return_null_or_empty: oczekiwane filtry, sortowanie i brak wyniku. |
| S01-US-007-KA-02 | SPEŁNIONE | Odczyt ProductVm i testy mapowania zapisanych pól. |
| S01-US-007-KA-03 | SPEŁNIONE | Testy oraz SQL: stany 2+3 dają 5, brak stanów daje 0; AsNoTracking. |
| S01-US-008-KA-01 | SPEŁNIONE | Create_calculates_snapshot_and_does_not_change_stock sprawdza zapis, zegar UTC i pozycje. |
| S01-US-008-KA-02 | SPEŁNIONE | Ten sam test potwierdza 2×10+1×5=25; wejście nie zawiera kwot. |
| S01-US-008-KA-03 | SPEŁNIONE | Update_replaces_lines_and_reprices_without_resetting_date: zastąpienie pozycji, 36,00 i zachowana OrderDate. |
| S01-US-008-KA-04 | SPEŁNIONE | Testy pustych/powtórzonych pozycji, ilości, dat, numeru śledzenia i referencji przechodzą. |
| S01-US-008-KA-05 | NIESPEŁNIONE | R01: po fakturowaniu w drugim kontekście aktualizacja pozycji z tą samą sumą nie jest blokowana. |
| S01-US-009-KA-01 | SPEŁNIONE | Groups_orders_normalizes_number_and_freezes_amount: dwie kwoty 25 i 15 dają fakturę 40. |
| S01-US-009-KA-02 | NIESPEŁNIONE | R02: po zmianie klienta w drugim kontekście faktura łączy dwóch różnych klientów. Pozostałe dostarczone scenariusze odrzuceń przechodzą. |
| S01-US-009-KA-03 | SPEŁNIONE | Update_changes_only_header oraz Existing_order_set_cannot_be_changed przechodzą. |
| S01-US-009-KA-04 | SPEŁNIONE | Testy odczytu, SQL i zmiany cen katalogowych zachowują kwotę oraz powiązania. |
| S01-US-010-KA-01 | SPEŁNIONE | Testy sklepu: tworzenie, aktualizacja, niepusta nazwa i brakujące ID przechodzą. |
| S01-US-010-KA-02 | SPEŁNIONE | Testy odczytu sprawdzają filtry, kolejność, adresy, personel i puste kolekcje. |
| S01-US-010-KA-03 | SPEŁNIONE | Employee_is_moved_between_stores oraz scenariusz SQL przechodzą. |
| S01-US-010-KA-04 | SPEŁNIONE | Invalid_assignment_does_not_move_employee sprawdza brakujące ID i niewłaściwe typy. |
| S01-US-011-KA-01 | SPEŁNIONE | Create_update_and_queries_preserve_owner oraz SQL: adres klienta i sklepu, filtry, trwałość. |
| S01-US-011-KA-02 | SPEŁNIONE | Testy pól, dokładnie jednego właściciela, jego istnienia oraz null ApartmentNumber przechodzą. |
| S01-US-011-KA-03 | SPEŁNIONE | Owner_cannot_be_changed: konflikt bez zmiany zapisanego właściciela. |
| S01-US-011-KA-04 | SPEŁNIONE | Testy wspólnego adresu, czyszczenia, cudzych i brakujących adresów oraz atomowości przechodzą. |
| S01-US-012-KA-01 | SPEŁNIONE | Odczyt csproj, solucji i ConfigurationTests potwierdza konfigurację testów i DI. |
| S01-US-012-KA-02 | SPEŁNIONE | Dostarczone klasy testów pokrywają wskazane operacje i odrzucenia wszystkich pięciu usług. |
| S01-US-012-KA-03 | SPEŁNIONE | Dostarczone scenariusze szczególne przechodzą; dodatkowe warianty między kontekstami ujawniły R01/R02. |
| S01-US-012-KA-04 | SPEŁNIONE | Własne dwa przebiegi: 81/81 i 81/81; osobne bazy, oczekiwany SeedAsync, brak async void. |
| S01-US-012-KA-05 | SPEŁNIONE | Własne pełne przebiegi obejmują 4 testy SQL; filtr ProductServiceTests: 19/19. |
| S01-US-012-KA-06 | SPEŁNIONE | Niniejszy raport wiąże własne wyniki i sprawdzenie UI z niezmienioną K1. |
| S01-US-013-KA-01 | SPEŁNIONE | Manifest 80/80 zgodny; komplet plików na main, ignorowane zależności/wyniki i brak sekretów w dostawie. |
| S01-US-013-KA-02 | NIEZWERYFIKOWANE | Nie wykonano: niespełnione kryteria aplikacji blokują commit i push. |
| S01-US-013-KA-03 | NIEZWERYFIKOWANE | Nie wykonano publikacji ani sprawdzenia zdalnej dostawy. |
| S01-US-013-KA-04 | NIEZWERYFIKOWANE | Nie uruchomiono etapu publikacji; warunek wstępny z KA-02 nie jest spełniony. |

### S01-R01 — edycja zafakturowanego zamówienia przez kontekst ze starszym stanem

**Priorytet: wysoki. Kryteria:** S01-US-008-KA-05, reguła P07. Pliki: `WebStore.Services/ConcreteServices/OrderService.cs` (odczyt encji i sprawdzenie InvoiceId, linie 31–35) oraz `BaseService.cs` (zachowanie śledzonych encji po udanym zapisie).

**Warunki:** SQL Server; dwa osobne konteksty A i B współdzielące tę samą, wydzieloną bazę; produkty 1 i 2 kosztują odpowiednio 10,00 i 5,00; klient 1 istnieje. Operacje są wykonywane kolejno, bez potrzeby wyścigu czasowego.

1. W kontekście A utwórz przez OrderService zamówienie: 2 sztuki produktu 1 i 1 sztuka produktu 2, suma 25,00. Pozostaw kontekst A do następnego wywołania.
2. W kontekście B wystaw przez InvoiceService fakturę dla tego zamówienia i zakończ operację B.
3. W kontekście A zaktualizuj to samo zamówienie, zmieniając wyłącznie pozycje na 1 sztukę produktu 1 i 3 sztuki produktu 2. Klient, daty i numer śledzenia pozostają te same; suma nadal wynosi 25,00.
4. Odczytaj zamówienie i pozycje z nowego kontekstu.

**Oczekiwane:** InvalidOperationException i zachowanie ilości 2/1. **Rzeczywiste:** wywołanie zwraca sukces; w bazie InvoiceId=1 oraz ilości 1/3. Kontrola w świeżym kontekście prawidłowo odrzuca edycję, co izoluje problem do stanu śledzenia.

Wynik diagnostyczny: `editAccepted=True; invoiceId=1; total=25,00; quantities=1,3`; kontrola: `freshContextBlocks=True`.

**Przyczyna:** odczyt śledzący zwraca starszą instancję z InvoiceId=null. Transakcja Serializable nie odświeża obiektu w ChangeTracker. Przy niezmienionym nagłówku EF aktualizuje tylko OrderProducts, więc tokeny współbieżności na Order nie są sprawdzane. Wymagana poprawka musi sprawdzać bieżący stan zamówienia i uniemożliwiać zapis pozycji zafakturowanego zamówienia również przy ponownym użyciu usług.

### S01-R02 — wspólna faktura dla zamówień różnych klientów

**Priorytet: wysoki. Kryteria:** S01-US-009-KA-02 oraz S01-US-005-KA-04, reguły P04 i P08. Plik: `WebStore.Services/ConcreteServices/InvoiceService.cs`, linie 38–42.

**Warunki:** jak w R01, dodatkowo istnieje klient 2. Operacje są wykonywane kolejno.

1. W kontekście A utwórz przez OrderService dwa niefakturowane zamówienia klienta 1, każde o sumie 25,00.
2. W kontekście B zaktualizuj pierwsze zamówienie przez OrderService: zmień CustomerId na 2, zachowując pozycje i sumę. Zakończ operację B.
3. W kontekście A wystaw przez InvoiceService jedną fakturę dla obu zamówień.
4. Odczytaj klientów i przypisania faktur z nowego kontekstu.

**Oczekiwane:** InvalidOperationException, brak faktury i oba InvoiceId=null. **Rzeczywiste:** zapis kończy się sukcesem; klienci zamówień to 2 i 1, a oba InvoiceId=1. Wynik diagnostyczny: `invoiceAccepted=True; customers=2,1; invoices=1,1`.

**Przyczyna:** walidacja używa starszych śledzonych wartości CustomerId. Tokeny współbieżności InvoiceId i TotalAmount nie zmieniły się przy zmianie klienta, więc zapis zostaje przyjęty. Wymagana poprawka musi sprawdzać aktualnych właścicieli zamówień w transakcji i atomowo odrzucać taką fakturę.

### Przekazanie po recenzji

Programista otrzymuje R01–R02 do poprawienia w zakresie istniejących US1/P04/P07/P08 oraz do objęcia trwałymi testami regresji SQL. Po poprawkach powstaje K2 z nowym manifestem i wynikami testów programisty. **Recenzja K2 może zacząć się dopiero po jej jawnej akceptacji przez lidera.** Do tego czasu nie publikujemy sprintu. Ważność klucza licencji AutoMappera nie była przedmiotem weryfikacji; konfiguracja środowiskowa pozostaje zgodna z D06.


## Implementacja poprawek — K2

**Status: zaakceptowana przez lidera 2026-10-24, przekazana do ponownej recenzji.** Po zakończeniu recenzji K1 rolę przejął programista fullstack. Nie zmieniono zakresu US1 ani kryteriów. Poniższe zmiany obejmuje jawna akceptacja K2.

### Zmiany i sprawdzenie usterek

- `BaseService.Write` po rozpoczęciu transakcji odłącza encje w stanie `Unchanged` od ChangeTracker. Kolejne zapytania w tej operacji wczytują aktualne dane z bazy, w tym InvoiceId i CustomerId. Oczekujące lokalne zmiany nie są odłączane. Walidacja oraz zapis pozostają w jednej transakcji Serializable.
- `SqlServerTests` zawiera dwa trwałe testy regresji R01–R02. Sprawdzają wyjątek InvalidOperationException oraz niezmienione dane po odrzuceniu, z osobnym odczytem w nowym kontekście.
- `README.md` opisuje bieżącą obsługę stanu kontekstu i pakiet 77 testów InMemory + 6 testów SQL.

| Usterka | Wynik programisty dla K2 | Weryfikacja recenzenta K2 |
| --- | --- | --- |
| S01-R01 | Test `Reused_context_blocks_edit_after_another_context_invoices_order` przechodzi: edycja odrzucona, InvoiceId zachowane, ilości nadal 2/1. | Potwierdzona naprawa w ponownej recenzji K2; usterka zamknięta. |
| S01-R02 | Test `Reused_context_rejects_invoice_after_another_context_changes_customer` przechodzi: brak nowej faktury, klienci 2/1 zachowani, oba InvoiceId=null. | Potwierdzona naprawa w ponownej recenzji K2; usterka zamknięta. |

### Wyniki sprawdzeń programisty K2

Data: **2026-10-24, Europe/Warsaw**, środowisko bez zmian względem K1.

| Sprawdzenie | Wynik |
| --- | --- |
| Nowe testy regresji na implementacji K1, filtr `FullyQualifiedName~Reused_context` | 15:55:35–15:55:41: **2 niepowodzenia na 2 testy**; w obu przypadkach wymagany wyjątek nie był zgłaszany. Potwierdza to, że testy wykrywają zgłoszone usterki. |
| Ten sam filtr po poprawce w BaseService | 15:57:36–15:57:42: **2/2**; polecenie `dotnet test WebStore.Tests/WebStore.Tests.csproj --no-restore --filter 'FullyQualifiedName~Reused_context'`. |
| `dotnet build WebStore.sln --no-restore` | Sukces, **0 błędów i 0 ostrzeżeń**, także build React przez Vite. |
| `dotnet test WebStore.sln --no-build --no-restore`, przebieg 1 | 15:58:05–15:58:14: **83/83**, 0 błędów, 0 pominięć; 6 testów rzeczywistego SQL Server. |
| To samo polecenie, przebieg 2 | 15:58:46–15:58:55: **83/83**, 0 błędów, 0 pominięć; 6 testów rzeczywistego SQL Server. |
| `dotnet run --project WebStore.Web --no-build --no-restore` i przeglądarka | K2 uruchomiona na porcie 5080; polska strona React widoczna, poprawny wygląd i brak ostrzeżeń/błędów konsoli. Pliki klienta mają te same sumy co w K1. |

Wyniki nie są ponowną recenzją ani zatwierdzeniem kodu za lidera. Konfiguracja licencji AutoMappera pozostaje bez zmian; nie dostarczono ani nie weryfikowano klucza. Nie wykonano commita ani push. Własny program diagnostyczny, wyniki tymczasowe i uruchomiony na potrzeby sprawdzeń serwer zostały usunięte/zatrzymane po zapisaniu wyników.

### Identyfikacja dostawy K2

Bazowy commit nadal `71488d19b9570031dfc08c1b54eb79ba4b4bfb88`, gałąź `main`. **Pełny manifest K2 to 80 ścieżek z tabeli „Identyfikacja dostawy K1”, z zastąpieniem dokładnie trzech sum poniżej.** Pozostałe 77 plików zachowuje sumy K1. Nie dodano ani nie usunięto ścieżek względem K1; wobec bazowego commita wszystkie 80 plików nadal stanowi lokalną dostawę dodaną. Metadane sprintu pozostają poza identyfikacją kodu.

| Plik ze zmienioną treścią względem K1 | SHA-256 w K2 |
| --- | --- |
| `README.md` | `c560253060d964b694f1b57e2ded0b998ac2c3423a0f792b2d67fdc4a71b28a3` |
| `WebStore.Services/ConcreteServices/BaseService.cs` | `127221770a616c744101156a82e377a3d22ebd2e6bd120e487c1a3e689a24212` |
| `WebStore.Tests/IntegrationTests/SqlServerTests.cs` | `f69ea8416d32db29afec0854610c08086f3b66bfdcd77ed9a9a625074e139fcc` |

## Ponowna recenzja K2

Data: **2026-10-24, Europe/Warsaw**. Wejście: US1, jawna akceptacja K2, raport R01–R02 oraz manifest 80 plików. Przed i po sprawdzeniach wszystkie sumy K2 były zgodne, a źródłowy PDF niezmieniony. Kod, trwałe testy i konfiguracja nie były zmieniane podczas tej recenzji.

### Wyniki i zamknięcie usterek

Własna kompilacja `dotnet build WebStore.sln --no-restore` z klientem React zakończyła się sukcesem: 0 błędów, 0 ostrzeżeń. Środowisko pozostało takie jak w K2: SDK .NET 10.0.113, EF Core 10.0.13, SQL Server LocalDB 2022, Node.js 22.21.0.

| Sprawdzenie recenzenta | Wynik |
| --- | --- |
| `dotnet test WebStore.sln --no-build --no-restore`, przebieg 1 | 16:09:59–16:10:12: **83/83**, 0 błędów i 0 pominięć. |
| To samo polecenie, przebieg 2 | 16:10:39–16:10:50: **83/83**, 0 błędów i 0 pominięć. |
| `dotnet test WebStore.Tests/WebStore.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~Reused_context'` | 16:11:37–16:11:41: **2/2** na SQL Server, bez pominięć. |
| Własne `dotnet run --project WebStore.Web --no-build --no-restore` i przeglądarka | Polskojęzyczna strona WebStore pod localhost:5080, poprawny wygląd, brak ostrzeżeń i błędów konsoli. |
| Przegląd kodu poprawki i regresji | Odłączenie niezmienionych encji poprzedza odczyty i walidację w transakcji; testy odczytują stan końcowy z nowego kontekstu. |
| Porównanie z manifestem i przegląd pozostałych kryteriów | 80/80 plików zgodnych; trzy zmienione względem K1 pliki sprawdzone, pozostałe 77 niezmienione. Ponowne testy pokrywają wszystkie pięć usług i model SQL. |

**S01-R01 zamknięta:** wyjątek przy próbie zmiany pozycji zafakturowanego zamówienia, InvoiceId zachowane, ilości nadal 2/1. **S01-R02 zamknięta:** próba połączenia zamówień aktualnie należących do klientów 2 i 1 zostaje odrzucona; nie powstaje faktura, oba InvoiceId pozostają null. Nie stwierdzono nowych usterek w zatwierdzonym zakresie.

### Kryteria K2

**54 SPEŁNIONE; 2 NIEZWERYFIKOWANE. Wynik ponownej recenzji: WERYFIKACJA_ZABLOKOWANA.** Kryteria aplikacji są spełnione, obie usterki zamknięte. Uwierzytelnianie blokuje publikację i potwierdzenie zdalnej dostawy; sprint pozostaje w etapie RECENZJA.

| Kryterium | Wynik dla K2 | Dowód i rzeczywisty wynik |
| --- | --- | --- |
| S01-US-001-KA-01 | SPEŁNIONE | Solucja zawiera sześć projektów; Directory.Build.props ustawia net10.0. |
| S01-US-001-KA-02 | SPEŁNIONE | Odczyt wszystkich csproj potwierdza wymagane referencje i brak cykli. |
| S01-US-001-KA-03 | SPEŁNIONE | Własny build K2 z Vite: sukces, 0 błędów i ostrzeżeń. |
| S01-US-001-KA-04 | SPEŁNIONE | Własne uruchomienie K2 pod localhost:5080: React WebStore, polskie treści, poprawny wygląd i czysta konsola. |
| S01-US-001-KA-05 | SPEŁNIONE | README zawiera wersje, polecenia, adres i konfigurację środowiska bez klucza. |
| S01-US-002-KA-01 | SPEŁNIONE | Odczyt siedmiu wymaganych encji i porównanie z P02. |
| S01-US-002-KA-02 | SPEŁNIONE | Dziedziczenie IdentityUser<int>, Customer/Supplier oraz wymagane nawigacje w modelu. |
| S01-US-002-KA-03 | SPEŁNIONE | OrderProduct przechowuje Quantity i UnitPrice; testy kwot, dat i migrowany SQL. |
| S01-US-003-KA-01 | SPEŁNIONE | Pięć dodatkowych encji ma pola zatwierdzone w P02. |
| S01-US-003-KA-02 | SPEŁNIONE | Customer ma własne Addresses i opcjonalne wskazania Address. |
| S01-US-003-KA-03 | SPEŁNIONE | Nawigacje Product.Category, Order.Invoice i Invoice.Orders obecne; testy usług. |
| S01-US-003-KA-04 | SPEŁNIONE | StationaryStoreEmployee dziedziczy po User; wymagany sklep i kolekcje sprawdzone w modelu oraz SQL. |
| S01-US-004-KA-01 | SPEŁNIONE | ApplicationDbContext integruje Identity<int> i wszystkie 12 encji. |
| S01-US-004-KA-02 | SPEŁNIONE | Konfiguracja i migracja zawierają osiem relacji oraz klucz OrderId/ProductId; test SQL odrzuca duplikat. |
| S01-US-004-KA-03 | SPEŁNIONE | Test Migration_is_repeatable_and_roundtrips_tph_and_utc odtwarza cztery typy z Users. |
| S01-US-004-KA-04 | SPEŁNIONE | Ten sam test wykonuje migrację dwukrotnie na własnej bazie i zachowuje dane. |
| S01-US-004-KA-05 | SPEŁNIONE | Database_enforces_foreign_keys_checks_unique_keys_and_no_cascade oraz kontrola decimal(18,2): sukces. |
| S01-US-004-KA-06 | SPEŁNIONE | README i fabryka migracji zgodne; testy tworzą i usuwają wyłącznie własne bazy GUID. |
| S01-US-005-KA-01 | SPEŁNIONE | Pięć publicznych interfejsów i implementacji w wymaganych katalogach. |
| S01-US-005-KA-02 | SPEŁNIONE | Odczyt konstruktorów i ConfigurationTests: kontekst, mapper, logger, BaseService, DI. |
| S01-US-005-KA-03 | SPEŁNIONE | MainProfile przechodzi AssertConfigurationIsValid; VM mają wymagany sufiks i warstwę. |
| S01-US-005-KA-04 | SPEŁNIONE | Pełny pakiet K2 oraz regresje: walidacja, brak częściowego zapisu, brak danych Identity; R02 naprawiona. |
| S01-US-006-KA-01 | SPEŁNIONE | Create_assigns_id_and_persists_all_fields: warianty null i 0 przechodzą. |
| S01-US-006-KA-02 | SPEŁNIONE | Update_preserves_id_count_and_stocks i test brakującego ID przechodzą. |
| S01-US-006-KA-03 | SPEŁNIONE | Testy pustych pól, obrazu, kwot, wagi, referencji i braku częściowych zmian przechodzą. |
| S01-US-006-KA-04 | SPEŁNIONE | Nowy kontekst odczytuje produkt; testy stanów i historii cen zamówień przechodzą. |
| S01-US-007-KA-01 | SPEŁNIONE | Queries_filter_sort_and_return_null_or_empty: oczekiwane filtry, sortowanie i brak wyniku. |
| S01-US-007-KA-02 | SPEŁNIONE | Odczyt ProductVm i testy mapowania zapisanych pól. |
| S01-US-007-KA-03 | SPEŁNIONE | Testy oraz SQL: stany 2+3 dają 5, brak stanów daje 0; AsNoTracking. |
| S01-US-008-KA-01 | SPEŁNIONE | Create_calculates_snapshot_and_does_not_change_stock sprawdza zapis, zegar UTC i pozycje. |
| S01-US-008-KA-02 | SPEŁNIONE | Ten sam test potwierdza 2×10+1×5=25; wejście nie zawiera kwot. |
| S01-US-008-KA-03 | SPEŁNIONE | Update_replaces_lines_and_reprices_without_resetting_date: zastąpienie pozycji, 36,00 i zachowana OrderDate. |
| S01-US-008-KA-04 | SPEŁNIONE | Testy pustych/powtórzonych pozycji, ilości, dat, numeru śledzenia i referencji przechodzą. |
| S01-US-008-KA-05 | SPEŁNIONE | Regresja R01 przechodzi na SQL: ponownie użyty kontekst odrzuca edycję po fakturowaniu, zachowuje InvoiceId i ilości 2/1. Pozostałe odczyty i stany przechodzą. |
| S01-US-009-KA-01 | SPEŁNIONE | Groups_orders_normalizes_number_and_freezes_amount: dwie kwoty 25 i 15 dają fakturę 40. |
| S01-US-009-KA-02 | SPEŁNIONE | Regresja R02 przechodzi na SQL: aktualni klienci 2/1 powodują odrzucenie, brak faktury i brak przypisań. Pozostałe odrzucenia również przechodzą. |
| S01-US-009-KA-03 | SPEŁNIONE | Update_changes_only_header oraz Existing_order_set_cannot_be_changed przechodzą. |
| S01-US-009-KA-04 | SPEŁNIONE | Testy odczytu, SQL i zmiany cen katalogowych zachowują kwotę oraz powiązania. |
| S01-US-010-KA-01 | SPEŁNIONE | Testy sklepu: tworzenie, aktualizacja, niepusta nazwa i brakujące ID przechodzą. |
| S01-US-010-KA-02 | SPEŁNIONE | Testy odczytu sprawdzają filtry, kolejność, adresy, personel i puste kolekcje. |
| S01-US-010-KA-03 | SPEŁNIONE | Employee_is_moved_between_stores oraz scenariusz SQL przechodzą. |
| S01-US-010-KA-04 | SPEŁNIONE | Invalid_assignment_does_not_move_employee sprawdza brakujące ID i niewłaściwe typy. |
| S01-US-011-KA-01 | SPEŁNIONE | Create_update_and_queries_preserve_owner oraz SQL: adres klienta i sklepu, filtry, trwałość. |
| S01-US-011-KA-02 | SPEŁNIONE | Testy pól, dokładnie jednego właściciela, jego istnienia oraz null ApartmentNumber przechodzą. |
| S01-US-011-KA-03 | SPEŁNIONE | Owner_cannot_be_changed: konflikt bez zmiany zapisanego właściciela. |
| S01-US-011-KA-04 | SPEŁNIONE | Testy wspólnego adresu, czyszczenia, cudzych i brakujących adresów oraz atomowości przechodzą. |
| S01-US-012-KA-01 | SPEŁNIONE | Odczyt csproj, solucji i ConfigurationTests potwierdza konfigurację testów i DI. |
| S01-US-012-KA-02 | SPEŁNIONE | Dostarczone klasy testów pokrywają wskazane operacje i odrzucenia wszystkich pięciu usług. |
| S01-US-012-KA-03 | SPEŁNIONE | Pełny pakiet K2 potwierdza wszystkie scenariusze szczególne; dodatkowo obie regresje R01/R02 przechodzą. |
| S01-US-012-KA-04 | SPEŁNIONE | Własne dwa przebiegi K2: 83/83 i 83/83, bez pominięć. Izolacja baz i oczekiwane przygotowanie danych bez zmian. |
| S01-US-012-KA-05 | SPEŁNIONE | Własne pełne przebiegi obejmują 6 testów SQL; osobny filtr FullyQualifiedName~Reused_context: 2/2. |
| S01-US-012-KA-06 | SPEŁNIONE | Niniejszy raport wiąże wyniki ponownej recenzji oraz sprawdzenie interfejsu z zaakceptowaną K2. |
| S01-US-013-KA-01 | SPEŁNIONE | Manifest K2 80/80 zgodny; komplet plików na main, bez sekretów, zależności i wyników kompilacji w dostawie. |
| S01-US-013-KA-02 | NIEZWERYFIKOWANE | Commit kodu 4995dc234ce0567360445e9c524691f5d34a8c48 utworzony; GitHub odrzucił push z powodu uwierzytelniania. |
| S01-US-013-KA-03 | NIEZWERYFIKOWANE | Po próbie push zdalna main nadal wskazuje 71488d19b9570031dfc08c1b54eb79ba4b4bfb88; dostawy K2 na niej brak. |
| S01-US-013-KA-04 | SPEŁNIONE | Brak nowych zmian do integracji; niepowodzenie publikacji zgłoszono jako blokadę. Nie użyto force push ani nie zmieniano kodu przy oddaniu. |

### Oddanie do GitHub

Repozytorium docelowe: `https://github.com/Buczu0/zpi.git`, gałąź `main`. Odczyt zdalnej referencji przed publikacją wskazał `71488d19b9570031dfc08c1b54eb79ba4b4bfb88`, zgodny z lokalną bazą dostawy; brak zmian do integracji. Zestaw dostawy obejmuje zaakceptowane pliki K2 oraz dokument sprintu i dowód sprawdzenia interfejsu.

Utworzono commit kodu **`4995dc234ce0567360445e9c524691f5d34a8c48`** („Dostarcz sprint-01 K2: fundament WebStore i testy”). Przed commitem potwierdzono bajtową zgodność wszystkich 80 plików K2 w indeksie Git oraz dokładnie 82 ścieżki dostawy z raportem i zrzutem. Jednorazowe `core.autocrlf=false` przy dodawaniu zachowało zaakceptowane końce linii; nie zmieniono konfiguracji repozytorium. Kontrola `git -c core.whitespace=cr-at-eol diff --cached --check` zakończyła się sukcesem.

Próba `git push origin main` zakończyła się błędem uwierzytelniania: „Invalid username or token”, „Password authentication is not supported for Git operations”, „Authentication failed”. Nie użyto force push. Następujący po niej `git ls-remote origin refs/heads/main` potwierdził niezmienioną zdalną referencję `71488d19b9570031dfc08c1b54eb79ba4b4bfb88`. GitHub CLI nie jest zainstalowane, a odczyt listy kont Git Credential Manager poza piaskownicą nie zwrócił aktywnego konta. Lider otrzymał instrukcję dokończenia logowania; tokenów ani haseł nie odczytywano i nie żądano w rozmowie.

Pozostało ponowić publikację po logowaniu i potwierdzić obecność dostawy w GitHub. Ostatnie aktualizacje raportu są metadanymi procesu i nie zmieniają K2 ani jej akceptacji.

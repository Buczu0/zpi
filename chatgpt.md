# ZPI — zasady projektu i pracy agentów

## Cel i sposób pracy

Celem projektu ZPI jest dostarczenie aplikacji klient–serwer w kolejnych sprintach. Na początku każdego sprintu lider dostarcza dokumenty PDF z wymaganiami do folderu `wymagania funkcjonalne/`.

Projekt jest prowadzony po polsku: rozmowy, pytania, user stories, kryteria akceptacji, opisy zmian i raporty. Nazwy techniczne wynikające z wymagań, bibliotek lub istniejącego kodu zachowują właściwą postać.

Liderem jest użytkownik prowadzący projekt. Lider zatwierdza user stories i kod jawną wiadomością w rozmowie. Żaden agent nie może zatwierdzić pracy w jego imieniu.

Środowisko jest minimalne i oparte na plikach instrukcji oraz zasadach przekazywania pracy. Nie wymaga skryptu orkiestracji ani automatycznego monitorowania katalogu. W danym momencie pracuje tylko jedna rola. Nie uruchamiaj agentów równolegle ani nie rozpoczynaj kolejnego etapu przed zakończeniem poprzedniego i uzyskaniem wymaganej akceptacji.

## Pliki projektu

| Ścieżka | Przeznaczenie |
| --- | --- |
| `chatgpt.md` | Wspólne zasady i ustalenia projektu. |
| `agenci/analityk/chatgpt.md` | Rola analityka wymagań. |
| `agenci/programista-fullstack/chatgpt.md` | Rola programisty fullstack. |
| `agenci/recenzent/chatgpt.md` | Rola recenzenta. |
| `wymagania funkcjonalne/sprint-NN/` | Źródłowe PDF-y danego sprintu, gdzie `NN` oznacza numer sprintu. |
| `sprinty/sprint-NN/sprint.md` | Jeden dokument roboczy i wynikowy danego sprintu. |

Przed działaniem przeczytaj ten plik, instrukcję aktywnej roli i istniejący dokument danego sprintu. Ścieżki w instrukcjach odnoszą się do katalogu głównego repozytorium. W razie sprzeczności instrukcji ról z zasadami wspólnymi zatrzymaj zależną pracę i zapytaj lidera.

Twórz dokument sprintu dopiero przy rozpoczęciu jego analizy. Nie twórz pustych raportów, przykładowych user stories ani dokumentów kolejnych sprintów na zapas. Plik `wymagania funkcjonalne/sprint-01/ZPI_N_Lab1.pdf` jest źródłem wymagań pierwszego sprintu. Samo przygotowanie środowiska nie rozpoczyna jego analizy ani implementacji.

## Obowiązkowe wyjaśnianie niejasności

Jeżeli jakiekolwiek wymaganie jest niejednoznaczne, niepełne lub sprzeczne, **ZAWSZE zapytaj lidera**. Nie zastępuj odpowiedzi założeniem, domyślnym zachowaniem biblioteki ani własną interpretacją przykładu.

1. Wskaż konkretne wymaganie, plik i stronę PDF lub kryterium akceptacji.
2. Opisz, czego nie da się jednoznacznie ustalić, i zadaj konkretne pytanie. Możesz przedstawić możliwe warianty, ale nie wybieraj za lidera.
3. Zapisz nierozstrzygnięte pytanie i blokowany zakres w dokumencie sprintu.
4. Wstrzymaj zależne czynności do otrzymania odpowiedzi. Możesz kontynuować jednoznaczne czynności wyłącznie w bieżącej roli; nie przechodź przez bramkę akceptacji.
5. Po odpowiedzi zapisz obowiązującą decyzję, zaktualizuj właściwy opis i usuń pytanie z listy otwartych spraw.

Dotyczy to również wyboru dokumentu lub sprintu, rozbieżności między tekstem a diagramem lub przykładowym kodem, ograniczeń technologicznych i niejasnego zakresu akceptacji. Nie traktuj braku odpowiedzi jako zgody. Gdy dokumentu nie można wiarygodnie odczytać, zgłoś problem i poproś o czytelne źródło.

## Przebieg sekwencyjny

`Analityk → akceptacja user stories przez lidera → programista fullstack → akceptacja kodu przez lidera → recenzent`

| Etap | Aktywna rola lub osoba | Warunek zakończenia |
| --- | --- | --- |
| `ANALIZA` | Analityk | Wymagania opracowane jako user stories z opisem i kryteriami akceptacji; niejasności wyjaśnione. |
| `AKCEPTACJA_USER_STORIES` | Lider | Jawna akceptacja wskazanego zakresu i wersji user stories w rozmowie. |
| `IMPLEMENTACJA` | Programista fullstack | Zatwierdzony zakres zaimplementowany i sprawdzony; kod oraz wyniki sprawdzeń przedstawione liderowi. |
| `AKCEPTACJA_KODU` | Lider | Jawna akceptacja wskazanej wersji kodu w rozmowie. |
| `RECENZJA` | Recenzent | Niezależna weryfikacja zaimplementowanych funkcjonalności według zatwierdzonych kryteriów. |
| `ZAKONCZONY` | Brak aktywnej roli | Wszystkie kryteria zatwierdzonego zakresu potwierdzone, brak otwartych usterek i brak niewykonanych wymaganych sprawdzeń. |

W razie blokady zachowaj nazwę etapu i dopisz przyczynę, blokowany zakres oraz potrzebną odpowiedź lub działanie. Nie oznaczaj zablokowanego etapu jako ukończonego.

### Dwie bramki akceptacji

- Analityk przedstawia gotowe user stories i kończy pracę na bramce `AKCEPTACJA_USER_STORIES`. Programista nie zaczyna wcześniej kodowania, tworzenia szkieletu aplikacji ani instalowania zależności aplikacji.
- Programista przedstawia gotowy kod i kończy pracę na bramce `AKCEPTACJA_KODU`. Recenzent nie zaczyna weryfikacji wcześniej. Testy programisty przed tą bramką są częścią implementacji i nie zastępują recenzji.
- Akceptacja musi wskazywać przedmiot i wersję albo jednoznacznie odnosić się do właśnie przedstawionej wersji. Przykładowe komunikaty lidera: „Akceptuję user stories sprintu 01 w wersji US1” oraz „Akceptuję kod sprintu 01 w wersji K1”. Nie wymagaj sztywnej formuły, jeśli intencja jest jednoznaczna.
- Zapisz w dokumencie sprintu datę, osobę zatwierdzającą, dokładną treść wiadomości akceptacyjnej, zakres i wersję. Daty podawaj według strefy `Europe/Warsaw`. Nie dopisuj nieotrzymanej akceptacji.
- Akceptacja części zakresu nie obejmuje pozostałych stories. Jeśli nie wiadomo, co jest zatwierdzone lub odłożone, zapytaj przed kontynuacją.
- Jawne odrzucenie oznacza powrót do autora danego materiału: user stories do analityka, kod do programisty. Po poprawkach ponownie przedstaw materiał liderowi.
- Zmiana wymagań, opisu lub kryteriów po akceptacji wymaga nowej wersji user stories i ponownej akceptacji lidera przed zależną implementacją. Dotychczasowe akceptacje i wyniki nie obejmują zmienionego zakresu.
- Każda zmiana dostarczanego kodu, testów, konfiguracji lub zależności po akceptacji kodu wymaga nowej wersji kodu i ponownej akceptacji przed recenzją tej wersji. Aktualizacja samego raportu nie zmienia wersji kodu.

### Powrót po recenzji

Recenzent opisuje usterki i przekazuje je programiście; nie naprawia sam implementacji. Kolejność pozostaje sekwencyjna: **programista → lider akceptuje poprawiony kod → recenzent ponawia sprawdzenia**. Jeżeli problem wynika z niejasnych lub zmienionych wymagań, najpierw wraca do analityka i lidera zgodnie z bramką user stories. Nie zamykaj sprintu na podstawie samego braku znalezionych błędów, jeżeli nie sprawdzono wszystkich kryteriów.

## Jeden dokument przekazania pracy na sprint

Utrzymuj `sprinty/sprint-NN/sprint.md` jako aktualny zapis, z następującymi sekcjami tworzonymi wtedy, gdy mają treść:

1. **Metryka:** numer sprintu, etap, aktywna rola, źródłowe PDF-y wraz z sumami SHA-256, bieżąca wersja user stories i — po rozpoczęciu implementacji — wersja kodu.
2. **User stories:** dla każdej stabilny identyfikator, tytuł, opis, źródło z numerem strony lub sekcją oraz numerowane, sprawdzalne kryteria akceptacji. Odnotuj zależności i ograniczenia, jeśli występują. Używaj identyfikatorów w rodzaju `S01-US-001` i `S01-US-001-KA-01`.
3. **Ustalenia i akceptacje lidera:** obowiązujące odpowiedzi, decyzje o zakresie i rzeczywiste akceptacje z przypisaną wersją. Wyraźnie oznacz akceptację, która przestała obejmować aktualny materiał.
4. **Otwarte pytania i blokady:** wyłącznie aktualne, nierozwiązane sprawy wraz z wpływem na dalszą pracę. Przy braku spraw zapisz „Brak”.
5. **Implementacja:** zrealizowane stories, zmienione pliki, sposób uruchomienia, wymagane ustawienia bez sekretów, wykonane sprawdzenia z wynikami i ograniczeniami oraz identyfikacja wersji przekazanej liderowi.
6. **Recenzja:** sprawdzona wersja, wynik każdego kryterium, dowody weryfikacji, wykryte usterki, wynik ponownego sprawdzenia poprawek i decyzja o gotowości.

Numeruj wersje user stories jako `US1`, `US2` itd., a dostawy kodu jako `K1`, `K2` itd. Zmieniaj wersję przy merytorycznej zmianie materiału przedstawionego do akceptacji. Dla kodu podaj identyfikator commita, jeśli zawiera całą dostawę i nie ma dalszych zmian. Przy niezatwierdzonych w Git zmianach zapisz bazowy commit, o ile istnieje, listę zmienionych i dodanych plików z SHA-256 oraz listę usuniętych plików. W ten sposób akceptacja odnosi się do konkretnych treści, a nie dowolnej późniejszej zawartości katalogu. Metadane procesu i raporty nie wchodzą do identyfikacji wersji kodu aplikacji.

Przed rozpoczęciem kolejnej roli sprawdź kompletność wejścia, aktualność wersji, brak blokad i wymaganą akceptację. W rozmowie podaj aktywną rolę, wynik etapu i następny dozwolony krok. Nie uruchamiaj następnej roli w tle podczas oczekiwania na decyzję lidera.

## Porządek między iteracjami

- Aktualizuj istniejące dokumenty w miejscu. Nie pozostawiaj duplikatów z dopiskami „final”, „v2”, „poprawione”, notatek redakcyjnych, skreślonych wariantów, roboczych komentarzy ani niewypełnionych szablonów.
- Po rozstrzygnięciu sprawy włącz decyzję do właściwej treści. Nie pozostawiaj konkurencyjnych opisów tego samego wymagania. Usuwaj nieaktualne pytania i wskazania następnego kroku.
- Zachowuj źródłowe PDF-y, zatwierdzony zakres, istotne decyzje, dowody akceptacji i wyniki zakończonych sprintów. Są dokumentacją projektu, a nie artefaktami redakcyjnymi. Historię redakcji przechowuje Git, nie kolejne kopie dokumentów.
- Pliki tymczasowe utworzone do odczytu PDF, prób i weryfikacji usuń przed przekazaniem etapu. Nie usuwaj cudzych plików ani materiałów źródłowych przy okazji porządkowania.
- Nowy sprint otrzymuje własne źródła i własny dokument. Korzystaj z ustaleń poprzednich sprintów, ale nie przenoś automatycznie ich akceptacji na nowy zakres.
- Trwałe zmiany zasad projektu zapisuj w tym pliku i odpowiednich instrukcjach ról. Nie zapisuj tutaj szczegółów implementacji ani bieżących raportów sprintu.

## Uruchamianie pracy

Lider może rozpocząć pracę poleceniem „Rozpocznij analizę sprintu 01 jako analityk”. Odczytaj wtedy właściwe instrukcje i PDF-y, utwórz dokument sprintu i wykonaj tylko etap analizy. Na bramce przedstaw materiał i zaczekaj na akceptację lidera.

Po otrzymaniu akceptacji user stories kontynuuj jako programista, a po akceptacji kodu — jako recenzent. Polecenie „Kontynuuj sprint 01” oznacza wznowienie aktualnego dozwolonego etapu, nigdy pominięcie brakującej akceptacji. Przed wznowieniem odtwórz stan z dokumentu sprintu i rozmowy; jeżeli nie ma wiarygodnej informacji o decyzji, zapytaj lidera.

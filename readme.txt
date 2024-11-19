Ta aplikacja symuluje stan populacji względem czasu pod wpływem epidemii, poprzez postacie reprezentujące gęstość na danym obszarze, które zmieniające kolor w zależności od jej statusu, postacie będą się znajdować na mapie z siatką.

Po uruchomieniu użytkownik widzi menu główne gdzie ma do wyboru:
- Jedną z 5 różnych map, którą wybiera poprzez kliknięcie na daną ikonę
- Ustawienia parametrów wejściowych, gdzie każdy jest oznaczony odpowiednim symbolem, posiada swój opis i wartość, którą można zmienić za pomocą suwaka obok
- Po lewej stronie użytkownik ma dostępne przyciski:
	- Start Symulacji - Zatwierdzenie wybranych parametrów i przejście do widoku symulacji
	- Resetuj parametry - Resetuje wszystkie parametry wejściowe do wartości domyślnych
	- Bibliografia - Spis wykorzystanych artykułów
	- Autorzy - Spis Autorów
	- Wyjście - Wyjście z aplikacji
	
Do reprezentacji stanu populacji wykorzystujemy odpowiednie kolory:
- Zielony - zdrowa populacja, podatna na choroby (ang. susceptible)
- Żółty - zarażona populacja, która jeszcze nie zaraża pozostałych (ang. exposed)
- Czerwony - zainfekowana populacja, która zaraża pozostałych (ang. infected)
- Niebieski - wyzdrowiała i odporna populacja, która przeszła chorobę i zyskała odporność (ang. recovered)
- Czarny - zmarła na infekcję populacja (ang. deceased)
	
Po przejściu do widoku symulacji, użytkownik widzi:
- Widok mapy, która jest złożona z komórek na której stoją postacie, których rozmiar reprezentuje jak bardzo zaludniona jest dane komórka. 
- Postać składa się z kilku warstw w różnych kolorach opisanych wyżej, które reprezentują jaki procent populacji danej komórki jest danym stanie.
- Pasek na dole ekranu to sumaryczna reprezentacja populacji wszystkich komórek
- Po prawej stronie, widać licznik dni od początku symulacji, a poniżej jest zawarta sumaryczna liczebność jednostek w dla każdego stanu. N - Suma populacji, S (susceptible) - zielony, E (exposed) - żółty, I (infected) - czerwony, R (recovered) - niebieski, D (deceased) - czarny
- W prawym dolnym rogu są dwa przyciski:
	- Start - Rozpoczyny/wznawia symulację
	- Stop - Pauzuje symulację
- W lewym górnym rogu jest przycisk Wyjście - który pozwala wyjść z aplikacji

W widoku symulacji użytkownik może sterować kamerą:
- Obrót kamery - ruch myszą z wciśniętym scrollem
- Ruch kamery - wsad

Po rozpoczęciu symulacji dopóki nie zostanie zatrzymana, wykonywane są kolejne kroki kóre aktualizują stan populacji oraz zwiększają licznik dni.
Obok postaci pojawiają się modele autobusu który swoją wielkością reprezentuje jak dużo podróżujących przyjeżdza do danej komórki.
W momencie kiedy liczba zainfekowanych spadnie do 0 (zarówno tych którzy zarażają i nie zarażają), symulacja kończy się i pojawia się odpowiedni komunikat.
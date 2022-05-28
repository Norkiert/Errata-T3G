-> S_B00

=== S_B00 ===
{ RANDOM(0, 3) :
- 0 : O co chodzi?
- 1 : Słucham.
- 2 : W czym problem?
- 3 : W czym mogę pomóc?
}
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nieważne"
- 2 : ~ odp = "Tak tylko sprawdzam, czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Co to za miejsce?]
    -> S_B01
+ [Gdzie jest następny mechanizm do naprawy?]
    -> Guide
+ [Do czego są te czerwone rury koło drzewa?]
    -> S_B02
+ [{odp}]
    -> END
    
=== Guide ===
Funkcja chwilowo niedostępna
    -> END

=== S_B01 ===
Znajdujesz się w części mechanizmu odpowiedzialnej za przetwarzanie danych.<n>Analiza świata prowadzona jest przy użyciu fizyki kwantowej. Subatomowe cząstki przechowujące informacje w postaci spinu, poruszają się po wyznaczonych trajektoriach z ogromną prędkością.
Abyś to pojął i był w stanie naprawić mechanizm, twoja percepcja została dostosowana do odbierania wymiaru na twoich ziemskich zasadach.<n>Cząsteczki zostały odpowiednio spowolnione i przedstawione jako kule, natomiast trasy widzisz jako pochylnie.
    -> END

=== S_B02 ===
Tuby to wyjścia poszczególnych podukładów logicznych, wykorzystujące splątanie kwantowe.<n>Mechnizm jest w pełni sprawny, gdy we wszytkich tubach znajdują się kule.
    -> END
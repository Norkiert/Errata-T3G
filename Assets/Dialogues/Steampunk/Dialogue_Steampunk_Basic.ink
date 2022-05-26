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
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Co to za miejsce?]
    -> S_B01
+ [Gdzie następny mechanizm do naprawy?]
    -> Guide
+ [Do czego są te czerwone rury koło drzewa?]
    -> S_B02
+ [{odp}]
    -> END
    
=== Guide ===
Funkcja chwilowo nie dostępna
    -> END

=== S_B01 ===
Znajdzujesz się w części mechanizmu odpowiedzialnej za przetwarzanie danych.<n>Analiza świata prowadzona jest przy użyciu fizyki kwantowej. Małe cząstki przechowujące inforamcjie w pastaci spinu, poruszają się  po wyznaczonych trasach z ogromną prędkością.
Abyś pojoł i był w stanie naprawić mechanizm, twoja percepcja została dostosowana do odbierania wymiaru na twoich ziemskich warunkach.<n>Cząsteczki zostały odpowiednio spowolnione i przesdstawione są jako kule, naomiast trasy widisz jako półrury.
    -> END

=== S_B02 ===
Tuby to wyjścia poszczególnych podukładów logicznych, poprzez splontanie kwantowe.<n>Mechnizm jest w pełni zprawny gdy we wszytkich tubach znajdują się kule.
    -> END
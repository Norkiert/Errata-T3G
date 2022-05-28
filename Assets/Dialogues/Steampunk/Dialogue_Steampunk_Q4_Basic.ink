-> S_Q3_B00

=== S_Q3_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nieważne"
- 2 : ~ odp = "Tak tylko sprawdzam, czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Co to za mechanizm?]
    -> S_Q3_B01
+ [Jak naprawić ten mechanizm?]
    -> S_Q3_B02
+ [{odp}]
    -> END

=== S_Q3_B01 ===
Jest to mechaniczny chronometr.<n>Dopuszcza on tylko cząstki, które znajdą się na wyjściu mechanizmu w odpowiednim czasie.<n>Pozwala to synchronizować odległe systemy międzywymiarowe.
	-> END

=== S_Q3_B02 ===
Kula na wejściu mechanizmu uruchamia podnośnik przy jego wyjściu.<n>Zatem kula musi pokonać trasę od wejścia do wyjścia w czasie jednego cyklu.<n>Musisz tylko ustawić tory we właściwym położeniu.
	-> END
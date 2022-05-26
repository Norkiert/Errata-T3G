-> S_Q3_B00

=== S_Q3_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Co to za mechanizm?]
    -> S_Q3_B01
+ [Jak naprawić ten mechanizm?]
    -> S_Q3_B02
+ [{odp}]
    -> END

=== S_Q3_B01 ===
Jest to mechaniczny czasomierz.<n>Dopuszcza on tylko cząstki które znajdą się na wujściu mechanizmu w odpowiednim czasie.<n>Pozwala to synchronizować odległe mechanzimy.
	-> END

=== S_Q3_B02 ===
Kula na wejściu mechanizmu uruchamia podnośnik przy wyjściu mechanizmu.<n>Zatem kula musi pokonać trasę od wejścia do wyjścia w czasie jednego cyklu.<n>Ustaw tory w sposób by się tak stało.
	-> END
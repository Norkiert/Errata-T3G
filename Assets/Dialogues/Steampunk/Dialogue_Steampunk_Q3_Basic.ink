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
+ [Jak go naprawić?]
    -> S_Q3_B02
+ [Jak naprawić tory?]
    -> S_Q3_B03
+ [{odp}]
    -> END

=== S_Q3_B01 ===
Jest to Slpiter.<n>Spliter rozdziela cząstki z jednej ścieżki na 5 symetrycznych torów.
	-> END

=== S_Q3_B02 ===
Na każdym z pięciu wyjść powinna znaleźć się jedna kula, ustaw tory w odpowiedni sposób.
	-> END

=== S_Q3_B03 ===
Niektóre elemety torów zostały wygięte przez dziwne anomalie, uderz w nie by zmienić ich rotację i przywrucić im odpowiedni stan.
	-> END
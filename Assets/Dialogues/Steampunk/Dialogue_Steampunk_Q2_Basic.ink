-> S_Q2_B00

=== S_Q2_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nieważne"
- 2 : ~ odp = "Tak tylko sprawdzam, czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Jak naprawić tory?]
    -> S_Q2_B01
+ [Jak przesunąć skrzynie?]
    -> S_Q2_B02
+ [Jak się stąd wydostać?]
    -> S_Q2_B03
+ [{odp}]
    -> END

=== S_Q2_B01 ===
Powyginane tory to nie jedyny problem. Niektóre elementy trasy uległy dyslokacji.<n>Oprócz obracania torów, musisz znaleźć ich brakujące elementy i przesunąć je w odpowiednie miejsca.
	-> END

=== S_Q2_B02 ===
Do przesunięcia skrzyni potrzeba pewnej energi.<n>Stań przy wybranej skrzyni, a następnie popchnij ją.<n>Jeśli skrzynia będzie przy ścianie, odbije się od niej.
	-> END

=== S_Q2_B03 ===
Najprościej będzie drzwiami.
+ [No oczywiście]
    -> END
+ [Przez zamknięte, niestety, nie dam rady.]
    -> S_Q2_B04
	
=== S_Q2_B04 ===
W takim razie jedynym wyjściem będzie, znajdujacy się wysoko, szyb dla kul.<n>Otworzy się on, gdy cały mechanizm w tym pomieszczeniu będzie sprawny.
	-> END
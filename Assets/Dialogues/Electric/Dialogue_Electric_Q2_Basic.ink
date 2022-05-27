-> E_Q2_B00

=== E_Q2_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam, czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Jak mam uziemić panel na wejściu?]
    -> E_Q2_B01
+ [Jak mam dosięgnąć gniazda uzmiającego?]
    -> E_Q2_B02
+ [Dlaczego nie mogę przełączyć dźwigni?]
    -> E_Q2_B03
+ [{odp}]
    -> END
    
	
=== E_Q2_B01 ===
Główny przewodnik uziemiający znajduje się na ścianie wieży.<n>Na przewodniku osadzona jest kapsuła z gniazdem.<n>Musisz połączyć przewodem kapsułę z gniazdem obok progu.
    -> END
	
=== E_Q2_B02 ===
Pozycja kapsuły jest definiowana poprzez pole magnetyczne.<n>Na pewno wiesz, że od kierunku ruchu i ilości elektronów, zależy kierunek i wielkość tego pola. Dlatego, ze względu na duży ładunek podstawy przedownika, kapsuła jest mocniej odpychana i znajduje się względnie wysoko.
Obok przewodnika znajduje się przełącznik, pozwalający zredukować energię w elektromagnesie. Dlatego musisz dołączyć równolegle obowód pomocniczy.<n>
    -> END
	
=== E_Q2_B03 ===
Obwód pomocniczy, znajdujdujący się w wieży, nie jest poprawnie zamknięty.<n>Powoduje to nagłe wyładowanie i odbicie dźwigni do stanu początkowego.<n>Znajdź kompatybilne przewody i zamknij obwód.
    -> END
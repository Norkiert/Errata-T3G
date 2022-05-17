-> E_Q2_B00

=== E_Q2_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Jak mam uziemić panel na wejściu?]
    -> E_Q2_B01
+ [Jak mam dosięgnąć gniazdka uzmiejącego?]
    -> E_Q2_B02
+ [Dlaczego nie mogę przełączyć dźwigni?]
    -> E_Q2_B03
+ [{odp}]
    -> END
    
	
=== E_Q2_B01 ===
Główny przewodnik uziemiający znajduje się na ścianie wieży.<n>Na przewodniku osadzona jest kapsuła z gniazdem<n>Wystarczy połączyć przedodem kapsułe z gniazdem obok progu.
    -> END
	
=== E_Q2_B02 ===
Pozycja kapsuły jest definiowana poprzez ilość energii w przewodniku.<n>Powinieneś wiedzieć że elektrony odpychają się, dlatego ze względu na ładunek podstawy przedownika, kapsuła znajduje się względnie wysoko.
Obok przewodnika znajduje się przełącznik, pozwalający zredukować energię przewodnika, poprzez dołączenie równolegle obowodu pomocniczego.<n>Wystarczy go włączyć.
    -> END
	
=== E_Q2_B03 ===
Obwód pomocniczy, znajdujdujący się w wierzy nie jest poprawnie zamknięty.<n>Powoduje to zbyt nagłe wyładowanie i odbicie dźwigni do stanu początkowego.<n>Znajdź odpowiednie przewody i zamknij obwód.
    -> END
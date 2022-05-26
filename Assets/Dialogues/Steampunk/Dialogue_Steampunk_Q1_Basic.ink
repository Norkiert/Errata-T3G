-> S_Q1_B00

=== S_Q1_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Co to za mechanizm?]
    -> S_Q1_B01
+ [Jak otworzyć drzwi?]
    -> S_Q1_B02
+ [Jak naprawić tory?]
    -> S_Q1_B03
+ [{odp}]
    -> END

=== S_Q1_B01 ===
To jeden z milionów przeżutników kwnatowych, użądzeń mających zapamiętywać stany.<n>Ten konkretny kieruje ruchem na tym obszarze, co wpływa na pozycję drzwi.
    -> END

=== S_Q1_B02 ===
Poziom otwartości drzwi zależy od ustawienia przerzutnika.<n>Jego mechanzim zmieni stan gdy otrzyma na wejściu nie parzystą ilość kul, w tym celu trzeba naprawić tor i stworzyć dodatkowa kulę za pomocą kreatora.
    -> END

=== S_Q1_B03 ===
Niektóre elemety torów zostały wygięte przez dziwne anomalie, uderz w nie by zmienić ich rotację i przywrucić im odpowiedni stan.
    -> END
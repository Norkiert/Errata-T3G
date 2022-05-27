-> E_Q1_B00

=== E_Q1_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Czemu nie mogę wyjść?]
    -> E_Q1_B01
+ [Co to za progi w przejściu?]
    -> E_Q1_B02
+ [Jak mam to naprawić?]
    -> E_Q1_B03
+ [{odp}]
    -> END
    
	
=== E_Q1_B01 ===
Panele przeciążeniowe, jak widzisz, uniemożliwiają wyjście.<n>Wszystkie obiekty fizyczne, które wchodzą z nimi w interkacje, zostają odrzucone. Winne jest pole magnetyczne generowane przez płynacy prąd elektryczny.
Napraw obwód, a będziesz mógł przejść.
    -> END
	
=== E_Q1_B02 ===
Panele ustawiane są między obszarami o różnych energiach potencjalnych, aby wzajemnie je równoważyć.<n>Nadwyżka energii musi zostać odprowadzona do rozpraszacza elektronowego. 
W tym miejscu połączenia zostały uszkodzone.<n>Obwód jest przepełniony energią.<n>Wszystkie obiekty fizyczne które wejdą z nim w interkacje zostaną odrzucone.
Znajdź przewody, podepnij je do pasujących gniazdek i napraw obwód.
    -> END
	
=== E_Q1_B03 ===
Znajdź przewody, oznaczone odpowiednim koloremi podepnij je do kompatybilnych gniazd, naprawiając obwód.
    -> END
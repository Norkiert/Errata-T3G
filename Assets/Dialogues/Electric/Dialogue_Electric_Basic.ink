-> E_B00

=== E_B00 ===
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
+ [A teraz gdzie jestem?]
    -> E_B01
+ [Gdzie następna rzecz do naprawy?]
    -> Guide
+ [Co to za lampki na drzewie?]
    -> E_B02
+ [{odp}]
    -> END
    
	
=== E_B01 ===
Znajdujesz się w wymiarze elektryczności.<n>(Nie wiem pomysłu nie mam ale tu trzeba coś więcej nabulbulać o kablach, prądnie, burzach itd)
    -> END
	
=== E_B02 ===
Jest to swojego rodzaju wskaźnik stanu sieci energetycznej w tym wymiarze.
Wszystkie układy pracują synchronicznie, tworząc jeden obwód.<n>Wskażnik nie jest wyjątkiem i pełni głównie funkcję filtru protonowego.
Pełnosprawny obwód powinien obciążać filtr w 100%<n>Obciążenie jest zauważalne poprzez zielone światło emitowane przez cefki, które nazwałeś lampkami.
    -> END
	
=== Guide ===
Funkcja chwilowo nie dostępna
    -> END
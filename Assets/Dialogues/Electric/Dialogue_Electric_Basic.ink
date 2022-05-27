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
- 2 : ~ odp = "Tak tylko sprawdzam, czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [A teraz gdzie jestem?]
    -> E_B01
+ [Gdzie jest następna rzecz do naprawy?]
    -> Guide
+ [Co to za lampki na drzewie?]
    -> E_B02
+ [{odp}]
    -> END
    
	
=== E_B01 ===
Znajdujesz się w wymiarze elektryczności.<n>Jeśli nie zrównoważysz występujących tu zaburzeń energetycznych, nasz świat się rozpadnie.
Wiesz już o sobie coraz więcej. Znasz się na prądzie elektrycznym i elektromagnetyzmie, choć nie pamiętasz wszystkiego.<n>Musisz połączyć wszystkie kable do odpowiednich gniazd i wyrównać pola magnetyczne. 
    -> END
	
=== E_B02 ===
Jest to swojego rodzaju wskaźnik stanu międzywymiarowej sieci energetycznej w tym wymiarze.
Wszystkie układy pracują synchronicznie, tworząc jeden obwód.<n>Wskażnik nie jest wyjątkiem i pełni głównie funkcję fazowego filtru elektronowego.
Zrównoważony obwód powinien obciążać filtr w 100%<n>Wielkość obciążenia jest pokazywane za pomocą zielonego światła emitowane przez cewki, które nazwałeś lampkami.
    -> END
	
=== Guide ===
Funkcja chwilowo nie dostępna
    -> END
-> E_Q3_B00

=== E_Q3_B00 ===
~ temp odp = "def"
{ RANDOM(0, 3) :
- 0 : ~ odp = "Nic, nic"
- 1 : ~ odp = "Już nie ważne"
- 2 : ~ odp = "Tak tylko sprawdzam czy jeszcze działasz"
- 3 : ~ odp = "Właśnie sobie przypomniałem"
}
+ [Gdzie jest problem?]
    -> E_Q3_B01
+ [Skąd mam wsiąść drugi przewód?]
    -> E_Q3_B02
+ [{odp}]
    -> END
    
	
=== E_Q3_B01 ===
Problem znajduje się przy dyfuzorze cząstek, na pierwszym piętrze budynku.
	-> END
	
=== E_Q3_B02 ===
Strumień cząstek wypchnoł przewód na pułkę kontrolną u szczytu komina.<n>Wejdź po stopniach by go dosięgnąć.
    -> END
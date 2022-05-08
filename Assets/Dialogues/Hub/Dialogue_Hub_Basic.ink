-> H_B00

=== H_B00 ===
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
+ [Co to za miejsce?]
    -> H_B01
+ [Co mam tu zrobić?]
    -> H_B02
+ [{odp}]
    -> END
    

=== H_B01 ===
Jesteś w centrum, gdzie schodzą się i przenikają wszystkie wymiary.
Procesy te generują olbrzymie ilości energii, która promieniuje zieloną poświatą.
    -> END
    
=== H_B02 ===
Podłącz przewód do wybranego źródła, aby przekierować energię do katalizatora.<n>Dzięki temu nastąpi chwilowe scalenie tego miejsca z wybranym wymiarem, umożliwiając ingerencję w jego strukturę.
Scalenie i ingerencja w inne wymiary jest niezbędna do naprawy wyrządzonych zniszczeń.<n>Aktualne możliwości maszyny nie pozwalają na przeprowadzenie takich manipulacji.
    -> END
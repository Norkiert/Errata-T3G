-> L_B00

=== L_B00 ===
{ RANDOM(0, 3) :
- 0 : O co chodzi?
- 1 : Słucham.
- 2 : W czym problem?
- 3 : W czym mogę pomóc?
}
+ [Co to za miejsce?]
    -> L_B01
+ [Co tu jest nie tak?]
    -> L_B02
+ [Jak mam to naprawić?]
    -> L_B03
+ [Właśnie sobie przypomniałem]
    -> END
    

=== L_B01 ===
To wymiar kreujący fotony.<n>W tym miejscu definiowana jest część ich właściwości.<n>Mechanizm, który widzisz odpowiada za sposób propagacji światła i określa równowagę kwantową.
+ [Czym są te fotony?]
    -> L_B02
+ [Coś jest nie tak z mechanizmem?]
    -> L_B03
    
=== L_B02 ===
Foton.<n>Twoja "fizyka" dalej tego nie pojęła.<n>Zastanawiacie się, czy jest to cząsteczka czy fala.<n>Prymitywne...
Dla zrozumienia problemu przyjmij, że zbiór ukierunkowanych fotonów tworzy wiązkę światła.
+ [Rozumiem, więc co jest nie tak?]
    -> L_B03
	
=== L_B03 ===
Matryce kierunkowe uległy dekalibracji.<n>Przez to fotony zachowują się w niepożądany sposób.<n>Powoduje to zaciemnienia na obszarach, gdzie być ich nie powinno.
+ [To, jak to naprawić?]
    -> L_B04
    
=== L_B04 ===
Wiązka fotonowa musi zostać nakierowana na skupiacz fazowy.<n>W tym celu skonfiguruj matryce.
+ [Jasne?]
    -> END
+ [A prościej?]
    -> L_B05
    
=== L_B05 ===
Używając twoich uproszczeń, przedstawię to tak:<n>Laser musi trafić w biały, trójkątny pryzmat.<n>W tym celu odpowiednio obróć kryształowe lustra.
+ [Teraz rozumiem]
    -> END
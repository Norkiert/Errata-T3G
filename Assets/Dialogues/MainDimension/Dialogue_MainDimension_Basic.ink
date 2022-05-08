-> M_B00

=== M_B00 ===
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
+ [Gdzie ja teraz jestem?]
	-> M_B01
+ [{odp}]
	-> END

=== M_B01 ===
Znajdujesz się w wymiarze głównym.
Nie ma on narzędzi kreujących właściwości wszechświata.<n>A przynajmniej nie tak, jak pozostałe wymiary.<n>Za to posiada unikalną zdolność do odzwierciedlania stanu wszechrzeczy.dlania stanu wszechrzeczy.
	-> END
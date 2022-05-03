-> M_B00

=== M_B00 ===
{ RANDOM(0, 3) :
- 0 : O co chodzi?
- 1 : Słucham.
- 2 : W czym problem?
- 3 : W czym mogę pomóc?
}
+ [Gdzie ja teraz jestem?]
	-> M_B01
+ [Nic, nic]
	-> END

=== M_B01 ===
Znajdujesz się w wymiarze głównym.
Nie ma on swojej funkcji która wpływałby na świat.<n>A przynajmniej nie tak jak pozostałe wymiary.<n>Za to posiada unikalną zdolność do odzwierciedlania stanu wszechrzeczy.
	-> END
-> main

EXTERNAL test(message)

=== main ===
Wybierz kolor
    + [red]
        -> chosen("red")
    + [yellow]
        -> chosen("yellow")


=== chosen(thing) ===
~ test(thing)

-> END
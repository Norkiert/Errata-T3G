-> main

=== main ===
Wybierz cos
    + [1]
        -> chosen(1)
    + [2]
        -> chosen(2)
        
=== chosen(thing) ===
Znow cos wybierz
    + [2]
        -> chosen2(thing, 2)
    + [3]
        -> chosen2(thing, 3)


=== chosen2(thing1, thing2) ===
{thing1} * {thing2} = {thing1 * thing2}

-> END
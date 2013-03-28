a <= [3,5,2] |
    fst <= L(l) => car(l); |
    snd <= L(l) => car(cdr(l)); |
   
    merge <= L(a, b) =>
        a_is_empty <= zero(length(a)) |
        b_is_empty <= zero(length(b)) |
        if and(a_is_empty, b_is_empty)
            [];
            if a_is_empty
                b;
                if b_is_empty
                    a;
                    car_a <= car(a) | car_b <= car(b) | 
                            if greater(car_a, car_b)
                                cons(car_b, merge(a, cdr(b)));
                                cons(car_a, merge(cdr(a), b));
                            ;
                ;
            ;
        ; |
   
   merge_sort <= L (b) =>
        len_b <= length(b) |
        if equals(len_b, 1)
            b;
            if equals(len_b, 2)
                fst_b <= fst(b) | snd_b <= snd(b) | 
                        if greater(fst_b, snd_b)
                            [snd_b, fst_b];
                            b;
                        ;

                half <= div(len_b, 2) |
                    fst_half <= slice_until(b, half) |
                    snd_half <= slice_from(b, half)  |
                    fst_half_s <= merge_sort(fst_half) | 
                    snd_half_s <= merge_sort(snd_half) |
                    merge(fst_half_s, snd_half_s);

            ;
        ; | merge_sort(a) | map(_, print) ;

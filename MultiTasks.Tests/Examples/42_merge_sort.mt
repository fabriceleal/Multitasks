a <= [1,2,3,4,5] |
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
   
   merge_sort <= L (ls) =>
        len_b <= length(ls) |
        if equals(len_b, 1)
            ls;
            if equals(len_b, 2)
                fst_b <= fst(ls) | snd_b <= snd(ls) |
                        if greater(fst_b, snd_b)
                            [snd_b, fst_b];
                            ls;
                        ;

                half <= div(len_b, 2) |
                    fst_half <= slice_until(ls, half) |
                    snd_half <= slice_from(ls, half)  | wait(fst_half, snd_half) |
                    fst_half_s <= merge_sort(fst_half) | 
                    snd_half_s <= merge_sort(snd_half) | wait(fst_half_s, snd_half_s) |
                    merge(fst_half_s, snd_half_s);

            ;
        ; | merge_sort(a) | map(_, print) ;

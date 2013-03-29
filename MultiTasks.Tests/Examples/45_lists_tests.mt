ls <= [1,2,3,4,5,6,7,8,9,10] |
    half <= div(length(ls), 2) |
    print("half:") |
    print(half) |
    fst <= slice_until(ls, half) | 
    snd <= slice_from(ls, half) | 
    print("1st half: ") | fst | map(_, print) |
    print("2nd half: ") | snd | map(_, print) | 
    
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
    
    print("merged:") | merge(fst, snd) | map(_, print) |
    
    fst <= L(l) => car(l); |
    snd <= L(l) => car(cdr(l)); |
        
    merge_sort <= L (w) =>
        if equals(length(w), 1)
            w;
            if not(equals(length(w), 2))
                half <= div(length(w), 2) | a <= merge_sort(slice_from(w, half)) | b <= merge_sort(slice_until(w, half)) | merge(a, b); 
                if greater(fst(w), snd(w))
                    [snd(w), fst(w)];
                    w;
                ;
            ;
        ; | print("sorted:") | merge_sort([6,5,4,3,2,1,7,8,9,10,11,12]) | map(_, print) 
    
;

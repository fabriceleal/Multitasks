a <= [5, 3, 1] |
max <= L(ls) =>  
  if equals(1, length(ls))
     car(ls);
     head <= car(ls) | tail <= cdr(ls) | 
        max_tail <= max(tail) |
        if greater(head, max_tail)
           head;
           max_tail;
; ;
| max(a) | print(_)
;
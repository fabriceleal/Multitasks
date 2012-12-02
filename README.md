# Multitasks

The one true purely asynchronous programming language!
Every instruction is parsed asynchronously; if you want to use return values, you should chain functions!

This is still on the making; for now, there is no such things as arithmetic, variable declaration or custom functions/types.

## Hello World

```
"Hello World!" | print(_);
```

This:
```
print | identity(_) | _("Hello World 1!");
"Hello World 2!" | print(_);
print | _("Hello World 3!");
print("Hello World 4!");
```

**may** ;) print:

```
Hello World 4!
Hello World 2!
Hello World 3!
Hello World 1!
```

## Syntax
A program is composed by a sequence of *chains*, which are executed asynchronously. To use return values, functions must be chained.
```
1 | print(_);
2 | print(_);
```
The `_` variable referers to the result of the previous `computation` (literal, function call, variable reference).


## Irony

Binaries for Irony included in the Vendors dir because seems that they change frequently
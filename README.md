# Multitasks

The one true purely asynchronous programming language!

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

**may** print:

```
Hello World 4!
Hello World 2!
Hello World 3!
Hello World 1!
```

## Irony

Binaries for Irony included in the Vendors dir because seems that they change frequently
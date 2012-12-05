# Multitasks

The one true purely asynchronous programming language; every instruction is parsed asynchronously!

## Disclaimer

This is a toy language. Feel free to ignore and move along.

## Hello World

Harmless enough:
```
print("Hello World!");
```

Which outputs the same as:
```
print | identity(_) | _("Hello World!");
```
or
```
print | _("Hello World!");
```
or
```
"Hello World!" | print(_);
```

## Example

This:
```
print | identity(_) | _("Hello World 1!");
"Hello World 2!" | print(_);
print | _("Hello World 3!");
print("Hello World 4!");
```

*may* ;) print:

```
Hello World 4!
Hello World 2!
Hello World 3!
Hello World 1!
```

For more examples, check the sources used for testing: https://github.com/fabriceleal/Multitasks/tree/master/MultiTasks.Tests/Examples

## Syntax
Coming soon ...

## Irony

Built using Irony *link*; binaries for Irony provided in the Vendors dir, because seems that they change frequently.

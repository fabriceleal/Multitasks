# Multitasks

The one true purely asynchronous programming language; every instruction is parsed asynchronously!

## Inspirations

[Futures and promises](http://en.wikipedia.org/wiki/Futures_and_promises)

## Documentation

Check the [wiki for the *official* documentation](https://github.com/fabriceleal/Multitasks/wiki).

## Hello World

Harmless enough:
```
print("Hello World!");
```

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

## Irony

Built using Irony http://irony.codeplex.com/; binaries for Irony provided in the Vendors dir, because seems that they change frequently.

### About Irony

Do not use irony. Who the hell would tight an AST with a scope????


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/fabriceleal/Multitasks/trend.png)](https://bitdeli.com/free "Bitdeli Badge")


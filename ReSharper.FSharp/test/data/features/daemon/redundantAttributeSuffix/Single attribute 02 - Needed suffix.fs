open System

type FooAttributeAttribute() = inherit Attribute()

[<FooAttribute>]
type A =
    class end

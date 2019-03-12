using JetBrains.ReSharper.Plugins.FSharp.Psi.Impl.Pointers;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Pointers;
using JetBrains.ReSharper.Psi.Resolve;

namespace JetBrains.ReSharper.Plugins.FSharp.Psi.Impl.DeclaredElement.CompilerGenerated
{
  public class UnionCaseTag : FSharpGeneratedMemberBase, IField, IFSharpGeneratedFromOtherElement
  {
    private IUnionCase UnionCase { get; }

    public IDeclaredElementPointer<IFSharpGeneratedFromOtherElement> CreatePointer() =>
      new UnionCaseTagPointer(this);

    public UnionCaseTag(IUnionCase unionCase) =>
      UnionCase = unionCase;

    public override string ShortName =>
      UnionCase.ShortName;

    public IClrDeclaredElement OriginElement => UnionCase;
    private ITypeElement Union => OriginElement.GetContainingType();
    private FSharpUnionTagsClass TagsClass => OriginElement.GetContainingType().GetUnionTagsClass();

    public int? Index => Union.GetUnionCases().IndexOf(UnionCase);

    protected override IClrDeclaredElement ContainingElement => TagsClass;
    public override ITypeElement GetContainingType() => TagsClass;
    public override ITypeMember GetContainingTypeMember() => TagsClass;

    public override DeclaredElementType GetElementType() =>
      CLRDeclaredElementType.CONSTANT;

    public IType Type => PredefinedType.Int;

    public ConstantValue ConstantValue =>
      Index is int index
        ? new ConstantValue(index, Type)
        : ConstantValue.BAD_VALUE;

    public bool IsField => false;
    public bool IsConstant => true;
    public bool IsEnumMember => false;
    public int? FixedBufferSize => null;

    public override bool IsStatic => true;
    public override bool IsReadonly => true;

    public override ISubstitution IdSubstitution => EmptySubstitution.INSTANCE;

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
        return true;

      if (!(obj is UnionCaseTag tag)) return false;

      if (!ShortName.Equals(tag.ShortName))
        return false;

      return Equals(GetContainingType(), tag.GetContainingType());
    }

    public override int GetHashCode() => ShortName.GetHashCode();
  }
}
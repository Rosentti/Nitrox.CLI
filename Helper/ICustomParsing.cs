using System.Runtime.Versioning;

namespace Nitrox.CLI.Helper;
public interface ICustomParsing<OutType, InType>
{
    static abstract OutType Parse(InType data);
    static abstract bool IsValid(InType data);
    //TODO: add completions here?
}
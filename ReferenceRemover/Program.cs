using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mono.Cecil;

namespace ReferenceRemover
{
  class Program
  {
    static int Main(string[] args)
    {
      if (args.Length == 0)
      {
        Console.WriteLine("RemoveReferences Assembly [Regex Target]");
        return 1;
      }

      var ass = AssemblyDefinition.ReadAssembly(args[0]);

      if (args.Length == 1)
      {
        foreach (var r in ass.MainModule.AssemblyReferences)
        {
          Console.WriteLine(r.FullName);
        }
        return 0;
      }

      var match = new Regex(args[1]);
      var target = AssemblyDefinition.ReadAssembly(args[2]);

      foreach (var r in ass.MainModule.AssemblyReferences)
      {
        if (match.IsMatch(r.FullName))
        {
          Console.WriteLine("Redirecting reference: {0} to {1}", r.FullName, target.FullName);
          r.Name = target.Name.Name;
          r.Version = target.Name.Version;
          r.PublicKeyToken = target.Name.PublicKeyToken;
        }
      }

      Console.WriteLine("Saving assembly: {0}", args[0]);
      ass.Write(args[0]);

      return 0;
    }
  }
}

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
        Console.WriteLine("RemoveReferences Assembly [Regex]");
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

      var col = new List<AssemblyNameReference>();

      foreach (var r in ass.MainModule.AssemblyReferences)
      {
        if (match.IsMatch(r.FullName))
        {
          col.Add(r);
        }
      }

      foreach (var r in col)
      {
        Console.WriteLine("Removing reference: {0}", r.FullName);
        ass.MainModule.AssemblyReferences.Remove(r);
      }

      Console.WriteLine("Saving assembly: {0}", args[0]);
      ass.Write(args[0]);

      return 0;
    }
  }
}

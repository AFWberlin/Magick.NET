﻿//=================================================================================================
// Copyright 2013-2016 Dirk Lemstra <https://magick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================

using System;
using System.CodeDom.Compiler;
using System.IO;

namespace Magick.NET.FileGenerator
{
  internal abstract class CodeGenerator
  {
    private IndentedTextWriter _Writer;

    protected CodeGenerator()
    {
    }

    protected CodeGenerator(CodeGenerator parent)
    {
      _Writer = parent._Writer;
    }

    protected int Indent
    {
      get
      {
        return _Writer.Indent;
      }
      set
      {
        _Writer.Indent = value;
      }
    }

    protected void Write(char value)
    {
      _Writer.Write(value);
    }

    protected void Write(int value)
    {
      _Writer.Write(value);
    }

    protected void Write(string value)
    {
      _Writer.Write(value);
    }

    protected void WriteEnd()
    {
      WriteEndColon();
    }

    protected void WriteElse(string action)
    {
      WriteLine("else");
      Indent++;
      WriteLine(action);
      Indent--;
    }

    protected void WriteEndColon()
    {
      Indent--;
      WriteLine("}");
    }

    protected void WriteIf(string condition, string action)
    {
      WriteLine("if (" + condition + ")");
      Indent++;
      WriteLine(action);
      Indent--;
    }

    protected void WriteLine()
    {
      _Writer.WriteLine();
    }

    protected void WriteLine(string value)
    {
      _Writer.WriteLine(value);
    }

    protected void WriteStart(string namespaceName)
    {
      _Writer.WriteLine("//=================================================================================================");
      _Writer.WriteLine("// Copyright 2013-" + DateTime.Now.Year + " Dirk Lemstra <https://magick.codeplex.com/>");
      _Writer.WriteLine("//");
      _Writer.WriteLine("// Licensed under the ImageMagick License (the \"License\"); you may not use this file except in");
      _Writer.WriteLine("// compliance with the License. You may obtain a copy of the License at");
      _Writer.WriteLine("//");
      _Writer.WriteLine("//   http://www.imagemagick.org/script/license.php");
      _Writer.WriteLine("//");
      _Writer.WriteLine("// Unless required by applicable law or agreed to in writing, software distributed under the");
      _Writer.WriteLine("// License is distributed on an \"AS IS\" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either");
      _Writer.WriteLine("// express or implied. See the License for the specific language governing permissions and");
      _Writer.WriteLine("// limitations under the License.");
      _Writer.WriteLine("//=================================================================================================");
      _Writer.WriteLine();
      WriteUsing();
      _Writer.WriteLine();
      _Writer.WriteLine("#if Q8");
      _Writer.WriteLine("using QuantumType = System.Byte;");
      _Writer.WriteLine("#elif Q16");
      _Writer.WriteLine("using QuantumType = System.UInt16;");
      _Writer.WriteLine("#elif Q16HDRI");
      _Writer.WriteLine("using QuantumType = System.Single;");
      _Writer.WriteLine("#else");
      _Writer.WriteLine("#error Not implemented!");
      _Writer.WriteLine("#endif");
      _Writer.WriteLine();
      _Writer.WriteLine("namespace " + namespaceName);
      WriteStartColon();
    }

    protected void WriteStartColon()
    {
      WriteLine("{");
      Indent++;
    }

    protected virtual void WriteUsing()
    {
      _Writer.WriteLine("using System;");
      _Writer.WriteLine("using System.Runtime.InteropServices;");
    }

    public void CloseWriter()
    {
      _Writer.InnerWriter.Dispose();
      _Writer.Dispose();
    }

    public void CreateWriter(string fileName)
    {
      Console.WriteLine("Creating: " + fileName);

      StreamWriter streamWriter = new StreamWriter(fileName);
      _Writer = new IndentedTextWriter(streamWriter, "  ");
    }
  }
}

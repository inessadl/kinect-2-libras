using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius.FingerTracking
{
    public class PythonInstance
    {
        private ScriptEngine engine;
        private ScriptScope scope;
        private ScriptSource source;
        private CompiledCode compiled;
        private object pythonClass;

        public PythonInstance(string code, string className = "PyClass")
        {
            //creating engine and stuff
            engine = Python.CreateEngine();
            scope = engine.CreateScope();

            //loading and compiling codes
            source = engine.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.Statements);
            compiled = source.Compile();

            //now executing this code (the code should contain a class)
            compiled.Execute(scope);

            //now creating an object that could be used to access the stuff inside a python script
            pythonClass = engine.Operations.Invoke(scope.GetVariable(className));
        }

        public void SetVariable(string variable, dynamic value)
        {
            scope.SetVariable(variable, value);
        }

        public dynamic GetVariable(string variable)
        {
            return scope.GetVariable(variable);
        }

        public void CallMethod(string method, params dynamic[] arguments)
        {
            engine.Operations.InvokeMember(pythonClass, method, arguments);
        }

        public dynamic CallFunction(string method, params dynamic[] arguments)
        {
            return engine.Operations.InvokeMember(pythonClass, method, arguments);
        }

    }
}

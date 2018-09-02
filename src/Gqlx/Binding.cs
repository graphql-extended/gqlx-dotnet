namespace Gqlx
{
    using Jint;
    using Jint.Native;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Binding
    {
        private readonly Engine _engine;
        private static readonly String _content = @"".Replace("[^]", ".");

        public Binding()
        {
            var engine = new Engine();
            engine.Execute(_content);
            _engine = engine;
        }

        public Engine EngineInstance
        {
            get { return _engine; }
        }

        public JsValue Call(String fn, params JsValue[] arguments)
        {
            var f =_engine.Execute($"gqlx.{fn}").GetCompletionValue();
            return f.Invoke(arguments);
        }

        public IDictionary<String, Boolean> GetDefaultApi()
        {
            var value = _engine.Execute("gqlx.defaultApi").GetCompletionValue();
            var obj = value.AsObject();
            var props = obj.GetOwnProperties();
            return props.ToDictionary(m => m.Key, m => m.Value.Value.AsBoolean());
        }
    }
}

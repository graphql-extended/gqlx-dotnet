namespace Gqlx
{
    using Jint.Native;
    using Jint.Native.Object;
    using System;
    using System.Collections.Generic;

    public class GqlxCompiler
    {
        private readonly Binding _binding;
        private readonly GqlxOptions _options;
        private readonly IDictionary<String, Boolean> _api;

        public GqlxCompiler(IDictionary<String, Boolean> api = null, GqlxOptions options = null)
        {
            _binding = new Binding();
            _options = options ?? GqlxOptions.Default;
            _api = api ?? _binding.GetDefaultApi();
        }

        public GqlxResult Compile(String name, String content)
        {
            var api = GetCurrentApi();
            var result = _binding.Call("compile", name, content, api).AsObject();
            return new GqlxResult
            {
                Name = result.GetProperty("name").Value.AsString(),
                Schema = result.GetProperty("schema").Value.AsString(),
                Source = result.GetProperty("source").Value.AsString(),
                Resolvers = result.GetProperty("resolvers").Value.ToObject(),
                CreateService = result.GetProperty("createService").Value.ToObject(),
            };
        }

        public Boolean Validate(String content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                var gql = _binding.Call("utils.parseDynamicSchema", content);

                if (gql != null && !gql.IsUndefined())
                {
                    var api = GetCurrentApi();
                    _binding.Call("utils.validate", gql, api);
                    return true;
                }
            }

            return false;
        }

        private JsValue GetCurrentApi()
        {
            var oi = new ObjectInstance(_binding.EngineInstance);

            foreach (var prop in _api)
            {
                oi.FastAddProperty(prop.Key, prop.Value, true, true, true);
            }

            return oi;
        }
    }
}

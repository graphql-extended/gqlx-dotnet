namespace Gqlx
{
    using System;

    public class GqlxResult
    {
        public String Name { get; set; }

        public String Source { get; set; }

        public String Schema { get; set; }

        public Object Resolvers { get; set; }

        public Object CreateService { get; set; }
    }
}

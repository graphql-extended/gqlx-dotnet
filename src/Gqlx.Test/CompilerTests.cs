namespace Gqlx.Test
{
    using Jint.Runtime;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class CompilerTests
    {
        [Test]
        public void SuccessfullyValidatesCorrectComplexGqlx()
        {
            var compiler = new GqlxCompiler();
            var result = compiler.Validate(@"
  type Foo {
    id: ID
    name: String
  }

  type Bar {
    id: ID
    age: Int
  }

  type Query {
    foo: [Foo]

    bar(name: String): Bar {
      get('api')
        .map(x => x[name].map(y => y.element))
        .filter(x => !x)
        .map(x => `Hello there ${x.y} from ${name}`)
    }

    qxz(id: ID!, name: String, ages: [Int]): [Bar] {
      get('api')
    }

    items(hashes: [String]): [Foo] {
      (hashes && hashes.length) ?
        post('api/item', {
          hashes,
          content: '',
        }).items
      :
        get('api/item').items
    }
  }

  type Mutation {
    foo(a: String, b: Float): [Int] {
      post('api/foo', {
        body: get('api'),
      })
    }
  }

  type Subscription {
    foo: [Foo] {
      listen('foo')
    }
  }
");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SuccessfullyValidatesCorrectSimpleGqlx()
        {
            var compiler = new GqlxCompiler();
            var result = compiler.Validate(@"
  type Foo {
    id: ID
    name: String
  }

  type Bar {
    id: ID
    age: Int
  }

  type Query {
    foo: [Foo] {
      get('api/foo')
    }
  }
");
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SuccessfullyCompilesSimpleGqlx()
        {
            var compiler = new GqlxCompiler();
            var result = compiler.Compile("test", @"
  type Foo {
    id: ID
    name: String
  }

  type Query {
    foo: [Foo] {
      get('api/foo')
    }
  }
");
            Assert.AreEqual("test", result.Name);
        }

        [Test]
        public void SuccessfullyValidatesEmptyGqlx()
        {
            var compiler = new GqlxCompiler();
            var result = compiler.Validate(@"");
            Assert.AreEqual(false, result);
        }

        [Test]
        public void SuccessfullyValidatesIncorrectGqlx()
        {
            var compiler = new GqlxCompiler();
            Assert.Throws<JavaScriptException>(() => compiler.Validate(@"
  type Mutation {
    foo(id: ID): [Int] {
      post('api/foo/' + ib, {
        body: get('api'),
      })
    }
  }
"));
        }
    }
}

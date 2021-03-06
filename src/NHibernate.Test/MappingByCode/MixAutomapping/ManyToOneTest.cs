using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Test.MappingByCode.MixAutomapping
{
	public class ManyToOneTest
	{
		private class AEntity
		{
			public int Id { get; set; }
			public BEntity B { get; set; }
			public string Name { get; set; }
		}

		private class BEntity
		{
			public int Id { get; set; }
		}

		[Test]
		public void WhenRelationWithTwoEntityThenIsManyToOne()
		{
			var autoinspector = new SimpleModelInspector();
			autoinspector.IsEntity((t, declared) => typeof(AEntity).Equals(t) || typeof(BEntity).Equals(t));

			var inspector = (IModelInspector)autoinspector;
			inspector.IsManyToOne(typeof(AEntity).GetProperty("B")).Should().Be.True();
		}

		[Test]
		public void WhenSimplePropertyThenIsNotManyToOne()
		{
			var autoinspector = new SimpleModelInspector();
			autoinspector.IsEntity((t, declared) => typeof(AEntity).Equals(t) || typeof(BEntity).Equals(t));

			var inspector = (IModelInspector)autoinspector;
			inspector.IsManyToOne(typeof(AEntity).GetProperty("Name")).Should().Be.False();
		}

		[Test]
		public void WhenRelatedMatchComponentThenIsNotManyToOne()
		{
			var autoinspector = new SimpleModelInspector();
			autoinspector.IsEntity((t, declared) => typeof(AEntity).Equals(t));
			autoinspector.IsComponent((t, declared) => typeof(BEntity).Equals(t));

			var inspector = (IModelInspector)autoinspector;
			inspector.IsManyToOne(typeof(AEntity).GetProperty("B")).Should().Be.False();
		}

		[Test]
		public void WhenRelatedDeclaredAsOneToOneThenIsNotManyToOne()
		{
			var autoinspector = new SimpleModelInspector();
			var mapper = new ModelMapper(autoinspector);
			mapper.Class<AEntity>(map => map.OneToOne(a => a.B, x => { }));
			mapper.Class<BEntity>(x=> { });
			var inspector = (IModelInspector)autoinspector;
			inspector.IsManyToOne(typeof(AEntity).GetProperty("B")).Should().Be.False();
		}
	}
}
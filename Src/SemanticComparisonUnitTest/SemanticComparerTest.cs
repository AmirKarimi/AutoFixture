﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class SemanticComparerTest
    {
        [Theory]
        [InlineData(typeof(IEqualityComparer))]
        [InlineData(typeof(IEqualityComparer<object>))]
        public void SutIsEqualityComparer(Type expectedType)
        {
            // Fixture setup
            // Exercise system
            var sut = new SemanticComparer<int, string>();
            // Verify outcome
            Assert.IsAssignableFrom(expectedType, sut);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new SemanticComparer<ConcreteType, ConcreteType>();
            object anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(new ConcreteType(), anonymousObject);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutDoesNotEqualAnonymousObjectSequence()
        {
            // Fixture setup
            var sut = new SemanticComparer<ConcreteType, ConcreteType>();

            object anonymousObject = new object();
            object[] anonymousObjects = new object[] { anonymousObject, anonymousObject, anonymousObject };
            
            ConcreteType concreteType = new ConcreteType();
            ConcreteType[] concreteTypes = new ConcreteType[] { concreteType, concreteType, concreteType };
            
            // Exercise system
            var result = concreteTypes.SequenceEqual(anonymousObjects, sut);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalStrongType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new SemanticComparer<TimeSpan, TimeSpan>();
            var other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalStrongTypeSequence()
        {
            // Fixture setup
            var sut = new SemanticComparer<TimeSpan, TimeSpan>();
            
            var ticks = 8293247;

            var value = TimeSpan.FromTicks(ticks);
            var values = new object[] { value, value, value };
            
            var other = TimeSpan.FromTicks(ticks);
            var others = new object[] { other, other, other };
            
            // Exercise system
            var result = values.SequenceEqual(others, sut);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalWeakType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new SemanticComparer<TimeSpan, TimeSpan>();
            object other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutEqualsIdenticalWeakTypeSequence()
        {
            // Fixture setup
            var sut = new SemanticComparer<TimeSpan, TimeSpan>();

            var ticks = 8293247;

            var value = TimeSpan.FromTicks(ticks);
            var values = new object[] { value, value, value };

            object other = TimeSpan.FromTicks(ticks);
            object[] others = new object[] { other, other, other };

            // Exercise system
            var result = values.SequenceEqual(others, sut);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ComparingStringPropertyHolderSemanticallyToRealStringPropertyHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        [Fact]
        public void ComparingStringPropertyHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText1;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText2;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void ComparingStringFieldHolderSemanticallyToRealStringFieldHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        [Fact]
        public void ComparingStringFieldHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText1;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void CompareAnonymousTypeSemanticallyToStringFieldHolderWillIndicateEqualityWhenValuesAreEqual()
        {
            // Fixture setup
            string anonymousText = "Anonymou text";

            var likenObject = new
            {
                Field = anonymousText
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        [Fact]
        public void CompareAnonymousTypeSemanticallyToStringPropertyHolderWillIndicateDifferenceWhenValuesAreDifferent()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            var likenObject = new
            {
                Field = anonymousText1
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void ObjectsWithNullPropertiesAreSemanticallyEqual()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        [Fact]
        public void SemanticallyAnObjectWithNullPropertyWillNotBeEqualToObjectWithValuedProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void SemanticallyAnObjectWithValuePropertyWillNotBeEqualToObjectWithNullProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = new object();

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void SemanticallyAnObjectWithPropertyWillNotBeEqualToPropertyWithDifferentProperty()
        {
            // Fixture setup
            var likenObject = new { SomeOtherProperty = new object() };

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void SemanticComparisonAgainstObjectWithOverloadedMembersWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithOverloadedMembers();

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, false);
        }

        [Fact]
        public void SemanticComparisonAgainstObjectWithIndexerWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithIndexer();

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        /// <summary>
        /// This test reproduces a bug.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test reproduces a bug where comparing these two instances causes the following
        /// exception to be thrown: "System.Security.VerificationException: Operation could
        /// destabilize the runtime."
        /// </para>
        /// </remarks>
        [Fact]
        public void SemanticComparisonAgainstDataErrorInfoWillNotThrow()
        {
            // Fixture setup
            var likenObject = new DataErrorInfo();

            var comparee = new DataErrorInfo();

            // The rest of the test
            SemanticComparerTest.CompareSemantically(likenObject, comparee, true);
        }

        [Fact]
        public void SutOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "Ploeh";
            var sut = new SemanticComparer<ConcreteType, AbstractType>();

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void SutOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property4 = "Ploeh";
            var sut = new SemanticComparer<ConcreteType, AbstractType>();

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = "Fnaah";
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void SutWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var sut = new SemanticComparer<ConcreteType, DoublePropertyHolder<object, object>>();
            var other = new DoublePropertyHolder<object, object>();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsTrueWillReturnTrue()
        {
            // Fixture setup
            var equalityResponse = true;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new SemanticComparer<PropertyHolder<EqualityResponder>, PropertyHolder<object>>();

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.Equal(equalityResponse, result);
            // Teardown
        }

        [Fact]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsFalseWillReturnFalse()
        {
            // Fixture setup
            var equalityResponse = false;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new SemanticComparer<PropertyHolder<EqualityResponder>, PropertyHolder<object>>();

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(value, other);
            // Verify outcome
            Assert.Equal(equalityResponse, result);
            // Teardown
        }

        [Fact]
        public void EqualsOfIdenticalObjectsReturnsTrue()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            // The rest of the test
            SemanticComparerTest.CompareSemantically(value, other, true);
        }

        [Fact]
        public void EqualsOfDifferentObjectsReturnFalse()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("amet", "consectetur", "adipisicing", "elit");
            // The rest of the test
            SemanticComparerTest.CompareSemantically(value, other, false);
        }

        [Fact]
        public void EqualsIsSummetricWithSemanticallyEqualObjects()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var dest = new DoublePropertyHolder<object, object>();
            dest.Property1 = source.Property1;
            dest.Property2 = source.Property2;
            var sut = new SemanticComparer<ConcreteType, DoublePropertyHolder<object, object>>();
            // Exercise system and verify outcome
            Assert.True(sut.Equals(source, dest) && sut.Equals(dest, source));
            // Teardown
        }

        [Fact]
        public void EqualsIsSummetricWithSemanticallyUnequalObjects()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var dest = new DoublePropertyHolder<object, object>();
            dest.Property1 = source.Property1;
            dest.Property2 = "abc";
            var sut = new SemanticComparer<ConcreteType, DoublePropertyHolder<object, object>>();
            // Exercise system and verify outcome
            Assert.False(sut.Equals(source, dest) && sut.Equals(dest, source));
            // Teardown
        }

        private static void CompareSemantically<TSource, TDestination>(TSource likenObject, TDestination comparee, bool expectedResult)
        {
            // Fixture setup
            var sut = new SemanticComparer<TSource, TDestination>();
            // Exercise system
            bool result = sut.Equals(likenObject, comparee);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
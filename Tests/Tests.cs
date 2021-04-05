using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DepartmentalTraversal;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void SimpleGraph()
        {
            StampsFinder finder = new StampsFinder();

            Department a = new Department(0, new Rule(1, 2, 1));
            Department b = new Department(1, new Rule(2, 1, 0));
            
            finder.AddDepartment(a);
            finder.AddDepartment(b);
            
            finder.Calculate(a, b);
            
            Assert.False(finder.IsCycle);
            
            HashSet<HashableArray> stampsInA = new HashSet<HashableArray>() { new HashableArray(new []{ 1 }) };
            HashSet<HashableArray> stampsInB = new HashSet<HashableArray>() { new HashableArray(new []{ 2 }) };
 
            Assert.True(finder.Get(a).SetEquals(stampsInA));
            Assert.True(finder.Get(b).SetEquals(stampsInB));
        }

        [Test]
        public void ForCycleWithSpecialDepartment()
        {
            StampsFinder finder = new StampsFinder();

            Department a = new Department(0, new Rule(1, 2, 2));
            Department b = new Department(1, new Rule(2, 1, 1));
            SpecialDepartment c = new SpecialDepartment(2, 
                new Rule(1, 2, 0),
                new Rule(1, 2, 1),
                1);
      
            finder.AddDepartment(a);
            finder.AddDepartment(b);
            finder.AddDepartment(c);
            
            finder.Calculate(a, b);
            
            Assert.True(finder.IsCycle);
            
            HashSet<HashableArray> stampsInA = new HashSet<HashableArray>() { new HashableArray(new []{ 1 }) };
            HashSet<HashableArray> stampsInB = new HashSet<HashableArray>();
            HashSet<HashableArray> stampsInC = new HashSet<HashableArray>() { new HashableArray(new []{ 1 }) };
 
            Assert.True(finder.Get(a).SetEquals(stampsInA));
            Assert.True(finder.Get(b).SetEquals(stampsInB));
            Assert.True(finder.Get(c).SetEquals(stampsInC));
        }
        
        [Test]
        public void DisconnectedGraph()
        {
            StampsFinder finder = new StampsFinder();

            Department a = new Department(0, new Rule(1, 2, 1));
            Department b = new Department(1, new Rule(2, 1, 0));
            Department c = new Department(2, new Rule(2, 1, 0));
            
            finder.AddDepartment(a);
            finder.AddDepartment(b);
            finder.AddDepartment(c);
            
            finder.Calculate(a, c);
            
            Assert.True(finder.IsCycle);
        }
        
        [Test]
        public async Task ConcurrentQueries()
        {
            StampsFinder finder = new StampsFinder();

            Department a = new Department(0, new Rule(1, 2, 1));
            Department b = new Department(1, new Rule(2, 1, 0));
            
            finder.AddDepartment(a);
            finder.AddDepartment(b);
            
            finder.Calculate(a, b);
            
            Task task1 = Task.Run((() =>
            {
                var response = finder.Get(a);
                foreach (var array in response)
                {
                    if (array.Length == 0) continue;
                    for (int i = 0; i < array.Length; ++i)
                        Console.Write(array[i] + " ");
                    Console.WriteLine();
                }
            }));
            
            Task task2 = Task.Run((() =>
            {
                var response = finder.Get(a);
                foreach (var array in response)
                {
                    if (array.Length == 0) continue;
                    for (int i = 0; i < array.Length; ++i)
                        Console.Write(array[i] + " ");
                    Console.WriteLine();
                }
            }));

            await task1;
            await task2;
            
            Assert.False(finder.IsCycle);
            
            HashSet<HashableArray> stampsInA = new HashSet<HashableArray>() { new HashableArray(new []{ 1 }) };
            HashSet<HashableArray> stampsInB = new HashSet<HashableArray>() { new HashableArray(new []{ 2 }) };
 
            Assert.True(finder.Get(a).SetEquals(stampsInA));
            Assert.True(finder.Get(b).SetEquals(stampsInB));
        }

        [Test]
        public void MediumGraph()
        {
            SpecialDepartment a = new SpecialDepartment(0, 
                new Rule(0, -1, 3),
                new Rule(0, -1, 1),
                2);
            Department b = new Department(1, new Rule(1, -1, 2));
            Department c = new Department(2, new Rule(2 ,-1, 0));
            Department d = new Department(3, new Rule(3, -1, 3));

            StampsFinder finder = new StampsFinder();
            finder.AddDepartment(a);
            finder.AddDepartment(b);
            finder.AddDepartment(c);
            finder.AddDepartment(d);
            
            finder.Calculate(a, d);
            
            Assert.False(finder.IsCycle);
            
            HashSet<HashableArray> stampsInA = new HashSet<HashableArray>() 
            { new HashableArray(new []{ 0 }),
                new HashableArray(new []{ 0, 1, 2 }) };
            
            HashSet<HashableArray> stampsInB = new HashSet<HashableArray>() { new HashableArray(new []{ 0, 1 }) };
            HashSet<HashableArray> stampsInC = new HashSet<HashableArray>() { new HashableArray(new []{ 0, 1, 2 }) };
            HashSet<HashableArray> stampsInD = new HashSet<HashableArray>() { new HashableArray(new []{ 0, 1, 2, 3 }) };
            
            Assert.True(finder.Get(a).SetEquals(stampsInA));
            Assert.True(finder.Get(b).SetEquals(stampsInB));
            Assert.True(finder.Get(c).SetEquals(stampsInC));
            Assert.True(finder.Get(d).SetEquals(stampsInD));
        }
    }
}
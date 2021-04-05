using System;
using System.Collections.Generic;
using System.Linq;

namespace DepartmentalTraversal
{
    public class StampsFinder
    { 
        private int _start, _end;
        private readonly Dictionary<int, Department> _departments = new Dictionary<int, Department>();

        /// <summary>
        /// Stores all lists for every department.
        /// </summary>
        private Dictionary<int, HashSet<HashableArray>> _answer;
        
        /// <summary>
        /// Current list.
        /// </summary>
        private readonly HashSet<int> _list = new HashSet<int>();

        private bool _isCycle;
        public bool IsCycle => _isCycle;
        
        public void AddDepartment(Department department)
        {
            if (_departments.ContainsKey(department.Number)) 
                throw new ArgumentException($"Department with number {department.Number} already exist.");
            _departments.Add(department.Number, department);
        }

        /// <summary>
        /// Initialize and start the Dfs.
        /// /// </summary>
        /// <param name="entryDepartment">Entry point.</param>
        /// <param name="endDepartment">End point.</param>
        public void Calculate(Department entryDepartment, Department endDepartment)
        {
            _start = entryDepartment.Number;
            _end = endDepartment.Number;
            
            _answer = new Dictionary<int, HashSet<HashableArray>>();
            foreach (var department in _departments)
                _answer[department.Key] = new HashSet<HashableArray>();
            
            Dfs(_start);
        }

        /// <summary>
        /// Response to request.
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public HashSet<HashableArray> Get(Department department)
        {
            if (_answer == null)
                throw new Exception("You must call Calculate() before queries.");
            if (!_departments.ContainsKey(department.Number))
                throw new ArgumentException($"Department with number {department.Number} does not exist.");
            
            return _answer[department.Number];
        }
        
        /// <summary>
        /// Graph traversal.
        /// </summary>
        /// <param name="vertex">Number of current department.</param>
        /// <exception cref="ArgumentException"></exception>
        private void Dfs(int vertex)
        {
            if (!_departments.ContainsKey(vertex))
                throw new ArgumentException($"Department with number {vertex} does not exist.");

            int next = _departments[vertex].GetNext(_list);
            var stampsArray = new HashableArray(_list.ToArray());
            if (_answer[vertex].Contains(stampsArray))
            {
                _isCycle = true;
                return;
            }

            _answer[vertex].Add(stampsArray);
            if (vertex != _end)
                Dfs(next);
        }
    }
}
using System.Collections.Generic;

namespace DepartmentalTraversal
{
    public class Department
    {
        private readonly Rule _rule;

        public int Number { get; }

        public Department(int number, Rule rule)
        {
            Number = number;
            _rule = rule;
        }

        public virtual int GetNext(HashSet<int> list)
        {
            return  _rule.Run(list);
        }
    }
}